/**
 * @file Game1.cs
 * @author Oscar Losada & Javier Sanz-Cruzado
 * @brief Class definition for Game1
 * @date 28/09/2014
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Prototype.TileEngine;
using Prototype.ScreenManager;
using Prototype.ScreenManager.Screens;
using Prototype.TileEngine.IsometricView;
using Prototype.Characters;
using Prototype.Characters.CharacterIO;

namespace Prototype
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        

        public static SpriteFont font;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ScreenManager.ScreenManager sm = new ScreenManager.ScreenManager(this, this.graphics);
            Components.Add(sm);

            Screen inicial = new InitialScreen();

            sm.addScreen(inicial);
            sm.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           // spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
         
            font = Content.Load<SpriteFont>("Fonts/Kootenay");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

           /* if (GamePad.GetState(PlayerIndex.One).IsConnected  && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            */
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
          /*  spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            // Draw map
            TileMap.getInstance().Draw(spriteBatch, Player.getInstance().Position);
            Player.getInstance().Draw(spriteBatch);
            spriteBatch.End();
            */
            base.Draw(gameTime);
        }
    }
}
