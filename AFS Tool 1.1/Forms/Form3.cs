using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NReco.VideoConverter;


namespace AFS_Tool_1._1
{
    public partial class Form3 : Form
    {
        private OpenFileDialog ofd = new OpenFileDialog();
        string outf;
        public Form3()
        {
            InitializeComponent();
        }


        private void Form3_Load(object sender, EventArgs e)
        {

        }
public void convert()
        {
            int index = 0;
            while (index >= 0 & index <= checked(this.listBox1.Items.Count - 1))
            {
                this.listBox1.SelectedIndex = index;
                if (this.listBox1.GetSelected(index))
                {
                    FFMpegConverter ffMpegConverter = new FFMpegConverter();
                    FFMpegInput[] inputs = new FFMpegInput[1]
                    {
            new FFMpegInput(listBox1.Text)
                    };
                    string output = listBox1.Text + outf;
                    ConvertSettings convertSettings1 = new ConvertSettings();
                    convertSettings1.CustomInputArgs = "-y -loglevel fatal -hide_banner -nostats";
                    ConvertSettings convertSettings2 = convertSettings1;
                    ffMpegConverter.ConvertMedia(inputs, output, (string)null, (OutputSettings)convertSettings2);
                    int num2 = (int)MessageBox.Show(listBox1.SelectedIndex.ToString() + "Converted With Sucess");
                }
                checked { ++index; }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        { 
            this.ofd.Multiselect = true;
            int num = (int)this.ofd.ShowDialog();
            this.listBox1.Items.AddRange((object[])this.ofd.FileNames);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outf = ".wav";
            convert();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outf = ".adx";
            convert();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }
    }
}
