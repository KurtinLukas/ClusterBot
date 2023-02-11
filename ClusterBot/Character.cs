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
        public void MoveBy(int posX, int posY)
        {
            if (Form1.ActiveForm == null || X + posX + width > Form1.ActiveForm.Width-15 || X+posX+width < 100 || Y + posY + height > Form1.ActiveForm.Height-35 || Y + posY + height < 100) 
                return;
            X += posX;
            Y += posY;
            //clear old position
            for (int i = X / 10; i < (X + width) / 10; i++)
            {
                for (int j = Y / 10; j < (Y + height) / 10; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Air;
                    mapGrid[i, j].charOnGrid = null;
                }
            }
            //create new position
            for (int i = X / 10 + 1; i < (X + width) / 10 - 2; i++)
            {
                for (int j = Y / 10 + 2; j < (Y + height) / 10 - 1; j++)
                {
                    mapGrid[i, j].material = GridItem.Material.Enemy;
                    mapGrid[i, j].charOnGrid = this;
                }
            }

            if (posX < 0){
                for(int i = Y / 10 + 2; i < (Y + height) / 10 - 1; i++)
                {
                    if (mapGrid[X / 10 - 1, i].material != GridItem.Material.Air)
                        X -= posX;
                    //mapGrid[X / 10 - 1, i].charOnGrid != null || 
                }
            }
            else if (posX > 0) {
                for (int i = Y / 10 + 2; i < (Y + height) / 10 - 1; i++)
                {
                    if (mapGrid[X / 10 + 11, i].material != GridItem.Material.Air) ;
                        //X -= posX;
                }
            } 
            //mapGrid[X / 10, Y / 10 + 2].charOnGrid != null || mapGrid[X / 10, Y / 10 + 8].charOnGrid != null))
            //mapGrid[X / 10 + 11, Y / 10 + 2].charOnGrid != null || mapGrid[X / 10 + 11, Y / 10 + 8].charOnGrid != null))
            {
                X -= posX;
            }
            if ((posY < 0 && (mapGrid[X / 10 + 1, Y / 10].charOnGrid != null || mapGrid[X / 10 + 10, Y / 10].charOnGrid != null)) ||
                (posY > 0 && (mapGrid[X / 10 + 1, Y / 10 + 11].charOnGrid != null || mapGrid[X / 10 + 10, Y / 10 + 11].charOnGrid != null)))
            {
                Y -= posY;
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
