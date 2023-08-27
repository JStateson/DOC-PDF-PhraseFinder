using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF_PhraseFinder
{
    public partial class PDFedits : Form
    {
        private class cPhraseTable
        {
            public string PhraseDOCX { get; set; }
            public string PhrasePDF { get; set; }
        }
        private List<cPhraseTable> phlist = new List<cPhraseTable>();
        public PDFedits(ref string[] strDOCX, ref string[] strPDF)
        {
            InitializeComponent();
            int n = strPDF.Length;
            for (int i = 0; i < n; i++)
            {
                cPhraseTable cpt = new cPhraseTable();
                cpt.PhrasePDF = strPDF[i];
                cpt.PhraseDOCX = strDOCX[i];
                phlist.Add(cpt);
            }
            dgv_edits.DataSource = phlist;
        }
    }
}
