
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
//using System.Timers;


// data mining PDF appplication
// copyright 2023, Joseph Stateson  github/jstateson  
/*
* Notes
* must add references to adobe and set imbed to false or no
* must select application and select settings to create settings.settings
* had to select os 8, not 10.  not sure why sdk is missing???
* copy usda.ico in resources.resx
* use Ctrl + k + c to comment out lines an u to uncomment
* 8/4/2023 adding "0:" or "1:" prefix to phrase to indicated if it was checked or not
* example code
* 1: https://community.adobe.com/t5/acrobat-sdk-discussions/extract-text-from-pdf-using-c/m-p/4002187
* 2: https://stackoverflow.com/questions/709606/programmatically-search-for-text-in-a-pdf-file-and-tell-the-page-number
*https://opensource.adobe.com/dc-acrobat-sdk-docs/library/interapp/IAC_API_OLE_Objects.html#50532405_34749
*https://opensource.adobe.com/dc-acrobat-sdk-docs/acrobatsdk/pdfs/acrobatsdk_samplesguide.pdf
*/

namespace PDF_PhraseFinder
{

    public partial class PhraseFinderForm : Form
    {

        private CAcroApp acroApp;
        private AcroAVDoc ThisDoc = null;
        private AcroAVDoc AVDoc = null;
        private CAcroAVPageView ThisDocView;
        private int[] ThisPageList;
        private int iCurrentPage = 0;
        private bool bStopEarly = false;
        private int NumPhrases = 5;
        private int TotalPDFPages, TotalMatches;
        //private IFields theseFields;
        private bool bFormDirty = false;
        private StringCollection scSavedWords;
        private string CurrentActivePhrase = "";
        private int iNullCount = 0;
        private int iCurrentPagePhraseCount = 0;
        private int iCurrentPagePhraseActive = 0;
        private int iCurrentRow = 0;
        private int[] SrtIndex;


        private List<cPhraseTable> phlist = new List<cPhraseTable>();   // table of phrases
        private cLocalSettings LocalSettings = new cLocalSettings();    // table of settings
        //private static System.Timers.Timer aTimer;

        private string[] InitialPhrase = new string[5] { "school lunch", "Civil Rights", "contract", "food service", "fixed price" };
        private string[] WorkingPhrases = new string[5]; // same as above but optomises somewhat for case sensitivity
        private bool[] bUsePhrase = new bool[5] { true, true, true, true, true };



        /// <summary>
        /// entry point for main form
        /// </summary>
        public PhraseFinderForm()
        {
            InitializeComponent();

            try
            {
                acroApp = new AcroAppClass();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adobe PRO or STANDARD is not present");
                this.Close();
            }
            NumPhrases = globals.ObtainProjectSettings(ref InitialPhrase, ref bUsePhrase);
            WorkingPhrases = new String[NumPhrases];
            globals.GetLocalSettings(ref LocalSettings);
            tbZoomPCT.Text = LocalSettings.PDFZoomPCT.ToString();
            cbIgnoreCase.Checked = LocalSettings.bIgnoreCase;
            FillPhrases();
            tbPdfName.Text = " (v) 1.0 (c)Stateson";
            globals.GiveInitialWarning();
            //aTimer = new System.Timers.Timer(100);
            //aTimer.Enabled = false;
            //aTimer.Elapsed += OnTimedEvent;
            //aTimer.AutoReset = false;

        }

        private bool GetPageCount()
        {
            CAcroPDDoc pdDoc = (CAcroPDDoc)AVDoc.GetPDDoc();
            Object jsObj;
            Type T;
            //Acquire the Acrobat JavaScript Object interface from the PDDoc object
            try
            {
                jsObj = pdDoc.GetJSObject();
                T = jsObj.GetType();
                tbMatches.Text += "Counting pages of " + tbPdfName.Text;
                bStopEarly = false;
                pbarLoading.Value = 0;
                TotalPDFPages = Convert.ToInt32(T.InvokeMember(
                             "numPages",
                             BindingFlags.GetProperty |
                             BindingFlags.Public |
                             BindingFlags.Instance,
                             null, jsObj, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Document is opened in another app.\r\n You may need to terminate Acrobat 32 DC");
                AVDoc = null;
                return false;
                //throw;
            }

            pbarLoading.Maximum = TotalPDFPages;
            tbNumPages.Text = TotalPDFPages.ToString();
            return TotalPDFPages > 0;
        }

        /// <summary>
        /// user click the file open so help them find a pdf and
        /// report some success or failure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ofd = new OpenFileDialog();
            ofd.DefaultExt = "*pdf";
            ofd.InitialDirectory = LocalSettings.strLastFolder;
            ofd.Filter = "(Adobe PDF)|*.pdf";
            tbMatches.Text = "";
            if (DialogResult.OK != ofd.ShowDialog())
            {
                tbMatches.Text = "ERROR:no PDF file found";
                searchPanel.Enabled = false;
                return;
            }
            tbPdfName.Text = ofd.FileName;
            LocalSettings.strLastFolder = Path.GetDirectoryName(ofd.FileName);

            // enable the run button if a document was loaded
            //btnRunSearch.Enabled = bOpenDocs(tbPdfName.Text);
            //gbPageCtrl.Visible = GetPageCount();
            //assume this is a request to load a new doc or reopen the current one
            if (AVDoc != null)
            {
                tbMatches.Text += "Closing Document...\r\n";
                AVDoc.Close(0);
            }
            tbMatches.Text += "Opening Document...\r\n";
            AVDoc = new AcroAVDoc();
            try
            {
                AVDoc.Open(ofd.FileName, "");
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            tbMatches.Text += "Document Open for searching\r\n";
            searchPanel.Enabled = GetPageCount();
            gbPageCtrl.Visible = searchPanel.Enabled;
            pbarLoading.Maximum = TotalPDFPages;
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
            Application.DoEvents();
        }

        //open file needs adobe professional (not always found) in addition to badly formed PDFs
        // i need to give warning if PRO is not on the system
        // must set interop type to false for acrobat modules
        private bool bOpenDocs(string strPath)
        {
            try
            {
                AcroPDDocClass objPages = new AcroPDDocClass();
                objPages.Open(strPath);
                TotalPDFPages = objPages.GetNumPages();
                tbNumPages.Text = TotalPDFPages.ToString();
                objPages.Close();
            }
            catch
            {
                tbMatches.Text = "You may not have logged into Adobe\r\n";
                tbMatches.Text += "Missing Adobe DLL (bad intall)\r\n or bad PDF file:" + tbPdfName.Text;
                return false;
            }
            return true;
        }


        private void FindMatches(ref string strBig, int j, int p)
        {
            string strPhrase = WorkingPhrases[j];
            int iWidth = strPhrase.Length;

            while (true)
            {
                int i = strBig.IndexOf(strPhrase);
                if (i == -1) return;
                phlist[j].AddPage(p);
                phlist[j].IncMatch();
                strBig = strBig.Remove(i, iWidth);
            }
        }

        private bool SearchThisFullPage(int p, ref Object jsObj, ref Type T)
        {
            string word, strBig = "";
            double numWords = 0;
            try
            {
                object[] getPageNumWordsParam = { p };
                numWords = (double)(T.InvokeMember(
                    "getPageNumWords",
                    BindingFlags.InvokeMethod |
                    BindingFlags.Public |
                    BindingFlags.Instance,
                    null, jsObj, getPageNumWordsParam));
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to read at page " + p.ToString());
                tbMatches.Text += "failed to read at page " + p.ToString();
                return false;
            }

            for (int i = 0; i < numWords; i++)
            {
                try
                {
                    //get a word
                    object[] getPageNthWordParam = { p, i };
                    word = (String)T.InvokeMember(
                        "getPageNthWord",
                        BindingFlags.InvokeMethod |
                        BindingFlags.Public |
                        BindingFlags.Instance,
                        null, jsObj, getPageNthWordParam);
                }
                catch (Exception e)
                {
                    word = "";
                    iNullCount++;
                }
                if (word != null)
                {
                    if (cbIgnoreCase.Checked) word = word.ToLower();
                    strBig += word;
                }
                strBig += " ";
            }

            for (int i = 0; i < NumPhrases; i++)
            {
                if (phlist[i].Select)
                    FindMatches(ref strBig, i, p);
            }
            return true;
        }


        /// <summary>
        /// This function highlights the phrase in  the pdf viewer
        /*
            '0 = AVZoomNoVary
            '1 = AVZoomFitPage
            '2 = AVZoomFitWidth
            '3 = AVZoomFitHeight
            '4 = AVZoomFitVisibleWidth
            '5 = AVZoomPreferred
        */


        private void ShowFoundPhrase()
        {
            if (iCurrentPage < 0) return;
            ThisDoc = new AcroAVDoc();
            ThisDoc.Open(tbPdfName.Text, "");
            ThisDoc.BringToFront();
            ThisDoc.SetViewMode(1); // (2)PDUseThumbs

            if (ThisDoc.IsValid())
            {
                Int16 pctValue = Convert.ToInt16(tbZoomPCT.Text); //probably need to "try" this conversion as user may type garbage in text box
                Int16 inxValue = Convert.ToInt16(cbZoom.SelectedIndex);
                if (pctValue < 0 || pctValue > 100) pctValue = 75;
                try
                {
                    ThisDocView = ThisDoc.GetAVPageView() as CAcroAVPageView;
                    ThisDocView.ZoomTo(inxValue, pctValue);
                    ThisDocView.GoTo(iCurrentPage - 1);
                    bool bFound = ThisDoc.FindText(CurrentActivePhrase,
                        cbIgnoreCase.Checked ? 0 : 1,
                        cbWholeWord.Checked ? 1 : 0,
                        0);
                }
                catch (Exception ex)
                {
                    int i = 0;
                }
            }
        }

        private void nudPage_ValueChanged(object sender, EventArgs e)
        {
            if (ThisPageList == null) return;
            int iVal = Convert.ToInt32(nudPage.Value);
            iCurrentPage = ThisPageList[iVal];
            tbViewPage.Text = iCurrentPage.ToString();
            ShowFoundPhrase();
            iCurrentPagePhraseActive = 0;
            iCurrentPagePhraseCount = phlist[iCurrentRow].WordsOnPage[Convert.ToInt32(nudPage.Value)];
            btnNext.Visible = iCurrentPagePhraseCount > 0;
            return;
        }

        //AcroRd32.exe /A "zoom=50&navpanes=1=OpenActions&search=batch" PdfFile
        // above search for the phrase "batch" is another way


        private bool RunSearch()
        {
            if (AVDoc.IsValid())
            {
                CAcroPDDoc pdDoc = (CAcroPDDoc)AVDoc.GetPDDoc();
                //Acquire the Acrobat JavaScript Object interface from the PDDoc object
                Object jsObj = pdDoc.GetJSObject();
                string OutText = "";
                TotalMatches = 0;
                iNullCount = 0;
                iCurrentPage = 1;
                Type T = jsObj.GetType();

                tbMatches.Text += "Searching ...\r\n";

                for (int p = 0; p < TotalPDFPages; p++)
                {
                    bool bOK = SearchThisFullPage(p, ref jsObj, ref T);
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
                cpt.InitPhrase(InitialPhrase[i], bUsePhrase[i]);
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
                cpt.InitPhrase(InitialPhrase[i], bUsePhrase[i]);
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
                row.Cells[2].Value = "";
                cpt = phlist[i];
                cpt.Number = "";
                cpt.nFollowing = 0;
                cpt.iNumber = 0;
                cpt.iDupPageCnt = 0;
                cpt.iLastPage = 0;
                cpt.strPages = "";
                cpt.nFollowing = 0;
                cpt.WordsOnPage.Clear();
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
            FormSortIndex();
            for (int i = 0; i < NumPhrases; i++)
            {
                j = SrtIndex[i];
                pTemp[i] = InitialPhrase[j];
                bTemp[i] = bUsePhrase[j];
            }
            for (int i = 0; i < NumPhrases; i++)
            {
                InitialPhrase[i] = pTemp[i];
                bUsePhrase[i] = bTemp[i];
            }
        }

        /// <summary>
        /// get the phrase out of that view data table and configure it for
        /// doing the searching.  User may have changed the phrase and not saved them
        /// be sure to check for bad construction of a phrase 
        /// </summary>
        private void FormWorkingFromTable()
        {
            string strTemp;
            for (int i = 0; i < NumPhrases; i++)
            {
                strTemp = phlist[i].Phrase;
                WorkingPhrases[i] = cbWholeWord.Checked ? " " : "";
                strTemp = cbIgnoreCase.Checked ? strTemp.ToLower() : strTemp;
                WorkingPhrases[i] += strTemp.Trim();
                WorkingPhrases[i] += cbWholeWord.Checked ? " " : "";
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
            for(int i = 0; i < NumPhrases;i++)
            {
                string strTemp = globals.RemoveWhiteSpace(phlist[i].Phrase);
                bool bErr = globals.CheckSyntax(strTemp);
                if (bErr) return true;
                if (strTemp != phlist[i].Phrase)
                {
                    bMustSort = true;
                    phlist[i].Phrase = strTemp;
                    InitialPhrase[i] = strTemp;
                }
            }
            if(bMustSort)
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
            FormWorkingFromTable();
            btnRunSearch.Enabled = false;
            btnStopScan.Enabled = true;
            globals.GiveRunWarning();
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
            ShowFoundPhrase();
        }


        private void SetNumeric_UpDn_Page()
        {
            nudPage.Maximum = ThisPageList.Length - 1;
            nudPage.Visible = ThisPageList.Length > 1;      // only show numeric up/down if more than 1 page
            btnNext.Visible = iCurrentPagePhraseCount > 0;
            // cannot let the event fire when resetting the value of the widget
            nudPage.ValueChanged -= nudPage_ValueChanged;
            nudPage.Value = 0;
            nudPage.ValueChanged += nudPage_ValueChanged;
            ShowFoundPhrase();
        }



        /// <summary>
        /// Get the list of pages that contains the wanted phrase and update the
        /// numeric up/down widget so the pages can be scrolled
        /// </summary>
        private void GetSelection()
        {
            Point ThisRC = dgv_phrases.CurrentCellAddress;
            iCurrentRow = ThisRC.Y;
            int iCol = ThisRC.X;
            if (iCol < 2) return; // allow editing the text or checkbox column
            iCurrentPage = -1;
            if (phlist[iCurrentRow].strPages != "")
            {
                ThisPageList = phlist[iCurrentRow].strPages.Split(',').Select(int.Parse).ToArray();
                iCurrentPage = ThisPageList[0];
                tbViewPage.Text = iCurrentPage.ToString();
                CurrentActivePhrase = phlist[iCurrentRow].Phrase;
                iCurrentPagePhraseActive = 0; // start of first (at least one) phrase found on the page
                iCurrentPagePhraseCount = phlist[iCurrentRow].WordsOnPage[0];
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
            if(n == 0)
            {
                DiaRes = MessageBox.Show(
                    "You need to highlight the entire row to delete a phrase","Warning");
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
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdatePhrasesFromTable();
            globals.SavePhraseSettings(ref InitialPhrase, ref bUsePhrase);
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
            if (ThisDoc == null) return;
            try
            {
                if (ThisDoc.IsValid())
                    ThisDoc.Close(1);
            }
            catch (Exception ex)
            { 
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // may want to prompt user to save changes ???
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
            for (int i = 0; i < NumPhrases; i++)
            {
                string str = (string)dgv_phrases.Rows[i].Cells[1].EditedFormattedValue;
                bool bUse = (bool)dgv_phrases.Rows[i].Cells[0].EditedFormattedValue;
                phlist[i].Phrase = str;
                InitialPhrase[i] = str;
                phlist[i].Select = bUse;
                bUsePhrase[i] = bUse;
            }
            bool bErr = ErrorsInTable();
            return bErr;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> strReturn = new List<string>();
            List<string> InitialPhraseChk = new List<string>();
            for(int i = 0; i < NumPhrases; i++)
            {
                string str = (string)dgv_phrases.Rows[i].Cells[1].EditedFormattedValue;
                bool bUse = (bool)dgv_phrases.Rows[i].Cells[0].EditedFormattedValue;
                InitialPhraseChk.Add((bUse ? "1:" : "0:") + str);
            }
            InitialParams ipSetup = new InitialParams(ref InitialPhraseChk, ref strReturn);
            ipSetup.ShowDialog();   // does not return unless dialog box closed
            if (strReturn.Count() == 0) return;
            NumPhrases = strReturn.Count();
            InitialPhrase = new string[NumPhrases];
            WorkingPhrases = new string[NumPhrases];
            bUsePhrase = new bool[NumPhrases];
            for (int i = 0; i < NumPhrases; i++)
            {
                bUsePhrase[i] = strReturn[i].Substring(0, 2) == "1:";
                InitialPhrase[i] = strReturn[i].Substring(2);
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
            btnNext.Visible = iCurrentPagePhraseCount > 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ThisPageList == null) return;
            bool bFound = ThisDoc.FindText(CurrentActivePhrase,
                cbIgnoreCase.Checked ? 0 : 1,
                cbWholeWord.Checked ? 1 : 0,
                0);

        }

        private void cbIgnoreCase_CheckedChanged_1(object sender, EventArgs e)
        {
            LocalSettings.bIgnoreCase = cbIgnoreCase.Checked;
        }

        private void PhraseFinderForm_Load(object sender, EventArgs e)
        {
            cbZoom.SelectedIndex = LocalSettings.PDFZoomInx;
        }

    }
}