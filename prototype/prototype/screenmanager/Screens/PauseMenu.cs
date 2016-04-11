using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prototype.TileEngine;
using Prototype.Utils;
using Prototype.TileEngine.IsometricView;

namespace Prototype.ScreenManager.Screens
{
    class PauseMenu : MenuScreen
    {

        Texture2D background;
        public PauseMenu(int minHeight, int maxHeight, Screen previous)
            : base("Pause menu", minHeight, maxHeight, previous)
        {
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            MenuEntry menEntr;
            int width = ScreenManager.Width;
            int height = ScreenManager.Height;

            menEntr = new MenuButton("Options", "Images/Buttons/OptionsColor", "Images/Buttons/OptionsBlackWhite");
            menEntr.Selected += onSelectedOptions;
            menEntr.Position = new Vector4((1 * width) / 3 + 30, 11 * height / 40, (1 * width) / 3 - 60, 3 * height / 20);
            AddEntry(menEntr);
            menEntr = new MenuButton("Controls", "Images/Buttons/ControlsColor", "Images/Buttons/ControlsBlackWhite");
            menEntr.Selected += onSelectedControls;
            menEntr.Position = new Vector4((1 * width) / 3 + 30, 18 * height / 40, (1 * width) / 3 - 60, 3 * height / 20);
            AddEntry(menEntr);
            menEntr = new MenuButton("Exit", "Images/Buttons/ExitColor", "Images/Buttons/ExitBlackWhite");
            menEntr.Selected += onSelectedExit;
            menEntr.Position = new Vector4((1 * width) / 3 + 30, 25 * height / 40, (1 * width) / 3 - 60, 3 * height / 20);
            AddEntry(menEntr);

            background = content.Load<Texture2D>("Images/Backgrounds/degradado");
            base.LoadContent(content);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            return;
        }

        public void onSelectedOptions(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new OptionsMenu(0, ScreenManager.Height, this));
            this.ScreenManager.setActiveScreen("Options Menu");
        }

        public void onSelectedControls(object ev, PlayerIndexEventArgs plIndex)
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

        public void onSelectedExit(object ev, PlayerIndexEventArgs plIndex)
        {
            this.ScreenManager.addScreen(new MainMenuScreen(ScreenManager.Height/4, ScreenManager.Height-10));
            this.ScreenManager.deleteScreen(this);

            
            this.ScreenManager.deleteScreen(this.ScreenManager.getScreen("Game Screen"));
            this.ScreenManager.setActiveScreen("Main menu");
            Camera.Initialize(ScreenManager.Game.Window);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.previousScreen.Draw(spriteBatch);
            int width = ScreenManager.Width;
            int height = ScreenManager.Height;
            Rectangle r= new Rectangle(width / 3, 20, width / 3, height-40);

           
            spriteBatch.Draw(background, r, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.99f);
            base.Draw(spriteBatch);
        }


        public override void onCancel()
        {
            this.ScreenManager.setActiveScreen("Game Screen");
        }

    }

}
