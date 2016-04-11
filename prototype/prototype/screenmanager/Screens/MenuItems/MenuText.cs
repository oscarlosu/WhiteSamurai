using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype.ScreenManager.Screens
{
    class MenuText : MenuEntry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text to draw</param>
        public MenuText(String text)
            : base(text)
        {
        }

        /// <summary>
        /// Loads content. In this case, nothing is done
        /// <see cref="MenuEntry.LoadContent"/>
        /// </summary>
        /// <param name="content">Content loader</param>
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {

        }

        /// <summary>
        /// Unloads content.
        /// <see cref="MenuEntry.UnloadContent"/>
        /// </summary>
        public override void UnloadContent()
        {
        }

        /// <summary>
        /// Draws the string
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="font">Font</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color menuColor;
            if (isHighlight == true)
            {
                menuColor = Color.Red;
            }
            else
            {
                menuColor = Color.Black;
            }

            spriteBatch.DrawString(font, text, new Vector2(Position.X, Position.Y), menuColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Draws the string if is fully visible
        /// </summary>
        /// <param name="spriteBatch">sprite batch</param>
        /// <param name="font">Font</param>
        /// <param name="minHeight">Minimum height of the screen</param>
        /// <param name="maxHeight">Maximum height of the screen</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, int minHeight, int maxHeight)
        {
            //If the entry is not visible, we don't have to draw it
            if (!this.isVisible)
                return;

            //If we can draw the complete string
            if (Position.Y >= minHeight && Position.Y + font.MeasureString(text).Y <= maxHeight)
            {
                Draw(spriteBatch, font);
            }
            else
            {
                return;
            }
        }
    }
}
