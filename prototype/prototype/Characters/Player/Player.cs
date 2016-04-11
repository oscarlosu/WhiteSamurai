using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Prototype.ScreenManager;
using Prototype.TileEngine;
using Prototype.TileEngine.Collisions;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine.MobileObject;
using Prototype.Characters;
using Prototype.GameState;

namespace Prototype.Characters
{
    public class Player : GameCharacter
    {
        #region Fields
            private bool keepAttacking = false;
            private bool isInvincible = false;
        #endregion

        // There should be a level 0 (key 0) collidable in the mobileObject
        // The level 0 collidable must be a pointCollidable, ie it must have points and not circles
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hp">Health points</param>
        /// <param name="collidables">Colliders for the character</param>
        /// <param name="position">Initial position</param>
        /// <param name="drawOffset"></param>
        /// <param name="spriteSheetName">Name of the spriteSheet</param>
        public Player(float hp, float dps, float regeneration, float speed, MobileObject mobile, String spriteSheet)
            : base(hp, dps, regeneration, speed, mobile, spriteSheet)
        {
            Mobile.AnimatedSprite.CurrentAnimationName = "idleSouth";
            Mobile.AnimatedSprite.Tint = Color.White;
            PreviousMove = IsoInfo.South;
            
        }

        #region GameMethods
            public override void Update(InputState input, GameTime gameTime)
            {
                Vector3 direction;
                regen = true;
                string animationName = getAction(input, gameTime, out direction);
                if (this.HP > 0) // If the player is still alive
                {
                    isInvincible = false;
                    if (animationName.Contains("attacking"))
                    {
                        if (!Mobile.AnimatedSprite.CurrentAnimationName.Contains("attacking"))
                            keepAttacking = false; // reset flag at start of attack
                        else
                            keepAttacking = true;
                    }

                    if (!canInterrupt(gameTime))
                    {
                        // Ignore commands
                        animationName = Mobile.AnimatedSprite.CurrentAnimationName;
                    }
                    else
                    {
                        if (keepAttacking)
                        {
                            keepAttacking = false;
                            // Ignore commands
                            animationName = Mobile.AnimatedSprite.CurrentAnimationName;
                        }
                        else if (Mobile.AnimatedSprite.CurrentAnimationName.Contains("attacking"))
                        {
                            Mobile.AnimatedSprite.CurrentFrameAnimation.Timer = 0.0f; // reset timer manually if the current frame is going to be interrupted
                        }
                    }


                    // Action logic
                    if (animationName.Contains("moving"))
                    {
                        
                        move(direction);
                    }
                    else if (animationName.Contains("attacking"))
                    {
                        regen = false;
                        attack(gameTime);
                    }
                    else if (animationName.Contains("dashing"))
                    {
                        regen = false;
                        isInvincible = true;
                        dash();
                    
                    }

                    regenerate(gameTime, regen);
                }
                else
                {
                    if (!this.Mobile.AnimatedSprite.CurrentAnimationName.Contains("death"))
                    {
                        animationName = getDeathAnimation();
                    }
                    else
                    {
                        animationName = this.Mobile.AnimatedSprite.CurrentAnimationName;
                        if (this.Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame == this.Mobile.AnimatedSprite.CurrentFrameAnimation.NFrames - 1)
                        {
                            die();
                        }
                    }

                }
                // Call parent's update method
                Mobile.Update(gameTime, animationName);
            }

            public override void UpdateOnWin(GameTime gameTime)
            {
                this.PreviousMove = IsoInfo.South;
                String animationName = "idleSouth";
                this.Mobile.Update(gameTime, animationName);
                Color color = this.Mobile.AnimatedSprite.Tint;
                color.R = (byte)MathHelper.Clamp(color.R - 1, 0, 255);
                color.G = (byte)MathHelper.Clamp(color.G - 1, 0, 255);
                color.B = (byte)MathHelper.Clamp(color.B - 1, 0, 255);

                if (color.R == color.G && color.G == color.B && color.R == 0)
                {
                    GameState.GameState.Winning = false;
                    GameState.GameState.GameWon = true;
                }
                this.Mobile.AnimatedSprite.Tint = color;
            }
        #endregion

            #region ActionMethods
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
                            if (enemy != -1)
                            {
                                GameCharacter gc = CharacterManager.Instance.getEnemy(enemy);
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
                    return Mobile.checkAttackEnemiesInCellList(-1, cellsToExplore, this.PreviousMove);
                }
                
                /// <summary>
                /// Executes the dash
                /// </summary>
                public override void dash()
                {
                    Mobile.move(-1, this.PreviousMove, 6.0f * Speed);

                }

                /// <summary>
                /// Moves the character
                /// </summary>
                /// <param name="direction">Movement direction</param>
                public override void move(Vector3 direction)
                {
                    Mobile.move(-1, direction, Speed);
                    this.PreviousMove = direction;
                }
            #endregion
            public override String getDeathAnimation()
        {
            String animationName = "death";
            Vector3 direction = PreviousMove;
            if (direction == IsoInfo.North)
            {
                animationName += "North";
            }
            else if (direction == IsoInfo.NorthEast)
            {
                animationName += "NorthEast";
            }
            else if (direction == IsoInfo.East)
            {
                animationName += "East";
            }
            else if (direction == IsoInfo.SouthEast)
            {
                animationName += "SouthEast";
            }
            else if (direction == IsoInfo.South)
            {
                animationName += "South";
            }
            else if (direction == IsoInfo.SouthWest)
            {
                animationName += "SouthWest";
            }
            else if (direction == IsoInfo.West)
            {
                animationName += "West";
            }
            else
            {
                animationName += "NorthWest";
            }

            return animationName;
        }

        
        private string getAction(InputState input, GameTime gameTime, out Vector3 direction)
        {
            KeyboardState state = input.CurrentKeyBoardState;
            GamePadState gpstate = input.CurrentGamePadState[0];

            // Control for move-related player input
            // Gamepad input
            Vector2 normalizedLeftThumbstick = gpstate.ThumbSticks.Left;
            normalizedLeftThumbstick.Normalize();
            double angle = Math.Acos(normalizedLeftThumbstick.X);
            if (normalizedLeftThumbstick.Y < 0)
            {
                angle = 2.0 * Math.PI - angle;
            }

            // Initialize output
            string animationName = null;
            direction = Vector3.Zero;

            animationName = InputHandler.determinePlayerAction(input, this.PreviousMove, out direction);
            if ((animationName.Contains("dashing") || animationName.Contains("attacking")))
            {
                direction = this.PreviousMove;
            }
            return animationName;
        }

        public override void drainHP(float hpDrain)
        {
            if(!isInvincible)
                base.drainHP(hpDrain);
        }
        public override void die()
        {
            GameState.GameState.GameOver = true;
        }
    }
}
