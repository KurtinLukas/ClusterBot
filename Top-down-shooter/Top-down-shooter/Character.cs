using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
        public int width = 100;
        public int height = 100;
        public int health = 100;
        private GridItem[,] mapGrid = Form1.mapGrid;

        //  metody
        //pohyb
        public void MoveBy(int posX, int posY, GridItem[,] mapGrid)
        {
            X = posX;
            Y = posY;
            position = new Point(posX, posY);
            if(posX > 0)
                for (int j = Y / 10; j <= (Y + height) / 10; j++)
                {
                    mapGrid[(X + width) / 10 - 2, j].material = GridItem.Material.Air;
                    mapGrid[(X + width) / 10 - 2, j].charOnGrid = this;
                }
            else if(posX < 0)
                for (int j = Y / 10; j <= (Y + height) / 10; j++)
                {
                    mapGrid[X / 10 + 1, j].material = GridItem.Material.Air;
                    mapGrid[X / 10 + 1, j].charOnGrid = this;
                }
            if(posY > 0)
                for (int i = X / 10; i <= (X + width) / 10; i++)
                {
                    mapGrid[i, (Y + height) / 10 - 2].material = GridItem.Material.Air;
                    mapGrid[i, (Y + height) / 10 - 2].charOnGrid = this;
                }
            else if(posY < 0)
                for (int i = X / 10; i <= (X + width) / 10; i++)
                {
                    mapGrid[i, Y / 10 + 2].material = GridItem.Material.Air;
                    mapGrid[i, Y / 10 + 2].charOnGrid = this;
                }
        }

        //akce
        public void Shoot(Point direction)
        {

        }

        //ostatní
        public void Die(GridItem[,] mapGrid)
        {
            for (int i = X / 10 + 1; i < (X + width) / 10 - 2; i++)
            {
                for (int j = Y / 10 + 2; j < (Y + height) / 10 - 1; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Air;
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
