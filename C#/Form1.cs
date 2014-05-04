using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;


namespace WindowsFormsApplication2
{ 
    
    public partial class CANdiy : Form
    {
        int time1,sec,min,h = 0;
        string stimer;
        string fileDir;
        decimal comport = 1;
        private int sortColumn = -1;

        transfer mess0 = new transfer();
        transfer mess1 = new transfer();
        transfer mess2 = new transfer();
        transfer mess3 = new transfer();
        transfer mess4 = new transfer();
        transfer mess5 = new transfer();

        public CANdiy()
        {
            InitializeComponent();
            creatMyListview();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Properties.Settings.Default.SerialPort;
        }

        private void serialThread()
        {
            var buffer = new byte[64];
            string myString = "";
            string st = "";
            int dl = 8;
            while (serialPort1.IsOpen)
            {

                try
                {
                    st = serialPort1.ReadTo("\n");
                }
                catch { }

                string[] parts = st.Split(' ');
                for (int i = 2; i<parts.Length; i++)
                {
                    myString += parts[i];
                    myString += " ";
                }

                try
                {
                    dl = Convert.ToInt32(parts[1]);
                }
                catch {
                    MessageBox.Show("Serial read thread convert faild");
                }

                if (label1.InvokeRequired)
                {
                    label1.Invoke(new MethodInvoker(() => PrintLabel(parts[0], dl, myString, true)));
                }else
                {
                    MessageBox.Show("Not able to Invoke");
                }
                myString = "";
                
            }
        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.PortName = "COM" + comport;
                    serialPort1.Open();
                }
                catch
                {
                    MessageBox.Show("Could not open port");
                }
            }
            else MessageBox.Show("The port is already open");
            if (serialPort1.IsOpen)
            {
                timer1.Interval = 10;
                timer1.Enabled = true;
                Thread th = new Thread(serialThread);
                th.IsBackground = true;
                th.Start();
                toolStripStatusLabel3.Text = "Running";
                Properties.Settings.Default.SerialPort = comport;
                Properties.Settings.Default.Save();
            }
        }

        private void creatMyListview()
        {
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            
            listView1.Columns.Add("Time", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Dir", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("ID", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Dl", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Data", -2, HorizontalAlignment.Left);

            this.Controls.Add(listView1);

        }

        public void PrintLabel(string id, int dl, string data, Boolean rx)
        {
            Boolean newItem = true;
            string dir = "";
            string time = "";
            if (rx)dir = "RX";
            else dir = "TX";

            if (Properties.Settings.Default.TimerSetting == 0) time = String.Format("{0}.{1}", sec, time1);
            else if(Properties.Settings.Default.TimerSetting == 1) time = stimer;
            else if (Properties.Settings.Default.TimerSetting == 2) time = DateTime.Now.ToString("HH:mm:ss");
            

            ListViewItem item1 = new ListViewItem("item1");
            item1.Name = id;
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Name == item1.Name)
                    {
                        item.Text = "" +time;
                        item.SubItems[4].Text = "" + data;
                        newItem = false;
                    }
                }
                if (newItem == true)
                {
                    item1.Text = "" + time;
                    item1.SubItems.Add(dir);
                    item1.SubItems.Add(id);
                    item1.SubItems.Add(""+dl);
                    item1.SubItems.Add(data);
                    listView1.Items.AddRange(new ListViewItem[] { item1 });
                }
                if (Properties.Settings.Default.AutoSortingON == true) listView1.Sort();
                
                if (checkBox2.Checked)
                {
                    string send = String.Format("{0} {1} {2} {3} {4} \n", time, id, dir, dl, data);
                    writeToFile(send);
                }
        }

        private void Stop_Button_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;
            timer6.Enabled = false;
            timer7.Enabled = false;
            while (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                }
                catch { }
            }
            toolStripStatusLabel3.Text = "Stopped";
            listView1.Items.Clear();
            
        }

        

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            comport = numericUpDown1.Value;
            
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            
            string[] dataArray = new string[64];
            mess0.id= textBox1.Text;
            try
            {
                mess0.dl = Convert.ToInt32(textBox2.Text);
            }
            catch { MessageBox.Show("Convert faild"); 
            }
            dataArray[0] = textBox3.Text;
            dataArray[1] = textBox4.Text;
            dataArray[2] = textBox5.Text;
            dataArray[3] = textBox6.Text;
            dataArray[4] = textBox7.Text;
            dataArray[5] = textBox8.Text;
            dataArray[6] = textBox9.Text;
            dataArray[7] = textBox10.Text;
            mess0.data = String.Join(" ", dataArray);
            SendFrame(mess0);

        }


        public Boolean SendFrame(transfer sendMess)
        {
            //string data = String.Join(" ", sendMess.dataArray());

            StringBuilder builtString = new StringBuilder();
            string[] sendArray = new string[8];

            for (int i = 0; i < sendMess.dl; i++)
            {
                sendArray = sendMess.dataArray();
                builtString = builtString.Append(sendArray[i] + ' ');
            }
            string dataString = builtString.ToString();

            string print = String.Format("{0,3:H} {1,1} {2}\n", sendMess.id, sendMess.dl, dataString);
    
            if(serialPort1.IsOpen && print.Length >= 5)
            {
                serialPort1.Write(print);
                toolStripStatusLabel1.Text = "Message now sent! ";
                PrintLabel(sendMess.id, sendMess.dl, dataString, false);
                return true;
            }
            else
            {
                MessageBox.Show("Port is not open");
                return false;
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int sendtimer = 100;
            try
            {
                sendtimer = Convert.ToInt32(maskedTextBox1.Text);
            }
            catch
            {
                MessageBox.Show("Not abel to convert string to int");
            }
            if (checkBox1.Checked && sendtimer != 0)
            {
                timer2.Interval = sendtimer;
                timer2.Enabled = true;
            }
            else timer2.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) saveFileDialog1.ShowDialog();
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox3.Checked && mess1.timer != 0)
            {
                timer3.Interval = mess1.timer;
                timer3.Enabled = true;
            }
            else timer3.Enabled = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox4.Checked && mess2.timer != 0)
            {
                timer4.Interval = mess2.timer;
                timer4.Enabled = true;
            }
            else timer5.Enabled = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox5.Checked && mess3.timer != 0)
            {
                timer5.Interval = mess3.timer;
                timer5.Enabled = true;
            }
            else timer5.Enabled = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox6.Checked && mess4.timer != 0)
            {
                timer6.Interval = mess4.timer;
                timer6.Enabled = true;
            }
            else timer6.Enabled = false;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox7.Checked && mess5.timer != 0)
            {
                timer7.Interval = mess5.timer;
                timer7.Enabled = true;
            }
            else timer7.Enabled = false;
        }

        private void saveFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {
            string timerForm = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //MM/dd/yyyy
            string startString = String.Format("This file was created by CANdiy {0} \n \n", timerForm);
            // Get file name.
            fileDir = saveFileDialog1.FileName;
            // Write to the file name selected.
            // ... You can write the text from a TextBox instead of a string literal.

            File.WriteAllText(fileDir, startString);
        }

        public void writeToFile(string st)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fileDir, true))
            {
                file.WriteLine(st);
            }
        }

        public void readFromFile()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileDir, true))
            {
                String logdata = file.ReadLine();
            }
        }

        private void messageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageSetup fs = new MessageSetup(mess1,mess2,mess3,mess4,mess5);
            fs.ShowDialog();            
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CANdiy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.A)
            {
                //PrintLabel(sendMessageID, sendDL, data, false);
                if (serialPort1.IsOpen)
                {
                    //serialPort1.Write(fs.message1);
                    toolStripStatusLabel1.Text = "Message1 now sent! ";
                    
                }
                toolStripStatusLabel2.Text = "Key";
                toolStripStatusLabel2.Visible = true;
            }
            toolStripStatusLabel2.Text = "Key";
            toolStripStatusLabel2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendFrame(mess1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendFrame(mess2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendFrame(mess3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendFrame(mess4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendFrame(mess5);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time1++;
            if (time1 >= 100)
            {
                sec++;
                time1 = 0;
                toolStripStatusLabel4.Text = String.Format("{0,2:d}:{1,2:d}:{2,2:d}", h, min, sec);
                if (sec >= 60)
                {
                    min++;
                    sec = 0;
                    if (min >= 60)
                    {
                        h++;
                        min = 0;
                    }
                }
            }
            stimer = String.Format("{0,2:d} : {1,2:d} : {2,2:d} : {3,2:d} ", h, min, sec, time1);
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //StringFormatFlags.DisplayFormatControl.CompareTo(maskedTextBox1.Text);
            SendFrame(mess0);
                //time2 = 0;
            toolStripStatusLabel2.Visible = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
           
            SendFrame(mess1);
            
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            SendFrame(mess2);
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            SendFrame(mess3);
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            SendFrame(mess4);
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            SendFrame(mess5);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                listView1.Sorting = SortOrder.Ascending;
                listView1.SetSortIcon(e.Column, SortOrder.Ascending);
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listView1.Sorting == SortOrder.Ascending)
                {
                    listView1.Sorting = SortOrder.Descending;
                    listView1.SetSortIcon(e.Column, SortOrder.Descending);
                }
                else
                {
                    listView1.Sorting = SortOrder.Ascending;
                    listView1.SetSortIcon(e.Column, SortOrder.Ascending);
                }
                    
            }

            // Call the sort method to manually sort.
            listView1.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsPage setting = new SettingsPage();
            setting.ShowDialog();
            //UserControl1 userSettings = new UserControl1();
            //userSettings.s; 
        } 
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }
        public int Compare(object x, object y) 
    {
        int returnVal= -1;
        returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                ((ListViewItem)y).SubItems[col].Text);
        // Determine whether the sort order is descending.
        if (order == SortOrder.Descending)
            // Invert the value returned by String.Compare.
            returnVal *= -1;
        return returnVal;
    }
    }
    
}
