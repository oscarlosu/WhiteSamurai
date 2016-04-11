using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.MobileObject;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Prototype.Projectiles
{
    class ProjectileManager
    {
        #region Fields
            /// <summary>
            /// List of existing projectiles
            /// </summary>
            private List<Projectile> projectiles;
            /// <summary>
            /// List of projectiles to remove
            /// </summary>
            private List<Projectile> brokenProjectiles;
            /// <summary>
            /// Texture for the projectiles
            /// </summary>
            private Texture2D projectileTexture;
            /// <summary>
            /// Instance
            /// </summary>
            private static ProjectileManager instance = null;
            /// <summary>
            /// Projectile model. Helps building new 
            /// </summary>
            private ProjectileModel model;
        #endregion

        #region Properties
            public static ProjectileManager Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new ProjectileManager();
                    }
                    return instance;
                }
            }

            public Texture2D ProjectileTexture
            {
                get
                {
                    return projectileTexture;
                }
            }
        #endregion

        #region Constructor
            private ProjectileManager()
            {
                this.projectiles = new List<Projectile>();
                this.brokenProjectiles = new List<Projectile>();
                this.model = ProjectileModel.readFromXML("Projectiles/XML/projectile.xml");
            }
        #endregion

        #region GameMethods
            public void LoadContent(ContentManager content)
            {
                this.projectiles.Clear();
                projectileTexture = content.Load<Texture2D>("Sprites/Projectiles/arrowDrawn");
            }

            public void Update(GameTime gameTime)
            {
                brokenProjectiles.Clear();
                foreach (Projectile proj in projectiles)
                {
                    proj.Update(gameTime);
                }

                foreach (Projectile proj in brokenProjectiles)
                {
                    projectiles.Remove(proj);
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                foreach (Projectile proj in projectiles)
                {
                    proj.Draw(spriteBatch);
                }
            }

        #endregion

        #region Methods
            public void addProjectile(Vector3 position, Vector3 direction)
            {
                Projectile proj = new Projectile(position, direction, model.speed, model.damage);
                this.projectiles.Add(proj);
            }

            public void removeProjectile(Projectile proj)
            {
                this.projectiles.Remove(proj);
            }

            public void removeProjectileList(List<Projectile> proj)
            {
                foreach (Projectile projectile in proj)
                {
                    removeProjectile(projectile);
                }
            }
            public void addBrokenProjectile(Projectile proj)
            {
                this.brokenProjectiles.Add(proj);
            }
        #endregion
    }
}
