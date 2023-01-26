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

        string basePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            player = new Player(500, 400);
            int pathRemoveIndex = basePath.IndexOf("Top-down-shooter") + 17;
            basePath = basePath.Remove(pathRemoveIndex);
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
            Bitmap playerMap = new Bitmap(basePath + "Assets/Textures/Hlava2.png");
            graphics.DrawImage(playerMap, player.X, player.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox2.Size = new Size(ActiveForm.Width, ActiveForm.Height);
        }
    }
}
