using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DOC_PhraseFinder
{
    internal static class globals
    {
        // BadLetters = ".,/|[]{}\\-_=!@#$%^&*()+`~,/;:'\"";

        private static string strAlpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string sMyVersion = "1";
        public static string BadLetters = ",/|[]{}\\-_=!@#$%^&*()+`~,/;:'\"";

        /// <summary>
        /// return true if the word is a whole word "legal" in "illegal" is not a whole word.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="iLoc"></param>
        /// <param name="iLen"></param>
        /// <returns></returns>
        public static bool IsWholeWord(string s, int iLoc, int iLen)
        {
            int n = s.Length;
            if (iLoc == 0) return true; //first letter in sentence
            string sLet = s.Substring(iLoc-1, 1);
            if(strAlpha.Contains(sLet)) return false;
            iLoc += iLen;
            if (iLoc >= n) return true;
            sLet = s.Substring(iLoc, 1);
            if (strAlpha.Contains(sLet)) return false;
            return true;
        }

        public static string[] StrToStrs(string strIn)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            return strIn.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        // return true if a problem with the phrase construction, else false;
        public static bool CheckSyntax(string aPhrase)
        {

            string strBad = "";
            int n = BadLetters.Length;
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
            foreach (string str in sStr)
            {
                strOut += str + " ";
            }
            return strOut.Trim();
        }

        public static string RemovePunctuation(string strIn)
        {
            string strTemp = strIn;
            foreach (char strChar in BadLetters)
            {
                if (strIn.Contains(strChar))
                {
                    strTemp = strTemp.Replace(strChar, ' ');
                }
            }
            return RemoveWhiteSpace(strTemp);
        }



        /// <summary>
        /// save the phrases in the users window property list
        /// </summary>
        /// <param name="InitialPhrases"></param>
        /// <param name="bUsePhrases"></param>
        public static void SavePhraseSettings(ref string[] InitialPhrases, ref bool[] bUsePhrases, ref bool[] bMatch)
        {
            // should be at AppData\Local\Microsoft\YOUR APPLICATION NAME File name is user.config
            int i = 0;
            StringCollection scSavedWords = new StringCollection();
            foreach (string str1 in InitialPhrases)
            {
                scSavedWords.Add((bUsePhrases[i] ? "1:" : "0:") + (bMatch[i] ? "1:" : "0:") + str1);
                i++;
            }
            Properties.Settings.Default.SearchPhrases = scSavedWords;
            Properties.Settings.Default.Version = sMyVersion;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Fetch any saved phrases and if none, then save the default phrases
        /// </summary>
        /// <param name="InitialPhrases"></param>
        /// <param name="bUsePhrases"></param>
        /// <returns></returns>
        public static int ObtainProjectSettings(ref string[] InitialPhrases, ref bool[] bUsePhrases, ref bool[] bExactMatch)
        {
            int n = 0;  // any setttings?
            int i, j;
            string[] SavedSettings;
            StringCollection scSavedWords = new StringCollection();
            string sVers = Properties.Settings.Default.Version;
            if (sVers != sMyVersion)
            {
                // there are no setting so write them out using the program defaults
                // this is only done once to get the checkbox value written to settings
                n = InitialPhrases.Length;
                i = 0;
                foreach (string str in InitialPhrases)
                {
                    scSavedWords.Add((bUsePhrases[i] ? "1:" : "0:") + (bExactMatch[i] ? "1:" : "0:") + str);
                    i++;
                }
                Properties.Settings.Default.SearchPhrases = scSavedWords;
                Properties.Settings.Default.Version = sMyVersion;
                Properties.Settings.Default.Save();
                return i;
            }
            n = Properties.Settings.Default.SearchPhrases.Count;
            if (n > 0)
            {
                SavedSettings = new string[Properties.Settings.Default.SearchPhrases.Count];
                Properties.Settings.Default.SearchPhrases.CopyTo(SavedSettings, 0);
                scSavedWords.AddRange(SavedSettings);
                InitialPhrases = new string[n];
                bUsePhrases = new bool[n];
                bExactMatch = new bool[n];
                j = 0;
                foreach (string str in scSavedWords)
                {
                    bUsePhrases[j] = ("1:" == str.Substring(0, 2));
                    bExactMatch[j] = ("1:" == str.Substring(2, 2));
                    InitialPhrases[j] = str.Substring(4);
                    j++;
                }
            }
            return n;
        }

        public static void SaveLocalSettings(ref cLocalSettings LocalSettings)
        {
            Properties.Settings.Default.IsLastFolder = LocalSettings.strLastFolder;
            Properties.Settings.Default.bIgnoreCase = LocalSettings.bIgnoreCase;
            Properties.Settings.Default.bWholeWord = LocalSettings.bWholeWord;
            Properties.Settings.Default.Save();
        }

        public static void GetLocalSettings(ref cLocalSettings LocalSettings)
        {
            LocalSettings.strLastFolder = Properties.Settings.Default.IsLastFolder;
            LocalSettings.bIgnoreCase = Properties.Settings.Default.bIgnoreCase;
            LocalSettings.bWholeWord = Properties.Settings.Default.bWholeWord;
            if (LocalSettings.strLastFolder == "")
            {
                LocalSettings.strLastFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                SaveLocalSettings(ref LocalSettings);
            }
        }

    }

    public class cLocalSettings         // used to restore user settings
    {
        public bool bExitEarly;             // for debugging or demo purpose only examine a limited number of page
        public string strLastFolder = "";     // where last DOC was obtained
        public bool bIgnoreCase = true;
        public bool bWholeWord = true;
    }

    // cSeriesOnPage is obtuse code where I added it to FoundOnPage.  needs to be
    // cleaned up as there is no need for a list of lists, only a list of phrases
    // to do todo to do todo 9/11/2023 not sure what is happening, sorry
    public class cSeriesOnPage
    {
        public List<string> SeriesOnPage = new List<string>(); 
    }

    public class cPhraseTable
    {
        public bool Select { get; set; }
        public bool Match { get; set; }
        public string Phrase { get; set; }
        public string Number { get; set; }
        public int iNumber;
        public int iDupPageCnt;
        public int iLastPage;
        public string strPages = "";
        public string[] strInSeries; // the words making up the phrase.  "charging lunch"
        public List<cSeriesOnPage> FoundInSeries = new List<cSeriesOnPage>();  // the found strings such as "charging xxx xxx lunch"
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

        public void InitPhrase(string aPhrase, bool bSelect, bool bMatch)
        {
            Select = bSelect;
            Match = bMatch;
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
            int iPage = jPage;
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
