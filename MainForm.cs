﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mozgovoi1_10
{
    public partial class MainForm : Form
    {
        private Graphics canvas;
        private ITurtle turtle;

        public MainForm()
        {
            InitializeComponent();
            canvas = canvasPanel.CreateGraphics();
            turtle = new SmoothTurtle(canvas, 200, 200);

            DrawPolygon(3, 100);
        }

        public void DrawPolygon(int anglesCount, int sideLength)
        {
            turtle.PenDown();
            for (int i = 0; i < anglesCount; i++)
            {
                turtle.Go(sideLength);
                turtle.TurnRight(2 * Math.PI / anglesCount);
            }
        }
    }
}