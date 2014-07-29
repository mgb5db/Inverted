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
		private int speed;
		private int x_accel;
		private double friction;
		public double x_vel;
		public double y_vel;
		public int movedX;
		private bool pushing;
		public double gravity = .5;
		public int maxFallSpeed = 10;
		private int jumpPoint = 0;
        public Player2 p2;
        private bool held;
        public Rectangle rect;
        private Vector2 normal;
        private Vector2 collisionDist = Vector2.Zero;

        public Player(int x, int y, int width, int height)
        {
            this.spriteX = x;
            this.spriteY = y;
            this.spriteWidth = width;
            this.spriteHeight = height;
			grounded = false;
			moving = false;
			pushing = false;
            held = false;
            rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);

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
        public Player2 getP2()
        {
            return p2;
        }
        public void setP2(Player2 p2)
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
            image = content.Load<Texture2D>("Aaron.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
        }

		public void Update(Controls controls, GameTime gameTime, List<Rectangle> collisionRects)
		{
			Move (controls, collisionRects);
			Jump (controls, gameTime);
		}

		public void Move(Controls controls, List<Rectangle> collisionRects)
		{

			// Sideways Acceleration
			if (controls.onPress(Keys.Right, Buttons.DPadRight))
				x_accel += speed;
			else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
				x_accel -= speed;
			if (controls.onPress(Keys.Left, Buttons.DPadLeft))
				x_accel -= speed;
			else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
				x_accel += speed;

			double playerFriction = pushing ? (friction * 3) : friction;
			x_vel = x_vel * (1 - playerFriction) + x_accel * .10;
			movedX = Convert.ToInt32(x_vel);
			spriteX += movedX;

			// Gravity
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

			grounded = false;

			// Check up/down collisions, then left/right
            rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
			checkYCollisions(collisionRects);
            

		}

        private void checkYCollisions(List<Rectangle> collisionRects)
        {
            //if (spriteY >= 300)
            //grounded = true;
            //else
            //grounded = false;

            //Reset collision dist
            collisionDist = Vector2.Zero;

            for (int i = 0; i < collisionRects.Count; i++)
            {
                if (IsColliding(rect, collisionRects[i]))
                {
                    //If there are multiple collision make sure we only react to the most severe
                    if (normal.Length() > collisionDist.Length())
                        collisionDist = normal;
                    grounded = true;
                }
            }
            //Update the players position
            float a = this.getX() + collisionDist.X;
            float b = this.getY() + collisionDist.Y;
            this.setX((int)a);
            this.setY((int)b);

            //for (int i = 0; i < collisionRects.Count; i++)
            //{
            //    if (IsColliding(player2.rect, map.collisionRects[i]))
            //    {
            //        //If there are multiple collision make sure we only react to the most severe
            //        if (normal.Length() > collisionDist2.Length())
            //            collisionDist2 = normal;
            //    }
            //}
            ////Update the players position

            //float c = player2.getX() + collisionDist2.X;
            //float d = player2.getY() + collisionDist2.Y;
            //player2.setX((int)c);
            //player2.setY((int)d);
            //base.Update(gameTime);
        }
		

		private void Jump(Controls controls, GameTime gameTime)
		{
			// Jump on button press
			if (controls.onPress(Keys.Up, Buttons.A) && grounded && !p2.getHold())
			{
                System.Diagnostics.Debug.WriteLine(held);
				y_vel = -10;
				jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
				grounded = false;
			}
			// Cut jump short on button release
			//else if (controls.onRelease(Keys.Space, Buttons.A) && y_vel < 0)
			//{
			//	y_vel /= 2;
			//}
		}
        public void Hold(Controls controls, Player2 p2)
        {
            p2 = this.p2;
            int sprite2Y = p2.getY() + 81;
            if (spriteY - 30 == sprite2Y && (spriteX - 15 <= p2.getX() && spriteX + 5 >= p2.getX()) && p2.getGrounded())
            {
                System.Diagnostics.Debug.WriteLine("Held is True");
                held = true;
            }
        }
        public void Drop(Controls controls, Player2 p2)
        {
            if (held)
            {
                setY(p2.getY() + 65);
                setX(p2.getX());
                if (controls.onPress(Keys.Down, Buttons.RightShoulder) && p2.getGrounded())
                {
                    System.Diagnostics.Debug.WriteLine("Held is False");
                    held = false;
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
