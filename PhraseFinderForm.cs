using Acrobat;
//using AFORMAUTLib;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;

using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Reflection.Metadata;
using System;
using Application = System.Windows.Forms.Application;
using System.Diagnostics;

//using System.Timers;


// data mining DOCX appplication
// copyright 2023, Joseph Stateson  github/jstateson  
/*
* use Ctrl + k + c to comment out lines an u to uncomment
* 
*/

namespace DOC_PhraseFinder
{

    public partial class PhraseFinderForm : Form
    {
        private bool bMyDebug = false;  // enable to see informational messages
        private bool bAllExact = true;    // if false then we need to turn the sentences into a string[]
        private int[] ThisPageList;
        private int iCurrentPage = 0;
        private bool bStopEarly = false;
        private int NumPhrases = 6;
        private int TotalDocPages, TotalMatches;
        private StringCollection scSavedWords;
        private string CurrentActivePhrase = "";
        private int iFoundInSentence; // this is index to the phrase that was found.  It is not fixed.
        private bool bUseFound; // used with iFoundInSentence to indicate the phrase is not fixed
        private int iNullCount = 0;
        private int iCurrentPagePhraseCount = 0;
        private int iCurrentPagePhraseActive = 0;
        private int iCurrentRow = 0;
        private int[] SrtIndex;

        private Microsoft.Office.Interop.Word.Application oWord;
        private Microsoft.Office.Interop.Word.Document oDoc;

        private List<cPhraseTable> phlist = new List<cPhraseTable>();   // table of phrases
        private cLocalSettings LocalSettings = new cLocalSettings();    // table of settings
        //private static System.Timers.Timer aTimer;

        private string[] InitialPhrase = new string[6] { "labor cost", "prices charged", "catered meals", "program regulations", "7 CFR 250.53(a)(11)", "7 CFR 250.53(a)(12)" };
        private string[] WorkingPhrases = new string[6]; // same as above but optomises somewhat for case sensitivity
        private bool[] bUsePhrase = new bool[6] { true, true, true, true, true, true };
        private bool[] bExactMatch = new bool[6] { true, false, true, true, true, true };
        private string ExpectedVersion = "1"; // 1: version has 4 columns in grid

        /// <summary>
        /// entry point for main form
        /// </summary>
        public PhraseFinderForm()
        {
            InitializeComponent();

            NumPhrases = globals.ObtainProjectSettings(ref InitialPhrase, ref bUsePhrase, ref bExactMatch);
            WorkingPhrases = new String[NumPhrases];
            globals.GetLocalSettings(ref LocalSettings);
            cbIgnoreCase.Checked = LocalSettings.bIgnoreCase;
            cbWholeWord.Checked = LocalSettings.bWholeWord;
            FillPhrases();
            tbPdfName.Text = " (v) 1.0 (c)Stateson";
            //globals.GiveInitialWarning();
            //aTimer = new System.Timers.Timer(100);
            //aTimer.Enabled = false;
            //aTimer.Elapsed += OnTimedEvent;
            //aTimer.AutoReset = false;

        }


        private bool GetPageCount()
        {
            TotalDocPages = oDoc.ComputeStatistics(WdStatistic.wdStatisticPages, false);
            pbarLoading.Maximum = TotalDocPages;
            tbNumPages.Text = TotalDocPages.ToString();
            return TotalDocPages > 0;
        }


        /// <summary>
        /// count number of matches for that were found for each phrase 
        /// and return the total number of matches
        /// </summary>
        /// <returns></returns>
        private int GetMatchCount()
        {
            int lCnt = 0;
            for (int i = 0; i < NumPhrases; i++)
            {
                int j = phlist[i].iNumber;
                lCnt += j;
                phlist[i].Number = j.ToString();
            }
            return lCnt;
        }

        private void AllowProgressEvent()
        {
            pbarLoading.Increment(1);
            pbarLoading.Update();
            pbarLoading.Refresh();
            System.Windows.Forms.Application.DoEvents();
        }



        /// <summary>
        /// strIn is a sentence or null
        /// i is index into the phlist and associated tables (use, match, phases, working)
        /// this function is only used when searching for combination of words in a sentence
        /// need to look for a period to stop the search!!! assume period with two spaces
        /// </summary>
        /// <param name="strIn"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private string FindAnyMatch(string strIn, int i, int n)
        {
            int iStart = 0;
            int i1 = -1;
            string EndSent = ". "; // NOT ALL USE TWO SPACES !!!! TODO TO DO
            string candidate = "";
            foreach (string str in phlist[i].strInSeries)
            {
                int iLoc = strIn.Substring(iStart).IndexOf(str);
                if (iLoc < 0) return "";
                if (cbWholeWord.Checked)
                {
                    if (!globals.IsWholeWord(strIn, iStart + iLoc, str.Length))
                    {
                        return "";
                    }

                }
                if (i1 < 0) i1 = iLoc;  // very first word is i1
                iStart += iLoc + str.Length; // next phrase must be start past here
                if (iStart >= n) return "";
            }
            candidate = strIn.Substring(i1, iStart - i1);
            if (candidate.Contains(EndSent)) return "";
            return candidate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBig1"></param>
        /// <param name="Bigs"></param>
        /// <param name="j"></param>
        /// <param name="p"></param>
        private void FindMatches(ref string strBig1, ref string[] Bigs, int j, int p)
        {
            string strBig = globals.RemoveWhiteSpace(strBig1);
            string[] strSentence = globals.StrToStrs(strBig);
            string strPhrase = WorkingPhrases[j];
            int iWidth = strPhrase.Length;
            int i = 0;
            int FoundCount = 0; //if more the one of boolean AND type on a page
            cSeriesOnPage sop = new cSeriesOnPage();
            if (bExactMatch[j])
            {
                while (true)
                {
                    i = strBig.IndexOf(strPhrase);
                    if (i == -1) return;
                    if (cbWholeWord.Checked)
                    {
                        if (!globals.IsWholeWord(strBig, i, strPhrase.Length))
                            return;
                    }
                    phlist[j].AddPage(p);
                    phlist[j].IncMatch();
                    strBig = strBig.Remove(i, iWidth);
                }
                return;
            }
            i = -1;
            foreach (string str in Bigs)
            {
                int n = str.Length;
                i++;
                if (n < iWidth) continue;   // cannot fit so cannot be found
                string strFound = FindAnyMatch(str, j, n);
                if (strFound != "")
                {
                    FoundCount++;
                    sop.SeriesOnPage.Add(strFound);
                    phlist[j].AddPage(p);
                    phlist[j].IncMatch();
                }
            }
            if (FoundCount > 0)
                phlist[j].FoundInSeries.Add(sop);
        }


        /// <summary>
        /// search this page. Note that C# app uses 0..npages-1 but word needs 1..npages
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool SearchPage(int pp)
        {
            int p = pp + 1;
            string aPage = "";
            string[] aPages = null;
            object What = Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage;
            object Which = Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToAbsolute;
            object Miss = System.Reflection.Missing.Value;
            object Start;
            object End;
            object CurrentPageNumber;
            object NextPageNumber;
            CurrentPageNumber = (Convert.ToInt32(p.ToString()));
            NextPageNumber = (Convert.ToInt32((p + 1).ToString()));
            // Get start position of current page
            try
            {
                Start = oWord.Selection.GoTo(ref What, ref Which, ref CurrentPageNumber, ref Miss).Start;
            }
            catch (Exception ex)
            {
                if (bMyDebug)
                    tbMatches.Text += ex.Message + "\r\n";
                MessageBox.Show("You may have closed the document\r\nExit this program and start over", "ERROR-1"); ;
                return false;
            }
            // Get end position of current page                                
            End = oWord.Selection.GoTo(ref What, ref Which, ref NextPageNumber, ref Miss).End;
            // Get text
            if (Convert.ToInt32(Start.ToString()) != Convert.ToInt32(End.ToString()))
            {
                aPage = oDoc.Range(ref Start, ref End).Text;
            }
            else
            {
                aPage = oDoc.Range(ref Start).Text;
            }
            if (bMyDebug)
            {
                string strTemp = "";
                if (aPage.Length >= 20)
                    strTemp = aPage.Substring(0, 20);
                else strTemp = aPage.ToString();

                tbMatches.Text += p.ToString("D4") + ": " + aPage.Length.ToString("D5") + " " + strTemp + "\r\n";
            }
            if (cbIgnoreCase.Checked) aPage = aPage.ToLower();
            if (!bAllExact) aPages = globals.StrToStrs(aPage);
            for (int i = 0; i < NumPhrases; i++)
            {
                if (phlist[i].Select)
                    FindMatches(ref aPage, ref aPages, i, p);
            }
            return true;
        }


        private void ShowFoundPage()
        {
            if (iCurrentPage < 0) return;
            if (oDoc == null) return;
            if (oWord == null) return;
            object What = Microsoft.Office.Interop.Word.WdGoToItem.wdGoToPage;
            object Which = Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToAbsolute;
            object Miss = System.Reflection.Missing.Value;
            object Start;
            object CurrentPageNumber;
            CurrentPageNumber = iCurrentPage; // (Convert.ToInt32(iCurrentPage.ToString()));
            // Get start position of current page
            try
            {
                Start = oWord.Selection.GoTo(ref What, ref Which, ref CurrentPageNumber, ref Miss).Start;
            }
            catch (Exception ex)
            {
                if (bMyDebug)
                    tbMatches.Text += "error " + ex.Message + "\r\n";
                MessageBox.Show("Document probably closed, please exit and restart", "ERROR");
                return;
            }
            oWord.Visible = true;
            object FindText = CurrentActivePhrase;
            object MyCase = !cbIgnoreCase.Checked;  // seems to be just the opposed of what I thought
            object MyWholeWord = cbWholeWord.Checked;
            if (bUseFound)
            {
                FindText = phlist[iCurrentRow].FoundInSeries[iFoundInSentence].SeriesOnPage[iCurrentPagePhraseActive];
            }
            if (bMyDebug)
                tbMatches.Text += "looking for " + FindText + "\r\n";
            oWord.Selection.Find.ClearFormatting();
            oWord.Selection.Find.Execute(FindText, MyCase, MyWholeWord);
        }


        private void nudPage_ValueChanged(object sender, EventArgs e)
        {
            if (ThisPageList == null) return;
            int iVal = Convert.ToInt32(nudPage.Value);
            iFoundInSentence = iVal;
            iCurrentPage = ThisPageList[iVal];
            tbViewPage.Text = iCurrentPage.ToString();
            iCurrentPagePhraseActive = 0;
            iCurrentPagePhraseCount = phlist[iCurrentRow].WordsOnPage[Convert.ToInt32(nudPage.Value)];
            btnNext.Visible = iCurrentPagePhraseCount > 1;
            ShowFoundPage();
            return;
        }

        //AcroRd32.exe /A "zoom=50&navpanes=1=OpenActions&search=batch" PdfFile
        // above search for the phrase "batch" is another way


        private bool RunSearch()
        {
            if (oDoc != null)
            {
                string OutText = "";
                TotalMatches = 0;
                iNullCount = 0;
                iCurrentPage = 1;
                tbMatches.Text += "Searching ...\r\n";

                for (int p = 0; p < TotalDocPages; p++)
                {
                    bool bOK = SearchPage(p);
                    if (!bOK)
                    {
                        tbMatches.Text += "problem reading doc at page " + p.ToString();
                        return false;
                    }
                    AllowProgressEvent();
                    if ((p % 10) == 0)
                    {
                        tbpageNum.Text = p.ToString();
                    }
                    if (bStopEarly)
                    {
                        bStopEarly = false;
                        break;
                    }
                }
                pbarLoading.Value = 0;   // clear the progress bar and show results of the pattern search
                for (int i = 0; i < NumPhrases; i++)
                {
                    if (phlist[i].iNumber > 0)
                    {
                        OutText += ">" + phlist[i].Phrase.ToUpper() + "<    found on following pages\r\n";
                        OutText += phlist[i].strPages + "\r\n";
                        OutText += "Total Duplicate pages: " + phlist[i].iDupPageCnt + "\r\n\r\n";
                    }
                }
                if (iNullCount > 0) tbMatches.Text += "Null words found:" + iNullCount.ToString() + "\r\n";
                tbMatches.Text += OutText;
                TotalMatches = GetMatchCount();
                tbTotalMatch.Text = TotalMatches.ToString();
                //avDoc.Close(1);
                dgv_phrases.DataSource = phlist.ToArray(); // connect results to the data grid view widget
                return true;
            }
            return false;
        }


        /// <summary>
        /// use only those phrases that have been selected
        /// </summary>
        private void FillPhrases()
        {
            cPhraseTable cpt;
            phlist.Clear();
            SortPhrasesList();
            for (int i = 0; i < NumPhrases; i++)
            {
                cpt = new cPhraseTable();
                cpt.InitPhrase(InitialPhrase[i], bUsePhrase[i], bExactMatch[i]);
                phlist.Add(cpt);
            }
            dgv_phrases.DataSource = phlist.ToArray();
            //MessageBox.Show("Got this far?");  // used for debugging an internal fault .NET7 ???
            //problem does not occur anymore
            //https://learn.microsoft.com/en-us/answers/questions/1340009/indexoutofrangeexception-but-error-occurs-only-for
        }

        /// <summary>
        /// Initial setup of phrases, mark all as selected
        /// </summary>
        private void FillNewPhrases()
        {
            cPhraseTable cpt;
            phlist.Clear();
            for (int i = 0; i < NumPhrases; i++)
            {
                cpt = new cPhraseTable();
                cpt.InitPhrase(InitialPhrase[i], bUsePhrase[i], bExactMatch[i]);
                phlist.Add(cpt);
            }
            dgv_phrases.DataSource = phlist.ToArray();
        }

        private void ClearLastResults()
        {
            int i = 0;
            cPhraseTable cpt;
            foreach (DataGridViewRow row in dgv_phrases.Rows)
            {
                row.Cells[3].Value = "";
                cpt = phlist[i];
                cpt.Number = "";
                cpt.nFollowing = 0;
                cpt.iNumber = 0;
                cpt.iDupPageCnt = 0;
                cpt.iLastPage = 0;
                cpt.strPages = "";
                cpt.nFollowing = 0;
                cpt.WordsOnPage.Clear();
                cpt.FoundInSeries.Clear();
                i++;
            }
            tbMatches.Clear();
        }

        private void FormSortIndex()
        {
            SrtIndex = new int[NumPhrases];
            int[] SrtValue = new int[NumPhrases];
            int j1, j2, k = NumPhrases - 1;
            for (int i = 0; i < NumPhrases; ++i)
            {
                SrtIndex[i] = i;
                SrtValue[i] = InitialPhrase[i].Length;
            }
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    j1 = SrtIndex[j];
                    j2 = SrtIndex[j + 1];
                    if (SrtValue[j1] < SrtValue[j2])
                    {
                        SrtIndex[j] = j2;
                        SrtIndex[j + 1] = j1;
                    }
                }
            }
        }

        private void SortPhrasesList()
        {
            int j;
            string[] pTemp = new string[NumPhrases];
            bool[] bTemp = new bool[NumPhrases];
            bool[] bExact = new bool[NumPhrases];
            FormSortIndex();
            for (int i = 0; i < NumPhrases; i++)
            {
                j = SrtIndex[i];
                pTemp[i] = InitialPhrase[j];
                bTemp[i] = bUsePhrase[j];
                bExact[i] = bExactMatch[j];
            }
            for (int i = 0; i < NumPhrases; i++)
            {
                InitialPhrase[i] = pTemp[i];
                bUsePhrase[i] = bTemp[i];
                bExactMatch[i] = bExact[i];
            }
        }


        /// <summary>
        /// Check for improper wording or characters in the phrase
        /// since the user may have edited the phrases be sure to sort them
        /// </summary>
        /// <returns></returns>
        private bool ErrorsInTable()
        {
            bool bMustSort = false; // length of phrase may have changed
            for (int i = 0; i < NumPhrases; i++)
            {
                string strTemp = globals.RemoveWhiteSpace(phlist[i].Phrase);
                if (strTemp != phlist[i].Phrase)
                {
                    bMustSort = true;
                    phlist[i].Phrase = strTemp;
                    InitialPhrase[i] = strTemp;
                }
            }
            if (bMustSort)
            {
                SortPhrasesList();
                FillNewPhrases();
            }
            dgv_phrases.Refresh();
            return false;
        }

        private void btnRunSearch_Click(object sender, EventArgs e)
        {
            ClearLastResults();
            if (SaveEditedValues()) return;
            btnRunSearch.Enabled = false;
            btnStopScan.Enabled = true;
            RunSearch();
            btnRunSearch.Enabled = true;
            btnStopScan.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int n = InitialPhrase.Length + 1;
            string[] OldPhrases = new string[n];
            bool[] bOld = new bool[n];
            WorkingPhrases = new string[n];
            for (int i = 0; i < n - 1; i++)
            {
                OldPhrases[i] = InitialPhrase[i];
                bOld[i] = bUsePhrase[i];
            }
            InitialPhrase = new string[n];
            bUsePhrase = new bool[n];
            for (int i = 0; i < n - 1; i++)
            {
                InitialPhrase[i] = OldPhrases[i];
                bUsePhrase[i] = bOld[i];
            }
            InitialPhrase[n - 1] = "change me then SAVE";
            bUsePhrase[n - 1] = true;
            NumPhrases = n;
            FillPhrases();
        }


        private void btnViewDoc_Click(object sender, EventArgs e)
        {
            iCurrentPage = Convert.ToInt32(tbViewPage.Text);
            ShowFoundPage();
        }


        private void SetNumeric_UpDn_Page()
        {
            nudPage.ValueChanged -= nudPage_ValueChanged;   // if max changes then value can also change!!
            nudPage.Maximum = ThisPageList.Length - 1;
            nudPage.Visible = ThisPageList.Length > 1;      // only show numeric up/down if more than 1 page
            btnNext.Visible = iCurrentPagePhraseCount > 0;
            // cannot let the event fire when resetting the value of the widget

            nudPage.Value = 0;
            nudPage.ValueChanged += nudPage_ValueChanged;
            ShowFoundPage();
        }



        /// <summary>
        /// Get the list of pages that contains the wanted phrase and update the
        /// numeric up/down widget so the pages can be scrolled.  if more than one phrase on a page
        /// then the "next phrase" is enabled
        /// </summary>
        private void GetSelection()
        {
            System.Drawing.Point ThisRC = dgv_phrases.CurrentCellAddress;
            iCurrentRow = ThisRC.Y;
            int iCol = ThisRC.X;
            if (iCol < 3) return; // allow editing the text or checkboxs column
            iCurrentPage = -1;
            iFoundInSentence = 0;
            if (phlist[iCurrentRow].strPages != "")
            {
                ThisPageList = phlist[iCurrentRow].strPages.Split(',').Select(int.Parse).ToArray();
                iCurrentPage = ThisPageList[0];
                tbViewPage.Text = iCurrentPage.ToString();
                CurrentActivePhrase = phlist[iCurrentRow].Phrase; // this is used if match must be exact
                iCurrentPagePhraseActive = 0; // start of first (at least one) phrase found on the page
                iCurrentPagePhraseCount = phlist[iCurrentRow].WordsOnPage[0];
                bUseFound = !phlist[iCurrentRow].Match;  // if not exact match then the phrase can change
                SetNumeric_UpDn_Page();
                AllowNextPhrase();
            }
        }

        private void dgv_phrases_Click(object sender, EventArgs e)
        {
            GetSelection();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int n = 0;
            int i = 0;
            DialogResult DiaRes;
            List<int> KeepList = new List<int>();
            bool[] bOld;
            foreach (DataGridViewRow dgvr in dgv_phrases.Rows)
            {
                if (dgvr.Selected)
                {
                    n++;
                }
                else KeepList.Add(i);
                i++;
            }
            if (n == 0)
            {
                DiaRes = MessageBox.Show(
                    "You need to highlight the entire row to delete a phrase", "Warning");
                return;
            }
            DiaRes = MessageBox.Show(
    "This operation will delete " + n + " highlighted filter phrases.  Are you sure?",
    "Warning: don't forget to save", MessageBoxButtons.OKCancel);
            n = NumPhrases - n;
            if (DiaRes == DialogResult.Yes)
            {
                WorkingPhrases = new string[n];
                bOld = new bool[n];
                i = 0;
                foreach (int j in KeepList)
                {
                    WorkingPhrases[i] = InitialPhrase[j];
                    bOld[i] = bUsePhrase[j];
                    i++;
                }
                NumPhrases = n;
                InitialPhrase = new string[n];
                bUsePhrase = new bool[n];
                i = 0;
                foreach (string str in WorkingPhrases)
                {
                    InitialPhrase[i] = str;
                    bUsePhrase[i] = bOld[i];
                    i++;
                }
                FillPhrases();
            }
        }


        // the phrase list was edited so copy the edits so they can be saved
        // the searching is done starting with the InitialPhrase list
        // and then setting up workingphrase and phlist 
        private void UpdatePhrasesFromTable()
        {
            for (int i = 0; i < NumPhrases; i++)
            {
                InitialPhrase[i] = phlist[i].Phrase.Trim();
                bUsePhrase[i] = phlist[i].Select;
                bExactMatch[i] = phlist[i].Match;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdatePhrasesFromTable();
            globals.SavePhraseSettings(ref InitialPhrase, ref bUsePhrase, ref bExactMatch);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NumPhrases; i++)
            {
                dgv_phrases.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnUncheckall_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NumPhrases; i++)
            {
                dgv_phrases.Rows[i].Cells[0].Value = false;
            }
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NumPhrases; i++)
            {
                dgv_phrases.Rows[i].Cells[0].Value = !(bool)dgv_phrases.Rows[i].Cells[0].Value;
            }
        }

        private void cbIgnoreCase_CheckedChanged(object sender, EventArgs e)
        {
            LocalSettings.bIgnoreCase = cbIgnoreCase.Checked;
        }


        private void PhraseFinderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            globals.SaveLocalSettings(ref LocalSettings);
            if (oDoc == null) return;
            try
            {
                oWord.Quit(false);
            }
            catch (Exception ex)
            {
                return;
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help MyHelp = new help();
            MyHelp.Show();  // this leaves dialog box on the screen
        }


        /// <summary>
        /// copy any edits in the data view table to the data array and verify them
        /// note that EditedFormattedValue may have user errors 
        /// </summary>
        private bool SaveEditedValues()
        {
            bAllExact = true;
            string strTemp;
            char[] delimiters = new char[] { ' ' };
            for (int i = 0; i < NumPhrases; i++)
            {
                string str = (string)dgv_phrases.Rows[i].Cells[2].EditedFormattedValue;
                bool bUse = (bool)dgv_phrases.Rows[i].Cells[0].EditedFormattedValue;
                bool bMat = (bool)dgv_phrases.Rows[i].Cells[1].EditedFormattedValue;
                phlist[i].Phrase = str;
                InitialPhrase[i] = str;
                phlist[i].Select = bUse;
                bUsePhrase[i] = bUse;
                phlist[i].Match = bMat;
                bExactMatch[i] = bMat;
                strTemp = phlist[i].Phrase;
                strTemp = cbIgnoreCase.Checked ? strTemp.ToLower() : strTemp;
                WorkingPhrases[i] = strTemp;
                phlist[i].strInSeries = strTemp.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                int n = phlist[i].strInSeries.Length;
                if (n == 0)
                {
                    MessageBox.Show("Cannot have an empty phrase", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
                if (n == 1 && !bMat)
                {
                    MessageBox.Show("If only one phrase word then must select MATCH", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgv_phrases.Rows[i].Cells[1].Value = true;
                    return true;
                }
                if (bUse && !bMat) bAllExact = false;   // have at lest one boolean
            }
            bool bErr = ErrorsInTable();
            return bErr;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> strReturn = new List<string>();
            List<string> InitialPhraseChk = new List<string>();
            for (int i = 0; i < NumPhrases; i++)
            {
                string str = (string)dgv_phrases.Rows[i].Cells[2].EditedFormattedValue;
                bool bUse = (bool)dgv_phrases.Rows[i].Cells[0].EditedFormattedValue;
                bool bMat = (bool)dgv_phrases.Rows[i].Cells[1].EditedFormattedValue;
                InitialPhraseChk.Add((bUse ? "1:" : "0:") + (bMat ? "1:" : "0:") + str);

            }
            InitialParams ipSetup = new InitialParams(ref InitialPhraseChk, ref strReturn);
            ipSetup.ShowDialog();   // does not return unless dialog box closed
            if (strReturn.Count() == 0) return;
            NumPhrases = strReturn.Count();
            InitialPhrase = new string[NumPhrases];
            WorkingPhrases = new string[NumPhrases];
            bUsePhrase = new bool[NumPhrases];
            bExactMatch = new bool[NumPhrases];
            for (int i = 0; i < NumPhrases; i++)
            {
                bUsePhrase[i] = strReturn[i].Substring(0, 2) == "1:";
                bExactMatch[i] = strReturn[i].Substring(2, 2) == "1:";
                InitialPhrase[i] = strReturn[i].Substring(4);
            }
            SortPhrasesList();
            FillNewPhrases();
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            bStopEarly = true;
        }

        /// <summary>
        /// if any additional phrases on the page then allow them to be selected
        /// </summary>
        private void AllowNextPhrase()
        {
            btnNext.Visible = iCurrentPagePhraseCount > 1;
            btnViewDoc.Visible = iCurrentPagePhraseCount > 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ThisPageList == null) return;
            object FindText = CurrentActivePhrase;
            object MyCase = !cbIgnoreCase.Checked;  // seems to be just the opposed of what I thought
            object MyWholeWord = cbWholeWord.Checked;
            iCurrentPagePhraseActive++;
            if (bUseFound)
            { 
                if (iCurrentPagePhraseActive == iCurrentPagePhraseCount)
                {
                    // need to bring up the page again so the first phrase can be seen
                    iCurrentPagePhraseActive = 0;
                    ShowFoundPage();
                    return;
                }
                ShowFoundPage();
            }
            if (iCurrentPagePhraseActive == iCurrentPagePhraseCount)
            {
                iCurrentPagePhraseActive = 0;
                ShowFoundPage();
                return;
            }
            oWord.Selection.Find.Execute(FindText, MyCase, MyWholeWord);

        }

        private void cbIgnoreCase_CheckedChanged_1(object sender, EventArgs e)
        {
            LocalSettings.bIgnoreCase = cbIgnoreCase.Checked;
        }


        private void OpenWord_Click(object sender, EventArgs e)
        {

            ofd = new OpenFileDialog();
            ofd.DefaultExt = "*docx";
            ofd.InitialDirectory = LocalSettings.strLastFolder;
            ofd.Filter = "(Word Doc)|*.docx";
            tbMatches.Text = "";
            if (DialogResult.OK != ofd.ShowDialog())
            {
                tbMatches.Text = "ERROR:no DOCX file found";
                searchPanel.Enabled = false;
                return;
            }
            tbPdfName.Text = ofd.FileName;
            LocalSettings.strLastFolder = Path.GetDirectoryName(ofd.FileName);
            if (oDoc != null)
            {
                tbMatches.Text += "Closing Document...\r\n";
                oWord.Quit(false);
                oDoc = null;
            }
            tbMatches.Text += "Opening Document...\r\n";
            oWord = new Word.Application();
            try
            {
                oDoc = oWord.Application.Documents.Open(ofd.FileName);
            }
            catch (Exception ex)
            {
                if (bMyDebug) tbMatches.Text += "error: " + ex.Message + "\r\n";
                MessageBox.Show("You may have closed the document, please exit the program", "ERROR-3");
                return;
            }
            oWord.Visible = false;
            oDoc.Activate();
            if (bMyDebug)
                tbMatches.Text += "Document Open for searching\r\n";
            searchPanel.Enabled = GetPageCount();
            gbPageCtrl.Visible = searchPanel.Enabled;
        }

        private void cbWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            LocalSettings.bWholeWord = cbWholeWord.Checked;
        }
    }
}