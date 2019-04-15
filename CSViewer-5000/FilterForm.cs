using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;

namespace CSViewer_5000
{
    public partial class FilterForm: Form
    {
        List<Station> Filteredlist = new List<Station>();
        public FilterForm(List<Station> newFilteredList)
        {
            Filteredlist = newFilteredList;
            InitializeComponent();
            GridViewCorrection();

            foreach (var item in Filteredlist)
            {
                dataGridView1.Rows.Add(
                           item.Name,
                           item.ID,
                           item.RegistrationDate,
                           item.Location,
                           item.Owner.Name,
                           item.Flow,
                           item.MainTenanceTime,
                           item.Road);
            }
        }

        public Form1 Form1
        {
            get => default(Form1);
            set {
            }
        }

        private void GridViewCorrection()
        {
            dataGridView1.Columns.Add("column 1", "Name");
            dataGridView1.Columns.Add("column 2", "ID");
            dataGridView1.Columns.Add("column 3", "Registration date");
            dataGridView1.Columns.Add("column 4", "Location");
            dataGridView1.Columns.Add("column 5", "Owner");
            dataGridView1.Columns.Add("column 6", "Flow");
            dataGridView1.Columns.Add("column 7", "Maintenance time");
            dataGridView1.Columns.Add("column 8", "Road");
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].Width *= 2;
            dataGridView1.Columns[2].Width *= 2;
            dataGridView1.Columns[0].Width *= 2;
            dataGridView1.Columns[3].Width *= 2;
            dataGridView1.Columns[4].Width *= 2;
            dataGridView1.Columns[7].Width *= 2;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Font F = new Font("Cambria", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = F;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            if (dataGridView1.Rows.Count == 0) return;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string savefilename = saveFileDialog1.FileName;
            // сохраняем текст в файл
                File.WriteAllText(savefilename, DataGridInString(dataGridView1), Encoding.Default);
                MessageBox.Show("Saved!");
            
        }
        private string DataGridInString(DataGridView dtg)
        {
            string text = "";
            for (int i = 0; i < dtg.Rows.Count; i++)
            {
                text += $"{dtg[0, i].Value.ToString()};{dtg[1, i].Value.ToString()};{dtg[2, i].Value.ToString()};{dtg[3, i].Value.ToString()};{dtg[4, i].Value.ToString()};{dtg[5, i].Value.ToString()};{dtg[6, i].Value.ToString()};{dtg[7, i].Value.ToString()}\n";
            }
            return text;
        } // Turn data in csv text file
    }


}
