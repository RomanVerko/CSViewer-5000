using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSViewer_5000
{
    public partial class NumberOfElementsForm: Form
    {
        
        
        public NumberOfElementsForm()
        {
            InitializeComponent();
            trackBar1.Scroll += trackBar1_Scroll;
        }

        public Form1 Form1
        {
            get => default(Form1);
            set {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Form1.NewSize(trackBar1.Value);
            Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = string.Format("Elements on table: {0}", trackBar1.Value);
        }
    }
}
