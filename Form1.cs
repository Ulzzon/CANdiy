﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace WindowsFormsApplication2
{

       /*
            public string id;
            public string dl;
            public string[] data;
        */
    
    
    public partial class CANdiy : Form
    {
        int time1,sec,min,h = 0;
        string stimer;
        string fileDir;
        decimal comport = 1;

        transfer mess0 = new transfer();
        //transfer send = new transfer();
        transfer mess1 = new transfer();
        transfer mess2 = new transfer();
        transfer mess3 = new transfer();
        transfer mess4 = new transfer();
        transfer mess5 = new transfer();
       // MessageSetup fs = new MessageSetup();

        public CANdiy()
        {
            InitializeComponent();
            creatMyListview();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void function1()
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

                    //serialPort1.Read(buffer, 0, 8);
                    //string s = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
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
                    /*
            
                        serialPort1.Read(buffer, 0, 1);
                        string s = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        label1.BeginInvoke(delegate { label1.Text = "Trade" + s; });

                    }*/
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                Thread th = new Thread(function1);
                th.IsBackground = true;
                th.Start();
                toolStripStatusLabel3.Text = "Running";
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
            if (rx)dir = "RX";
            else dir = "TX";
            
            label1.Text = "tid: " + time1;

            ListViewItem item1 = new ListViewItem("item1");
            item1.Name = id;
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Name == item1.Name)
                    {
                        item.Text = "" + stimer;
                        item.SubItems[4].Text = "" + data;
                        newItem = false;
                    }
                }
                if (newItem == true)
                {
                    item1.Text = "" + stimer;
                    item1.SubItems.Add(dir);
                    item1.SubItems.Add(id);
                    item1.SubItems.Add(""+dl);
                    item1.SubItems.Add(data);
                    listView1.Items.AddRange(new ListViewItem[] { item1 });
                }
                if (checkBox2.Checked)
                {
                    string send = String.Format("{0} {1} {2} {3} {4} \n", stimer, id, dir, dl, data);
                    writeToFile(send);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
            timer1.Enabled = false;
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            mess0.data = dataArray.ToString();
            SendFrame(mess0);

        }


        public Boolean SendFrame(transfer sendMess)
        {
            //string data = String.Join(" ", sendMess.data);
            string print = String.Format("{0,3:H} {1,1} {2}\n", sendMess.id, sendMess.dl, sendMess.data);

            PrintLabel(sendMess.id, sendMess.dl, sendMess.data, false);
            if(serialPort1.IsOpen && print.Length >= 5)
            {
                serialPort1.Write(print);
                toolStripStatusLabel1.Text = "Message now sent! ";
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
                timer3.Interval = mess2.timer;
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
            // Get file name.
            fileDir = saveFileDialog1.FileName;
            // Write to the file name selected.
            // ... You can write the text from a TextBox instead of a string literal.
            File.WriteAllText(fileDir, "The start of log file \n \n");
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
            if (mess1.timer != 0)
            {
                SendFrame(mess1);
            }
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
    }

     }


    
