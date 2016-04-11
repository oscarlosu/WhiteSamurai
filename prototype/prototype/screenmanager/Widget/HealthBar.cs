using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Prototype.ScreenManager.Widget
{
    class HealthBar
    {
        #region Fields
            float maxHP;
            int porcentaje;
            Texture2D barTexture;
        #endregion

        #region Constructor
            public HealthBar(float maxHP)
            {
                this.maxHP = maxHP;
                this.porcentaje = 100;
            }
            public void LoadContent(ContentManager content)
            {
                barTexture = content.Load<Texture2D>("Images/Widget/HealthBar");
            }

            public void Update(float HP, GameTime gameTime)
            {
                porcentaje = (int) Math.Floor(HP/maxHP * 100);
            }

            public void Draw(SpriteBatch spriteBatch, int ScreenHeight, int ScreenWidth)
            {
                spriteBatch.Draw(barTexture, new Rectangle(30, 30, barTexture.Width, 44), new Rectangle(0, 45, barTexture.Width, 44), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.989f);
                spriteBatch.Draw(barTexture, new Rectangle(30, 30, (int)(barTexture.Width * ((double)porcentaje / 100.0)), 44), new Rectangle(0, 45, barTexture.Width, 44), Color.DarkGray, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9899f);
                spriteBatch.Draw(barTexture, new Rectangle(30, 30, barTexture.Width, 44), new Rectangle(0, 0, barTexture.Width, 44), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.98999f);
            }
        #endregion
    }
}
