using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Prototype.ScreenManager.Screens
{
    class ResolutionMenu : MenuScreen
    {
        Texture2D background;
        public ResolutionMenu(int minHeight, int maxHeight, Screen previous) : base("Resolution Menu", minHeight, maxHeight, previous)
        {
        }


        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
            int width = ScreenManager.Width;
            int height = ScreenManager.Height;
            background = content.Load<Texture2D>("Images/blackWhiteUroboros");

            int horizInitial = Math.Min(2 * width / 3, width - (9 * height / 20) - 20);

            Vector2 medString;

            MenuEntry menEntr = new MenuText("Predetermined");
            medString = base.menuFont.MeasureString("Predetermined");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 10 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("1920x1080");
            medString = base.menuFont.MeasureString("1920x1080");
            menEntr.Selected += onSelected1920x1080;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 12 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("1440x900");
            medString = base.menuFont.MeasureString("1440x900");
            menEntr.Selected += onSelected1440x900;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 14 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("1280x800");
            medString = base.menuFont.MeasureString("1280x800");
            menEntr.Selected += onSelected1280x800;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 16 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("1024x768");
            medString = base.menuFont.MeasureString("1024x768");
            menEntr.Selected += onSelected1024x768;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 18 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("800x600");
            medString = base.menuFont.MeasureString("800x600");
            menEntr.Selected += onSelected800x600;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 20 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new MenuText("Back");
            medString = base.menuFont.MeasureString("Back");
            menEntr.Selected += onSelectedBack;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 22 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            base.LoadContent(content);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.Width, ScreenManager.Height), Color.White);
            spriteBatch.DrawString(base.menuFont, "Select the screen resolution", new Vector2(ScreenManager.Width / 2 - menuFont.MeasureString("Select the screen resolution").X / 2, ScreenManager.Height/5), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
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

        public void onSelected1920x1080(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution(1920, 1080);
        }
        public void onSelected1440x900(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution(1440, 900);
        }
        public void onSelected1280x800(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution(1280, 800);
        }
        public void onSelected1024x768(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution(1024, 768);
        }
        public void onSelected800x600(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution(800, 600);
        }
        public void onSelectedPredetermined(object ev, PlayerIndexEventArgs pl)
        {
            ScreenManager.setResolution();
        }
    }

}
