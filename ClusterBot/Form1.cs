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
using System.IO;

using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Top_down_shooter
{
    public partial class Form1 : Form
    {
        // Character
        Character player;
        List<Bullet> bullets = new List<Bullet>();
        List<Consumable> ammoBoxes = new List<Consumable>();
        List<Consumable> medkits = new List<Consumable>();
        List<Label> labels = new List<Label>();
        List<int> ticks = new List<int>();

        // Menu
        Menu menu;

        Random rng = new Random();
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        int speed;
        double diagonalSpeed;
        int mouseX;
        int mouseY;
        int timer = 0;
        bool generateEnemies = true;

        int score = 0;
        int highscore;
        int killCount = 0;
        int ammoCount = 50;

        private string keyLogger = "";
        private bool debugMode = false;

        public static GridItem[,] mapGrid;
        public static List<Character> enemies = new List<Character>();

        string basePath = Assembly.GetExecutingAssembly().CodeBase.Substring(8);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //fullscreen
            //TopMost = true;
            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;
            int pathRemoveIndex = basePath.IndexOf("ClusterBot") + 10;
            basePath = basePath.Remove(pathRemoveIndex) + "/";

            this.KeyPreview = true;
            menu = new Menu(this, timer1, basePath);

            player = new Character(400, 400);
            player.isEnemy = false;
            speed = 5;
            diagonalSpeed = speed / Math.Sqrt(2);
            //highscore = int.Parse(System.IO.File.ReadAllText(basePath + @"Save\Highscore.txt"));
            highscore = 0;
            label4.Text = "Highscore: " + highscore;
            //MessageBox.Show(basePath);    //<-- při změně cesty se musí přenastavit! (2 řádky nahoru)
            IntPtr cursor = LoadCursorFromFile(basePath + @"Assets\Textures\Cursor.cur");
            Cursor = new Cursor(cursor);
            CenterToScreen();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //deklarace gridu
            Bitmap mapImg = new Bitmap(basePath + "Assets/Textures/map.png");
            mapGrid = new GridItem[ActiveForm.Width / 10, ActiveForm.Height / 10];
            for (int i = 0; i < ActiveForm.Width / 10; i++)
            {
                for (int j = 0; j < ActiveForm.Height / 10; j++)
                {
                    mapGrid[i, j] = new GridItem(i * 10, j * 10);
                    Color clr = mapImg.GetPixel(i, j);
                    switch (clr.ToArgb())
                    {
                        //bílá 
                        case -1: mapGrid[i, j].material = GridItem.Material.Air; break;
                        //černá
                        case -16777216: mapGrid[i, j].material = GridItem.Material.Wall; break;
                        //default žlutá v MS paint (např. bedna)
                        case -3584: mapGrid[i, j].material = GridItem.Material.Object; break;
                        default: mapGrid[i, j].material = GridItem.Material.Air; break;
                    }
                }
            }
            foreach (Character c in enemies)
            {
                ValidateCharGrid(c);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (ActiveForm == null) return;

            //rekalkulace gridu
            mapGrid = new GridItem[ActiveForm.Width / 10, ActiveForm.Height / 10];
            for (int i = 0; i < ActiveForm.Width / 10; i++)
            {
                for (int j = 0; j < ActiveForm.Height / 10; j++)
                {
                    mapGrid[i, j] = new GridItem(i * 10, j * 10, GridItem.Material.Air);
                }
            }

            pictureBox2.Size = new Size(ActiveForm.Width, ActiveForm.Height);

            // GUI positioning
            progressBar1.Location = new Point(ActiveForm.Width - progressBar1.Width - 30, progressBar1.Location.Y);
            label3.Location = new Point(progressBar1.Location.X - label3.Width - 30, label3.Location.Y);
            label2.Location = new Point(ActiveForm.Width / 3, label2.Location.Y);
        }

        Point moveVector = new Point(0, 0);
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //console
            if (e.KeyCode == Keys.Oemtilde)
            {
                keyLogger = "";
            }
            else keyLogger += (char)e.KeyValue;

            switch (keyLogger)
            {
                case "DEBUG":
                    if (debugMode)
                        debugMode = false;
                    else
                        debugMode = true;
                    break;
                case "BOSS":
                    for (int i = 0; i < 20; i++)
                    {
                        SpawnEnemy();
                    }
                    break;
                case "KILL":
                    for (int i = enemies.Count - 1; i >= 0; i--)
                    {
                        enemies[i].Die();
                        enemies.Remove(enemies[i]);
                    }
                    break;
                case "STOP":
                    generateEnemies = false;
                    break;
                case "START":
                    generateEnemies = true;
                    break;
            }

            switch (e.KeyCode)
            {
                case Keys.A: left = true; break;
                case Keys.D: right = true; break;
                case Keys.W: up = true; break;
                case Keys.S: down = true; break;
                case Keys.M: SpawnEnemy(); break;
                case Keys.Escape: menu.Show(); break;
                case Keys.Space: Encrypt(basePath + @"\Save\Highscore.xyz"); break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: left = false; moveVector.X = 0; break;
                case Keys.D: right = false; moveVector.X = 0; break;
                case Keys.W: up = false; moveVector.Y = 0; break;
                case Keys.S: down = false; moveVector.Y = 0; break;
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (!menu.visible)
            {
                if (ammoCount > 0 && e.Button == MouseButtons.Left)
                {
                    score -= 3;
                    ammoCount--;
                    ShootBullet(player);
                }
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.Y < 0) return;
            // Update mouse position
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer++;
            if (ActiveForm == null) return; //pokud je forma minimalizovaná tak ActiveForm == null
            // Movement & player grid calculation
            if (left)
            {
                moveVector.X = -speed;
            }
            if (right)
            {
                moveVector.X = speed;
            }
            if (up)
            {
                moveVector.Y = -speed;
            }
            if (down)
            {
                moveVector.Y = speed;
            }

            if ((left && up) || (left && down) || (right && up) || (right && down))
                speed = (int)diagonalSpeed;
            else
                speed = 5;
            player.MoveBy(moveVector.X, moveVector.Y);

            // Bullet movement
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                b.X += b.speedX;
                b.Y += b.speedY;

                if (b.X > 1915 || b.Y > 1045) continue;
                //damage enemies
                if (b.X < ActiveForm.Width && b.Y < ActiveForm.Height && b.X > 0 && b.Y > 0 && mapGrid[b.X / 10, b.Y / 10].material != GridItem.Material.Air)
                {
                    Character tempChar = mapGrid[b.X / 10, b.Y / 10].charOnGrid;
                    if (tempChar != null)
                    {
                        if (tempChar != b.originChar && b.originChar.isEnemy != tempChar.isEnemy) //action for enemies hit
                        {
                            bullets.Remove(b);
                            if (tempChar == player)
                            {
                                player.health -= 7;

                                if (player.health <= 0)
                                {
                                    player.health = 0;
                                    
                                    if (score < 0)
                                        MessageBox.Show("Really man? Negative score? Try again, for your own sake.",
                                        "You're actually so bad.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    else
                                        MessageBox.Show((score > highscore ? "Good job! You beat the current highscore of " + highscore + " points by killing "
                                            : "You died, but managed to kill ") + killCount + " enemies and earned a " +
                                        (score < 1000 ? "disappointing" : score < 3000 ? "solid" : score < 8000 ? "amazing" : "tremendous")
                                        + " score of " + score,
                                        "You dead.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                    if (score >= highscore)
                                    {
                                        StreamWriter writer = new StreamWriter(basePath + @"Save\Highscore.txt");
                                        writer.Write(score.ToString());
                                        writer.Close();
                                    }
                                    Application.Restart();
                                }
                                progressBar1.Value = player.health;
                            }
                            else
                            {
                                tempChar.health -= 20;
                            }
                            if (tempChar.health <= 0)
                            {
                                score += 100;
                                enemies.Remove(tempChar);
                                tempChar.Die();
                                //SpawnEnemy();
                                killCount++;

                                new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Death.wav");
                                if (rng.Next(0, 2) == 0) //generate ammo box
                                {
                                    Consumable box = new Consumable(tempChar.X + 25, tempChar.Y + 25, basePath + @"Assets\Textures\AmmoBox.png");
                                    ammoBoxes.Add(box);
                                }
                                else //generate medkit
                                {
                                    Consumable medkit = new Consumable(tempChar.X + 25, tempChar.Y + 25, basePath + @"Assets\Textures\Medkit.png");
                                    medkits.Add(medkit);
                                }
                            }
                        }
                    }
                    if (mapGrid[b.X / 10, b.Y / 10].material == GridItem.Material.Wall)
                    {
                        bullets.Remove(b);
                    }
                }
            }

            if (score > highscore)
                label1.BackColor = Color.Lime;
            else
                label1.BackColor = Color.Transparent;


            // Ammo pickup
            if (ammoCount == 0)
                label2.BackColor = Color.Red;
            else
                label2.BackColor = Color.Transparent;

            for (int i = 0; i < ammoBoxes.Count; i++)
            {
                Consumable ammo = ammoBoxes[i];
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
            //medkit pickup
            for (int i = 0; i < medkits.Count; i++)
            {
                Consumable med = medkits[i];
                if ((player.X + player.width) > (med.X) && (player.X) < (med.X + 50))
                {
                    if ((player.Y + player.height) > med.Y && (player.Y) < (med.Y + 50))
                    {
                        player.health += 30;
                        if (player.health > 100)
                            player.health = 100;
                        medkits.RemoveAt(i);
                        Label lbl = new Label();
                        lbl.Name = i.ToString();
                        lbl.Text = "+" + 30;
                        lbl.Location = new Point(player.centerX, player.centerY);
                        lbl.AutoSize = true;
                        lbl.Font = new Font("Verdana", 15);
                        lbl.BackColor = Color.Red;
                        lbl.ForeColor = Color.White;
                        Controls.Add(lbl);
                        lbl.BringToFront();
                        labels.Add(lbl);
                        ticks.Add(30);
                        new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Reload.wav");
                        progressBar1.Value = player.health;
                    }
                }
            }
            //label animation
            for (int i = 0; i < labels.Count; i++)
            {
                ticks[i]--;
                labels[i].Location = new Point(labels[i].Location.X, labels[i].Location.Y - 2);
                if (ticks[i] <= 0)
                {
                    this.Controls.Remove(labels[i]);
                    labels.RemoveAt(i);
                    ticks.RemoveAt(i);
                }
            }
            // Player angle
            player.angle = CalcAngle(player, mouseX, mouseY);


            if (generateEnemies && timer % 200 == 0)
            {
                for (int i = 0; i < timer / (1500 * i * 0.5); i++)
                    SpawnEnemy();
            }
            if (timer % 500 == 0)
            {
                if (rng.Next(0, 2) == 0)
                {
                    ammoBoxes.Add(new Consumable(rng.Next(25, ActiveForm.Width - 125), rng.Next(80, ActiveForm.Height - 155), basePath + @"Assets\Textures\AmmoBox.png"));
                }
                else
                {
                    medkits.Add(new Consumable(rng.Next(25, ActiveForm.Width - 125), rng.Next(80, ActiveForm.Height - 155), basePath + @"Assets\Textures\Medkit.png"));
                }
            }
            // Enemy handler
            foreach (Character enemy in enemies)
            {
                enemy.angle = CalcAngle(enemy, player.centerX, player.centerY);
                if (rng.Next(1, 100) == 1)
                    ShootBullet(enemy);

                if (Math.Abs(enemy.X - enemy.target.X) <= 5)
                    enemy.target.X = enemy.X;
                if (Math.Abs(enemy.Y - enemy.target.Y) <= 5)
                    enemy.target.Y = enemy.Y;

                if (enemy.position == enemy.target)//nový cíl pokud ke stávajícímu dojde
                    enemy.target = new Point(rng.Next(25, ActiveForm.Width - 125), rng.Next(80, ActiveForm.Height - 155));
                //pohyb k cíli
                enemy.MoveBy(enemy.X > enemy.target.X ? -3 : enemy.target.X == enemy.X ? 0 : 3, enemy.Y > enemy.target.Y ? -3 : enemy.target.Y == enemy.Y ? 0 : 3);
            }

            label1.Text = "Score: " + score;
            label2.Text = "Ammo: " + ammoCount;

            pictureBox2.Invalidate();
        }

        public double CalcAngle(Character character, int x, int y)
        {
            double calcAngle;
            character.centerX = character.X + character.width / 2;
            character.centerY = character.Y + character.height / 2;
            int diffX = character.centerX - x;
            int diffY = Math.Abs(character.centerY - y);
            double prepona = Math.Sqrt(diffY * diffY + diffX * diffX);
            if (diffX < 0)
                calcAngle = Math.Acos(diffX / prepona);
            else
                calcAngle = Math.Asin(diffY / prepona);
            calcAngle *= 180 / Math.PI;
            if (character.centerY < y)
                calcAngle = 360 - calcAngle;
            return calcAngle - 90 - 2000 / prepona;
        }

        public void ShootBullet(Character character)
        {
            // Shoot a bullet
            double bulletAngle = character.angle + rng.Next(-2, 3);
            double rad = bulletAngle * Math.PI / 180;

            //chyba, kulka se pohybuje po stejným vektoru ale z pušky, takže má offset
            Bullet bullet = new Bullet(character.centerX - 3 + (int)(Math.Sin(Math.PI / 2 + rad) * 30), character.centerY - 8 - (int)(Math.Cos(Math.PI / 2 + rad) * 35), (float)character.angle, false);
            bullet.speedX = (int)(Math.Sin(rad) * bullet.speed);
            bullet.speedY = (int)-(Math.Cos(rad) * bullet.speed);
            bullet.rotation = (float)character.angle;
            bullet.originChar = character;
            bullets.Add(bullet);

            //Přehraje zvuk v cestě
            new Thread(new ParameterizedThreadStart(PlaySound)).Start(basePath + @"\Assets\SFX\Shoot.wav");
        }

        public static void PlaySound(object path)
        {
            new SoundPlayer((string)path).Play();
        }


        public void Encrypt(string path)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            string password = @"crypt123";
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            string cryptfile = path;
            FileStream fsCrypt = new FileStream(cryptfile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();
            CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(ofd.FileName, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);
            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }

        private void game_graphics(object sender, PaintEventArgs e)
        {
            if (ActiveForm == null || double.IsNaN(player.angle)) return;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Graphics graphics = e.Graphics;
            GraphicsState state = graphics.Save();

            Bitmap playerMap = new Bitmap(basePath + "Assets/Textures/PlayerIcon.png");
            Image bulletImage = Image.FromFile(basePath + "Assets/Textures/Bullet.png");
            Image enemyImage = Image.FromFile(basePath + "Assets/Textures/EnemyIcon.png");

            // Draw ammo boxes
            foreach (Consumable box in ammoBoxes)
                graphics.DrawImage(Image.FromFile(box.texturePath), box.X, box.Y);
            foreach (Consumable med in medkits)
                graphics.DrawImage(Image.FromFile(med.texturePath), med.X, med.Y);

            // Draw bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                state = graphics.Save();
                Bullet b = bullets[i];
                if (b.X < pictureBox2.Width && b.Y < pictureBox2.Height && b.X > 0 && b.Y > 0)
                {
                    Rotate(b.X, b.Y, b.rotation, graphics);
                    graphics.DrawImage(bulletImage, b.X, b.Y);
                }
                else
                    bullets.RemoveAt(i);
                graphics.Restore(state);
            }
            graphics.Restore(state);

            // Enemy rotation
            foreach (Character enemy in enemies)
            {
                state = graphics.Save();
                Rotate(enemy.centerX, enemy.centerY, enemy.angle, graphics);
                graphics.DrawImage(enemyImage, enemy.X, enemy.Y);
                graphics.Restore(state);
                Color color;
                if (enemy.health <= 40)
                    color = Color.Red;
                else
                    color = Color.Lime;
                graphics.DrawRectangle(new Pen(color, 5), enemy.X + 10, enemy.Y + 120, (int)(0.7 * enemy.health), 5);
            }
            graphics.Restore(state);
            if (debugMode)
            {
                foreach (GridItem gi in mapGrid)
                {
                    if (gi.material != GridItem.Material.Air)
                        graphics.DrawRectangle(new Pen(Brushes.Red), gi.X, gi.Y, 10, 10);
                }
            }
            // Player rotation
            Rotate(player.centerX, player.centerY, player.angle, graphics);
            graphics.DrawImage(playerMap, player.X, player.Y);
        }

        public void Rotate(int x, int y, double angle, Graphics g)
        {
            g.TranslateTransform(x, y);
            g.RotateTransform((float)angle);
            g.TranslateTransform(-x, -y);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string fileName);

        public void SpawnEnemy(Point spawnPoint)
        {
            Character enemy = new Character(spawnPoint);
            enemy.isEnemy = true;
            enemies.Add(enemy);
            ValidateCharGrid(enemy);
        }
        public void SpawnEnemy()
        {
            Thread.Sleep(1);
            Character enemy = new Character(rng.Next(25, ActiveForm.Width - 125), rng.Next(60, ActiveForm.Height - 155));
            enemy.isEnemy = true;
            enemy.target = new Point(rng.Next(25, ActiveForm.Width - 125), rng.Next(80, ActiveForm.Height - 155));
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
                    if (j * 10 < pictureBox2.Height && i * 10 < pictureBox2.Width)
                    {
                        mapGrid[i, j].material = enemy ? GridItem.Material.Enemy : GridItem.Material.Player;
                        mapGrid[i, j].charOnGrid = c;
                    }
                }
            }
        }
    }
}
