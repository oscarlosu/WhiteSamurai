using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype.ScreenManager.Screens
{
    class OptionsMenu : MenuScreen
    {
        Texture2D background;
        public OptionsMenu(int minHeight, int maxHeight, Screen previous) : base("Options Menu", minHeight, maxHeight, previous)
        {
        }


        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
            int width = ScreenManager.Width;
            int height = ScreenManager.Height;
            background = content.Load<Texture2D>("Images/blackWhiteUroboros");

            int horizInitial = Math.Min(2 * width / 3, width - (9 * height / 20) - 20);

            MenuEntry menEntr = new MenuButton("Screen", "Images/Buttons/ScreenColor", "Images/Buttons/ScreenBlackWhite");
            menEntr.Selected += onSelectedScreen;
            menEntr.Position = new Vector4((1 * width) / 3 + 30, 11 * height / 40, (1 * width) / 3 - 60, 3 * height / 20);
            this.AddEntry(menEntr);

            menEntr = new MenuButton("Back", "Images/Buttons/BackColor", "Images/Buttons/BackBlackWhite");
            menEntr.Selected += onSelectedBack;
            menEntr.Position = new Vector4((1 * width) / 3 + 30, 18 * height / 40, (1 * width) / 3 - 60, 3 * height / 20);
            this.AddEntry(menEntr);

            base.LoadContent(content);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.Width, ScreenManager.Height), Color.White);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void UnloadContent()
        {
        }

        public void onSelectedBack(object ev, PlayerIndexEventArgs pl)
        {
            onCancel();
        }

        public void onSelectedScreen(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.addScreen(new ResolutionMenu(ScreenManager.Height / 3, ScreenManager.Height - 10, this));
            ScreenManager.setActiveScreen("Resolution Menu");
        }
    }
}
