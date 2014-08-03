using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
namespace Platformer
{
    class Endline : Sprite
    {
        public Rectangle rect;
        
        public Endline(int x, int y, int width, int height)
        {
            this.spriteX = x;
            this.spriteY = y;
            this.spriteWidth = width;
            this.spriteHeight = height;
            rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);

        }

        public void LoadContent(ContentManager content)
        {
           image = content.Load<Texture2D>("finish.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(spriteX,spriteY,spriteWidth,spriteHeight), Color.White);
        }
        public bool checkCollision(Player player1, Player player2)
        {
            if (this.rect.Intersects(player2.rect) && this.rect.Intersects(player1.rect))
            {
                return true;
            }
            return false;
        }
    }
}
