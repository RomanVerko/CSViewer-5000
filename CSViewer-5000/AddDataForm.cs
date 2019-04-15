using System;
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
    public partial class AddDataForm: Form
    {
        int number,number2;
        
        public AddDataForm(Station station, int n,int m)
        {
           
            InitializeComponent();
            number = n;
            number2 = m;
            label1.Text = $"Element {n} out of {m}";
            textBox1.Text = station.Name;
            textBox2.Text = station.ID.ToString();
            textBox3.Text = station.RegistrationDate;
            textBox4.Text = station.Location;
            textBox5.Text = station.Owner.Name;
            textBox6.Text = station.Flow.ToString();
            textBox7.Text = station.MainTenanceTime.ToString();
            textBox8.Text = station.Road;
        }
        
        public Station newstation = new Station();
       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name, rd, loc, own, road, error = "wrong data in:\n";
                int id, flow, mt;
                bool flag = true;

                name = textBox1.Text;
                name = name.Trim();
                if (name != null && !name.Equals("")) newstation.Name = name;
                else { flag = false; error += "name\n"; };

                rd = textBox3.Text;
                rd = rd.Trim();
                if (rd != null && !rd.Equals("")) newstation.RegistrationDate = rd;
                else { flag = false; error += "registration date\n"; };

                loc = textBox4.Text;
                loc = loc.Trim();
                if (loc != null && !loc.Equals("")) newstation.Location = loc;
                else { flag = false; error += "location\n"; };

                own = textBox5.Text;
                own = own.Trim();
                if (own != null && !own.Equals("")) newstation.Owner.Name = own;
                else { flag = false; error += "owner\n"; };

                road = textBox8.Text;
                road = road.Trim();
                if (road != null && !road.Equals("")) newstation.Road = road;
                else { flag = false; error += "road\n"; };

                if (!int.TryParse(textBox2.Text, out id)) { flag = false; error += "id\n"; }
                else newstation.ID = id;
                if (!int.TryParse(textBox6.Text, out flow)) { flag = false; error += "flow\n"; }
                else newstation.Flow = flow;
                if (!int.TryParse(textBox7.Text, out mt)) { flag = false; error += "maintenance\n"; }
                else newstation.MainTenanceTime = mt;

                
                if (flag) {
                    if (number <= number2)
                    {
                        Form1.StartRefilling(number,newstation);
                    }
                    else { Form1.AddElement(1,newstation); }
                    Close();
                }

                else { MessageBox.Show(error); return; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        public Form1 Form1
        {
            get => default(Form1);
            set {
            }
        }
    }
}
