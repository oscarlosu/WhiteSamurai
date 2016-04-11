using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Prototype.Utils;

namespace Prototype.ScreenManager
{
    class ScreenManager : DrawableGameComponent
    {
        #region Fields
        /// <summary>
        /// List of the managed screens
        /// </summary>
        private List<Screen> screens;
        /// <summary>
        /// Shows if the screen manager has been initialized
        /// </summary>
        private bool isInitialized;
        /// <summary>
        /// SpriteBatch used to draw the contents of the different screens
        /// </summary>
        private SpriteBatch spriteBatch;
        /// <summary>
        /// Controls the input of the game
        /// </summary>
        private InputState input;
        /// <summary>
        /// Screen to draw
        /// </summary>
        private int activeScreen;
        /// <summary>
        /// Game's Graphics Device Manager
        /// </summary>
        private GraphicsDeviceManager graphics;
        #endregion

        #region Properties
        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }
        /// <summary>
        /// Controls the input of the game
        /// </summary>
        public InputState Input
        {
            get
            {
                return input;
            }
        }
        /// <summary>
        /// Screen height
        /// </summary>
        public int Height
        {
            get
            {
                return Game.Window.ClientBounds.Height;
            }
        }
        /// <summary>
        /// Screen width
        /// </summary>
        public int Width
        {
            get
            {
                return Game.Window.ClientBounds.Width;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor of the Screen Manager class
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="graphics">Graphics Device Manager</param>
        public ScreenManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.screens = new List<Screen>();
            this.input = new InputState();
            this.activeScreen = 0;
            this.graphics = graphics;
            this.isInitialized = false;
        }
        #endregion

        #region GameMethods
        /// <summary>
        /// Initializes the screen manager component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            MusicManager.Initialize();
            MusicManager.LoadContent(Game.Content);
            this.isInitialized = true;
        }
        /// <summary>
        /// Loads graphic contents
        /// </summary>
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            foreach (Screen screen in this.screens)
            {
                screen.LoadContent(content);
            }
        }

        /// <summary>
        /// Unloads graphic contents
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Screen screen in this.screens)
            {
                screen.UnloadContent();
            }
        }
        /// <summary>
        /// Allows the selected to run game logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            input.Update();
            MusicManager.Update(gameTime);
            if (this.activeScreen < screens.Count)
            {
                screens.ElementAt(this.activeScreen).Update(gameTime);
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Draws the different screens
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            if (screens.Count > this.activeScreen)
            {
                screens.ElementAt(this.activeScreen).Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a screen to the manager. If the Screen Manager has been initialized, loads the screen contents
        /// </summary>
        /// <param name="screen">New screen to be managed</param>
        public void addScreen(Screen screen)
        {
            //If the screen manager is initialized, the Contents for the screen must be loaded
            screen.ScreenManager = this;
            if (this.isInitialized == true)
            {
                screen.LoadContent(Game.Content);
            }

            screens.Add(screen);
        }
        /// <summary>
        /// Removes a screen
        /// </summary>
        /// <param name="screen">Screen to be removed</param>
        public void deleteScreen(Screen screen)
        {
            if (this.isInitialized == true && this.screens.Contains(screen))
            {
                screen.UnloadContent();
                screens.Remove(screen);
            }
        }
        /// <summary>
        /// Finds a screen in the screen list from the name
        /// </summary>
        /// <param name="name">Name of the screen</param>
        /// <returns>The screen if exists, null if not</returns>
        public Screen getScreen(String name)
        {
            foreach (Screen screen in screens)
            {
                if (screen.Name == name)
                {
                    return screen;
                }
            }
            return null;
        }
        /// <summary>
        /// Selects a screen to draw. If the screen is paused, then, this method makes it active.
        /// </summary>
        /// <param name="screenName">Nname of the screen</param>
        public void setActiveScreen(string screenName)
        {
            Screen src = this.getScreen(screenName);
            if (src != null)
            {
                this.activeScreen = this.screens.IndexOf(src);
            }
        }
        /// <summary>
        /// Changes the resolution of the screen
        /// </summary>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        public void setResolution(int width, int height)
        {
            Pair<int, int> oldRes = new Pair<int, int>(this.Width, this.Height);
            Pair<int, int> newRes;

            //Change the resolution
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.PreferredBackBufferWidth = width;
            graphics.ApplyChanges();

            newRes = new Pair<int, int>(this.Width, this.Height);
            //Apply changes to every screen
            foreach (Screen scr in screens)
            {
                scr.changeResolution(oldRes, newRes);
            }
        }

        /// <summary>
        /// Changes the resolution of the screen to the default resolution of the computer
        /// </summary>
        public void setResolution()
        {
            setResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }
        /// <summary>
        /// Closes the game
        /// </summary>
        public void exitGame()
        {
            this.Game.Exit();
        }
        #endregion



    }
}
