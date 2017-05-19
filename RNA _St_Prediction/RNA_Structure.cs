using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace RNA__St_Prediction
{
    public class RNA_Structure
    {
        public string RNA_Sequence;
        public int Score;
        public int[,] Mat;
        public string Sequence;
        public RNA_Structure()
        {
            RNA_Sequence = "";
            Sequence = "";
            Score = 0;
        }

        public bool check(char amino_acid1, char amino_acid2)                 //Check BasePairing of RNA 
        {
            if (amino_acid1 == 'C' && amino_acid2 == 'G' || amino_acid1 == 'G' && amino_acid2 == 'C' || amino_acid1 == 'A' && amino_acid2 == 'U' || amino_acid1 == 'U' && amino_acid2 == 'A' || amino_acid1 == 'G' && amino_acid2 == 'U' || amino_acid1 == 'U' && amino_acid2 == 'G')
                return true;
            return false;
        }
        public int[,] Fill_Matrix(RNA_Structure R)         //Nussino Algorithm 
        {
            int k = 1, temp = 0;
            int flag = 0;
            Mat = new int[R.RNA_Sequence.Length , R.RNA_Sequence.Length];
            int Diagonals = 0;
            while (Diagonals != R.RNA_Sequence.Length - 1)
            {
                for (int i = temp; i < 1; i++)
                {
                    for (int j = k; j < R.RNA_Sequence.Length; j++)
                    {
                        List<int> list = new List<int>();
                        int Max = 0, sum = 0;
                        if ((j - i) > 2)
                        {
                            for (int x = i + 1; x < j - 1; x++)
                            {
                                sum = Mat[i, x] + Mat[x + 1, j];
                                list.Add(sum);
                            }
                            for (int x = 0; x < list.Count(); x++)
                            {
                                if (Max <= list[x])
                                    Max = list[x];
                            }
                            list.Clear();

                        }
                        char char1 = R.RNA_Sequence[i];
                        char char2 = R.RNA_Sequence[j];
                        if (check(char1, char2))
                            flag = 1;
                        else
                            flag = 0;
                        int val1 = Mat[i + 1, j], val2 = Mat[i, j - 1], val3 = Mat[i + 1, j - 1] + flag;
                        Mat[i, j] = Max_Val(val1, val2, val3, Max);
                        i += 1;
                    }
                    k += 1;
                }
                Diagonals += 1;
            }
            return R.Mat;
           
        }

        public static int Max_Val(int w, int x, int y, int z)
        {
            if (w >= x && w >= y && w >= z)
                return w;
            else if (x >= w && x >= y && x >= z)
                return x;
            else if (y >= w && y >= x && y >= z)
                return y;
            else  //(z >= w && z >= x && z >= y)
                return z;
        }
        public List<string> Get_seq(string Accession)
        {
            WebClient client = new WebClient();
            List<string> Sequence_Information = new List<string>();
            string RNA_DataBase = "nuccore";             //This will changes according to RNA Database
            string url = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db="
                    + RNA_DataBase + "&id=" + Accession + "&rettype=fasta&retmode=text";
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            string[] gene = s.Split('\n');
            s = "";
            for (int i = 1; i < gene.Length; i++)
                s += gene[i];
            s = s.Replace('T', 'U');
            string Sequence = s;
            string[] name = gene[0].Split(' ');
            string Name = name[1];
            for (int i = 2; i < name.Length; i++)
                Name += " " + name[i];
            Sequence_Information.Add(Sequence);
            Sequence_Information.Add(Name);
            return Sequence_Information;
        }
        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public string Modify_Structure(string Output, string RNA_Seq)
        {
            bool flag = false;
            if (RNA_Seq.Length % 2 != 0)
            {
                flag = true;
            }
            int Len = Output.Length * 2;
            int Mid = (Len - 1) / 2;
            string Output2 = "";
            int x = Output.Length - 1;

            for (int y = 0; y < Output.Length; y++)
            {
                char char1 = Output[x];
                if (Output[x] == ')')
                    Output2 += '(';
                else
                {
                    if (flag == true && Output.Length - y == 1)
                        break;
                    Output2 += '.';
                }
                x--;
            }
            return Output2 + Output;
        }
        public void RNAStructurePredictionTraceback(int[,] arr, ref string RNA, string Output, int i, int j)
        {
            if (i == j || i > j)
            {

                Output = Reverse(Output);
                string Final_Output = Modify_Structure(Output, RNA);
                Sequence = Final_Output;
                return;
            }
            else if (check(RNA[i], RNA[j]))
            {
                // 
                if (arr[i, j] == arr[i + 1, j - 1] + 1)
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += ")", i + 1, j - 1);
                else if (arr[i, j] == arr[i + 1, j])
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += ".", i + 1, j);
                else if (arr[i, j] == arr[i, j - 1])
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += ".", i, j - 1);
                else if ((j - i) > 2)
                {
                    int sum = 0, Max = 0, L = 0, M = 0, K = 0;
                    for (int x = i + 1; x <= j - 1; x++)
                    {
                        sum = arr[i, x] + arr[x + 1, j];
                        if (Max < sum)
                        {
                            Max = sum;
                            L = i;
                            M = j;
                            K = x;
                        }
                    }
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += "", L, K);
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += "", K + 1, M);
                }
            }
            else
            {
                if (arr[i, j] == arr[i + 1, j - 1] + 0)
                {
                    if (i + 1 == j - 1 || i + 1 > j - 1)
                        RNAStructurePredictionTraceback(arr, ref RNA, Output += "..", i + 1, j - 1);
                    else
                        RNAStructurePredictionTraceback(arr, ref RNA, Output += ".", i + 1, j - 1);
                }
                else if (arr[i, j] == arr[i + 1, j])
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += ".", i + 1, j);
                else if (arr[i, j] == arr[i, j - 1])
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += ".", i, j - 1);
                else if ((j - i) > 2)
                {
                    int sum = 0, Max = 0, L = 0, M = 0, K = 0;
                    for (int x = i + 1; x <= j - 1; x++)
                    {
                        sum = arr[i, x] + arr[x + 1, j];
                        if (Max < sum)
                        {
                            Max = sum;
                            L = i;
                            M = j;
                            K = x;
                        }
                    }
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += "", L, K);
                    RNAStructurePredictionTraceback(arr, ref RNA, Output += "", K + 1, M);
                }
            }
        }
        public string Get_IpAddress()
        {
            string PcName = Dns.GetHostName();
            string PC_info = PcName;
            return PcName;
        }
    }

}
