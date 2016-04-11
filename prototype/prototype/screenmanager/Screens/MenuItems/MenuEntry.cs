using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Prototype.ScreenManager.Screens
{
    abstract class MenuEntry
    {
        #region Fields
        /// <summary>
        /// Text that represents the menu entry
        /// </summary>
        protected String text;
        /// <summary>
        /// Indicates if the entry is highlighted or not
        /// </summary>
        protected bool isHighlight;
        /// <summary>
        /// Indicates if the entry is visible or not
        /// </summary>
        protected bool isVisible = true;
        /// <summary>
        /// Position and size of the menuEntry (X and Y are the positions, and Z and W are width and height)
        /// </summary>
        protected Vector4 position;
        /// <summary>
        /// Event handler for the entry
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;
        #endregion


        #region Properties
        /// <summary>
        /// Position
        /// </summary>
        public Vector4 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text that represents the menu entry</param>
        public MenuEntry(string text)
        {
            this.text = text;
        }
        #endregion




        /// <summary>
        /// If the entry is selected, executes the handler
        /// </summary>
        /// <param name="playerIndex"></param>
        public virtual void OnSelectedEntry(PlayerIndex playerIndex, bool keyboard)
        {
            if (Selected != null)
            {
                Selected(this, new PlayerIndexEventArgs(playerIndex, keyboard));
            }
        }

        /// <summary>
        /// Highlights the entry
        /// </summary>
        public void highlight()
        {
            this.isHighlight = true;
        }

        /// <summary>
        /// The entry stops being highlighted
        /// </summary>
        public void unHighlight()
        {
            this.isHighlight = false;
        }

        /// <summary>
        /// The entry stops drawing
        /// </summary>
        public void Hide()
        {
            this.isVisible = false;
        }

        /// <summary>
        /// The entry stops drawing
        /// </summary>
        public void Show()
        {
            this.isVisible = true;
        }


        /// <summary>
        /// Draws the entry
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="font">Font</param>
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font);
        /// <summary>
        /// Draws the entry partially
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="font">Font</param>
        /// <param name="minHeight">Minimum Height of the screen area for the menu</param>
        /// <param name="maxHeight">Maximum height of the screen area for the menu</param>
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font, int minHeight, int maxHeight);
        /// <summary>
        /// Loads content
        /// </summary>
        /// <param name="content">Content manager</param>
        public abstract void LoadContent(ContentManager content);
        /// <summary>
        /// Unloads content
        /// </summary>
        public abstract void UnloadContent();
    }
}
