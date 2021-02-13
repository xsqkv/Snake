using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        void Update(Bitmap map,Color clr)
        {
            Graphics graphics = null;
            try
            {
                graphics = Graphics.FromImage(map);
            }
            catch { }
            graphics.Clear(clr);

        }
        void FillColor(Bitmap bit, Color color)
        
        {
            for (int l = 0; l < bit.Width; l++)
            {
                for (int i = 0; i < bit.Height; i++)
                {
                    bit.SetPixel(i, l, color);
                }
            }
        }
        void Game_Over()
        {
            MessageBox.Show($"Вы проиграли                  \n                            Ваш Счёт: {och} ", "Проигрыш!", MessageBoxButtons.OK);
            foreach (var s in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                s.Kill();
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(delegate () { Play(); });
            thread.Start();
            Thread Th = new Thread(delegate () { Contr(); });
            Th.Start();
        }
        string but = "";
        short och = 0;
        void Play()
        {
            Random random = new Random();
            int width = 300;
            int height = 300;
            Bitmap bitmap = new Bitmap(width, height);
            int EatX = Convert.ToInt32(random.Next(1, bitmap.Height/10).ToString()+ "0");
            int EatY = Convert.ToInt32(random.Next(1, bitmap.Width/10).ToString()+ "0");
            int PlayerX = height/2;
            int PlayerY = width;
            List<string> list = new List<string>();
            list.Add($"{PlayerX};{PlayerY}");
            list.Add($"{PlayerX};{PlayerY+1}");
            list.Add($"{PlayerX};{PlayerY + 2}");
            FillColor(bitmap, Color.FromArgb(100, 0, 0, 0));
            short axisX = 0;
            short axisY = 0;
            but = "W";
            for (; ; )
            {
                Bitmap Set(int x, int y, Bitmap bit, Color clr, int size)
                {
                    x /= size;
                    y /= size;
                    x *= size;
                    y *= size;
                    if (x > size || y > size)
                    {
                        for (int k = x - size; k < x; k++)
                        {
                            for (int m = y - size; m < y; m++)
                            {
                                try { bit.SetPixel(k, m, clr); } catch { }
                            }
                        }
                    }
                    else
                    {
                        for (int k = x - size; k < x; k++)
                        {
                            for (int m = y - size; m < y; m++)
                            {
                                bit.SetPixel(k, m, clr);
                            }
                        }
                    }

                    return bit;
                }
                void Eat()
                {
                    och++;
                    list.Add($"{list[list.Count - 1].Split(';')[0]};{list[list.Count - 1].Split(';')[1]}");
                    EatX = Convert.ToInt32(random.Next(1, bitmap.Height / 10).ToString() + "0");
                    EatY = Convert.ToInt32(random.Next(1, bitmap.Width / 10).ToString() + "0");
                }
                switch (but)
                {
                    case "W":
                        PlayerY -= 10;
                        axisY++;
                        axisX = 0;
                        break;
                    case "A":
                        PlayerX -= 10;
                        axisX++;
                        axisY = 0;
                        break;
                    case "S":
                        PlayerY += 10;
                        axisY++;
                        axisX = 0;
                        break;
                    case "D":
                        PlayerX += 10;
                        axisX++;
                        axisY = 0;
                        break;
                }
                list.Remove(list[0]);
                list.Add($"{PlayerX};{PlayerY}");
                Update(bitmap, Color.FromArgb(100, 0, 0, 0));
                //Яблоко
                Set(EatX, EatY, bitmap, Color.FromArgb(255, 255, 100, 100), 10);
                //Змейка
                int s = 0;
                foreach (var u in list)
                {
                    if (s < 150)
                    {
                        s += 3;
                    }
                    Set(Convert.ToInt32(u.Split(';')[0]), Convert.ToInt32(u.Split(';')[1]), bitmap, Color.FromArgb(255, 0, 100 + s, 0), 10);
                }
                if (PlayerX > height || PlayerY > width || PlayerX < 10 || PlayerY < 10)
                {
                    Game_Over();
                }
                //boom
                for (int i = 0; i < list.Count - 2; i++)
                {
                    if (list[i] == list[list.Count - 1])
                    {
                        Game_Over();
                    }
                }
                //Display
                pictureBox1.Image = bitmap;
                if (PlayerX > height || PlayerY > width || PlayerX < 10 || PlayerY < 10)
                {
                    Game_Over();
                }
                //Eat
                if (PlayerX == EatX && PlayerY == EatY)
                {
                    Eat();
                }
                try { this.Invoke((MethodInvoker)delegate () { label3.Text = $"Очки: {och}"; }); } catch { }
                Thread.Sleep(130);
            }
        }
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        void Contr()
        {
            for (; ; )
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(1);
                    for (Int32 i = 0; i < 255; i++)
                    {

                        int keys_ = GetAsyncKeyState(i);
                        if (keys_ == 1 || keys_ == -32767)
                        {
                            if ((Keys)i == Keys.W)
                            {
                                if (but == "S")
                                {

                                }
                                else
                                {
                                    but = "W";
                                }
                            }
                            if ((Keys)i == Keys.A)
                            {
                                if (but == "D")
                                {

                                }
                                else
                                {
                                    but = "A";
                                }
                            }
                            if ((Keys)i == Keys.S)
                            {
                                if (but == "W")
                                {

                                }
                                else
                                {
                                    but = "S";
                                }
                            }
                            if ((Keys)i == Keys.D)
                            {
                                if (but == "A")
                                {

                                }
                                else
                                {
                                    but = "D";
                                }
                            }
                            Thread.Sleep(100);
                        }
                    }
                }
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            foreach (var s in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                s.Kill();
            }
        }
    }   
}
