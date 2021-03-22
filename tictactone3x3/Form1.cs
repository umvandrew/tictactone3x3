using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.MessageBox;

namespace tictactone3x3
{
    public partial class Form1 : Form
    {
        
        //базовые постоянные, отступы, пазмер клеток
        public static float xstart = 50, ystart = 50, step = 50;
        //"поля" с отметками ходов
        public static int[,] littleMap = new int [3, 3];

        public static int[,] winComb = new int[3, 2]; //начальная и конечная точка выйгрышной комбинации 

        public static int[] countWin = new int[2] {0,0};
        //переменная определяет чей сейчас ход: true - крестики, false - нолики
        public bool turn = true;
        public bool isPlaying = false;

        public static int winx, winy, winvx, winvy;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            //рисуем поле 
            for (int i = 50; i <= 200; i+=50)
            {
                g.DrawLine(new Pen(Color.Black, 2f), i, xstart, i, 200);
                g.DrawLine(new Pen(Color.Black, 2f), 200, i, ystart, i);
            }
            
            //рисуем крестик/нолик, если клик мышкой был
            if (isPlaying)
            {
                for (int i = 0; i < 3; i++)
                {
                    for ( int j = 0; j < 3; j++)
                    {
                        if (littleMap[i,j]==1)
                        {
                            g.DrawLine(new Pen(Color.Black, 2f), xstart + 5 + i * step, ystart + 5 + j * step,
                                xstart - 5 + (i + 1) * step, ystart + 5 + j * step + 40);
                            g.DrawLine(new Pen(Color.Black, 2f), xstart + 5 + i * step, ystart + 5 + j * step + 40,
                                xstart + 5 + i * step + 40, ystart + 5 + j * step);

                        }
                        if (littleMap[i, j] == 2)
                        {
                            g.DrawEllipse(new Pen(Color.Red, 2f), xstart + 5 + i * step, ystart + 5 + j * step, 40, 40);
                        
                        }
                    }
                }
            }
            
            //проверка на победу 
            for (int c = 1; c <= 2 ; c++)
            {
                if (CheckWin(c))
                {
                    g.DrawLine(new Pen(Color.Orange, 5f), xstart + 25 + (winComb[0, 0] * step), ystart + 25 + (winComb[0, 1] * step), 
                        xstart + 25 + (winComb[2, 0] * step), ystart + 25 + (winComb[2, 1] * step ));
                    // g.DrawLine(new Pen(Color.Aqua, 3f), xstart + 25 + (winy + winvy) * step, ystart + 25 + (winx + winvx) * step, 
                    //   xstart + 25 + (winy + 2 * winvy) * step, ystart + 25 + (winx + 2*winvx) * step);

                    ClearMap();
                    
                    countWin[c-1] += 1;
                    MessageBox.Show("Победа "+c+" игрока") ;
                    if (c == 1) textBox1.Text = (countWin[0]).ToString();
                    if (c == 2) textBox2.Text = (countWin[1]).ToString();

                }
            }

            //ничья
            if (CheckMap())
            {
                MessageBox.Show("Нет победителя");
                ClearMap();
            }
        }

        //проверка победы 
        private static bool CheckWin(int c)
        {
            for (var i = 0; i < 3; i++) 
            {
                if (CheckLine(0, i, 1, 0, c)) // проверим линию по y
                {
                    winx = 0;
                    winy = i;
                    winvx = 1;
                    winvy = 0;
                    return true;   
                }
                if(CheckLine(i, 0, 0, 1, c)) // проверим линию по х 
                {
                    winx = i;
                    winy = 0;
                    winvx = 0;
                    winvy = 1;
                    return true;   
                }
            }
            if (CheckLine(0, 0, 1, 1,  c)) // проверим по диагонали х -у 
            {
                winx = 0;
                winy = 0;
                winvx = 1;
                winvy = 1;
                return true;   
            }
            if (CheckLine(0, 2, 1, -1,  c)) // проверим по диагонали х -у 
            {
                winx = 0;
                winy = 2;
                winvx = 1;
                winvy = -1;
                return true;   
            }
            return false;
        }
        
        // проверка линии
        private static bool CheckLine(int x, int y, int vx, int vy, int c)
        {
            for (var i = 0; i < 3; i++)
            {
                if (littleMap[(y + i * vy), (x + i * vx)] == c)
                {
                    winComb[i, 0] = (y + i * vy);
                    winComb[i, 1] = (x + i * vx);
                }

                if (littleMap[(y + i * vy), (x + i * vx)] != c) return false; // проверим одинаковые-ли символы в ячейки
            }
            return true;
        }
        
        //проверка на пустые клетки(заполненность поля)
        private static bool CheckMap()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (littleMap[i, j] == 0) return false;
                }
            }

            return true;
        }

        //Очистить поле, подготовиться к следующей игре 
        private void ClearMap()
        {
             
            for (var i = 0; i < 3; i++) 
            {
                for (var j = 0; j < 3; j++)
                {
                    littleMap[i, j] = 0;
                }
            }
            
            isPlaying = false;
            turn = true;
            button1.Text = "Играсть снова";
            button1.BackColor = Color.Gold;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isPlaying)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        if ((e.X > xstart + i * step)
                            && (e.X < xstart + (i + 1) * step)
                            && (e.Y > ystart + j * step)
                            && (e.Y < ystart + (j + 1) * step))
                        {
                            if ((littleMap[i, j] != 1) && (littleMap[i, j] != 2))
                            {
                                if (turn)
                                {
                                    littleMap[i, j] = 1;
                                    turn = false;
                                }
                                else if (turn == false)
                                {
                                    littleMap[i, j] = 2;
                                    turn = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Клетка занята, выберите другую!");
                            }
                        }

                        Invalidate();
                    }
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isPlaying = true;
            button1.Text = "Идет игра";
            button1.BackColor = Color.Chartreuse;
            Invalidate();
        }
    }
}