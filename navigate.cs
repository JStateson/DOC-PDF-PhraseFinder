using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOC_PhraseFinder
{
    public partial class navigate : Form
    {
        public PhraseFinderForm pff;
        public TreeNode mainNode;
        private string LastPhraseLookedUp = "";
        private bool bExpanded = false;
        public navigate(Form refForm, string DocName)
        {
            InitializeComponent();
            pff = (PhraseFinderForm)refForm;
            mainNode = new TreeNode();
            mainNode.Text = DocName;
            ShowTree();
        }

        private void ExpandAll()
        {
            foreach (TreeNode node in tvPhrases.Nodes)
            {
                node.Expand();
                foreach (TreeNode subnode in node.Nodes)
                    subnode.Expand();
            }
        }

        private void CollapseAll()
        {
            foreach (TreeNode node in tvPhrases.Nodes)
            {
                node.Collapse();
                foreach (TreeNode subnode in node.Nodes)
                    subnode.Collapse();
            }
        }

        public void ShowTree()
        {
            TreeNode n, c, dn;
            string[] ThesePages;
            cSeriesOnPage sop;
            int m = 0, i = 1;
            foreach (cPhraseTable pt in pff.phlist)
            {
                if (pt.Select)
                {
                    m = pt.FoundInSeries.Count;
                    if (m == 0) continue;
                    n = new TreeNode();
                    n.Name = i.ToString();
                    i++;
                    n.Text = pt.Phrase;
                    if (m > 0)
                    {
                        ThesePages = new string[m];
                        ThesePages = pt.strPages.Split(',');
                        for (int j = 0; j < m; j++)
                        {
                            c = new TreeNode();
                            c.Name = ThesePages[j];
                            sop = pt.FoundInSeries[j];
                            c.Text = "SHOW:" + sop.SeriesOnPage[0];
                            // may be more than 1 on this page
                            int k = sop.SeriesOnPage.Count;
                            if (k > 0)
                            {
                                //dn = new TreeNode();
                                //dn.Name = k.ToString();
                                //dn.Text = "next";
                                if (k > 1)
                                    c.ForeColor = Color.Red;
                                c.Tag = k.ToString();
                            }
                            n.Nodes.Add(c);
                        }
                    }
                    tvPhrases.Nodes.Add(n);
                }
            }
            mainNode.Collapse(true);
        }

        private void tvPhrases_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string sPhrase = e.Node.Text;
            if (sPhrase.Substring(0, 5) == "SHOW:")
            {
                int iPage = Convert.ToInt32(e.Node.Name);
                btnNext.Visible = ((string)e.Node.Tag != "1");
                //int iCnt = phlist[iCurrentRow].WordsOnPage[Convert.ToInt32(nudPage.Value)];
                int iCnt = Convert.ToInt32((string)e.Node.Tag);
                LastPhraseLookedUp = sPhrase.Substring(5);
                pff.ShowThisOne(LastPhraseLookedUp, iPage, iCnt);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnNext.Visible = pff.ShowNextOnPage(LastPhraseLookedUp);
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (bExpanded)
            {
                CollapseAll();
                btnExpand.Text = "Expand All";
            }
            else
            {
                ExpandAll();
                btnExpand.Text = "Collapse All";
            }
            bExpanded = !bExpanded;
        }

        private void navigate_Deactivate(object sender, EventArgs e)
        {
            pff.RestoreMainForm();
        }
    }
}
