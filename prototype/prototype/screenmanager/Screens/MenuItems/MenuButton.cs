using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Prototype.ScreenManager.Screens
{
    class MenuButton : MenuEntry
    {
        /// <summary>
        /// Name of the image file button to show when the entry is highlighted
        /// </summary>
        String highButtonString;
        /// <summary>
        /// Name of the image file button to show when the entry is not highlighted
        /// </summary>
        String basicButtonString;
        /// <summary>
        /// Image of the button to show when the entry is highlighted
        /// </summary>
        Texture2D highligthedButton;
        /// <summary>
        /// Image of the button to show when the entry is not highlighted
        /// </summary>
        Texture2D basicButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text of the button</param>
        /// <param name="highlightedButton">Name of the image file button to show when the entry is highlighted</param>
        /// <param name="basicButton">Name of the image file button to show when the entry is not highlighted</param>
        public MenuButton(String text, String highlightedButton, String basicButton)
            : base(text)
        {
            this.highButtonString = highlightedButton;
            this.basicButtonString = basicButton;
        }

        /// <summary>
        /// Loads the content of the entry. In this case, loads the two textures for the buttons
        /// <see cref="MenuEntry.LoadContent"/>
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            highligthedButton = content.Load<Texture2D>(highButtonString);
            basicButton = content.Load<Texture2D>(basicButtonString);
        }

        /// <summary>
        /// Unloads content
        /// </summary>
        public override void UnloadContent()
        {
        }

        /// <summary>
        /// Draws the complete button
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        /// <param name="font">Font</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (this.isHighlight == true)
            {
                spriteBatch.Draw(highligthedButton, new Rectangle((int)Position.X, (int)Position.Y, (int)Position.Z, (int)Position.W),null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(basicButton, new Rectangle((int)Position.X, (int)Position.Y, (int)Position.Z, (int)Position.W), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
            }
        }

        /// <summary>
        /// Draws the visible part of the button between a minimum height and a maximum height of the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="font">Font</param>
        /// <param name="minHeight">minimum height of the screen</param>
        /// <param name="maxHeight">maximum height of the screen</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, int minHeight, int maxHeight)
        {
            float heightBottom = Position.Y + Position.W - minHeight;
            float heightTop = maxHeight - Position.Y;

            //If the entry is not visible, we don't have to draw it
            if (!this.isVisible)
                return;

            //If we can draw the complete button
            if (Position.Y >= minHeight && Position.Y + Position.W <= maxHeight)
            {
                Draw(spriteBatch, font);
            }
            else if (Position.Y >= minHeight) //If the bottom of the button is out of the area
            {
                if (this.isHighlight)
                {
                    spriteBatch.Draw(highligthedButton, new Rectangle((int)Position.X, (int)Position.Y, (int)Position.Z, (int)heightTop), new Rectangle(0, 0, basicButton.Width, (int)(heightTop * ((float)basicButton.Height / Position.W))), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
                }
                else
                {
                    spriteBatch.Draw(basicButton, new Rectangle((int)Position.X, (int)Position.Y, (int)Position.Z, (int)heightTop), new Rectangle(0, 0, basicButton.Width, (int)(heightTop * ((float)basicButton.Height / Position.W))), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
                }
            }
            else if (Position.Y + Position.W <= maxHeight)
            {
                Rectangle destiny = new Rectangle((int)Position.X, (int)minHeight, (int)Position.Z, (int)heightBottom);
                int final = (int)(basicButton.Height - heightBottom * (((float)basicButton.Height) / Position.W));
                Rectangle source = new Rectangle(0, (int)(basicButton.Height - heightBottom * (((float)basicButton.Height) / Position.W)), basicButton.Width, basicButton.Height - final);
                spriteBatch.Draw(basicButton, destiny, source, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
            }
        }
    }
}
