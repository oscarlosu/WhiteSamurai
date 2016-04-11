using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Prototype.Utils;
using Microsoft.Xna.Framework.Media;


namespace Prototype.ScreenManager.Screens
{
    class EndGameScreen : Screen
    {
        /// <summary>
        /// Font to print the game over message
        /// </summary>
        private SpriteFont gameOverFont;
        /// <summary>
        /// Font to print the text to change screens
        /// </summary>
        private SpriteFont continueFont;
        /// <summary>
        /// background image
        /// </summary>
        private Texture2D background;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndGameScreen()
            : base("End Game Screen")
        {
            GameState.GameState.GameWon = false;
        }

        /// <summary>
        /// Loads content
        /// <see cref="Screen.LoadContent"/>
        /// </summary>
        /// <param name="content">Content manager</param>
        public override void LoadContent(ContentManager content)
        {
            continueFont = content.Load<SpriteFont>("Fonts/menuFont");
            gameOverFont = content.Load<SpriteFont>("Fonts/Title");
            background = content.Load<Texture2D>("Images/Backgrounds/background");
            MusicManager.startSong("gameOverSong");

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
                MusicManager.endSong();
                this.ScreenManager.addScreen(new InitialScreen());
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
            spriteBatch.Draw(background, new Rectangle(0, ScreenManager.Height / 2, ScreenManager.Width, ScreenManager.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(gameOverFont, "The End", new Vector2(ScreenManager.Width/2 - continueFont.MeasureString("The End").X, ScreenManager.Height/2 - continueFont.MeasureString("Game Over").Y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(gameOverFont, "The End", new Vector2(ScreenManager.Width/2 - continueFont.MeasureString("The End").X, ScreenManager.Height / 2 - continueFont.MeasureString("Game Over").Y/2), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.FlipVertically, 1);
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
