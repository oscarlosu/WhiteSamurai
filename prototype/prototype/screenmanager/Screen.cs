using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Prototype.Utils;

namespace Prototype.ScreenManager
{
    abstract class Screen
    {
        #region Fields
        /// <summary>
        /// Name of the screen
        /// </summary>
        private String name;

        /// <summary>
        /// Screen manager which contains the screen
        /// </summary>
        private ScreenManager sm;
        #endregion

        #region Properties
        /// <summary>
        /// Property that allows obtaining the value of name, and changing it from other classes.
        /// </summary>
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Property that allows obtaining the screen manager or changing it
        /// </summary>
        public ScreenManager ScreenManager
        {
            get
            {
                return sm;
            }
            set
            {
                sm = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the class screen
        /// </summary>
        /// <param name="name">Name given to the screen</param>
        public Screen(String name)
        {
            this.name = name;
        }
        #endregion

        #region GameMethods
        /// <summary>
        /// Loads the contents to be used by the screen
        /// </summary>
        /// <param name="content">Content manager</param>
        abstract public void LoadContent(ContentManager content);
        /// <summary>
        /// Unloads the contents used by the screen
        /// </summary>
        abstract public void UnloadContent();
        /// <summary>
        /// Updates the several elements of the screen in the game loop
        /// </summary>
        /// <param name="gameTime">Game timing state</param>
        /// <param name="sm">Screen Manager</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Handles the input for the screen
        /// </summary>
        /// <param name="input">Input state</param>
        abstract public void HandleInput(InputState input);
        /// <summary>
        /// Draws the contents of the screen
        /// </summary>
        /// <param name="spriteBatch">Initialized spriteBatch to use in the drawing of the screen</param>
        abstract public void Draw(SpriteBatch spriteBatch);
        #endregion

        #region Methods
        /// <summary>
        /// Closes a screen. That screen will not be in the screen manager anymore
        /// </summary>
        public virtual void ExitScreen()
        {
            sm.deleteScreen(this);
        }

        /// <summary>
        /// Changes the resolution of the screen
        /// </summary>
        /// <param name="oldRes">Old resolution</param>
        /// <param name="newRes">New resolution</param>
        public abstract void changeResolution(Pair<int, int> oldRes, Pair<int, int> newRes);
        #endregion
    }
}
