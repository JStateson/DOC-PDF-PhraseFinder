using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_PhraseFinder
{
    internal static class globals
    {

        public static string BadLetters = ".,/|[]{}\\-_=!@#$%^&*()+`~,./;:'\"";

        // return true if a problem with the phrase construction, else false;
        public static bool CheckSyntax(string aPhrase)
        {

            string strBad = "";
            int j, n = BadLetters.Length;
            for (int i = 0; i < n; i++)
            {
                if (aPhrase.Contains(BadLetters.Substring(i, 1)))
                {
                    strBad += BadLetters.Substring(i, 1) + " ";
                }
            }
            if (strBad != "")
            {
                MessageBox.Show("Cannot have characters: " + strBad + " in phrase " + aPhrase, "Bad phrase found");
                return true;
            }
            return false;
        }

        /// <summary>
        /// ensure that exactly one space is between each word in a phrase
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string RemoveWhiteSpace(string strIn)
        {
            char[] whitespace = new char[] { ' ', '\t' };
            string[] sStr = strIn.Split(whitespace, StringSplitOptions.RemoveEmptyEntries);
            string strOut = "";
            foreach(string str in sStr)
            {
                strOut += str + " ";
            }
            return strOut;
        }




        /// <summary>
        /// save the phrases in the users window property list
        /// </summary>
        /// <param name="InitialPhrases"></param>
        /// <param name="bUsePhrases"></param>
        public static void SavePhraseSettings(ref string[] InitialPhrases, ref bool[] bUsePhrases)
        {
            // should be at AppData\Local\Microsoft\YOUR APPLICATION NAME File name is user.config
            int i = 0;
            StringCollection scSavedWords = new StringCollection();
            foreach (string str1 in InitialPhrases)
            {
                scSavedWords.Add((bUsePhrases[i++] ? "1:" : "0:") + str1);
            }
            Properties.Settings.Default.SearchPhrases = scSavedWords;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Fetch any saved phrases and if none, then save the default phrases
        /// </summary>
        /// <param name="InitialPhrases"></param>
        /// <param name="bUsePhrases"></param>
        /// <returns></returns>
        public static int ObtainProjectSettings(ref string[] InitialPhrases, ref bool[] bUsePhrases)
        {
            int n = 0;  // any setttings?
            int i, j;
            string[] SavedSettings;
            StringCollection scSavedWords = new StringCollection();
            if (null != Properties.Settings.Default.SearchPhrases)
                n = Properties.Settings.Default.SearchPhrases.Count;
            if (n > 0)
            {
                SavedSettings = new string[Properties.Settings.Default.SearchPhrases.Count];
                Properties.Settings.Default.SearchPhrases.CopyTo(SavedSettings, 0);
                scSavedWords.AddRange(SavedSettings);
                InitialPhrases = new string[n];
                //WorkingPhrases = new string[n];
                bUsePhrases = new bool[n];
                j = 0;
                foreach (string str in scSavedWords)
                {
                    i = str.Length;
                    if (i < 3)
                    {
                        Debug.Assert(false);
                        return 0;    // cannot be this small
                    }
                    i = str.IndexOf(":");   // expecting 0: or 1:
                    if (i < 0)
                    {   // if missing then set checkmark
                        bUsePhrases[j] = true;
                        InitialPhrases[j] = str;
                    }
                    else
                    {
                        bUsePhrases[j] = "1" == str.Substring(0, 1);
                        InitialPhrases[j] = str.Substring(2);
                    }
                    j++;
                }
            }
            else
            {
                // there are no setting so write them out using the program defaults
                // this is only done once to get the checkbox value written to settings
                n = InitialPhrases.Length;
                i = 0;
                foreach (string str in InitialPhrases)
                {
                    scSavedWords.Add((bUsePhrases[i] ? "1:" : "0:") + str);
                    i++;
                }
                Properties.Settings.Default.SearchPhrases = scSavedWords;
                Properties.Settings.Default.Save();
            }
            return n;
        }

        public static void SaveLocalSettings(ref cLocalSettings LocalSettings)
        {
            Properties.Settings.Default.IsLastFolder = LocalSettings.strLastFolder;
            Properties.Settings.Default.bIgnoreCase = LocalSettings.bIgnoreCase;
            Properties.Settings.Default.PDFZoomInx = LocalSettings.PDFZoomInx;
            Properties.Settings.Default.PDFZoomPCT = LocalSettings.PDFZoomPCT;
            Properties.Settings.Default.Save();
        }

        public static void GetLocalSettings(ref cLocalSettings LocalSettings)
        {
            LocalSettings.strLastFolder = Properties.Settings.Default.IsLastFolder;
            LocalSettings.bIgnoreCase = Properties.Settings.Default.bIgnoreCase;
            LocalSettings.PDFZoomInx = Properties.Settings.Default.PDFZoomInx;
            LocalSettings.PDFZoomPCT = Properties.Settings.Default.PDFZoomPCT;
            if (LocalSettings.strLastFolder == "")
            {
                LocalSettings.strLastFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                SaveLocalSettings(ref LocalSettings);
            }
        }


        public static string GiveRunWarning()
        {
            string strWarning =
@"While the search is running, do not edit or close the
document.  Watch the document and be sure to dismiss any
popups else the search will stop and the program may
freeze.  When the search is finished, click only on any of
the match page numbers in the 3rd column under the
title 'Numbers'.  Selecting other columns allows editing
the table.  Click 'stop' if you want to quit, then exit
the program. ALWAYS QUIT THIS APPLICATION BEFORE
CLOSING ANY PDF DOCUMENT";
            MessageBox.Show(strWarning, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return strWarning;
        }
        public static string GiveInitialWarning()
        {
            string strPhoneHome =
@"Adobe Acrobat Pro and Standard verify licensing at
startup of Acrobat and randomly at various times while
this Acrobat app is running.  When this happens the
Phrase Finder Application (PFA) will freeze.

The freeze may last for 10 or more seconds.  During this
freeze please do not click on any PFA form or the Acrobat
document itself.  If the freeze lasts more than 30 seconds
you may have to close or possibly terminate PFA including
the Acrobat-32 process show in the task manager.

Clicking on the form while the acrobat is waiting for
license verification can cause Windows to report this
application as Non Responding";
            MessageBox.Show(strPhoneHome, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return strPhoneHome;
        }
    }

    public class cLocalSettings         // used to restore user settings
    {
        public bool bExitEarly;             // for debugging or demo purpose only examine a limited number of page
        public string strLastFolder = "";     // where last PDF was obtained
        public bool bIgnoreCase = true;
        public int PDFZoomPCT = 75; // percent but not used unless inx correlelates
        public int PDFZoomInx = 1;  // AVZoomFitPage
    }

    public class cPhraseTable
    {
        public bool Select { get; set; }
        public string Phrase { get; set; }
        public string Number { get; set; }
        public int iNumber;
        public int iDupPageCnt;
        public int iLastPage;
        public string strPages = "";
        public string[] strInSeries;
        public List<int> WordsOnPage = new List<int>();
        public int nFollowing; // number of words to check in sequence

        // count the number of following words that must match
        private int CountWords(string strIn)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            strInSeries = strIn.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int n = strInSeries.Length;
            if (n > 1) return n - 1; // if 2 words then must check one more word
            return 0;
        }


        public void InitPhrase(string aPhrase)
        {
            Select = true;
            Number = " ";
            iNumber = 0;
            strPages = "";
            iDupPageCnt = 0;
            iLastPage = -1;
            Phrase = aPhrase;
            nFollowing = CountWords(aPhrase);
        }

        public void InitPhrase(string aPhrase, bool bSelect)
        {
            Select = bSelect;
            Number = " ";
            iNumber = 0;
            strPages = "";
            iDupPageCnt = 0;
            iLastPage = -1;
            Phrase = aPhrase;
            nFollowing = CountWords(aPhrase);
        }

        public void AddPage(int jPage) // do not add the same page twice
        {
            int iPage = jPage + 1;
            if (strPages == "")
            {
                strPages = iPage.ToString();
                iLastPage = iPage;
                WordsOnPage.Add(1);
            }
            else
            {
                if (iLastPage == iPage)
                {
                    iDupPageCnt++;
                    WordsOnPage[^1]++;  // increment the last page count
                    return;
                }
                strPages += "," + iPage.ToString();
                WordsOnPage.Add(1);
                iLastPage = iPage;
            }
        }
        public void IncMatch()
        {
            iNumber++;
        }
    }


}
