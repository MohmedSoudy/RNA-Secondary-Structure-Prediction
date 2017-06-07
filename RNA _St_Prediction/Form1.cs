using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace RNA__St_Prediction
{
    public partial class Form1 : Form
    {
        public static RNA_Structure RNA = new RNA_Structure();
        public static bool Check = false;
        public static string Seq_info = "";
        Result Res = new Result();
        public Form1()
        {
            InitializeComponent();
        }
        public string Search_Query(string input , string File_Name , int Start, int index)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(File_Name);
            string Sequence = sr.ReadToEnd().ToString();
            string[] s = Sequence.Split('\n');
            for (int i = Start; i < s.Length; i+=index)
            {
                string[] Input = s[i].Split(':');
                string []Final = Input[1].Split('\r');
                string S = Final[0].Replace(" ", string.Empty);
                if (S == input)
                {
                    MessageBox.Show("Query have been searched Before For more details go to line : " + i);
                    Process.Start(File_Name);
                    return "-1";
                }
            }
            sr.Close();
            return "";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Enter_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox1.Enabled = true;
            button2.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Browse_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox3.Enabled = false;
            groupBox2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void Download_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Enter.Checked)
            {
                RNA.RNA_Sequence = textBox1.Text.ToUpper();
                if (RNA.RNA_Sequence.Trim() == "")              //Removes Any Whitespace From our string
                {
                    textBox1.Clear();
                    return;
                }
                for (int i = 0; i < RNA.RNA_Sequence.Length; i++)
                {
                    if (char.IsNumber(RNA.RNA_Sequence[i]))
                    {
                        MessageBox.Show("Your Sequence containts Number");
                        textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                        textBox1.SelectionLength = textBox1.Text.Length;
                        RNA.RNA_Sequence = textBox1.Text;
                        return;
                    }
                    if (RNA.RNA_Sequence[i] != 'A' && RNA.RNA_Sequence[i] != 'C' && RNA.RNA_Sequence[i] != 'G' && RNA.RNA_Sequence[i] != 'U')
                    {
                        MessageBox.Show("Enter A valid RNA Sequence");
                        textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                        textBox1.SelectionLength = textBox1.Text.Length;
                        RNA.RNA_Sequence = textBox1.Text;
                        return;
                    }
                    if (RNA.RNA_Sequence == "")
                    {
                        MessageBox.Show("Enter A valid RNA Sequence");
                        return;
                    }
                   
                }
                string u = Search_Query(RNA.RNA_Sequence , "Score.txt" , 2 , 8);
                if (u != "")
                    return;
              
            }
            else if (Download.Checked)
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("Enter A Valid Accession");
                    return;
                }
                List<string> Sequence = RNA.Get_seq(textBox3.Text);
                RNA.RNA_Sequence = Sequence[0];
                Seq_info = Sequence[1];
                Check = true;
                string u = Search_Query(RNA.RNA_Sequence, "Download.txt" , 3 , 9);
                if (u != "")
                    return;
            }
            this.Visible = false;
            Res.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RNA.RNA_Sequence = textBox1.Text.ToUpper();
            if (RNA.RNA_Sequence.Trim() == "")              //Removes Any Whitespace From our string
            {
                textBox1.Clear();
                return;
            }
            for (int i = 0; i < RNA.RNA_Sequence.Length; i++)
            {
                if (char.IsNumber(RNA.RNA_Sequence[i]))
                {
                    MessageBox.Show("Your Sequence containts Number");
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                    textBox1.SelectionLength = textBox1.Text.Length;
                    RNA.RNA_Sequence = textBox1.Text;
                    return;
                }
                if (RNA.RNA_Sequence[i] != 'A' && RNA.RNA_Sequence[i] != 'C' && RNA.RNA_Sequence[i] != 'G' && RNA.RNA_Sequence[i] != 'U')
                {
                    MessageBox.Show("Enter A valid RNA Sequence");
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                    textBox1.SelectionLength = textBox1.Text.Length;
                    RNA.RNA_Sequence = textBox1.Text;
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text|*.txt|All|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                textBox2.Text = openFileDialog1.FileName;
                string Sequence = sr.ReadToEnd().ToString();
                Sequence = Sequence.ToUpper();
                Sequence = Sequence.Replace(System.Environment.NewLine, string.Empty);
                for (int i = 0; i < Sequence.Length; i++)
                {
                    if (char.IsNumber(Sequence[i]))
                    {
                        MessageBox.Show("Your Sequence containts Number");
                        textBox2.Text = "";
                        return;
                    }
                    if (Sequence[i] != 'A' && Sequence[i] != 'C' && Sequence[i] != 'G' && Sequence[i] != 'U')
                    {
                        MessageBox.Show("Enter A valid RNA Sequence");
                        textBox2.Text = "";
                        return;
                    }
                    
                }
                RNA.RNA_Sequence = Sequence;
            }
            if (RNA.RNA_Sequence == "")
            {
                MessageBox.Show("Enter A valid RNA Sequence");
                return;
            }
            string u = Search_Query(RNA.RNA_Sequence , "Score.txt" , 2 ,8);
            if (u != "")
                return;
        }

    }
}
