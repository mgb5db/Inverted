using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Platformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// Optionally takes a GameLoop argument, when the menu must be able to
    /// resume the current GameLoop. Otherwise, the reference would be lost
    /// and the gameLoop garbage collected.
    public class Menu : DrawableGameComponent
    {

        GameLoop gameLoop;
        SoundBank soundBank;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont menuItem;
        string[] items;
        int selection;
        bool gameStart;
        Texture2D title;
        Texture2D selectionArrow;
        Texture2D menubg;
        Vector2 arrowLocation;

        KeyboardState oldState_;

        public Menu(Game game, GameLoop gameLoop)
            : base(game)
        {
            this.gameLoop = gameLoop;
            gameStart = (gameLoop == null);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run. This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //soundBank = (SoundBank)Game.Services.GetService(typeof(SoundBank));
            selection = 0;
            items = new string[] { "New Game", "Exit" };
            oldState_ = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));

            menubg = Game.Content.Load<Texture2D>("MainMenu");
            selectionArrow = Game.Content.Load<Texture2D>("SelectArrow");
            arrowLocation = new Vector2(900, 500);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Wonder why we test for this condition? Just replace gameStart_ by true and
            // try running the game. The answer should be instantaneous.
            if (gameStart)
            {
                //soundBank.PlayCue("NewLevel");
                gameStart = false;
            }

            KeyboardState newState = Keyboard.GetState();

            // Get keys pressed now that weren't pressed before
            var newPressedKeys = from k in newState.GetPressedKeys()
                                 where !(oldState_.GetPressedKeys().Contains(k))
                                 select k;

            // Scroll through menu items
            if (newPressedKeys.Contains(Keys.Down))
            {
                selection++;
                selection %= items.Length;
                Console.WriteLine(selection);
                //soundBank.PlayCue("PacMAnEat1");
                Console.WriteLine("Down!");
            }
            else if (newPressedKeys.Contains(Keys.Up))
            {
                selection--;
                selection = (selection < 0 ? items.Length - 1 : selection);
                Console.WriteLine(selection);
                //soundBank.PlayCue("PacManEat2");
                Console.WriteLine("Up!");
            }
            else if (newPressedKeys.Contains(Keys.Enter))
            {
                menuAction();
                Console.WriteLine("Enter!");
            }

            // Update keyboard state for next update
            oldState_ = newState;


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // The menu is a main component, so it is responsible for initializing the sprite batch each frame
            spriteBatch.Begin();

            // Draw title
            spriteBatch.Draw(menubg, new Vector2(0, 0));
            
            // Draw items
            if (selection == 0)
            {
                spriteBatch.Draw(selectionArrow, arrowLocation);
            }
            if (selection == 1)
            {
                spriteBatch.Draw(selectionArrow, new Vector2(900,620));
            }  
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void menuAction()
        {
            Game.Components.Remove(this);
            switch (items[selection])
            {
                case ("New Game"):
                    Game.Components.Add(new GameLoop(Game));
                    break;
                case ("Exit"):
                    Game.Exit();
                    break;
                default:
                    throw new ArgumentException("\"" + items[selection] + "\" is not a valid case");

            }
        }

    }
}