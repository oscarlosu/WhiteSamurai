using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.MobileObject;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine;
using Prototype.Characters.Enemies;

namespace Prototype.Characters
{
    public class EnemyBoss : EnemyMelee
    {
        public EnemyBoss(int id, float hp, float dps, float regeneration, float speed, float visionRadio, MobileObject mobile, String spriteSheet) :
            base(id, hp, dps, regeneration, speed, visionRadio, mobile, spriteSheet)
        {
            Mobile.AnimatedSprite.Tint = Color.Black;
        }

        /// <summary>
        /// Attacks
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void attack(GameTime gameTime)
        {
            if (InterrumptibleAttackFrames.Contains(Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame + 1) && Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration)
            {
                List<int> enemiesAttacked = attackPositions();

                foreach (int enemy in enemiesAttacked)
                {
                    Random rnd = new Random();
                    if (enemy != -1 && enemy != ID)
                    {
                        GameCharacter gc = CharacterManager.Instance.getEnemy(enemy);
                        float HPDrain = this.DPS / 5 + (rnd.Next(-1, 1) * ((float)rnd.NextDouble())) / 10.0f;
                        gc.drainHP(HPDrain);
                    }
                    else if (enemy == -1)
                    {
                        GameCharacter gc = CharacterManager.Instance.Player;
                        float HPDrain = this.DPS / 5 + (rnd.Next(-1, 1) * ((float)rnd.NextDouble())) / 10.0f;
                        gc.drainHP(HPDrain);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of attacked enemies
        /// </summary>
        /// <returns>The list of attacked enemies</returns>
        private List<int> attackPositions()
        {
            List<int> collidedEnemies = new List<int>();
            Vector2 currentCell = Coordinates.toCell(Position);
            float currentElevation = MapManager.Instance.CurrentMap.Cells[(int)currentCell.Y][(int)currentCell.X].getTerrainElevation(Position);
            List<Vector2> cellsToExplore = new List<Vector2>();

            cellsToExplore.Add(currentCell);
            if (this.PreviousMove == IsoInfo.North) //If the character is heading NORTH
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
            }
            else if (this.PreviousMove == IsoInfo.NorthEast) //If the character is heading NORTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.East) //If the character is heading EAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.SouthEast) //If the character is heading SOUTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.South) //If the character is heading SOUTH
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.SouthWest) //If the character is heading SOUTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.West) //If the character is heading WEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
            }
            else //directionHeading == IsoInfo.NorthWest // If the character is heading NORTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
            }
            //Check the collisions
            return Mobile.checkAttackEnemiesInCellList(ID, cellsToExplore, this.PreviousMove);
        }

        public override void die()
        {
            base.die();
            GameState.GameState.Winning = true;
        }
    }
}
