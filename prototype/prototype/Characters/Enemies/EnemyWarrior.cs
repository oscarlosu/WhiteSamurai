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
    public class EnemyWarrior : EnemyMelee
    {
        public EnemyWarrior(int id, float hp, float dps, float regeneration, float speed, float visionRadio, MobileObject mobile, String spriteSheet) :
            base(id, hp, dps, regeneration, speed, visionRadio, mobile, spriteSheet)
        {
            Mobile.AnimatedSprite.Tint = Color.Gray;
        }

        public override void attack(GameTime gameTime)
        {
            List<int> enemiesAttacked = new List<int>();

            if (Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame + 1 == InterrumptibleAttackFrames.First() && Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration)
            {
                enemiesAttacked = attackPositionsFirstAttack();
            }
            else if(InterrumptibleAttackFrames.Contains(Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame + 1) && Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration)
            {
                enemiesAttacked = attackPositionsSecondAttack();
            }

            foreach (int enemy in enemiesAttacked)
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
                if (gc != this)
                {
                    Random rnd = new Random();
                    float HPDrain = this.DPS / 5 + (rnd.Next(-1, 1) * ((float)rnd.NextDouble())) / 10.0f;
                    gc.drainHP(HPDrain);
                }
            }
            
            
        }
        /// <summary>
        /// Gets the list of attacked enemies
        /// </summary>
        /// <returns>The list of attacked enemies</returns>
        private List<int> attackPositionsSecondAttack()
        {
            List<int> collidedEnemies = new List<int>();
            Vector2 currentCell = Coordinates.toCell(Position);
            float currentElevation = MapManager.Instance.CurrentMap.Cells[(int)currentCell.Y][(int)currentCell.X].getTerrainElevation(Position);
            List<Vector2> cellsToExplore = new List<Vector2>();

            cellsToExplore.Add(currentCell);
            if (this.PreviousMove == IsoInfo.North) //If the character is heading NORTH
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y - 2));
            }
            else if (this.PreviousMove == IsoInfo.NorthEast) //If the character is heading NORTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y));
            }
            else if (this.PreviousMove == IsoInfo.East) //If the character is heading EAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.SouthEast) //If the character is heading SOUTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.South) //If the character is heading SOUTH
            {
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));;
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.SouthWest) //If the character is heading SOUTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y));
            }
            else if (this.PreviousMove == IsoInfo.West) //If the character is heading WEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y - 2));
            }
            else //directionHeading == IsoInfo.NorthWest // If the character is heading NORTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 2));
            }
            //Check the collisions
            return Mobile.checkAttackEnemiesInCellList(ID, cellsToExplore, this.PreviousMove);
        }

        /// <summary>
        /// Gets the list of attacked enemies
        /// </summary>
        /// <returns>The list of attacked enemies</returns>
        private List<int> attackPositionsFirstAttack()
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
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y));
            }
            else if (this.PreviousMove == IsoInfo.NorthEast) //If the character is heading NORTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.East) //If the character is heading EAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 2, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.SouthEast) //If the character is heading SOUTHEAST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.South) //If the character is heading SOUTH
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 2));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y + 2));
            }
            else if (this.PreviousMove == IsoInfo.SouthWest) //If the character is heading SOUTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y + 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y + 1));
            }
            else if (this.PreviousMove == IsoInfo.West) //If the character is heading WEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 2, currentCell.Y));
            }
            else //directionHeading == IsoInfo.NorthWest // If the character is heading NORTHWEST
            {
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 1));
                cellsToExplore.Add(new Vector2(currentCell.X - 1, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X, currentCell.Y - 2));
                cellsToExplore.Add(new Vector2(currentCell.X + 1, currentCell.Y - 2));
            }
            //Check the collisions
            return Mobile.checkAttackEnemiesInCellList(ID, cellsToExplore, this.PreviousMove);
        }


    }
}
