using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Top_down_shooter
{
    public class Character
    {
        //proměnný
        public Point position;
        public int X = 0; //default position
        public int Y = 0;
        public int centerX;
        public int centerY;
        public double angle;
        public int width = 100;
        public int height = 100;
        public int health = 100;
        public bool isEnemy;
        private GridItem[,] mapGrid;
        private bool collisionRight;
        private bool collisionLeft;
        private bool collisionUp;
        private bool collisionDown;
        public Point target;

        //  metody
        //pohyb
        public void MoveBy(int posX, int posY)
        {
            if (Form1.ActiveForm == null)
                return;
            mapGrid = Form1.mapGrid;

            collisionRight = X + posX + width > Form1.ActiveForm.Width - 15;
            collisionLeft = X + posX < 0;
            collisionUp = Y + posY < 65;
            collisionDown = Y + posY + height > Form1.ActiveForm.Height - 45;

            //can check for gridItem movement for optimalization

            //collision
            if (posX > 0 && !collisionRight) //pohyb doprava
            {
                X += posX;
                for (int i = Y / 10 + 1; i < Y / 10 + 10; i++)
                {
                    if (mapGrid[X / 10 + 8, i].material != GridItem.Material.Air)
                    {
                        X -= posX;
                        collisionRight = true;
                        break;
                    }
                }
            }
            else if (posX < 0 && !collisionLeft) //pohyb doleva
            {
                X += posX;
                for (int i = Y / 10 + 1; i < Y / 10 + 10; i++)
                {
                    if (mapGrid[X / 10+1, i].material != GridItem.Material.Air)
                    {
                        X -= posX;
                        collisionLeft = true;
                        break;
                    }
                }
            }
            if (posY > 0 && !collisionDown) //pohyb dolů
            {
                Y += posY;
                for (int i = X / 10+1; i < X / 10 + 9; i++)
                {
                    if (mapGrid[i, Y / 10 + 9].material != GridItem.Material.Air)
                    {
                        Y -= posY;
                        collisionDown = true;
                        break;
                    }
                }
            }
            else if (posY < 0 && !collisionUp) //pohyb nahoru
            {
                Y += posY;
                for (int i = X / 10+1; i < X / 10 + 9; i++)
                {
                    if (mapGrid[i, Y / 10 + 1].material != GridItem.Material.Air)
                    {
                        Y -= posY;
                        collisionUp = true;
                        break;
                    }
                }
            }

            if (!collisionRight)
            {
                for (int i = Y / 10 + 1; i < Y / 10 + 10; i++)
                {
                    mapGrid[X / 10 + 8, i].material = GridItem.Material.Air;
                    mapGrid[X / 10 + 8, i].charOnGrid = null;
                }
            }
            if (!collisionLeft)
            {
                for (int i = Y / 10 + 1; i < Y / 10 + 10; i++)
                {
                    mapGrid[X / 10+1, i].material = GridItem.Material.Air;
                    mapGrid[X / 10+1, i].charOnGrid = null;
                }
            }
            if (!collisionUp)
            {
                for (int i = X / 10+1; i < X / 10 + 9; i++)
                {
                    mapGrid[i, Y / 10 + 1].material = GridItem.Material.Air;
                    mapGrid[i, Y / 10 + 1].charOnGrid = null;
                }
            }
            if (!collisionDown)
            {
                for (int i = X / 10+1; i < X / 10 + 9; i++)
                {
                    mapGrid[i, Y / 10 + 9].material = GridItem.Material.Air;
                    mapGrid[i, Y / 10 + 9].charOnGrid = null;
                }
            }
            
            //create new position
            for (int i = X / 10 + 2; i < X / 10 + 8; i++)
            {
                for (int j = Y / 10 + 2; j < Y / 10 + 9; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Enemy;
                    mapGrid[i, j].charOnGrid = this;
                }
            }
            if ((X == position.X && Y == position.Y))
                target = Form1.spawnPoints[new Random().Next(0, Form1.spawnPoints.Count())];
            position = new Point(X, Y);
        }

        //ostatní
        public void Die()
        {
            for (int i = X / 10 + 1; i < X / 10 + 8; i++)
            {
                for (int j = Y / 10 + 2; j < Y / 10 + 9; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Air;
                    mapGrid[i, j].charOnGrid = null;
                }
            }
        }

        //deklarace
        public Character(int posX, int posY)
        {
            X = posX;
            Y = posY;
            position = new Point(posX, posY);
        }
        public Character(Point posPoint)
        {
            X = posPoint.X;
            Y = posPoint.Y;
            position = posPoint;
        }
    }
}
