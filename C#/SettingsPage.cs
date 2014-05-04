using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class SettingsPage : Form
    {
        public SettingsPage()
        {
            InitializeComponent();

            if (Properties.Settings.Default.TimerSetting == 0)
            {
                radioButton1.Checked = true;
            }
            else if (Properties.Settings.Default.TimerSetting == 1 )
            {
                radioButton2.Checked = true;
            }
            else if (Properties.Settings.Default.TimerSetting == 2)
            {
                 radioButton3.Checked = true;
            }

            if (Properties.Settings.Default.AutoSortingON == true) checkBox1.Checked = true;
            else checkBox1.Checked = false;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Executed when any radio button is changed.
            // ... It is wired up to every single radio button.
            // Search for first radio button in GroupBox.
            string result1 = null;
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        result1 = radio.Name;
                    }
                }
            }
            if(result1 == radioButton1.Name){
                Properties.Settings.Default.TimerSetting = 0;
            }
            else if (result1 == radioButton2.Name)
            {
                Properties.Settings.Default.TimerSetting = 1;
            }
            else if (result1 == radioButton3.Name)
            {
                Properties.Settings.Default.TimerSetting = 2;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) Properties.Settings.Default.AutoSortingON = true;
            else Properties.Settings.Default.AutoSortingON = false;
            
        }


    }
}
