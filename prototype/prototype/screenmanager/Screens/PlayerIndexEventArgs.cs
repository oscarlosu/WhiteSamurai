using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Prototype.ScreenManager.Screens
{
    /// <summary>
    /// Auxiliary class for event handlers in menus. Contains the player index that selects the menu entry
    /// </summary>
    class PlayerIndexEventArgs : EventArgs
    {
        /// <summary>
        /// Player index
        /// </summary>
        private PlayerIndex playerIndex;

        /// <summary>
        /// keyboard
        /// </summary>
        private Boolean keyboard;

        /// <summary>
        /// <see cref="playerIndex"/>
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get
            {
                return this.playerIndex;
            }
        }

        /// <summary>
        /// <see cref="keyboard"/>
        /// </summary>
        public Boolean Keyboard
        {
            get
            {
                return this.keyboard;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pl">Player index</param>
        public PlayerIndexEventArgs(PlayerIndex pl, bool keyboard)
        {
            this.playerIndex = pl;
            this.keyboard = keyboard;
        }
    }
}
