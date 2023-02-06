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
using System.Media;

namespace Top_down_shooter
{
    public partial class Form1 : Form
    {
        Character player;
        List<Bullet> bullets = new List<Bullet>();
        List<AmmoBox> ammoBoxes = new List<AmmoBox>();
        List<Label> labels = new List<Label>();
        List<int> ticks = new List<int>();
        Random rng = new Random();
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed;
        double diagonalSpeed;
        double angle;
        int mouseX;
        int mouseY;

        int killCount = 0;
        int ammoCount = 100;

        public static GridItem[,] mapGrid;
        public static List<Character> enemies = new List<Character>();

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
            this.Cursor = ActuallyLoadCursor(basePath + @"Assets\Textures\Cursor.cur");

            
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
            for(int i = 0; i < 3; i++)
                SpawnEnemy();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (ActiveForm == null) return;

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
            foreach(Character c in enemies)
            {
                ValidateCharGrid(c);
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
            if (ammoCount > 0)
            {
                ammoCount--;
                // Shoot a bullet
                double bulletAngle = angle + rng.Next(-2, 3);
                double rad = bulletAngle * Math.PI / 180;
                //hodně cursed výpočty
                //chyba, kulka se pohybuje po stejným vektoru ale z pušky, takže má offset
                Bullet bullet = new Bullet(player.centerX - 3 + (int)(Math.Sin(Math.PI / 2 + rad) * 30), player.centerY - 8 - (int)(Math.Cos(Math.PI / 2 + rad) * 35), (float)angle, false);
                bullet.speedX = (int)(Math.Sin(rad) * bullet.speed);
                bullet.speedY = (int)-(Math.Cos(rad) * bullet.speed);
                bullet.rotation = (float)angle;
                bullets.Add(bullet);

                //Přehraje zvuk v cestě
                new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Shoot.wav");
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.Y < 0) return;
            // Update mouse position
            mouseX = e.X;
            mouseY = e.Y;

            label4.Text = "GridItem: " + mapGrid[e.X / 10, e.Y / 10].ToString();  //e.X / 10, e.Y / 10
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ActiveForm == null) return; //pokud je forma minimalizovaná tak ActiveForm == null
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
            for(int i = 0; i < bullets.Count; i++) 
            {
                Bullet b = bullets[i];
                b.X += b.speedX;
                b.Y += b.speedY;

                if (b.X > 1915 || b.Y > 1045) continue;
                //damage enemies
                if (b.X < ActiveForm.Width && b.Y < ActiveForm.Height && b.X > 0 && b.Y > 0 && mapGrid[b.X / 10, b.Y / 10].charOnGrid != null)
                {
                    Character tempChar = mapGrid[b.X / 10, b.Y / 10].charOnGrid;
                    if(enemies.Contains(tempChar)) //action for enemies hit
                    {
                        tempChar.health -= 20;
                        bullets.Remove(b);
                        if(tempChar.health <= 0)
                        {
                            tempChar.Die(mapGrid);
                            enemies.Remove(tempChar);
                            SpawnEnemy();
                            killCount++;
                            label1.Text = "Kills: " + killCount;
                            new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Death.wav");
                            if (rng.Next(0,2) == 0)
                            {
                                AmmoBox box = new AmmoBox();
                                box.X = tempChar.X + 25;
                                box.Y = tempChar.Y + 25;
                                ammoBoxes.Add(box);
                            }    
                        }
                    }
                }
            }

            //generate new player grid
            ValidateCharGrid(player);

            // Ammo pickup
            label2.Text = "Ammo: " + ammoCount;
            if (ammoCount == 0)
                label2.BackColor = Color.Red;
            else
                label2.BackColor = Color.White;
            for (int i = 0; i < ammoBoxes.Count; i++)
            {
                AmmoBox ammo = ammoBoxes[i];
                if ((player.X + player.width) > (ammo.X) && (player.X) < (ammo.X + 50))
                {
                    if ((player.Y + player.height) > ammo.Y && (player.Y) < (ammo.Y + 50))
                    {
                        int ran = rng.Next(10, 21);
                        ammoCount += ran;
                        ammoBoxes.RemoveAt(i);
                        Label lbl = new Label();
                        lbl.Name = i.ToString();
                        lbl.Text = "+" + ran;
                        lbl.Location = new Point(player.centerX, player.centerY);
                        lbl.AutoSize = true;
                        lbl.Font = new Font("Verdana", 15);
                        lbl.BackColor = Color.Black;
                        lbl.ForeColor = Color.Lime;
                        this.Controls.Add(lbl);
                        lbl.BringToFront();
                        labels.Add(lbl);
                        ticks.Add(30);
                        new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Reload.wav");
                    }
                }
            }
            
            for (int i = 0; i < labels.Count; i++)
            {
                ticks[i]--;
                labels[i].Location = new Point(labels[i].Location.X, labels[i].Location.Y-2);
                if (ticks[i] <= 0)
                {
                    this.Controls.Remove(labels[i]);
                    labels.RemoveAt(i);
                    ticks.RemoveAt(i);
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

        public static void PlaySound(object path)
        {
            new SoundPlayer((string)path).Play();
        }

        private void game_graphics(object sender, PaintEventArgs e)
        {
            if (ActiveForm == null || double.IsNaN(angle)) return;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Graphics graphics = e.Graphics;
            GraphicsState state = graphics.Save();
            
            Bitmap playerMap = new Bitmap(basePath + "Assets/Textures/PlayerIcon.png");
            Image bulletImage = Image.FromFile(basePath + "Assets/Textures/Bullet.png");
            Image enemyImage = Image.FromFile(basePath + "Assets/Textures/EnemyIcon.png");
            Image ammoImage = Image.FromFile(basePath + "Assets/Textures/AmmoBox.png");

            // Draw ammo boxes
            foreach (AmmoBox box in ammoBoxes)
                graphics.DrawImage(ammoImage, box.X, box.Y);

            // Draw bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                state = graphics.Save();
                Bullet b = bullets[i];
                if (b.X < ActiveForm.Width && b.Y < ActiveForm.Height && b.X > 0 && b.Y > 0)
                {
                    //save a restore mají vrátit Graphics do stavu před úpravou, samozřejmě nefunguje
                    //graphics.TranslateTransform(bulletImage.Width/2, bulletImage.Height/2);
                    graphics.TranslateTransform(b.X, b.Y);
                    
                    graphics.RotateTransform(b.rotation);
                    graphics.TranslateTransform(-b.X, -b.Y);
                    graphics.DrawImage(bulletImage, b.X, b.Y);
                    //b.Draw(bulletImage, angle, e.Graphics);
                }
                else
                    bullets.RemoveAt(i);
                graphics.Restore(state);
            }
            graphics.Restore(state);
            foreach(Character enemy in enemies)
            {
                graphics.DrawImage(enemyImage, enemy.X, enemy.Y);
                graphics.DrawRectangle(new Pen(Color.Lime, 5), enemy.X + 10, enemy.Y + 120, (int)(0.7 * enemy.health), 10);
            }
            // Image rotation
            graphics.TranslateTransform(player.centerX, player.centerY);
            graphics.RotateTransform((float)angle);
            graphics.TranslateTransform(-player.centerX, -player.centerY);

            graphics.DrawImage(playerMap, player.X, player.Y);
        }

        public static Cursor ActuallyLoadCursor(String path)
        {
            return new Cursor(LoadCursorFromFile(path));
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string fileName);

        public void SpawnEnemy(Point spawnPoint)
        {
            Character enemy = new Character(spawnPoint);
            enemies.Add(enemy);
            ValidateCharGrid(enemy);
        }
        public void SpawnEnemy()
        {
            Thread.Sleep(1);
            Character enemy = new Character(rng.Next(0,ActiveForm.Width - 100), rng.Next(0, rng.Next(ActiveForm.Height - 100)));
            enemies.Add(enemy);
            ValidateCharGrid(enemy);
        }

        public void ValidateCharGrid(Character c)
        {
            bool enemy = enemies.Contains(c);

            for (int i = c.X / 10 + 1; i < (c.X + c.width) / 10 - 2; i++)
            {
                for (int j = c.Y / 10 + 2; j < (c.Y + c.height) / 10 - 1; j++)
                {
                    if(j * 10 < ActiveForm.Height && i * 10 < ActiveForm.Width)
                    {
                        mapGrid[i, j].material = enemy ? GridItem.Material.Enemy : GridItem.Material.Player;
                        mapGrid[i, j].charOnGrid = c;
                    }
                }
            }
        }
    }
}
