using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace Top_down_shooter
{
    public partial class Form1 : Form
    {
        Character player;
        List<Bullet> bullets = new List<Bullet>();
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed;
        double diagonalSpeed;
        double angle;
        int mouseX;
        int mouseY;

        GridItem[,] mapGrid;

        string basePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player = new Character(400, 300);
            speed = 10; 
            diagonalSpeed = speed / Math.Sqrt(2);
            int pathRemoveIndex = basePath.IndexOf("Top-down-shooter") + 17;
            basePath = basePath.Remove(pathRemoveIndex);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //deklarace gridu
            mapGrid = new GridItem[ActiveForm.Width / 10, ActiveForm.Height / 10];
            for (int i = 0; i < ActiveForm.Width / 10; i++)
            {
                for (int j = 0; j < ActiveForm.Height / 10; j++)
                {
                    mapGrid[i, j] = new GridItem(i * 10, j * 10, GridItem.Material.Air);
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (ActiveForm != null)
                pictureBox2.Size = new Size(ActiveForm.Width, ActiveForm.Height);

            // GUI positioning
            progressBar1.Location = new Point(ActiveForm.Width - progressBar1.Width - 30, progressBar1.Location.Y);
            label3.Location = new Point(progressBar1.Location.X - label3.Width - 30, label3.Location.Y);
            label2.Location = new Point(ActiveForm.Width / 3, label2.Location.Y);

            //rekalkulace gridu
            mapGrid = new GridItem[ActiveForm.Width / 10, ActiveForm.Height / 10];
            for (int i = 0; i < ActiveForm.Width / 10; i++)
            {
                for (int j = 0; j < ActiveForm.Height / 10; j++)
                {
                    mapGrid[i, j] = new GridItem(i * 10, j * 10, GridItem.Material.Air);
                }
            }
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

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            // Shoot a bullet
            Bullet bullet = new Bullet(player.centerX, player.centerY, angle);
            double rad = angle * Math.PI / 180;
            bullet.speedX = (int)(Math.Sin(rad) * bullet.speed);
            bullet.speedY = (int)-(Math.Cos(rad) * bullet.speed);
            bullets.Add(bullet);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            // Update mouse position
            mouseX = e.X;
            mouseY = e.Y;

            label4.Text = "GridItem: " + mapGrid[e.X / 10, e.Y / 10].ToString();  //e.X / 10, e.Y / 10
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Movement & player grid calculation
            if (left && player.X > 5)
            {
                for(int j = player.Y/10; j <= (player.Y + player.height)/10; j++)
                    mapGrid[(player.X + player.width)/10 - 2, j].material = GridItem.Material.Air;

                player.X -= speed;
            }
            if (right && player.X < ActiveForm.Width - player.width - 20)
            {
                for (int j = player.Y / 10; j <= (player.Y + player.height) / 10; j++)
                    mapGrid[player.X / 10 + 1, j].material = GridItem.Material.Air;

                player.X += speed;
            }
            if (up && player.Y > 60)
            {
                for (int i = player.X / 10; i <= (player.X + player.width) / 10; i++)
                    mapGrid[i, (player.Y + player.height)/10 - 2].material = GridItem.Material.Air;

                player.Y -= speed;
            }
            if (down && player.Y < ActiveForm.Height - player.height - 45)
            {
                for (int i = player.X / 10; i <= (player.X + player.width) / 10; i++)
                    mapGrid[i, player.Y / 10 + 2].material = GridItem.Material.Air;

                player.Y += speed;
            }
            if ((left && up) || (left && down) || (right && up) || (right && down))
                speed = (int)diagonalSpeed;
            else
                speed = 10;

            // Bullet movement
            foreach (Bullet b in bullets)
            {
                b.X += b.speedX;
                b.Y += b.speedY;
            }
            
            //generate new player grid
            for(int i = player.X/10 + 1; i < (player.X + player.width)/10 -2; i++)
            {
                for(int j = player.Y/10 + 2; j < (player.Y + player.height)/10 -1; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Player;
                }
            }

            // Aiming angle calculation
            player.centerX = player.X + player.width / 2;
            player.centerY = player.Y + player.height / 2;
            int diffX = player.centerX - mouseX;
            int diffY = Math.Abs(player.centerY - mouseY);
            double prepona = Math.Sqrt(diffY * diffY + diffX * diffX);
            if (diffX < 0)
                angle = Math.Acos(diffX / prepona);
            else
                angle = Math.Asin(diffY / prepona);
            angle *= 180 / Math.PI;
            if (player.centerY < mouseY)
                angle = 360 - angle;
            angle -= 90;

            pictureBox2.Invalidate();
        }

        private void game_graphics(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Bitmap playerMap = new Bitmap(basePath + "Assets/Textures/Hlava_5.png");
            Bitmap bulletMap = new Bitmap(basePath + "Assets/Textures/bullet.png");

            // Draw bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                if (b.X < ActiveForm.Width && b.Y < ActiveForm.Height && b.X > 0 && b.Y > 0)
                    graphics.DrawImage(bulletMap, b.X, b.Y);
                else
                    bullets.RemoveAt(i);
            }
                
            // Image rotation
            graphics.TranslateTransform(player.centerX, player.centerY);
            if (!double.IsNaN(angle)) graphics.RotateTransform(float.Parse(angle.ToString()));
            graphics.TranslateTransform(-player.centerX, -player.centerY);

            graphics.DrawImage(playerMap, player.X, player.Y);
        }
    }
}
