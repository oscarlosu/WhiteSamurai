using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Prototype.TileEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Prototype.Utils;
using Microsoft.Xna.Framework.Media;

namespace Prototype.ScreenManager.Screens
{
    class InitialScreen : Screen
    {
        /// <summary>
        /// Font to print the text to change screens
        /// </summary>
        private SpriteFont continueFont;

        /// <summary>
        /// game logo
        /// </summary>
        private Texture2D logo;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public InitialScreen()
            : base("Initial Screen")
        {
        }

        /// <summary>
        /// Loads content
        /// <see cref="Screen.LoadContent"/>
        /// </summary>
        /// <param name="content">Content manager</param>
        public override void LoadContent(ContentManager content)
        {
            continueFont = content.Load<SpriteFont>("Fonts/menuFont");
            logo = content.Load<Texture2D>("Images/WhiteSamuraiLogo");
        }

        /// <summary>
        /// Unloads content
        /// <see cref="Screen.UnloadContent"/>
        /// </summary>
        public override void UnloadContent()
        {

        }

        /// <summary>
        /// Game loop method.
        /// <see cref="Screen.Update"/>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this.HandleInput(ScreenManager.Input);
        }

        /// <summary>
        /// Handles the input. If key Enter or button Start is pressed, then, changes to MainMenuScreen
        /// <see cref="Screen.HandleInput"/>
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputState input)
        {
            if (input.hasInputChanged())
            {
                this.ScreenManager.addScreen(new MainMenuScreen(this.ScreenManager.Height / 4, this.ScreenManager.Height - 10));
                this.ScreenManager.deleteScreen(this);
            }
        }

        /// <summary>
        /// Draws the screen
        /// <see cref="Screen.Draw"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            int width = 3 * ScreenManager.Width / 4;
            int height = 20 + ScreenManager.Height / 4;
            spriteBatch.Draw(logo, new Rectangle(ScreenManager.Width/4, ScreenManager.Height/5, ScreenManager.Width / 2, ScreenManager.Height / 3), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.DrawString(continueFont, "Press any key", new Vector2(ScreenManager.Width/2 - continueFont.MeasureString("Press any key").X/2, 8 * ScreenManager.Height/15), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Changes the resolution of the screen. Nothing needs to be done in this method
        /// <see cref="Screen.changeResolution"/>
        /// </summary>
        /// <param name="oldRes">Old resolution</param>
        /// <param name="newRes">New resolution</param>
        public override void changeResolution(Pair<int, int> oldRes, Pair<int, int> newRes)
        {
        }
    }
}
