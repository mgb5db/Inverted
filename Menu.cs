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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string[] items;
        int selection;
        bool gameStart;

        Texture2D selectionArrow;
        Texture2D menubg;
        Vector2 arrowLocation;

        KeyboardState oldState_;

        SoundEffect select;
        SoundEffect start;
        Song theme;

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
            //Selection of menu option
            selection = 0;

            //String of menu options
            //That don't matter too much because we cheese it with an image
            items = new string[] { "New Game", "Credits", "Exit" };
            oldState_ = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //For Some reason getting the SpriteBatch from Services doesn't work.
            //Just initialized it with a new one. No issues to far.
            //spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));

            menubg = Game.Content.Load<Texture2D>("MainMenu");
            selectionArrow = Game.Content.Load<Texture2D>("SelectArrow");
            arrowLocation = new Vector2(970, 470);

            //theme = Game.Content.Load<Song>("WWWW.wav");           
            select = Game.Content.Load<SoundEffect>("select.wav");
            start = Game.Content.Load<SoundEffect>("start.wav");

            MediaPlayer.Volume = 1.0f;
            //MediaPlayer.Play(theme);

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
                select.Play();
            }
            else if (newPressedKeys.Contains(Keys.Up))
            {
                selection--;
                selection = (selection < 0 ? items.Length - 1 : selection);
                select.Play();
            }
            else if (newPressedKeys.Contains(Keys.Enter))
            {
                menuAction();
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
            spriteBatch.Draw(menubg, new Rectangle(0, 0, 1280, 768), Color.White);
            
            // Draw items
            if (selection == 0)
            {
                spriteBatch.Draw(selectionArrow, arrowLocation);
            }
            if (selection == 1)
            {
                spriteBatch.Draw(selectionArrow, new Vector2(970, 570));
            }
            if (selection == 2)
            {
                spriteBatch.Draw(selectionArrow, new Vector2(970, 670));
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
                    start.Play();
                    Game.Components.Add(new Screen(Game, "Tutorial1"));
                    //Game.Components.Add(new GameLoop(Game, 1));
                    break;
                case ("Credits"):
                    start.Play();
                    Game.Components.Add(new Screen(Game, "Credits"));
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