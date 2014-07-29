using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Platformer
{
	class Player : Sprite
    {
		private bool moving;
		private bool grounded;
        private bool air;
		private int speed;
		private int x_accel;
		private double friction;
		public double x_vel;
		public double y_vel;
		public int movedX;
		private bool pushing;
		public double gravity;
        public int maxFallSpeed;
		private int jumpPoint = 0;
        public Player p2;
        private bool held;
        public Rectangle rect;
        private Vector2 normal;
        private Vector2 collisionDist = Vector2.Zero;
        private bool inverted;
        private ContentManager c;
        private SpriteEffects flip;

        public Player(int x, int y, int width, int height, bool inverted)
        {
            this.spriteX = x;
            this.spriteY = y;
            this.spriteWidth = width;
            this.spriteHeight = height;
            this.inverted = inverted;
			grounded = false;
			moving = false;
			pushing = false;
            held = false;
            rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);

            if (inverted)
            {
                gravity = -.5;
                maxFallSpeed = -10;
            }
            else
            {
                gravity = .5;
                maxFallSpeed = 10;
            }

			// Movement
			speed = 5;
			friction = .15;
			x_accel = 0;
			x_vel = 0;
			y_vel = 0;
			movedX = 0;
        }

        public int getX(){
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
        public Player getP2()
        {
            return p2;
        }
        public void setP2(Player p2)
        {
            this.p2 = p2;
        }
        public bool getGrounded()
        {
            return grounded;
        }
        public bool getHold()
        {
            return held;
        }
        public void setHold(bool held)
        {
            this.held = held;
        }

        public void LoadContent(ContentManager content)
        {
            c = content;
            if (inverted)
                image = content.Load<Texture2D>("Benny.png");
            else
                image = content.Load<Texture2D>("Aaron.png");

        }

        public void LoadContent(String imageName)
        {
            image = c.Load<Texture2D>(imageName);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), null, Color.White, 0.0f, new Vector2(0 , 0), flip, 0.0f);
        }

		public void Update(Controls controls, GameTime gameTime, List<Rectangle> collisionRects)
		{
            Hold(controls);
            Drop(controls);
            Move (controls, collisionRects, gameTime);
			Jump (controls, gameTime);
            
		}

		public void Move(Controls controls, List<Rectangle> collisionRects, GameTime gameTime)
		{

			// Sideways Acceleration
            if (inverted)
            {
                if (controls.onPress(Keys.D, Buttons.DPadRight))
                {
                    x_accel += speed;
                    flip = SpriteEffects.None;
                }
                else if (controls.onRelease(Keys.D, Buttons.DPadRight))
                    x_accel -= speed;
                if (controls.onPress(Keys.A, Buttons.DPadLeft))
                {
                    x_accel -= speed;
                    flip = SpriteEffects.FlipHorizontally;
                }
                else if (controls.onRelease(Keys.A, Buttons.DPadLeft))
                    x_accel += speed;
            }
            else
            {

                if (controls.onPress(Keys.Right, Buttons.DPadRight))
                {
                    x_accel += speed;
                    flip = SpriteEffects.None;
                }
                else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                    x_accel -= speed;
                if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                {
                    x_accel -= speed;
                    flip = SpriteEffects.FlipHorizontally;
                }
                else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                    x_accel += speed;
            }
			

			double playerFriction = pushing ? (friction * 3) : friction;
			x_vel = x_vel * (1 - playerFriction) + x_accel * .10;
			movedX = Convert.ToInt32(x_vel);
			spriteX += movedX;

			// Gravity

            if (inverted)
            {
                if (!grounded)
                {
                    y_vel += gravity;
                    if (y_vel < maxFallSpeed)
                        y_vel = maxFallSpeed;
                    spriteY += Convert.ToInt32(y_vel);
                }
                else
                {
                    y_vel = -1;
                }
            }
            else
            {
                if (!grounded)
                {
                    y_vel += gravity;
                    if (y_vel > maxFallSpeed)
                        y_vel = maxFallSpeed;
                    spriteY += Convert.ToInt32(y_vel);
                }
                else
                {
                    y_vel = 1;
                }
            }

			grounded = false;

			// Check up/down collisions, then left/right
            rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
			checkYCollisions(collisionRects);
            

		}

        private void checkYCollisions(List<Rectangle> collisionRects)
        {
            //Reset collision dist
            collisionDist = Vector2.Zero;

            for (int i = 0; i < collisionRects.Count; i++)
            {
                if (IsColliding(rect, collisionRects[i]))
                {
                    //If there are multiple collision make sure we only react to the most severe
                    if (normal.Length() > collisionDist.Length())
                        collisionDist = normal;
                    if (inverted)
                        LoadContent("Benny.png");
                    else
                        LoadContent("Aaronstand1.png");
                    grounded = true;
                }
            }
            //Update the players position
            float a = this.getX() + collisionDist.X;
            //float b = this.getY() + collisionDist.Y; //Hacky code :)
            this.setX((int)a);
            //this.setY((int)b);
        }
		

		private void Jump(Controls controls, GameTime gameTime)
		{
			// Jump on button press
            if (inverted)
            {
                if (controls.onPress(Keys.S, Buttons.A) && grounded && !p2.getHold())
                {
                    y_vel = 10;
                    jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                    grounded = false;
                }
            }
            else
            {
                if (controls.onPress(Keys.Up, Buttons.A) && grounded && !p2.getHold())
                {
                    LoadContent("Aaronjump.png");
                    y_vel = -10;
                    jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                    grounded = false;
                }
            }
		}
        public void Hold(Controls controls)
        {
           
            if (inverted)
            {
                
                int sprite2Y = p2.getY() - 81;
                if (spriteY > sprite2Y && (spriteX - 15 <= p2.getX() && spriteX + 5 >= p2.getX()) && p2.getGrounded())
                {
                    held = true;
                }
            }
            else
            {
                
                int sprite2Y = p2.getY() + 81;
                if (spriteY < sprite2Y && (spriteX - 15 <= p2.getX() && spriteX + 5 >= p2.getX()) && p2.getGrounded())
                {
                    held = true;
                }
            }
            
        }
        public void Drop(Controls controls)
        {
            if (held)
            {
                

                if (inverted)
                {
                    setY(p2.getY() - 58);
                    setX(p2.getX());
                    if (controls.onPress(Keys.W, Buttons.LeftShoulder) && p2.getGrounded())
                    {
                        setY(p2.getY() - 74);
                        held = false;
                    }
                }
                else
                {
                    setY(p2.getY() + 58);
                    setX(p2.getX());
                    if (controls.onPress(Keys.Down, Buttons.RightShoulder) && p2.getGrounded())
                    {
                        setY(p2.getY() + 74);
                        held = false;
                    }
                }
                
            }
        }

        private bool IsColliding(Rectangle body1, Rectangle body2)
        {
            //Reset the normal vector
            normal = Vector2.Zero;

            //Get the centre of each body
            Vector2 body1Centre = new Vector2(body1.X + (body1.Width / 2), body1.Y + (body1.Height / 2));
            Vector2 body2Centre = new Vector2(body2.X + (body2.Width / 2), body2.Y + (body2.Height / 2));

            //Declare 2 local vectors
            Vector2 distance, absDistance;

            //xMag and yMag represent the magnitudes of the x and y components of the normal vector
            float xMag, yMag;

            //Calculate the difference in position of the two rectangles
            distance = body1Centre - body2Centre;

            //Get the combined half heights/widths of the rects
            float xAdd = ((body1.Width) + (body2.Width)) / 2.0f;
            float yAdd = ((body1.Height) + (body2.Height)) / 2.0f;

            //Calculate absDistance, according to distance
            absDistance.X = (distance.X < 0) ? -distance.X : distance.X;
            absDistance.Y = (distance.Y < 0) ? -distance.Y : distance.Y;

            //Check if there is a collision
            if (!((absDistance.X < xAdd) && (absDistance.Y < yAdd)))
                return false;

            //The magnitude of the normal vector is determined by the overlap in the rectangles.
            xMag = xAdd - absDistance.X;
            yMag = yAdd - absDistance.Y;

            //Only adjust the normal vector in the direction of the least significant overlap.
            if (xMag < yMag)
                normal.X = (distance.X > 0) ? xMag : -xMag;
            else
                normal.Y = (distance.Y > 0) ? yMag : -yMag;

            //There was a collision, return true
            return true;
        }
    }
}
