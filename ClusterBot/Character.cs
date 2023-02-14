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
        private GridItem[,] mapGrid = Form1.mapGrid;
        private bool collisionRight;
        private bool collisionLeft;
        private bool collisionUp;
        private bool collisionDown;
        //  metody
        //pohybw
        public void MoveBy(int posX, int posY)
        {
            if (Form1.ActiveForm == null) 
                return;
            
            collisionRight = X + posX + width > Form1.ActiveForm.Width - 55;
            collisionLeft = X + posX < 15;
            collisionUp = Y + posY < 80;
            collisionDown = Y + posY + height > Form1.ActiveForm.Height - 35;
            
            //can check for gridItem movement for optimalization

            //collision
            if (posX > 0 && !collisionRight) //pohyb doprava
            {
                for (int i = Y / 10 + 2; i < Y / 10 + 9; i++)
                {
                    if (mapGrid[X / 10 + 9, i].material != GridItem.Material.Air)
                    {
                        X -= posX;
                        collisionRight = true;
                        break;
                    }
                }
            }
            else if (posX < 0 && !collisionLeft) //pohyb doleva
            {
                for (int i = Y / 10 + 2; i < Y / 10 + 9; i++)
                {
                    if (mapGrid[X / 10, i].material != GridItem.Material.Air)
                    {
                        X -= posX;
                        collisionLeft = true;
                        break;
                    }
                }
            }
            if (posY > 0 && !collisionDown) //pohyb dolů
            {
                for (int i = X / 10 + 1; i < X / 10 + 8; i++)
                {
                    if (mapGrid[i, Y / 10 + 10].material != GridItem.Material.Air)
                    {
                        Y -= posY;
                        collisionDown = true;
                        break;
                    }
                }
            }
            else if (posY < 0 && !collisionUp) //pohyb nahoru
            {
                for (int i = X / 10 + 1; i < X / 10 + 8; i++)
                {
                    if (mapGrid[i, Y / 10 + 1].material != GridItem.Material.Air)
                    {
                        Y -= posY;
                        collisionUp = true;
                        break;
                    }
                }
            }

            return; if (!collisionRight)
            {
                for (int i = Y / 10 + 2; i < (Y + height) / 10 - 1; i++)
                {
                    mapGrid[X / 10 + 9, i].material = GridItem.Material.Air;
                }
                return;
            }
            if (!collisionLeft)
            {
                for (int i = Y / 10 + 2; i < (Y + height) / 10 - 1; i++)
                {
                    mapGrid[X / 10, i].material = GridItem.Material.Air;
                }
                return;
            }
            if (!collisionUp)
            {
                for (int i = X / 10 + 1; i < (X + width) / 10 + 8; i++)
                {
                    mapGrid[i, Y / 10 + 1].material = GridItem.Material.Air;
                }
                return;
            }
            if (!collisionDown)
            {
                for (int i = X / 10 + 1; i < (X + width) / 10 + 8; i++)
                {
                    mapGrid[i, Y / 10 + 9].material = GridItem.Material.Air;
                }
                return;
            }

            //clear old position when no collisions
            /*for (int i = X / 10; i < (X + width) / 10 - 1; i++)
            {
                for (int j = Y / 10 + 1; j < (Y + height) / 10; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Air;
                    mapGrid[i, j].charOnGrid = null;
                }
            }*/
            //create new position
            for (int i = X / 10 + 1; i < (X + width) / 10 - 2; i++)
            {
                for (int j = Y / 10 + 2; j < (Y + height) / 10 - 1; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Enemy;
                    mapGrid[i, j].charOnGrid = this;
                }
            }

            
            position = new Point(X, Y);
        }

        //akce
        public void Shoot(Point direction)
        {

        }

        //ostatní
        public void Die()
        {
            for (int i = X / 10 + 1; i < (X + width) / 10 - 2; i++)
            {
                for (int j = Y / 10 + 2; j < (Y + height) / 10 - 1; j++)
                {
                    //mapGrid[i, j].material = GridItem.Material.Air;
                    mapGrid[i, j].charOnGrid = null;
                }
            }
        }

        //deklarace
        public Character()
        {
            position = new Point(X, Y);
        }
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
