using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    class Player : Sprite
    {
        private bool grounded;
        private bool inAir;
        private bool stand;
        private bool walk;
        private bool held;
        private bool inverted;

        private int speed;
        private int x_accel;
        public double x_vel;
        public double y_vel;
        public int movedX;

        public double gravity;
        public int maxFallSpeed;
        private double friction;
        private int jumpPoint = 0;

        public Player p2;
        public Rectangle rect;
        private Vector2 normal;
        private Vector2 collisionDist = Vector2.Zero;
        private ContentManager c;
        private SpriteEffects flip;
        public SoundEffect jumpEffect;
        
        private int time;
        
        public Player(int x, int y, int width, int height, bool inverted)
        {
            this.spriteX = x;
            this.spriteY = y;
            this.spriteWidth = width;
            this.spriteHeight = height;
            this.inverted = inverted;

            grounded = false;
            held = false;
            inAir = true;
            time = 0;
            stand = false;
            walk = false;
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
            jumpEffect = content.Load<SoundEffect>("jump.wav");
        }

        public void LoadContent(String imageName)
        {
            image = c.Load<Texture2D>(imageName);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), null, Color.White, 0.0f, new Vector2(0, 0), flip, 0.0f);
        }

        public void Update(Controls controls, GameTime gameTime, List<Rectangle> collisionRects)
        {
            Hold(controls);
            Drop(controls);
            Move(controls, collisionRects, gameTime);
            Jump(controls, gameTime);
        }

        public void Move(Controls controls, List<Rectangle> collisionRects, GameTime gameTime)
        {
            //Get time for timer for animations!
            time += gameTime.ElapsedGameTime.Milliseconds;

            //Standing animations! 500ms interval
            if (time > 500 && x_vel > -.5 && x_vel < .5)
            {
                if (!inverted)
                {
                    if (p2.getHold())
                    {
                        LoadContent("Aaronstandh");
                        time -= 500;
                    }
                    else if (!stand && !p2.getHold())
                    {
                        LoadContent("Aaronstand2");
                        stand = true;
                        time -= 500;
                    }
                    else
                    {
                        LoadContent("Aaronstand1");
                        stand = false;
                        time -= 500;
                    }
                }
                else
                {
                    if (p2.getHold())
                    {
                        LoadContent("Bennystandh");
                        time -= 500;
                    }
                    else if (!stand && !p2.getHold())
                    {
                        LoadContent("Bennystand2");
                        stand = true;
                        time -= 500;
                    }
                    else
                    {
                        LoadContent("Bennystand1");
                        stand = false;
                        time -= 500;
                    }
                }
            }

            //Walking Animations! 100ms interval
            if (time > 100 && (x_vel <= -.5 || x_vel >= .5))
            {
                if (!inverted)
                {
                    if (!walk && p2.getHold())
                    {
                        LoadContent("Aaronwalkh1");
                        walk = true;
                        time -= 100;
                    }
                    else if (walk && p2.getHold())
                    {
                        LoadContent("Aaronwalkh2");
                        walk = false;
                        time -= 100;
                    }
                    else if (!walk)
                    {
                        LoadContent("Aaronwalk1");
                        walk = true;
                        time -= 100;
                    }
                    else
                    {
                        LoadContent("Aaronwalk2");
                        walk = false;
                        time -= 100;
                    }
                }
                else
                {
                    if (!walk && p2.getHold())
                    {
                        LoadContent("Bennywalkh1");
                        walk = true;
                        time -= 100;
                    }
                    else if (walk && p2.getHold())
                    {
                        LoadContent("Bennywalkh2");
                        walk = false;
                        time -= 100;
                    }
                    else if (!walk)
                    {
                        LoadContent("Bennywalk1");
                        walk = true;
                        time -= 100;
                    }
                    else
                    {
                        LoadContent("Bennywalk2");
                        walk = false;
                        time -= 100;
                    }
                }
            }

            // Sideways Acceleration
            if (inverted)
            {
                if (controls.onPress(Keys.D, Buttons.DPadRight))
                {
                    x_accel += speed;
                    flip = SpriteEffects.None;
                }
                else if (controls.onRelease(Keys.D, Buttons.DPadRight))
                {
                    if (x_vel == 0)
                        x_accel = 0;
                    else
                        x_accel -= speed;
                }
                    
                if (controls.onPress(Keys.A, Buttons.DPadLeft))
                {
                    x_accel -= speed;
                    flip = SpriteEffects.FlipHorizontally;
                }
                else if (controls.onRelease(Keys.A, Buttons.DPadLeft))
                {
                    if (x_vel == 0)
                        x_accel = 0;
                    else
                        x_accel += speed;
                }
            }
            else
            {
                if (controls.onPress(Keys.Right, Buttons.DPadRight))
                {
                    x_accel += speed;
                    flip = SpriteEffects.None;
                }
                else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                {
                    if (x_vel == 0)
                        x_accel = 0;
                    else
                        x_accel -= speed;
                }
                if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                {
                    x_accel -= speed;
                    flip = SpriteEffects.FlipHorizontally;
                }
                else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                {
                    if (x_vel == 0)
                        x_accel = 0;
                    else
                        x_accel += speed;
                }
            }

            x_vel = x_vel * (1 - friction) + x_accel * .10;
            movedX = Convert.ToInt32(x_vel);
            spriteX += movedX;

            // Gravity
            if (inverted)
            {
                y_vel += gravity;
                if (y_vel < maxFallSpeed)
                    y_vel = maxFallSpeed;
                spriteY += Convert.ToInt32(y_vel);
            }
            else
            {
                y_vel += gravity;
                if (y_vel > maxFallSpeed)
                    y_vel = maxFallSpeed;
                spriteY += Convert.ToInt32(y_vel);
            }

            //grounded = false;
            if (inAir)
            {
                if (inverted)
                {
                    if (p2.getHold())
                        LoadContent("Bennyjumph.png");
                    else if (held)
                        LoadContent("Bennyheld.png");
                    else
                        LoadContent("Bennyjump.png");
                }
                else
                {
                    if (p2.getHold())
                        LoadContent("Aaronjumph.png");
                    else if (held)
                        LoadContent("Aaronheld.png");
                    else
                        LoadContent("Aaronjump.png");
                }
            }

            // Check up/down collisions, then left/right
            // If held, don't check
            
            if (p2.getHold())
            {
                if (!inverted)
                {
                    rect = new Rectangle(spriteX, spriteY - 64, spriteWidth, spriteHeight * 2);
                    checkYCollisions(collisionRects);
                }
                else
                {
                    rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight * 2);
                    checkYCollisions(collisionRects);
                }
                
            }
            else if (!held)
            {
                rect = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
                checkYCollisions(collisionRects);
            }

        }

        private void checkYCollisions(List<Rectangle> collisionRects)
        {
            //Reset collision dist
            collisionDist = Vector2.Zero;
            bool collided = false;
            for (int i = 0; i < collisionRects.Count; i++)
            {

                if (IsColliding(rect, collisionRects[i]))
                {
                    collided = true;
                    //If there are multiple collision make sure we only react to the most severe
                    if (normal.Length() > collisionDist.Length())
                        collisionDist = normal;
                    //Avoid being grounded on wall
                    if (collisionDist.X == 0)
                    {
                        grounded = true;
                        inAir = false;
                        y_vel = 0;
                        //Fixing animation in case of wall collision
                        if (inverted)
                            if (x_vel < .5 && x_vel > -.5)
                            {
                                if (stand && !p2.getHold())
                                {
                                    LoadContent("Bennystand2");
                                }
                                else if (p2.getHold())
                                {
                                    LoadContent("Bennystandh");
                                }
                                else
                                {
                                    LoadContent("Bennystand1");
                                }
                            }
                            else
                            {
                                if (!walk && p2.getHold())
                                {
                                    LoadContent("Bennywalkh1");
                                }
                                else if (walk && p2.getHold())
                                {
                                    LoadContent("Bennywalkh2");
                                }
                                else if (walk)
                                {
                                    LoadContent("Bennywalk1");
                                }
                                else
                                {
                                    LoadContent("Bennywalk2");
                                }
                            }
                        else
                        {
                            if (x_vel < .5 && x_vel > -.5)
                            {
                                if (stand && !p2.getHold())
                                {
                                    LoadContent("Aaronstand2");
                                }
                                else if (p2.getHold())
                                {
                                    LoadContent("Aaronstandh");
                                }
                                else
                                {
                                    LoadContent("Aaronstand1");
                                }
                            }
                            else
                            {
                                if (!walk && p2.getHold())
                                {
                                    LoadContent("Aaronwalkh1");
                                }
                                else if (walk && p2.getHold())
                                {
                                    LoadContent("Aaronwalkh2");
                                }
                                else if (walk)
                                {
                                    LoadContent("Aaronwalk1");
                                }
                                else
                                {
                                    LoadContent("Aaronwalk2");
                                }
                            }
                        }

                    }
                    else
                    {
                        grounded = false;
                    }

                }

            }
            if (!collided)
                inAir = true;


            //Update the players position
            double a = this.getX() + collisionDist.X;
            double b = this.getY() + collisionDist.Y; //Hacky code :)
            this.setX((int)a);
            this.setY((int)b);


        }


        private void Jump(Controls controls, GameTime gameTime)
        {
            // Jump on button press
            if (inverted)
            {
                if (controls.onPress(Keys.S, Buttons.A) && grounded && !p2.getHold())
                {
                    LoadContent("Bennyjump.png");
                    y_vel = 9.8;
                    jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                    grounded = false;
                    jumpEffect.Play();
                }
            }
            else
            {
                if (controls.onPress(Keys.Up, Buttons.A) && grounded && !p2.getHold())
                {
                    LoadContent("Aaronjump.png");
                    y_vel = -9.8;
                    jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                    grounded = false;
                    jumpEffect.Play();
                }
            }
        }

        //Holding. Jump within a certain threshold to hold.
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

        //Dropped.
        public void Drop(Controls controls)
        {
            if (held)
            {


                if (inverted)
                {
                    setY(p2.getY() - 53);
                    setX(p2.getX());
                    if (controls.onPress(Keys.W, Buttons.LeftShoulder) && p2.getGrounded())
                    {
                        setY(p2.getY() - 74);
                        held = false;
                    }
                }
                else
                {
                    setY(p2.getY() + 53);
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
            double xMag, yMag;

            //Calculate the difference in position of the two rectangles
            distance = body1Centre - body2Centre;

            //Get the combined half heights/widths of the rects
            double xAdd = ((body1.Width) + (body2.Width)) / 2;
            double yAdd = ((body1.Height) + (body2.Height)) / 2;

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
                normal.X = (float)((distance.X > 0) ? xMag : -xMag);
            else
                normal.Y = (float)((distance.Y > 0) ? yMag : -yMag);

            ////There was a collision, return true
            return true;
        }

    }
}
