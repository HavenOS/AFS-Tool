using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AFS_Tool_1._1
{
    //preciso pasar o novo tamanho do arquivo quando importar


    public partial class Form1 : Form
    {
        bool row;
        private double lastoff = 0;

        private OpenFileDialog ofd = new OpenFileDialog();
        private OpenFileDialog ofdimport = new OpenFileDialog();
        private int ParseOffset;
        string parsename;
        int valuetotal;
        private int parseyear;
        private int parseoriginalsize;
        private int getlastoffset;
        private int getlastlenght;
        private int ParseLenght;
        private int Offsets;
        int fileinfo;
        public Form1()
        {
            InitializeComponent();
        }
        public static uint Swap(uint val)
        {
            val = val >> 16 | val << 16;
            val = (val & 4278255360U) >> 8 | (uint)(((int)val & 16711935) << 8);
            return val;
        }
        private static int ReverseBytes(int val) => (int)BitConverter.ToInt16(BitConverter.GetBytes(val), 0);

        private void ParsetoData()
        {
            string str = Convert.ToString(this.ParseOffset);
            List<int> intList = new List<int>();
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            foreach (int num in str)
            {
                dataGridViewRow.CreateCells(this.dataGridView1);
                dataGridViewRow.Cells[0].Value = (object)this.ParseOffset;
                dataGridViewRow.Cells[1].Value = (object)this.ParseLenght;
            }
            this.dataGridView1.Rows.Add(dataGridViewRow);
            foreach (DataGridViewRow row in (IEnumerable)this.dataGridView1.Rows)
            {
                if (row.Cells["Column2"].Value.ToString() == "0")
                    this.dataGridView1.Rows.Remove(dataGridViewRow);
            }
        }


        public void Read(string filename)
        {
            if (dataGridView1.Rows.Count != 0 && dataGridView1.Rows != null)
            {
                dataGridView1.Rows.Clear();
            }
                

                int num1 = 0;
            BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(filename));
            int num2 = 0;
            if (num2 <= 4)
            {
                binaryReader.BaseStream.Position = (long)num2;
                num1 = (int)BitConverter.ToInt16(binaryReader.ReadBytes(4), 0);
                int num3 = num2 + 1;
            }
            if (num1.ToString() == "17985")
            {
               for (int i = 4; i <= 4; i++)
                {
                    binaryReader.BaseStream.Position = i;
                    valuetotal = binaryReader.ReadByte();
                    label1.Text = valuetotal.ToString();
                }
                for (int index = 8; index <= 175; index += 8)
                {
                    int count1 = 4;
                    int count2 = 4;
                    binaryReader.BaseStream.Position = (long)index;
                    this.Offsets = BitConverter.ToInt32(binaryReader.ReadBytes(count1), 0);
                    int int32_1 = Convert.ToInt32(index + 4);
                    binaryReader.BaseStream.Seek((long)int32_1, SeekOrigin.Begin);
                    byte[] numArray = binaryReader.ReadBytes(count2);
                    this.ParseOffset = this.Offsets;
                    int int32_2 = BitConverter.ToInt32(numArray, 0);
                    this.ParseLenght = int32_2;
                    if (int32_2.ToString().Length < 4)
                    {
                        this.ParseLenght = int32_2;
                    }
                    lastoff = lastoff + ParseLenght;
                    ParsetoData();
                }
                
            }
            else
            {
                int num4 = (int)MessageBox.Show("This is not AFS File");
            }
            binaryReader.Close();
           
            
            

        }


        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        private static Stack<int> GetStack()
        {
            Stack<int> intStack = new Stack<int>();
            intStack.Push(100);
            intStack.Push(1000);
            intStack.Push(10000);
            return intStack;
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_DragDrop(System.Object sender, System.Windows.Forms.DragEventArgs e)
        {

        }
        private void Form1_DragEnter(System.Object sender, System.Windows.Forms.DragEventArgs e)
        {

        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open afs file
            if (dataGridView1.DataSource == null)
                dataGridView1.Rows.Clear();
            ofd.Filter = "AFS Files|*.afs";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Read(ofd.FileName);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
 
            string s1 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column1"].Value.ToString();
            string s2 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column2"].Value.ToString();
            int num1 = int.Parse(s1);
            int count = int.Parse(s2);
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column3")
            {
                saveFileDialog1.Filter = "SFD|*.sfd|MPG|*.mpg|ADX|*.adx";
                saveFileDialog1.FileName = num1.ToString();
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    exportfile(num1, count);
                }
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column4")
            {
                importfile(num1, count);
            }


        }
        //export the file
        private void exportfile(int offset, int lenghts)
        {
            BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(ofd.FileName));
            long length = new FileInfo(ofd.FileName).Length;
            int num2 = offset;
            if ((long)num2 <= length)
            {
                string fileName = saveFileDialog1.FileName;
                binaryReader.BaseStream.Position = (long)num2;
                byte[] bytes = binaryReader.ReadBytes(lenghts);
                File.WriteAllBytes(fileName, bytes);

                int num3 = num2 + 1;
            }
            binaryReader.Close();
        }
        //made backup of the main file
        private void backup()
        {
            BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(ofd.FileName));
            long length = new FileInfo(ofd.FileName).Length;
            string fileName = ofd.FileName + ".bk";
            binaryReader.BaseStream.Position = 0;
            byte[] bytes = binaryReader.ReadBytes((int)length);
            File.WriteAllBytes(fileName, bytes);
            binaryReader.Close();


        }

        // to import files
        private void importfile(int offset, int lenght)
        {
            backup();
            string s1 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column1"].Value.ToString();
            string s2 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column2"].Value.ToString();
            lenght = int.Parse(s2);
            BinaryWriter binaryWriter = new BinaryWriter((Stream)File.OpenWrite(ofd.FileName));
            this.ofdimport.Filter = "Exported|*.exported|SFD|*.sfd|MPG|*.mpg|ADX|*.adx";
            this.ofdimport.Title = "Choose a file to import";
            this.ofdimport.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (this.ofdimport.ShowDialog() == DialogResult.OK)
            {
                BinaryReader binaryReader1 = new BinaryReader((Stream)File.OpenRead(this.ofdimport.FileName));
                long num4 = new FileInfo(ofd.FileName).Length;

                int num2 = offset;
                if ((long)num2 <= num4)
                {
                    binaryWriter.BaseStream.Position = (long)num2;
                    byte[] buffer = binaryReader1.ReadBytes(lenght);
                    binaryWriter.Write(buffer);
                    int num3 = num2 + 1;

                }

                binaryWriter.Close();
                binaryReader1.Close();
                updatefile(lenght, ofdimport.FileName);
                Read(ofd.FileName);
                rebuild();

            }

        }

        //update the file when import new one
        private void updatefile(int originalsize, string filename)
        {
            var length = new System.IO.FileInfo(filename).Length;
            int valOriginal = originalsize;
            int valSubstitute = Convert.ToInt32(length);
            ReverseBytes(valSubstitute);

            int valLength = BitConverter.GetBytes(valOriginal).Length;

            using (var reader = new BinaryReader(
                File.Open(ofd.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                int position = 0;
                while (position < 2048)
                {
                    reader.BaseStream.Position = position;
                    if (reader.ReadInt32() == valOriginal)
                    {
                        using (var writer = new BinaryWriter(
                            File.Open(ofd.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
                        {
                            writer.BaseStream.Position = position;
                            writer.Write(valSubstitute);
                            writer.Close();
                        };

                        break;
                    }
                    position += 1;
                }
                reader.Close();
            };
            using (var reader = new BinaryReader(
    File.Open(ofd.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                int position = getlastoffset;
                while (position <= getlastlenght)
                {
                    reader.BaseStream.Position = position;
                    if (reader.ReadInt32() == valOriginal)
                    {
                        using (var writer = new BinaryWriter(
                            File.Open(ofd.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
                        {
                            writer.BaseStream.Position = position;
                            writer.Write(valSubstitute);
                            writer.Close();
                        };

                        break;
                    }
                    position += 1;
                }
                reader.Close();
            };

        }

        private void convertToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new instance of the AboutBox class
            AboutBox1 settingsForm = new AboutBox1();

            // Show the settings form
            settingsForm.Show();
        }

        private void legendsOfModdingToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Form2 class
            Form2 settingsForm = new Form2();

            //open video converter window
            settingsForm.Show();
        }

        private void audioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new instance of the audio converter window
            Form3 settingsForm = new Form3();

            //open audio converter window
            settingsForm.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //rebuild afs files
public void rebuild()
        {
            byte[] bytes = { };
            byte[] header = { };
            string rebuildname = ofd.FileName + ".new";
            double count = 0;


                int num1 = 2048;
                

                BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(ofd.FileName));
                long length = new FileInfo(ofd.FileName).Length;
            if (0 <= ofd.FileName.Length) 
            {
                int num2 = num1;
                if (0 <= 2048)
                {
                    binaryReader.BaseStream.Position = 0;
                    long numBytes = 2048;
                    header = binaryReader.ReadBytes((int)numBytes);


                }
                if ((long)num2 <= length)
                {
                    binaryReader.BaseStream.Position = num1;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string s2 = row.Cells["Column2"].Value.ToString();
                        count = count + int.Parse(s2) + 2048;

                    }
                    bytes = binaryReader.ReadBytes(Convert.ToInt32(count + count));
                }
            }
                binaryReader.Close();

                using (FileStream fs = new FileStream(rebuildname, FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(header, 0, header.Length);
                    bw.Write(bytes, 0, bytes.Length);
                    bw.Close();
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                using (FileStream fs = new FileStream(rebuildname, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(header, 0, header.Length);
                        bw.Write(bytes, 0, bytes.Length);
                        bw.Close();
                    }
                }
                string s3 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column1"].Value.ToString();
                int oldoffset = int.Parse(s3);
                string s4 = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["Column2"].Value.ToString();
                int newoffset = int.Parse(s4);
                var length2 = new System.IO.FileInfo(rebuildname).Length;
                int valSubstitute = newoffset + 2048;
                

                using (var reader = new BinaryReader(
                    File.Open(rebuildname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    int position = 0;
                    while (position <= 2048)
                    {
                        reader.BaseStream.Position = position;
                        if (reader.ReadInt32() == oldoffset)
                        {
                            using (var writer = new BinaryWriter(
                                File.Open(rebuildname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
                            {
                                writer.BaseStream.Position = position;
                                writer.Write(int.Parse(valSubstitute.ToString()));
                                writer.Close();
                            };

                            
                        }
                        position ++;
                    }
                    reader.Close();

                };
            }
            MessageBox.Show("New file generated was " + rebuildname);
            Read(rebuildname);

        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    //batch export 


                    string s1 = row.Cells["Column1"].Value.ToString();
                    string s2 = row.Cells["Column2"].Value.ToString();
                    int num1 = int.Parse(s1);
                    int count = int.Parse(s2);
                    string exportname = folderBrowserDialog1.SelectedPath + @"\" + num1.ToString() + ".exported";

                    BinaryReader binaryReader = new BinaryReader((Stream)File.OpenRead(ofd.FileName));
                    long length = new FileInfo(ofd.FileName).Length;
                    int num2 = num1;
                    if ((long)num2 <= length)
                    {
                        binaryReader.BaseStream.Position = (long)num2;
                        byte[] bytes = binaryReader.ReadBytes(count);
                        File.WriteAllBytes(exportname, bytes);

                        int num3 = num2 + 1;
                    }
                    binaryReader.Close();




                }
                MessageBox.Show("All files exported to" + folderBrowserDialog1.SelectedPath);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //code to import files
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath + @"\";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (string fileName1 in Directory.GetFiles(path))
                    {
                        string s1 = row.Cells["Column1"].Value.ToString();
                        string s2 = row.Cells["Column2"].Value.ToString();
                        int num1 = int.Parse(s1);
                        int count = int.Parse(s2);
                        BinaryWriter binaryWriter = new BinaryWriter((Stream)File.OpenWrite(ofd.FileName));
                        BinaryReader binaryReader1 = new BinaryReader((Stream)File.OpenRead(fileName1));
                        long num4 = new FileInfo(ofd.FileName).Length;

                        int num2 = num1;
                        if (num1 <= num4)
                        {
                            binaryWriter.BaseStream.Position = num1;
                            byte[] buffer = binaryReader1.ReadBytes(count);
                            binaryWriter.Write(buffer);
                            num1++;
                        }

                        binaryWriter.Close();
                        binaryReader1.Close();
                        
                    }
                }
                MessageBox.Show("All files imported");


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://moddinghubbrs.forumotion.com/t3-afs-tool-1-1#3");
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
    
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            rebuild();
        }
    }
}
