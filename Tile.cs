using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Platformer
{
    class Tile : Sprite
    {
        
        public Tile(int x, int y, int width, int height)
        {
            this.spriteX = x;
            this.spriteY = y;
            this.spriteWidth = width;
            this.spriteHeight = height;
        }
        public int getX()
        {
            return spriteX;
        }
        public int getY()
        {
            return spriteY;
        }
        public void setX(int x)
        {
            spriteX = x;
        }
        public void setY(int y)
        {
            spriteY = y;
        }

        public void LoadContent(ContentManager content)
        {
            image = content.Load<Texture2D>("prep2.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
        }

        public void checkCollision(Player a, Player2 b)
        {
            int spriteX1 = a.getX();
            int spriteX2 = b.getX();
            int spriteY1 = a.getY();
            int spriteY2 = b.getY();
            if (spriteX == spriteX1 && spriteX == spriteX2 && spriteY == spriteY1 && spriteY == spriteY2)
            {
                System.Diagnostics.Debug.WriteLine("collision True");
            }
        }
    }
}
