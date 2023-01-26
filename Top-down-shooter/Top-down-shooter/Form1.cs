using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top_down_shooter
{
    public partial class Form1 : Form
    {
        Player player;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed = 10;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            player = new Player(500, 400);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (left)
                player.X -= speed;
            if (right)
                player.X += speed;
            if (up)
                player.Y -= speed;
            if (down)
                player.Y += speed;
            if ((left && up) || (left && down) || (right && up) || (right && down))
                speed = 5;
            else
                speed = 10;
            pictureBox1.Location = new Point(player.X, player.Y);
            pictureBox2.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                left = true;
            if (e.KeyCode == Keys.D)
                right = true;
            if (e.KeyCode == Keys.W)
                up = true;
            if (e.KeyCode == Keys.S)
                down = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                left = false;
            if (e.KeyCode == Keys.D)
                right = false;
            if (e.KeyCode == Keys.W)
                up = false;
            if (e.KeyCode == Keys.S)
                down = false;
        }

        private void game_graphics(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.Black);
            SolidBrush player_brush = new SolidBrush(Color.Red);
            graphics.FillRectangle(player_brush, player.X, player.Y, 50, 50);
            graphics.FillEllipse(new SolidBrush(Color.Yellow), player.X + 5, player.Y + 10, 10, 10);
            graphics.FillEllipse(new SolidBrush(Color.Yellow), player.X + 50 - 20, player.Y + 10, 10, 10);
        }
    }
}
