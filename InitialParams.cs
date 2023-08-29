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

namespace DOC_PhraseFinder
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


        private void btnApply_Click(object sender, EventArgs e)
        {
            bool bBad = AddMissingCheckmarks(true);
            if (bBad)
            {
                DialogResult dialogresult = MessageBox.Show("You had at least one missing checkbox.\r\nCancel exit? or click OK to continue", "Warning", MessageBoxButtons.OKCancel);
                if (dialogresult == DialogResult.Cancel) return;
            }
            str1 = globals.StrToStrs(tbPhrases.Text);

            foreach (string str in str1)
            {
                OutStr.Add(str);
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
            for (int i = 0; i < n; i++)
            {
                string strTemp = globals.RemoveWhiteSpace(strIn[i]);
                strIn[i] = strTemp;
                strOut += strTemp;
            }
            tbPhrases.Text = strOut;
        }



        private bool AddSelectCheckmarks(bool bChk)
        {
            string[] strs = globals.StrToStrs(tbPhrases.Text.Trim());
            string strOut = "";
            bool bAnyMissing = false;
            foreach (string strA in strs)
            {
                string str = globals.RemoveWhiteSpace(strA);
                if (str == "") continue;
                bool bValid = HasValidChecks(str);
                if (bValid)
                {
                    strOut += (bChk ? "1:" : "0:") + str.Substring(2) + "\r\n";
                }
                else
                {
                    string strPrefix = str.Substring(0, 2);
                    bAnyMissing = true;
                    if (strPrefix == "1:" || strPrefix == "0:") // assume this is the "match"
                    {
                        strOut += (bChk ? "1:" : "0:") + str + "\r\n";
                    }
                    else
                    {   //assume exact match for default when nothing presented
                        strOut += (bChk ? "1:" : "0:") + "1:" + str + "\r\n";
                    }
                }
            }
            tbPhrases.Text = strOut;
            return bAnyMissing;
        }


        private bool AddMissingCheckmarks(bool bChk)
        {
            string[] strs = globals.StrToStrs(tbPhrases.Text.Trim());
            string strOut = "";
            bool bAnyMissing = false;
            foreach (string strA in strs)
            {
                string str = globals.RemoveWhiteSpace(strA);
                if (str == "") continue;
                bool bValid = HasValidChecks(str);
                if (bValid)
                {
                    strOut += str + "\r\n";
                    continue;
                }
                else
                {
                    string strPrefix = str.Substring(0, 2);
                    bAnyMissing = true;
                    if (strPrefix == "1:" || strPrefix == "0:") // assume this is the "match"
                    {
                        strOut += (bChk ? "1:" : "0:") + str + "\r\n";
                    }
                    else
                    {   //assume exact match for default when nothing presented
                        strOut += (bChk ? "1:" : "0:") + "1:" + str + "\r\n";
                    }
                }
            }
            tbPhrases.Text = strOut;
            return bAnyMissing;
        }

        private bool HasValidChecks(string strTemp)
        {
            string str = strTemp.Substring(0, 4);
            if (str == "0:0:") return true;
            if (str == "0:1:") return true;
            if (str == "1:0:") return true;
            if (str == "1:1:") return true;
            return false;
        }


        private void btnUnChk_Click(object sender, EventArgs e)
        {
            AddSelectCheckmarks(false);
        }

        private void bltnChkAll_Click(object sender, EventArgs e)
        {
            AddSelectCheckmarks(true);
        }
    }
}
