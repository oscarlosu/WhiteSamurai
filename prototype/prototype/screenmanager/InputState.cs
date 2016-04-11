using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Prototype.ScreenManager
{
    /// <summary>
    /// Class that saves the state of the input controllers at the beginning of the game loop
    /// </summary>
    public class InputState
    {
        #region fields
        /// <summary>
        /// Number of inputs to consider
        /// </summary>
        public const int MAX_INPUTS = 1;
        /// <summary>
        /// Checks if the gamepads are connected
        /// </summary>
        private bool[] isGamePadConnected;
        /// <summary>
        /// Current GamePads State
        /// </summary>
        private GamePadState[] currentGamePadState;
        /// <summary>
        /// Previous GamePads State
        /// </summary>
        private GamePadState[] previousGamePadState;
        /// <summary>
        /// Current Keyboard state
        /// </summary>
        private KeyboardState currentKeyboardState;
        /// <summary>
        /// Previous Keyboard state
        /// </summary>
        private KeyboardState previousKeyboardState;
        /// <summary>
        /// Current mouse state
        /// </summary>
        private MouseState currentMouseState;
        /// <summary>
        /// Previous mouse state
        /// </summary>
        private MouseState previousMouseState;
        #endregion

        #region properties
        /// <summary>
        /// <see cref="isGamePadConnected"/>
        /// </summary>
        public bool[] IsGamePadConnected
        {
            get
            {
                return isGamePadConnected;
            }
        }

        /// <summary>
        /// <see cref="currentGamePadState"/>
        /// </summary>
        public GamePadState[] CurrentGamePadState
        {
            get
            {
                return currentGamePadState;
            }
        }
        /// <summary>
        /// <see cref="previousGameState"/>
        /// </summary>
        public GamePadState[] PreviousGamePadState
        {
            get
            {
                return previousGamePadState;
            }
        }
        /// <summary>
        /// <see cref="currentKeyboardState"/>
        /// </summary>
        public KeyboardState CurrentKeyBoardState
        {
            get
            {
                return currentKeyboardState;
            }
        }
        /// <summary>
        /// <see cref="previousKeyboardState"/>
        /// </summary>
        public KeyboardState PreviousKeyboardState
        {
            get
            {
                return previousKeyboardState;
            }
        }
        /// <summary>
        /// <see cref="currentMouseState"/>
        /// </summary>
        public MouseState CurrentMouseState
        {
            get
            {
                return currentMouseState;
            }
        }
        /// <summary>
        /// <see cref="previousMouseState"/>
        /// </summary>
        public MouseState PreviousMouseState
        {
            get
            {
                return previousMouseState;
            }
        }

        #endregion

        #region constructors & update

        /// <summary>
        /// Constructor
        /// </summary>
        public InputState()
        {
            this.currentGamePadState = new GamePadState[MAX_INPUTS];
            this.previousGamePadState = new GamePadState[MAX_INPUTS];
            this.isGamePadConnected = new bool[MAX_INPUTS];
        }

        /// <summary>
        /// Updates the inputState
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MAX_INPUTS; ++i)
            {
                this.previousGamePadState[i] = this.currentGamePadState[i];
                this.currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                this.isGamePadConnected[i] = this.currentGamePadState[i].IsConnected;
            }

            this.previousKeyboardState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();
        }

        #endregion

        #region keyboard methods

        /// <summary>
        /// Controls if a key is pressed
        /// </summary>
        /// <param name="key">Key we wanto to check</param>
        /// <returns>True if it is pressed, False if not</returns>
        public bool isKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
        
        /// <summary>
        /// Controls if a key has been newly pressed.
        /// </summary>
        /// <param name="key">The key we want to know if it is pressed</param>
        /// <returns>True if has been pressed, and had not been mantained pressed, False if it is not pressed or has been mantained</returns>
        public bool isKeyNewlyPressed(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key) == false)
            {
                return false;
            }

            if (previousKeyboardState.IsKeyDown(key) == true)
                return false;

            return true;
        }

        #endregion

        #region gamepad methods

        /// <summary>
        /// Checks if a gamepad button has been pressed by a determinated player
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <param name="playerIndex">Player to be checked</param>
        /// <returns>True if the button is pressed, false if not</returns>
        public bool isGamePadButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            int plIndex = (int)playerIndex;
            if (plIndex >= MAX_INPUTS)
            {
                return false;
            }
            
            return currentGamePadState[plIndex].IsButtonDown(button);
          
        }

        /// <summary>
        /// Checks if a gamepad button has been pressed by any player
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <param name="playerIndex">Player that has pressed the button</param>
        /// <returns>True if someone has pressed the button, false if not</returns>
        public bool isGamePadButtonPressed(Buttons button, out PlayerIndex? playerIndex)
        {
            bool pressed;
            for (int i = 0; i < MAX_INPUTS; ++i)
            {
                playerIndex = (PlayerIndex)i;
                pressed = isGamePadButtonPressed(button, (PlayerIndex)i);
                if (pressed == true)
                    return true;
            }
            playerIndex = null;
            return false;
        }
        
        /// <summary>
        /// Controls if a GamePad button has been newly pressed
        /// </summary>
        /// <param name="button">The button we want to check if it is pressed</param>
        /// <param name="playerIndex">Index of the player we want to check</param>
        /// <returns>True if the button has been pressed, and has not been mantained pressed, False if it is not pressed, it has been mantained pressed
        /// or the player index exceeds the allowed number of players</returns>
        public bool isGamePadButtonNewlyPressed(Buttons button, PlayerIndex playerIndex)
        {
            int plIndex = (int)playerIndex;

            if (plIndex >= MAX_INPUTS) //If we access to a player that cannot play, return false
            {
                return false;
            }

            if (this.currentGamePadState[plIndex].IsButtonDown(button) == false)
            {
                return false;
            }

            if (this.previousGamePadState[plIndex].IsButtonDown(button) == true)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Controls if a player has pressed a button
        /// </summary>
        /// <param name="button">The button we need to know if it has been newly pressed.</param>
        /// <param name="playerIndex">The player that has pressed the button</param>
        /// <returns>True if the button has been pressed, False if it has not been pressed, or it has been mantained pressed since the last iteration
        /// of the game loop
        /// </returns>
        public bool isGamePadButtonNewlyPressed(Buttons button, out PlayerIndex? playerIndex)
        {
            bool result;
            for (int i = 0; i < MAX_INPUTS; ++i)
            {
                result = isGamePadButtonNewlyPressed(button, (PlayerIndex)i);
                if (result == true)
                {
                    playerIndex = (PlayerIndex) i;
                    return true;
                }
            }
            playerIndex = null;
            return false;
        }
        #endregion

        #region common methods
        /// <summary>
        /// Checks if input has changed
        /// </summary>
        /// <returns>True if input has changed, false if hasn't</returns>
        public bool hasInputChanged()
        {
            
            var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));
            var keysList = (Keys[])Enum.GetValues(typeof(Keys));
            if (this.IsGamePadConnected[0])
            {
                foreach (var button in buttonList)
                {
                    if (this.isGamePadButtonNewlyPressed(button, PlayerIndex.One))
                        return true;
                }
            }

            foreach (var key in keysList)
            {
                if (this.isKeyNewlyPressed(key))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
