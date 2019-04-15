using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
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
    public partial class Form1: Form
    {

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.RestoreDirectory = true;
            RefillEvent += (position, newstation) => {
                dataGridView1.Rows.RemoveAt(position);
                dataGridView1.Rows.Insert(position, newstation.Name, newstation.ID, newstation.RegistrationDate, newstation.Location, newstation.Owner.Name, newstation.Flow, newstation.MainTenanceTime, newstation.Road);
            };
            AddnewElementEvent += (position, newstation) => {
                dataGridView1.Rows.Add(newstation.Name, newstation.ID, newstation.RegistrationDate, newstation.Location, newstation.Owner.Name, newstation.Flow, newstation.MainTenanceTime, newstation.Road);
            };
        }
        
        public List<int> ElementsWithoutData = new List<int>();
        public delegate void Action(int num);
        private static event Action ChangeSizeEvent;
        public static int RefillItems;
        public delegate void Act2(int position, Station newstation);
        private static event Act2 RefillEvent;
        private static event Act2 AddnewElementEvent;
        public static void AddElement(int nomatter, Station station) { AddnewElementEvent(nomatter, station); }
        public static void StartRefilling(int position, Station newstation) { RefillEvent(position,newstation); }
        
        public static void NewSize(int num) { ChangeSizeEvent(num);
            
        } //Resizing Form1 using data from NumberOfElementsForm
       
        public List<Station> stations = new List<Station>();
        public List<int> NumOfErrors = new List<int>();
        public List<string> NameOfErrors = new List<string>();
        bool IsOpened = false;
        string filename;
        public void OpenNumOfElemform()
        {
            NumberOfElementsForm form2 = new NumberOfElementsForm();
            form2.Show();
            ChangeSizeEvent += (num) => { Height = 144 + 24 * num; };
        } // Correcting number of visible elements

        int minID = 0, maxID = 0, minFlow = 0, maxFlow = 0, minMT = 0, maxMT = 0;

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            

            if (IsOpened)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save this file?\nAll your changes can be lost.", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveToolStripMenuItem_Click(sender, e);
                }
                IsOpened = false;  
            }
            IsOpened = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            stations.Clear();
            GridViewCorrection();
            OpenNumOfElemform();
            filename = openFileDialog1.FileName;
            NumOfErrors.Clear();
            NameOfErrors.Clear();
            ElementsWithoutData.Clear();
            try
            { 
                using (StreamReader sr = new StreamReader(filename, Encoding.Default))
                {
                    int ID, Flow, MT,num = -1;
                   
                    string Line ;
                    string[] strarr;

                   
                    while ((Line = sr.ReadLine()) != null)
                    {
                        if (Line.Equals("Name;ID;RegistrationDate;Location;Owner;Flow;MaintenanceTime;Road")) continue;
                        num++;
                        strarr = Line.Split(';');
                        if (strarr.Length!=8)
                        {
                        NumOfErrors.Add(num);
                        NameOfErrors.Add(strarr[0]);
                        continue;
                        } 
                        
                        int.TryParse(strarr[1], out ID);
                        int.TryParse(strarr[5], out Flow);
                        int.TryParse(strarr[6], out MT);
                        stations.Add(new Station(strarr[0], ID, strarr[2], strarr[3], new Owner(strarr[4]), Flow, MT, strarr[7]));
                        if (strarr[0]=="" || ID==0 || strarr[2]==""|| strarr[3]=="" || strarr[4]=="" || Flow == 0 || MT == 0 || strarr[7] == "")
                        {
                            ElementsWithoutData.Add(num);
                            NameOfErrors.Add(Line);
                        }

                        if (num == 0) { minID = ID; maxID = ID; minFlow = Flow;maxFlow = Flow;minMT = MT;maxMT = MT; }
                        if (ID > maxID) maxID = ID;
                        if (ID < minID) minID = ID;
                        if (Flow > maxFlow) maxFlow = Flow;
                        if (Flow < minFlow) minFlow = Flow;
                        if (MT > maxMT) maxMT = MT;
                        if (MT < minMT) minMT = MT;
                    } // creating new elements in stations list
                
                    for (int i = 0; i < stations.Count; i++)
                    {
                        dataGridView1.Rows.Add(
                            stations[i].Name, 
                            stations[i].ID,
                            stations[i].RegistrationDate, 
                            stations[i].Location,
                            stations[i].Owner.Name,
                            stations[i].Flow, 
                            stations[i].MainTenanceTime,
                            stations[i].Road);
                    } // adding rows in table
                   
                }
                
            

                if (NumOfErrors.Count + ElementsWithoutData.Count != 0)
                {
                    string warning = "";
                    if (NumOfErrors.Count != 0) warning += $"{NumOfErrors.Count} elements with incorrect input type.\n";
                    if (ElementsWithoutData.Count != 0) warning += $"{ElementsWithoutData.Count} elements without some input data.\n";
                    warning += "Please fill it by using menu button";
                    MessageBox.Show(warning, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                } // Warning about missing data
            }
            catch (Exception er) { MessageBox.Show(er.Message); }
        } // Open & parse new file
        
        private void fillDataInElementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
                int id, flow, mt;
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int.TryParse(dataGridView1[1, i].Value.ToString(), out id);
                    int.TryParse(dataGridView1[5, i].Value.ToString(), out flow);
                    int.TryParse(dataGridView1[6, i].Value.ToString(), out mt);

                    if (dataGridView1[0, i].Value.ToString() == null || dataGridView1[0, i].Value.ToString().Equals("") || dataGridView1[2, i].Value.ToString() == null || dataGridView1[2, i].Value.ToString().Equals("") || dataGridView1[3, i].Value.ToString() == null || dataGridView1[3, i].Value.ToString().Equals("") || dataGridView1[4, i].Value.ToString() == null || dataGridView1[4, i].Value.ToString().Equals("") || dataGridView1[7, i].Value.ToString() == null || dataGridView1[7, i].Value.ToString().Equals(""))
                    {
                        Station station = new Station(dataGridView1[0, i].Value.ToString(), id, dataGridView1[2, i].Value.ToString(), dataGridView1[3, i].Value.ToString(), new Owner(dataGridView1[4, i].Value.ToString()), flow, mt, dataGridView1[7, i].Value.ToString());
                        station = AddData(station, i, dataGridView1.Rows.Count);
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            
        } //Refill all empty data

        private void DeveloperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Verko Roman, 2019\n HSE CS 183");
        } // Developer info in menu strip

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ChangeElementToolStripMenuItem_Click(sender, e);
            } catch(Exception ex) { MessageBox.Show(ex.Message); }
        } // Changing data in row by Double clicking

        private void ChangeElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Station newst = new Station();
                int CP = dataGridView1.SelectedRows[0].Index;
                int id, flow, mt;
                int.TryParse(dataGridView1[1, CP].Value.ToString(), out id);
                int.TryParse(dataGridView1[5, CP].Value.ToString(), out flow);
                int.TryParse(dataGridView1[6, CP].Value.ToString(), out mt);
                newst = new Station(dataGridView1[0, CP].Value.ToString(), id, dataGridView1[2, CP].Value.ToString(), dataGridView1[3, CP].Value.ToString(), new Owner(dataGridView1[4, CP].Value.ToString()), flow, mt, dataGridView1[7, CP].Value.ToString());
                newst = AddData(newst, CP, dataGridView1.Rows.Count);
            }
            catch (ArgumentOutOfRangeException) { MessageBox.Show("Select any row", "Warning"); }
            catch (Exception) { MessageBox.Show("Exeption"); }
        } // Changing data in row using menu strip

        private void ownerStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (stations.Count == 0) return;
                Station[] ownstat = new Station[dataGridView1.Rows.Count];
                int id = 0, flow = 0, mt = 0;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int.TryParse(dataGridView1[1, i].Value.ToString(), out id);
                    int.TryParse(dataGridView1[5, i].Value.ToString(), out flow);
                    int.TryParse(dataGridView1[6, i].Value.ToString(), out mt);
                    ownstat[i] = new Station(dataGridView1[0, i].Value.ToString(), id, dataGridView1[2, i].Value.ToString(), dataGridView1[3, i].Value.ToString(), new Owner(dataGridView1[4, i].Value.ToString()), flow, mt, dataGridView1[7, i].Value.ToString());
                }
                for (int j = 0; j < ownstat.Length; j++)
                {
                    for (int k = 0; k < ownstat.Length; k++)
                    {
                        if (ownstat[j].Owner.Name.Equals(ownstat[k].Owner.Name) && j != k) ownstat[j].otherOwners++;
                    }
                }
                Array.Sort(ownstat);
                List<Station> ownstatlist = new List<Station>();
                foreach (var item in ownstat)
                {
                    ownstatlist.Add(item);
                }
                FilterForm Ff = new FilterForm(ownstatlist);
                Ff.Show();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

        } //todo fliter by owners

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int example;
                if (textBox1.Text.Trim() == null || textBox1.Text.Trim().Equals("") || !int.TryParse(textBox1.Text, out example)) { MessageBox.Show("no or wrong data"); return; }
                if (stations.Count == 0) return;
                int.TryParse(textBox1.Text, out example);
                stations.Clear();
                int id, flow, mt;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int.TryParse(dataGridView1[1, i].Value.ToString(), out id);
                    int.TryParse(dataGridView1[5, i].Value.ToString(), out flow);
                    int.TryParse(dataGridView1[6, i].Value.ToString(), out mt);
                    stations.Add(new Station(dataGridView1[0, i].Value.ToString(), id, dataGridView1[2, i].Value.ToString(), dataGridView1[3, i].Value.ToString(), new Owner(dataGridView1[4, i].Value.ToString()), flow, mt, dataGridView1[7, i].Value.ToString()));
                }
                List<Station> FilteredList = new List<Station>();
                foreach (var item in stations)
                {
                    if (item.Flow == example) FilteredList.Add(item);
                }

                FilterForm Ff = new FilterForm(FilteredList);
                Ff.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        } //FilterByFlow

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() == null || textBox1.Text.Trim().Equals("")) { MessageBox.Show("no or wrong data"); return; }
                string example = textBox1.Text.Trim();
                if (stations.Count == 0) return;
                stations.Clear();
                int id, flow, mt;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int.TryParse(dataGridView1[1, i].Value.ToString(), out id);
                    int.TryParse(dataGridView1[5, i].Value.ToString(), out flow);
                    int.TryParse(dataGridView1[6, i].Value.ToString(), out mt);
                    stations.Add(new Station(dataGridView1[0, i].Value.ToString(), id, dataGridView1[2, i].Value.ToString(), dataGridView1[3, i].Value.ToString(), new Owner(dataGridView1[4, i].Value.ToString()), flow, mt, dataGridView1[7, i].Value.ToString()));
                }
                List<Station> FilteredList = new List<Station>();
                foreach (var item in stations)
                {
                    if (item.Road.ToLower().Contains(example.ToLower())) FilteredList.Add(item);
                }

                FilterForm Ff = new FilterForm(FilteredList);
                Ff.Show();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

        }//FilterByRoad

        private void addElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsOpened) return;
                Station newStation = new Station();
                stations.Add(newStation);
                stations.Add(AddData(newStation, dataGridView1.Rows.Count + 1, dataGridView1.Rows.Count));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        } //Adding new element

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;

            try
            {
                // сохраняем текст в файл
                if (File.Exists(filename))
                {
                    File.WriteAllText(filename, DataGridInString(dataGridView1),Encoding.Default);
                }
                else
                {
                    MessageBox.Show("This file no longer exists");
                }

                MessageBox.Show("Saved!");
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

        } // Save in current file

        private void addToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            { 
                if (dataGridView1.Rows.Count == 0) return;
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем выбранный файл
                string savefilename = saveFileDialog1.FileName;
                // сохраняем текст в файл
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.CheckFileExists = false;
                File.AppendAllText(savefilename, DataGridInString(dataGridView1),Encoding.Default);
                MessageBox.Show("Added!");
                saveFileDialog1.CheckFileExists = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        } // Add elements to another file

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsOpened)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save this file?\nAll your changes can be lost.", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveToolStripMenuItem_Click(sender, e);
                }
                IsOpened = false;
            } 
        } // Ask to save file before quiting

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { 
            if (dataGridView1.Rows.Count == 0) return;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string savefilename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            if (File.Exists(savefilename))
            {
                    filename = savefilename;
                    SaveToolStripMenuItem_Click(sender, e);
            }
            else
            {
                File.WriteAllText(savefilename, DataGridInString(dataGridView1), Encoding.Default);
                MessageBox.Show("Saved!");
            }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        } // Change directory of saving file

        private void CreateNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { 
            if (IsOpened)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save this file?\nAll your changes can be lost.", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveToolStripMenuItem_Click(sender, e);
                }
                IsOpened = false;
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            stations.Clear();
            GridViewCorrection();
            string newdir = Directory.GetCurrentDirectory();
            File.WriteAllText(newdir + "new.csv", DataGridInString(dataGridView1), Encoding.Default);
            filename = newdir = "new.csv";
            MessageBox.Show("Created new.csv");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            IsOpened = true;
        }// New file

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
        } //Making new table in DataGridView1
        /// <summary>
        /// Adding data to the element in grid
        /// </summary>
        /// <param name="station">element</param>
        /// <param name="n">number of element</param>
        /// <param name="m">count of all elements</param>
        /// <returns></returns>
        public Station AddData(Station station, int n, int m)
        {
            AddDataForm form3 = new AddDataForm(station,n,m);
            form3.Show();
            return form3.newstation;
        } // Opening Form for adding new element
        /// <summary>
        /// String representation of all data (CSV)
        /// </summary>
        /// <param name="dtg">datagridview</param>
        /// <returns></returns>
        private string DataGridInString(DataGridView dtg)
        {
            string text = "";
          //  text += "Name;ID;RegistrationDate;Location;Owner;Flow;MaintenanceTime;Road\n";
            for (int i = 0; i < dtg.Rows.Count; i++)
            {
                text += $"{dtg[0,i].Value.ToString()};{dtg[1, i].Value.ToString()};{dtg[2, i].Value.ToString()};{dtg[3, i].Value.ToString()};{dtg[4, i].Value.ToString()};{dtg[5, i].Value.ToString()};{dtg[6, i].Value.ToString()};{dtg[7, i].Value.ToString()}\n";
            }
            return text;
        } // Turn data in csv text file
    }
}
