using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DirectoryInfo arr;
        Random randomNumber = new Random();
        FileInfo[] fi;
        bool checkPicture = true;
        int indexMusic = 0;
        int timeMusic;
        string minutes, seconds, aPath;
        
      KrasimirTrapper.MP3Player mp = new KrasimirTrapper.MP3Player();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            aPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            SearchMusic(aPath);
   
        }

        private void SearchMusic(string path)
        {
            arr = new DirectoryInfo(path);
            fi = arr.GetFiles("*.mp3");
            //добавляем список файлов в listview
            foreach (FileInfo fc in fi) listBox1.Items.Add(fc.Name);
            label1.Text = "0:00";
            label2.Text = "0:00";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!mp.IsPaused)
            {   
                buttonPlayPause.BackgroundImage = ((Properties.Resources.PlayMusic));
                mp.Pause();
                timer1.Stop();
            }
            else
            {
                buttonPlayPause.BackgroundImage = ((Properties.Resources.Pause));
                mp.Play();
                timer1.Start();
                listBox1.SelectedIndex = indexMusic;
            }
            
        }

        private void Play(int index) 
        {
            listBox1.SelectedIndex = index;
            trackBar1.Value = 0;
            buttonPlayPause.BackgroundImage = ((Properties.Resources.Pause));
           mp.Open(Path.Combine(Application.StartupPath, arr.FullName + "\\" + fi[index])); //открываем трек

            trackBar1.Maximum = (int)(mp.AudioLength / 1000); //длина трека
            if (((int)mp.AudioLength / 1000) % 60 < 10) seconds = "0" + ((int)(mp.AudioLength / 1000 % 60)).ToString();
            else seconds = ((int)(mp.AudioLength / 1000 % 60)).ToString();
            label1.Text = ((int)(mp.AudioLength / 1000 / 60)).ToString() + ":" + seconds; //время
            timeMusic = (int)(mp.AudioLength / 1000);
            mp.Play();
            trackBar1.Maximum = timeMusic;
            timer1.Start();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.Play();
            indexMusic = listBox1.SelectedIndex;
            Play(indexMusic);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (checkPicture)
            {
                indexMusic += 1;
                if (indexMusic >= fi.Length)
                {
                    mp.Play();
                    indexMusic = 0;
                    Play(indexMusic);
                }
                else
                {
                    mp.Play();
                    label1.Text = indexMusic.ToString();
                    listBox1.SelectedIndex = indexMusic;
                    Play(indexMusic);
                }
            }
            else
            {
                indexMusic = randomNumber.Next(0, fi.Length - 1);
                Play(indexMusic);
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (indexMusic == 0)
            {
                listBox1.SelectedIndex = indexMusic;
                Play(indexMusic);
            }
            else
            {
                indexMusic -= 1;
                label1.Text = indexMusic.ToString();
                Play(indexMusic);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (((int)(mp.CurrentPosition / 1000) == 0) && (trackBar1.Value > 0))
            {
                mp.Stop();
                timer1.Stop();
                if (checkPicture) indexMusic += 1;
                else indexMusic = randomNumber.Next(0, fi.Length);
                Play(indexMusic);
            }
            else
            { 
                trackBar1.Value = (int)(mp.CurrentPosition / 1000);
                minutes = (((int)(mp.CurrentPosition / 1000)) / 60).ToString();
                if (((int)(mp.CurrentPosition / 1000)) % 60 < 10) seconds = "0" + (((int)(mp.CurrentPosition / 1000)) % 60).ToString();
                else { seconds = (((int)(mp.CurrentPosition / 1000)) % 60).ToString(); }
                label2.Text = minutes + ":" + seconds;
            }
        }

        private void buttonRand_Click(object sender, EventArgs e)
        {
            if (!checkPicture)
            {
                buttonRand.BackgroundImage = ((Properties.Resources.ShuffleOff));
                checkPicture = true;
            }
            else { buttonRand.BackgroundImage = ((Properties.Resources.Shuffle)); checkPicture = false; }


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mp.Seek((ulong)(trackBar1.Value * 1000));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.Description = "Выберите папку, \n" + "в которой находится музыка";
            fb.ShowNewFolderButton = false;
            fb.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (fb.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                aPath = fb.SelectedPath;
                SearchMusic(aPath);
            }
        }
    }

   
}

