using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prototype.ScreenManager;
using Prototype.Characters.Strategies;

namespace Prototype.Characters.Enemies
{
    public class EnemyGroup
    {
        #region Fields
            /// <summary>
            /// List of enemies
            /// </summary>
            private List<Enemy> enemies;
            /// <summary>
            /// Group Leader
            /// </summary>
            private Enemy leader = null;
            private Boolean disbanded = false;
        #endregion

        #region Properties
            /// <summary>
            /// List of enemies
            /// </summary>
            public List<Enemy> Enemies
            {
                get
                {
                    return enemies;
                }
            }

            public Boolean Disbanded
            {
                get
                {
                    return disbanded;
                }
            }
        #endregion

            #region Constructor
            public EnemyGroup()
            {
                enemies = new List<Enemy>();
            }
            #endregion

            #region GameMethods
            

            public void Update(GameTime gameTime, Vector3 playerPosition, InputState input)
            {
                List<Enemy> toDelete = new List<Enemy>();
                Boolean calm = true;
                foreach (Enemy enemy in enemies)
                {
                    
                    if (enemy.State == State.DEAD)
                    {
                        toDelete.Add(enemy);
                    }
                    else if(calm == true)
                    {
                        if ((enemy.Position - playerPosition).Length() < enemy.VisionRadio)
                        {
                            calm = false;
                        }
                    }                    
                }

                foreach (Enemy enemy in toDelete)
                {
                    this.deleteEnemy(enemy);
                }

                //Check the leader
                if (leader.State == State.DEAD)
                {
                    forcedDisband();
                }
                else if ((leader.Position - playerPosition).Length() < leader.VisionRadio)
                {
                    calm = false;
                }

                if (calm == true)
                {
                    calmGroup();
                    disband();
                }
                
                
            }
        #endregion

        #region Methods
            /// <summary>
            /// Adds an enemy
            /// </summary>
            /// <param name="enemy">Enemy to add</param>
            public void addEnemy(Enemy enemy)
            {
                if (enemy != null)
                {
                    if (leader == null)
                    {
                        leader = enemy;
                    }
                    else
                    {
                        enemies.Add(enemy);
                    }
                    enemy.enemyGroup = this;
                }
            }
        
            /// <summary>
            /// Deletes an enemy
            /// </summary>
            /// <param name="enemy">Enemy to delete</param>
            public void deleteEnemy(Enemy enemy)
            {
                if (enemy != null && enemies.Contains(enemy))
                {
                    enemy.enemyGroup = null;
                    enemies.Remove(enemy);
                }
            }

            /// <summary>
            /// Alerts a group of enemies
            /// </summary>
            public void alertGroup()
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.State = State.GROUPALERTED;
                }
            }

            /// <summary>
            /// Calms a group of enemies
            /// </summary>
            public void calmGroup()
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.State = State.IDLE;
                }
            }

            /// <summary>
            /// Makes the fusion of two groups of enemies
            /// </summary>
            /// <param name="group"></param>
            public void fuseGroups(EnemyGroup group)
            {
                this.enemies.AddRange(group.Enemies);
                foreach (Enemy enemy in group.Enemies)
                {
                    enemy.enemyGroup = group;
                }
                group.Enemies.Clear();
                group.disband();
            }

            /// <summary>
            /// Disbands the group
            /// </summary>
            public void disband()
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.enemyGroup = null;
                }
                this.leader.Strategy = new AlertStrategy();
                disbanded = true;
            }

            /// <summary>
            /// Makes half the group berserker and half the group run away
            /// </summary>
            public void forcedDisband()
            {
                Random rnd = new Random();
                foreach(Enemy enemy in enemies)
                {
                    if(enemy.GetType() != typeof(EnemyBoss)) //The boss is unaffected by the death of the leader of the group
                    {
                        if (rnd.Next(-10, 10) > 0)
                        {
                            enemy.State = State.BERSERKER;
                        }
                        else
                        {
                            enemy.Strategy = new FlyStrategy();
                            enemy.State = State.ALERTED;
                        }
                    }
                }

                disband();
            }
        #endregion
    }
}
