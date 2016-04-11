using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Prototype.Utils;
using Microsoft.Xna.Framework.Input;

namespace Prototype.ScreenManager.Screens
{
    class ControlsScreen : Screen
    {
        private Texture2D controls;
        private String controlsImage;
        private Screen previousScreen;
        /// <summary>
        /// Constructor
        /// </summary>
        public ControlsScreen(String controlsImage, Screen previousScreen)
            : base("Controls Screen")
        {
            this.controlsImage = controlsImage;
            this.previousScreen = previousScreen;
        }

        /// <summary>
        /// Loads content
        /// <see cref="Screen.LoadContent"/>
        /// </summary>
        /// <param name="content">Content manager</param>
        public override void LoadContent(ContentManager content)
        {
            controls = content.Load<Texture2D>(controlsImage);
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
            if (input.isKeyNewlyPressed(Keys.Escape) || input.isGamePadButtonNewlyPressed(Buttons.B, PlayerIndex.One))
            {
                this.ScreenManager.deleteScreen(this);
                this.ScreenManager.setActiveScreen(previousScreen.Name);
            }
        }

        /// <summary>
        /// Draws the screen
        /// <see cref="Screen.Draw"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(controls, new Rectangle(ScreenManager.Width/4, ScreenManager.Height/4, ScreenManager.Width/2, ScreenManager.Height/2), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            
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
