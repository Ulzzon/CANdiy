using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;

namespace WindowsFormsApplication2
{
    public partial class MessageSetup : Form
    {
        //public string message1 = "0x111 7 11 11 11 11 11 11 11 11\n";
        //public string message2 = "";
        //public string message3 = "";
        //public string message4 = "";
        //public string message5 = "";
        transfer mess1 = new transfer();
        CANdiy fs = new CANdiy();
        private transfer trans1, trans2, trans3, trans4, trans5; 
        public MessageSetup(transfer m1,transfer m2,transfer m3, transfer m4, transfer m5)
        {
            trans1 = m1;
            trans2 = m2;
            trans3 = m3;
            trans4 = m4;
            trans5 = m5;

            InitializeComponent();
            textBox1_1.Text = trans1.id;
            textBox1_2.Text = trans1.dl.ToString();
            string[] t1data = trans1.dataArray();
            textBox1_3.Text = t1data[0];
            textBox1_4.Text = t1data[1];
            textBox1_5.Text = t1data[2];
            textBox1_6.Text = t1data[3];
            textBox1_7.Text = t1data[4];
            textBox1_8.Text = t1data[5];
            textBox1_9.Text = t1data[6];
            textBox1_10.Text = t1data[7];
            textBox1_11.Text = trans1.timer.ToString();

            textBox2_1.Text = trans2.id;
            textBox2_2.Text = trans2.dl.ToString();
            string[] t2data = trans2.dataArray();
            textBox2_3.Text = t2data[0];
            textBox2_4.Text = t2data[1];
            textBox2_5.Text = t2data[2];
            textBox2_6.Text = t2data[3];
            textBox2_7.Text = t2data[4];
            textBox2_8.Text = t2data[5];
            textBox2_9.Text = t2data[6];
            textBox2_10.Text = t2data[7];
            textBox2_11.Text = trans2.timer.ToString();

            textBox3_1.Text = trans3.id;
            textBox3_2.Text = trans3.dl.ToString();
            string[] t3data = trans3.dataArray();
            textBox3_3.Text = t3data[0];
            textBox3_4.Text = t3data[1];
            textBox3_5.Text = t3data[2];
            textBox3_6.Text = t3data[3];
            textBox3_7.Text = t3data[4];
            textBox3_8.Text = t3data[5];
            textBox3_9.Text = t3data[6];
            textBox3_10.Text = t3data[7];
            textBox3_11.Text = trans3.timer.ToString();

            textBox4_1.Text = trans4.id;
            textBox4_2.Text = trans4.dl.ToString();
            string[] t4data = trans4.dataArray();
            textBox4_3.Text = t4data[0];
            textBox4_4.Text = t4data[1];
            textBox4_5.Text = t4data[2];
            textBox4_6.Text = t4data[3];
            textBox4_7.Text = t4data[4];
            textBox4_8.Text = t4data[5];
            textBox4_9.Text = t4data[6];
            textBox4_10.Text = t4data[7];
            textBox4_11.Text = trans4.timer.ToString();

            textBox5_1.Text = trans5.id;
            textBox5_2.Text = trans5.dl.ToString();
            string[] t5data = trans5.dataArray();
            textBox5_3.Text = t5data[0];
            textBox5_4.Text = t5data[1];
            textBox5_5.Text = t5data[2];
            textBox5_6.Text = t5data[3];
            textBox5_7.Text = t5data[4];
            textBox5_8.Text = t5data[5];
            textBox5_9.Text = t5data[6];
            textBox5_10.Text = t5data[7];
            textBox5_11.Text = trans5.timer.ToString();
        }


        private void MessageSetup_Load(object sender, EventArgs e)
        {        
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            //GetAccessibilityObjectById(CANdiy.
            string[] dataArray = new string[8];
            
            trans1.id = textBox1_1.Text;
            try
            {
                trans1.dl = Convert.ToInt32(textBox1_2.Text);
                trans1.timer = Convert.ToInt32(textBox1_11.Text);
            }
            catch {
                MessageBox.Show("Convert faild");
            }
            dataArray[0] = textBox1_3.Text;
            dataArray[1] = textBox1_4.Text;
            dataArray[2] = textBox1_5.Text;
            dataArray[3] = textBox1_6.Text;
            dataArray[4] = textBox1_7.Text;
            dataArray[5] = textBox1_8.Text;
            dataArray[6] = textBox1_9.Text;
            dataArray[7] = textBox1_10.Text;
            trans1.data = String.Join(" ", dataArray);
            
            //fs.getMes(mess1, 1);
            //string data = ;
            label4.Text = trans1.data;
            //CANdiy.getMes(mess, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //GetAccessibilityObjectById(CANdiy.
            string[] dataArray = new string[8];
            trans2.id = textBox1_1.Text;
            try
            {
                trans2.dl = Convert.ToInt32(textBox2_2.Text);
                trans2.timer = Convert.ToInt32(textBox2_11.Text);
            }
            catch { }
            dataArray[0] = textBox1_3.Text;
            dataArray[1] = textBox1_4.Text;
            dataArray[2] = textBox1_5.Text;
            dataArray[3] = textBox1_6.Text;
            dataArray[4] = textBox1_7.Text;
            dataArray[5] = textBox1_8.Text;
            dataArray[6] = textBox1_9.Text;
            dataArray[7] = textBox1_10.Text;
            trans2.data = String.Join(" ", dataArray);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] dataArray = new string[8];
            trans3.id = textBox1_1.Text;
            try
            {
                trans3.dl = Convert.ToInt32(textBox3_2.Text);
                trans3.timer = Convert.ToInt32(textBox3_11.Text);
            }
            catch { }
            dataArray[0] = textBox1_3.Text;
            dataArray[1] = textBox1_4.Text;
            dataArray[2] = textBox1_5.Text;
            dataArray[3] = textBox1_6.Text;
            dataArray[4] = textBox1_7.Text;
            dataArray[5] = textBox1_8.Text;
            dataArray[6] = textBox1_9.Text;
            dataArray[7] = textBox1_10.Text;
            trans3.data = String.Join(" ", dataArray);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] dataArray = new string[8];
            trans4.id = textBox4_1.Text;
            try
            {
                trans4.dl = Convert.ToInt32(textBox4_2.Text);
                trans4.timer = Convert.ToInt32(textBox4_11.Text);
            }
            catch { }
            dataArray[0] = textBox4_3.Text;
            dataArray[1] = textBox4_4.Text;
            dataArray[2] = textBox4_5.Text;
            dataArray[3] = textBox4_6.Text;
            dataArray[4] = textBox4_7.Text;
            dataArray[5] = textBox4_8.Text;
            dataArray[6] = textBox4_9.Text;
            dataArray[7] = textBox4_10.Text;
            trans4.data = String.Join(" ", dataArray);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] dataArray = new string[8];
            trans5.id = textBox5_1.Text;
            try
            {
                trans5.dl = Convert.ToInt32(textBox5_2.Text);
                trans5.timer = Convert.ToInt32(textBox5_11.Text);
            }
            catch { }
            dataArray[0] = textBox5_3.Text;
            dataArray[1] = textBox5_4.Text;
            dataArray[2] = textBox5_5.Text;
            dataArray[3] = textBox5_6.Text;
            dataArray[4] = textBox5_7.Text;
            dataArray[5] = textBox5_8.Text;
            dataArray[6] = textBox5_9.Text;
            dataArray[7] = textBox5_10.Text;
            trans5.data = String.Join(" ", dataArray);
        }


    }
}
