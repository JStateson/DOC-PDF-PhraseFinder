using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF_PhraseFinder
{
    public partial class InitialParams : Form
    {
        private List<string> OutStr;
        private string[] str1;
        private string[] str2;
        public InitialParams(ref List<string> InitialPhraseChk, ref List<string> strReturn)
        {
            InitializeComponent();
            tbPhrases.Text = "";
            foreach (string Phrase in InitialPhraseChk)
            {
                tbPhrases.Text += Phrase + "\r\n";
            }
            OutStr = strReturn;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbPhrases.Text = "";
        }

        private string[] StrToStrs(string strIn)
        {
            char[] delimiters = new char[] { '\r', '\n' };
            return strIn.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            bool bBad = AddMissingCheckmarks(true);
            if (bBad)
            {
                DialogResult dialogresult = MessageBox.Show("You had at least one missing checkbox.\r\nCancel exit? or click OK to continue", "Warning", MessageBoxButtons.OKCancel);
                if (dialogresult == DialogResult.Cancel) return;
            }
            str1 = StrToStrs(tbPhrases.Text);

            foreach (string str in str1)
            {
                OutStr.Add(str + "\r\n");
            }
            this.Close();
        }

        // first two chars of phrase might have 1: or 0:
        // return true of a problem, else false;


        private void btnCancel_Click(object sender, EventArgs e)
        {
            OutStr.Clear();
            this.Close();
        }

        private void FixWhiteSpace(ref string[] strIn)
        {
            int n = strIn.Length;
            string strOut = "";
            for(int i = 0; i < n; i++)
            {
                string strTemp = globals.RemoveWhiteSpace(strIn[i]);
                strIn[i] = strTemp;
                strOut += strTemp;
            }
            tbPhrases.Text = strOut;
        }

        private void btnChkErr_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] strDOC = StrToStrs(tbPhrases.Text);
            int n = strDOC.Length;
            string[] strPDF = new string[n];
            foreach(string strA in strDOC)
            {
                strPDF[i] = globals.RemovePunctuation(strA);
                i++;
            }
            PDFedits EditSetup = new PDFedits(ref strDOC, ref strPDF);
            EditSetup.ShowDialog();
        }


        private string SetChk(bool bChk, string str)
        {
            return (bChk ? "1:" : "0:") + str.Substring(2) + "\r\n";
        }

        private void PutCheckmarks(bool bChk)
        {
            string[] strs = StrToStrs(tbPhrases.Text.Trim());
            string strOut = "";
            foreach (string str in strs)
            {
                string strPrefix = str.Substring(0, 2);
                if (strPrefix == "1:" || strPrefix == "0:")
                {
                    strOut += SetChk(bChk, str);
                }
                else
                {
                    strOut += (bChk ? "1:" : "0:") + str + "\r\n";
                }
            }
            tbPhrases.Text = strOut;
        }

        private bool AddMissingCheckmarks(bool bChk)
        {
            string[] strs = StrToStrs(tbPhrases.Text.Trim());
            string strOut = "";
            bool bAnyMissing = false;
            foreach (string strA in strs)
            {
                string str = globals.RemoveWhiteSpace(strA);
                string strPrefix = str.Substring(0, 2);
                if (strPrefix == "1:" || strPrefix == "0:")
                {
                    strOut += str + "\r\n";
                }
                else
                {
                    strOut += (bChk ? "1:" : "0:") + str + "\r\n";
                    bAnyMissing = true;
                }
            }
            if (bAnyMissing)
                tbPhrases.Text = strOut;
            return bAnyMissing;
        }

        private void btnUnChk_Click(object sender, EventArgs e)
        {
            PutCheckmarks(false);
        }

        private void bltnChkAll_Click(object sender, EventArgs e)
        {
            PutCheckmarks(true);
        }
    }
}
