using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace Top_down_shooter
{
    public partial class Form1 : Form
    {
        Player player;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed = 20;
        float angle = 0;

        string basePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            player = new Player(400, 300);
            int pathRemoveIndex = basePath.IndexOf("Top-down-shooter") + 17;
            basePath = basePath.Remove(pathRemoveIndex);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Movement
            if (left && player.X > 0)
                player.X -= speed;
            if (right && player.X < ActiveForm.Width - player.width)
                player.X += speed;
            if (up && player.Y > 75)
                player.Y -= speed;
            if (down && player.Y < ActiveForm.Height - player.height)
                player.Y += speed;
            if ((left && up) || (left && down) || (right && up) || (right && down))
                speed = 10;
            else
                speed = 20;

            /* Aiming
            int diffX = Math.Abs(player.X - MousePosition.X);
            int diffY = Math.Abs(player.Y - MousePosition.Y);
            */

            pictureBox2.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: left = true; break;
                case Keys.D: right = true; break;
                case Keys.W: up = true; break;
                case Keys.S: down = true; break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: left = false; break;
                case Keys.D: right = false; break;
                case Keys.W: up = false; break;
                case Keys.S: down = false; break;
            }
        }

        private void game_graphics(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Bitmap playerMap = new Bitmap(basePath + "Assets/Textures/hlavatest.png");

            //graphics.RotateTransform(angle, MatrixOrder.Append);

            graphics.DrawImage(playerMap, player.X, player.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox2.Size = new Size(ActiveForm.Width, ActiveForm.Height);
        }
    }
}
