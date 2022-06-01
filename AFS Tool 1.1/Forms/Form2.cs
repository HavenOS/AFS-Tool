using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFS_Tool_1._1
{
    public partial class Form2 : Form
    {
        private string audio;
        private string bitrate;
        private string videoC;
        private string pixelformat;
        private string Neigh = " -sws_flags neighbor";
        private string outf;
        private string DARs;
        string haveaudio = "no";
        private OpenFileDialog ofd = new OpenFileDialog();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           comboBox1.SelectedIndex = 0;
           comboBox2.SelectedIndex = 0;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                haveaudio = "yes";
            }
            if (checkBox1.Checked == false)
            {
                haveaudio = "no";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string audioCodecs = "";
            DARs = " -aspect " + textBox5.Text;
            float num1 = float.Parse(textBox4.Text);
            if (comboBox2.SelectedIndex == 0)
            {
                audioCodecs = "mp2";
                videoC = " mpeg1video";
                outf = ".mpg";
            }
            if (comboBox2.SelectedIndex == 1)
            {
                videoC = " ffv1";
                audioCodecs = "copy";
                pixelformat = " -pix_fmt gbrp10le";
                outf = ".avi";
            }
            if (checkBox1.Checked == true)
            {
                audio = " -map 0 -brand mp2";

            }
            else
            {
                audio = " -an -y";
            }
               
               
            if (textBox2.Text.Length > 0)
            {
                bitrate = " -b " + textBox2.Text + "k";
            }
                
            int index = 0;
            while (index >= 0 & index <= checked(this.listBox1.Items.Count - 1))
            {
              
                this.listBox1.SelectedIndex = index;
                if (this.listBox1.GetSelected(index))
                {
                    string frt = " -r " + comboBox1.Text;
                    FFMpegConverter ffMpegConverter = new FFMpegConverter();
                    FFMpegInput[] inputs = new FFMpegInput[1]
                    {
            new FFMpegInput(this.listBox1.Text)
                    };
                    string output = this.listBox1.Text + this.outf;
                    ConvertSettings convertSettings1 = new ConvertSettings();
                    if (checkBox1.Checked == true)
                    {
                        convertSettings1.AudioCodec = audioCodecs;
                        }
                    convertSettings1.MaxDuration = num1;
                    convertSettings1.VideoFrameSize = textBox3.Text;
                    convertSettings1.VideoCodec = this.videoC;
                    convertSettings1.CustomOutputArgs = frt + bitrate + DARs + pixelformat + audio + Neigh;
                    ConvertSettings convertSettings2 = convertSettings1;
                    ffMpegConverter.ConvertMedia(inputs, output, (string)null, (OutputSettings)convertSettings2);
                    int num2 = (int)MessageBox.Show(listBox1.SelectedIndex.ToString() + "Converted With Sucess");
                }
                checked { ++index; }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ofd.Multiselect = true;
            int num = (int)this.ofd.ShowDialog();
            this.listBox1.Items.AddRange((object[])this.ofd.FileNames);
        }
    }
}
