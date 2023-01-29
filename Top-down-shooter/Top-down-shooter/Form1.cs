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
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed;
        double diagonalSpeed;
        double angle = 0;
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
            
            //generate new player grid
            for(int i = player.X/10 + 1; i < (player.X + player.width)/10 -2; i++)
            {
                for(int j = player.Y/10 + 2; j < (player.Y + player.height)/10 -1; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Player;
                }
            }

            // Aiming
            int centerX = player.X + player.width / 2;
            int centerY = player.Y + player.height / 2;
            int diffX = centerX - mouseX;
            int diffY = Math.Abs(centerY - mouseY);
            double prepona = Math.Sqrt(diffY * diffY + diffX * diffX);
            if (diffX < 0)
                angle = Math.Acos(diffX / prepona);
            else
                angle = Math.Asin(diffY / prepona);
            angle *= 180 / Math.PI;
            angle = 180 - angle;
            if (centerY < mouseY)
                angle = 360 - angle;

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
            Bitmap playerMap = new Bitmap(@"D:\School\SPST\RPR\Projekt 2\Top-down-shooter\" + "Assets/Textures/Hlava_5.png");                  // zmenit

            int centerX = player.X + player.width / 2;
            int centerY = player.Y + player.width / 2;
            graphics.TranslateTransform(centerX, centerY);
            graphics.RotateTransform(-float.Parse(angle.ToString()) + 90);
            graphics.TranslateTransform(-centerX, -centerY);

            graphics.DrawImage(playerMap, player.X, player.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(ActiveForm != null)
                pictureBox2.Size = new Size(ActiveForm.Width, ActiveForm.Height);

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

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            label4.Text = "GridItem: " + mapGrid[e.X / 10, e.Y / 10].ToString();  //e.X / 10, e.Y / 10
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
    }
}
