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

namespace tictactone3x3
{
    public partial class Form1 : Form
    {
        
        //базовые постоянные, отступы, пазмер клеток
        public static float xstart = 50, ystart = 50, step = 50;
        //"поля" с отметками ходов
        public static int[,] littleMap = new int [3, 3];

        public static int[,] winComb = new int[5, 5];
        //переменная определяет чей сейчас ход: true - крестики, false - нолики
        bool turn = true;


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
            for (int i = 0; i < 3; i++)
            {
                for ( int j = 0; j < 3; j++)
                {
                    if (littleMap[i,j]==1)
                    {
                        g.DrawLine(new Pen(Color.Black, 2f), xstart + 5 + i * step, ystart + 5 + j * step, xstart - 5 + (i + 1) * step, ystart + 5 + j * step + 40);
                        g.DrawLine(new Pen(Color.Black, 2f), xstart + 5 + i * step, ystart + 5 + j * step + 40, xstart + 5 + i * step +40, ystart + 5 + j * step );
                        
                    }
                    if (littleMap[i, j] == 2)
                    {
                        g.DrawEllipse(new Pen(Color.Red, 2f), xstart + 5 + i * step, ystart + 5 + j * step, 40, 40);
                        
                    }
                }
            }


            
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if ((e.X > xstart + i * step) && (e.X < xstart + (i + 1) * step) && (e.Y > ystart + j * step) 
                        && (e.Y < ystart + (j + 1) * step))
                    {
                        if ((littleMap[i,j]!=1)&& (littleMap[i, j] != 2))
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
                    if (checkWin(1))
                    {
                        MessageBox.Show("Победа 1 игрока");
                    
                    }
                    if (checkWin(2)) MessageBox.Show("Победа 2 игрока");
                    
                    Invalidate();
                }
                
            }
        }
        
        private static bool checkWin(int c)
        {
            for (int i = 0; i < 3; i++) 
            {
                if (checkLine(0, i, 1, 0, c)) return true;   // проверим линию по y
                if(checkLine(i, 0, 0, 1, c)) return true;   // проверим линию по х 
            }
            if (checkLine(0, 0, 1, 1,  c)) return true;  // проверим по диагонали х -у 
            if (checkLine(0, 2, 1, -1,  c)) return true;  // проверим по диагонали х -у 
            return false;
        }
        
        // проверка линии
        private static bool checkLine(int x, int y, int vx, int vy, int c)
        {
            for (int i = 0; i < 3; i++) if (littleMap[(y + i * vy),(x + i * vx)] != c) return false;   // проверим одинаковые-ли символы в ячейка
            return true;
        }
    }
}