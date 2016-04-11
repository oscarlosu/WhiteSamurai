using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine.Collisions;
using Prototype.Characters;
using Prototype.TileEngine.Map;
using Prototype.TileEngine;

namespace Prototype.Projectiles
{
    class Projectile
    {
        #region Fields
            /// <summary>
            /// Position of the projectile
            /// </summary>
            Vector3 position;
            /// <summary>
            /// Direction of the projectile
            /// </summary>
            Vector3 direction;
            /// <summary>
            /// Frame of the spriteSheet
            /// </summary>
            int frame;
            /// <summary>
            /// Collider
            /// </summary>
            Collidable collider;
            /// <summary>
            /// Depth of the projectile
            /// </summary>
            float depth;
            /// <summary>
            /// Screen position of the projectile
            /// </summary>
            Vector2 screenPosition;
            /// <summary>
            /// Speed of the projectile
            /// </summary>
            float speed;
            /// <summary>
            /// Damage inflicted by the projectile
            /// </summary>
            float damage;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Initial position</param>
        /// <param name="direction">Initial Direction</param>
        /// <param name="damage">Damage inflicted by the projectile</param>
        /// <param name="speed">Speed of the projectile</param>
        public Projectile(Vector3 position, Vector3 direction, float speed, float damage)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.damage = damage;
            if (this.direction == IsoInfo.South)
            {
                frame = 0;
            }
            else if (this.direction == IsoInfo.SouthWest)
            {
                frame = 1;
            }
            else if (this.direction == IsoInfo.West)
            {
                frame = 2;
            }
            else if (this.direction == IsoInfo.NorthWest)
            {
                frame = 3;
            }
            else if (this.direction == IsoInfo.North)
            {
                frame = 4;
            }
            else if (this.direction == IsoInfo.NorthEast)
            {
                frame = 5;
            }
            else if (this.direction == IsoInfo.East)
            {
                frame = 6;
            }
            else
            {
                frame = 7;
            }

            List<Circle> ls = new List<Circle>();
            ls.Add(new Circle(new Vector2(0, 0), 5));
            ls.Add(new Circle(new Vector2(direction.X, direction.Y), 5));
            ls.Add(new Circle(new Vector2(-direction.X, -direction.Y), 5));
            collider = new Collidable(ls);
        }
        public void Update(GameTime gameTime)
        {
            Vector2 currentCell = Coordinates.toCell(position);
            Vector3 destinyPosition = position + direction * speed;
            Vector2 destinyCell = Coordinates.toCell(destinyPosition);
            int level = (int) Math.Floor(position.Z / IsoInfo.TileElevation) + 2;
            //Study currentCell
            if (currentCell.X >= 0 && currentCell.Y >= 0 && currentCell.X < MapManager.Instance.CurrentMap.NCols && currentCell.Y < MapManager.Instance.CurrentMap.NRows)
            {
                List<int> enemies = CharacterManager.Instance.Map.getCell((int) currentCell.Y, (int) currentCell.X).EnemyIds;
                foreach (int enemy in enemies)
                {
                    GameCharacter gc;
                    if (enemy == -1)
                    {
                        gc = CharacterManager.Instance.Player;
                    }
                    else
                    {
                        gc = CharacterManager.Instance.getEnemy(enemy);
                    }

                    int characterLevel = (int)Math.Floor(gc.Position.Z / IsoInfo.TileElevation);
                    Dictionary<float, Collidable> dict;
                    if (gc.Mobile.Collidables.TryGetValue(gc.Mobile.AnimatedSprite.CurrentAnimationName, out dict))
                    {
                        if (level == characterLevel + 1)
                        {
                            Collidable col;
                            if (dict.TryGetValue(1, out col))
                            {
                                if (this.collider.collide(new Vector2(destinyPosition.X, destinyPosition.Y), col, new Vector2(gc.Position.X, gc.Position.Y)))
                                {
                                    ProjectileManager.Instance.addBrokenProjectile(this);
                                    gc.drainHP(damage);
                                }
                            }
                        }
                        else if (level == characterLevel + 2)
                        {
                            Collidable col;
                            if (dict.TryGetValue(2, out col))
                            {
                                if (this.collider.collide(new Vector2(destinyPosition.X, destinyPosition.Y), col, new Vector2(gc.Position.X, gc.Position.Y)))
                                {
                                    ProjectileManager.Instance.addBrokenProjectile(this);
                                    gc.drainHP(damage);
                                    return;
                                }
                            }
                        }
                    }
                }

            }
            if (currentCell != destinyCell)
            {
                if (destinyCell.X >= 0 && destinyCell.Y >= 0 && destinyCell.X < MapManager.Instance.CurrentMap.NCols && destinyCell.Y < MapManager.Instance.CurrentMap.NRows)
                {
                    //Check enemies in cell
                    List<int> enemies = CharacterManager.Instance.Map.getCell((int) currentCell.Y, (int) currentCell.X).EnemyIds;
                    foreach (int enemy in enemies)
                    {
                        GameCharacter gc;
                        if (enemy == -1)
                        {
                            gc = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            gc = CharacterManager.Instance.getEnemy(enemy);
                        }

                        int characterLevel = (int)Math.Floor(gc.Position.Z / IsoInfo.TileElevation);
                        Dictionary<float, Collidable> dict;
                        if (gc.Mobile.Collidables.TryGetValue(gc.Mobile.AnimatedSprite.CurrentAnimationName, out dict))
                        {
                            if (level == characterLevel + 1)
                            {
                                Collidable col;
                                if (dict.TryGetValue(1, out col))
                                {
                                    if (this.collider.collide(new Vector2(destinyPosition.X, destinyPosition.Y), col, new Vector2(gc.Position.X, gc.Position.Y)))
                                    {
                                        ProjectileManager.Instance.addBrokenProjectile(this);
                                        gc.drainHP(10.0f);
                                    }
                                }
                            }
                            else if (level == characterLevel + 2)
                            {
                                Collidable col;
                                if (dict.TryGetValue(2, out col))
                                {
                                    if (this.collider.collide(new Vector2(destinyPosition.X, destinyPosition.Y), col, new Vector2(gc.Position.X, gc.Position.Y)))
                                    {
                                        ProjectileManager.Instance.addBrokenProjectile(this);
                                        gc.drainHP(10.0f);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    MapCell mc = MapManager.Instance.CurrentMap.Cells[(int)destinyCell.Y][(int)destinyCell.X];
                    if ( mc.TerrainTileCount + mc.TerrainObjectTileCount > level)
                    {
                        ProjectileManager.Instance.addBrokenProjectile(this);
                        return;
                    }
                }
                else
                {
                    ProjectileManager.Instance.addBrokenProjectile(this);
                    return;
                }
                //Study destiny cell
            }
            position = this.position + direction * speed;
            depth = Camera.getDepth(position, DrawableType.MOBILE_OBJECT);
            screenPosition = Coordinates.toScreen(position) - Camera.Position -new Vector2(64, 108);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ProjectileManager.Instance.ProjectileTexture, screenPosition, new Rectangle(frame*128, 0, 128, 128), Color.Gray, 0, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}
