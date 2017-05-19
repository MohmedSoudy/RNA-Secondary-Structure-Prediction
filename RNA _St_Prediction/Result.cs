using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace RNA__St_Prediction
{
    public partial class Result : Form
    {
        public static string Out = "";
        public static string File_Path = "Score.txt";
        public static string Down_Path = "Download.txt";
        public static DateTime Date { get; set; }

        public Result()
        {
            InitializeComponent();
        }

        private void Result_Load(object sender, EventArgs e)
        {
            int[,] Matrix = Form1.RNA.Fill_Matrix(Form1.RNA);
            Form1.RNA.RNAStructurePredictionTraceback(Matrix, ref Form1.RNA.RNA_Sequence, Out, 0, Form1.RNA.RNA_Sequence.Length - 1);
            textBox1.Text = Form1.RNA.Sequence;
            textBox2.Text = Matrix[0, Form1.RNA.RNA_Sequence.Length - 1].ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Query = Form1.RNA.Get_IpAddress();
            IPHostEntry Ip = Dns.GetHostByName(Query);
            IPAddress[] Ip_address = Ip.AddressList;
            string IP_Address = Ip_address[0].ToString();
            if (Form1.Check)
            {
                using (StreamWriter St = File.AppendText(Down_Path))
                {
                    St.WriteLine("RNA Species Name : " + Form1.Seq_info);
                    St.WriteLine("User Name : " + Query);
                    St.WriteLine("IP address : " + IP_Address);
                    St.WriteLine("Query input : " + Form1.RNA.RNA_Sequence);
                    St.WriteLine("Quert Output : " + Form1.RNA.Sequence);
                    St.WriteLine("Score : " + textBox2.Text);
                    Date = DateTime.Now;
                    St.WriteLine("Date of Prediction : " + Date);
                    St.WriteLine(Environment.NewLine);

                }
            }
            else
            {
                using (StreamWriter St = File.AppendText(File_Path))
                {
                    St.WriteLine("User Name : " + Query);
                    St.WriteLine("IP address : " + IP_Address);
                    St.WriteLine("Query input : " + Form1.RNA.RNA_Sequence);
                    St.WriteLine("Quert Output : " + Form1.RNA.Sequence);
                    St.WriteLine("Score : " + textBox2.Text);
                    Date = DateTime.Now;
                    St.WriteLine("Date of Prediction : " + Date);
                    St.WriteLine(Environment.NewLine);
                }
            }
            MessageBox.Show("Query Saved Successfully!!");
            Application.Exit();
        }
    }
}
