using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prototype.Utils;
using Microsoft.Xna.Framework.Media;


namespace Prototype.ScreenManager.Screens
{
    class MainMenuScreen : MenuScreen
    {
        /// <summary>
        /// background
        /// </summary>
        Texture2D background;
        /// <summary>
        /// game logo
        /// </summary>
        Texture2D logo;
        /// <summary>
        /// Constructor of the main menu screen
        /// </summary>
        /// <param name="minHeight">Minimum height for the menu items</param>
        /// <param name="maxHeight">Maximum height for the menu items</param>
        public MainMenuScreen(int minHeight, int maxHeight)
            : base("Main menu", minHeight, maxHeight)
        {

        }

        /// <summary>
        /// Loads content. Adds the different menu entries of this menu (Buttons "New Game", "Load", "Controls", "Options" and "Exit").
        /// Also loads the background and the logo
        /// <see cref="Screen.LoadContent"/>
        /// </summary>
        /// <param name="content">Content loader</param>
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
            int width = ScreenManager.Width;
            int height = ScreenManager.Height;

            int horizInitial = Math.Min(2 * width / 3, width - (9 * height / 20) - 20);

            MenuEntry menEntr = new MenuButton("New game", "Images/Buttons/PlayColor", "Images/Buttons/PlayBlackWhite");
            menEntr.Selected += onSelectedNewGame;
            menEntr.Position = new Vector4(horizInitial, 12 * height / 40, (8 * height) / 20, 8.0f/3.0f * height / 20);
            this.AddEntry(menEntr);

            /*menEntr = new MenuButton("Load", "Images/Buttons/LoadColor", "Images/Buttons/LoadBlackWhite");
            menEntr.Selected += onSelectedLoadScreen;
            menEntr.Position = new Vector4(horizInitial, 17 * height / 40, (9 * height) / 20, 3 * height / 20);
            this.AddEntry(menEntr);
            */
            menEntr = new MenuButton("Controls", "Images/Buttons/ControlsColor", "Images/Buttons/ControlsBlackWhite");
            menEntr.Selected += onSelectedControlScreen;
            menEntr.Position = new Vector4(horizInitial, 19 * height / 40, (8 * height) / 20, 8.0f / 3.0f * height / 20);
            this.AddEntry(menEntr);

            menEntr = new MenuButton("Options", "Images/Buttons/OptionsColor", "Images/Buttons/OptionsBlackWhite");
            menEntr.Selected += onSelectedOptionsMenuScreen;
            menEntr.Position = new Vector4(horizInitial, 26 * height / 40, (8 * height) / 20, 8.0f / 3.0f * height / 20);
            this.AddEntry(menEntr);

            menEntr = new MenuButton("Credits", "Images/Buttons/CreditsColor", "Images/Buttons/CreditsBlackWhite");
            menEntr.Selected += onSelectedOptionsCreditsScreen;
            menEntr.Position = new Vector4(horizInitial, 33 * height / 40, (8 * height) / 20, 8.0f / 3.0f * height / 20);
            this.AddEntry(menEntr);

            menEntr = new MenuButton("Exit", "Images/Buttons/ExitColor", "Images/Buttons/ExitBlackWhite");
            menEntr.Selected += onSelectedExit;
            menEntr.Position = new Vector4(horizInitial, 40 * height / 40, (8 * height) / 20, 8.0f / 3.0f * height / 20);
            this.AddEntry(menEntr);

            foreach (MenuEntry menuEntr in menuEntries)
            {
                if (isEntryVisible(menuEntr))
                {
                    menuEntr.Show();
                }
                else
                {
                    menuEntr.Hide();
                }
            }

            background = content.Load<Texture2D>("Images/Backgrounds/MainMenuBackground");
            logo = content.Load<Texture2D>("Images/WhiteSamuraiLogo");

            MusicManager.changeSong("menuSong");

        }

        /// <summary>
        /// Updates the screen
        /// </summary>
        /// <param name="gameTime">game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Unloads the content of the screen
        /// </summary>
        public override void UnloadContent()
        {
            return;
        }

        /// <summary>
        /// If the new game option is chosen, starts a game screen
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        public void onSelectedNewGame(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new LoadingScreen(ScreenManager, true, new GameScreen()));

            base.onCancel();

        }

        /// <summary>
        /// If the load option is chosen, starts a load screen (TODO)
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        /*public void onSelectedLoadScreen(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new GameScreen());
            base.onCancel();
        }*/

        /// <summary>
        /// If the controls option is chosen, starts the controls screen
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        public void onSelectedControlScreen(object ev, PlayerIndexEventArgs plIndex)
        {
            if (plIndex.Keyboard)
            {
                this.ScreenManager.addScreen(new ControlsScreen("Images/Controls/KeyBoardControls", this));
            }
            else
            {
                this.ScreenManager.addScreen(new ControlsScreen("Images/Controls/controllerControls", this));
            }
            this.ScreenManager.setActiveScreen("Controls Screen");
        }

        /// <summary>
        /// If the options option is chosen, starts the options menu screen (TODO)
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        public void onSelectedOptionsMenuScreen(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new OptionsMenu(0, ScreenManager.Height, this));
            this.ScreenManager.setActiveScreen("Options Menu");
        }

        /// <summary>
        /// If the options option is chosen, starts the options menu screen (TODO)
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        public void onSelectedOptionsCreditsScreen(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new CreditsScreen(ScreenManager.Height / 30, ScreenManager.Height - 10, this));
            this.ScreenManager.setActiveScreen("Credits Screen");
        }
        /// <summary>
        /// If the exit option is chosen, exits the game
        /// </summary>
        /// <param name="ev">Event</param>
        /// <param name="plIndex">Player index that has selected the option</param>
        public void onSelectedExit(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.exitGame();
        }

        /// <summary>
        /// Action to do if the menu is cancelled. In this case, returns to initialScreen
        /// </summary>
        public override void onCancel()
        {
            MusicManager.endSong();
            this.ScreenManager.addScreen(new InitialScreen());
            base.onCancel();
        }

        /// <summary>
        /// Draws the screen
        /// <see cref="Screen.Draw"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(ScreenManager.Width/10, ScreenManager.Height/10, 4*ScreenManager.Width/5, 4*ScreenManager.Height/5), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(logo, new Rectangle(ScreenManager.Width - 4 * ScreenManager.Height / 5, 20, 3 * ScreenManager.Height / 4, ScreenManager.Height / 4), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

    }
}
