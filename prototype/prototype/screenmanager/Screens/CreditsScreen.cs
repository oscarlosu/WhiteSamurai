using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Prototype.ScreenManager.Screens
{
    class CreditsScreen : MenuScreen
    {
        Texture2D background;
        public CreditsScreen(int minHeight, int maxHeight, Screen previous)
            : base("Credits Screen", minHeight, maxHeight, previous)
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

            MenuEntry menEntr = new CreditText("Design and Programming", true);
            medString = base.menuFont.MeasureString("Design and Programming");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 4 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = menEntr = new CreditText("Oscar Losada", false);
            medString = base.menuFont.MeasureString("Oscar Losada");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2,  6 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Javier Sanz-Cruzado", false);
            medString = base.menuFont.MeasureString("Javier Sanz-Cruzado");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 8 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Music and Art", true);
            medString = base.menuFont.MeasureString("Music and Art");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 10 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Oscar Losada", false);
            medString = base.menuFont.MeasureString("Oscar Losada");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 12 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Special Thanks", true);
            medString = base.menuFont.MeasureString("Special Thanks");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 14 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Rafael Aburto", false);
            medString = base.menuFont.MeasureString("Rafael Aburto");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 16 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Juan Sidrach", false);
            medString = base.menuFont.MeasureString("Juan Sidrach");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 18 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("Rafael Vindel", false);
            medString = base.menuFont.MeasureString("Rafael Vindel");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 20 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            menEntr = new CreditText("David Flores", false);
            medString = base.menuFont.MeasureString("David Flores");
            menEntr.Selected += onSelectedPredetermined;
            menEntr.Position = new Vector4(width / 2 - medString.X / 2, 22 * height / 30, medString.X, medString.Y);
            this.AddEntry(menEntr);

            MusicManager.changeSong("gameOverSong");
            base.LoadContent(content);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.Width, ScreenManager.Height), Color.White);
            spriteBatch.DrawString(base.menuFont, "White Samurai Credits", new Vector2(ScreenManager.Width / 2 - menuFont.MeasureString("White Samurai Credits").X / 2, ScreenManager.Height / 20), Color.SlateGray, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
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

        public void onSelectedPredetermined(object ev, PlayerIndexEventArgs pl)
        {

        }

        public override void onCancel()
        {
            MusicManager.changeSong("menuSong");
            base.onCancel();
        }
    }
}
