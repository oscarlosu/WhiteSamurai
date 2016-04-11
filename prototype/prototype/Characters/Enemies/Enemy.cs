using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;
using Prototype.Characters.Strategies;
using Microsoft.Xna.Framework.Graphics;
using Prototype.ScreenManager;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine.MobileObject;
using Prototype.Characters.Enemies;

namespace Prototype.Characters
{
    public abstract class Enemy : GameCharacter
    {
        #region Fields
            /// <summary>
            /// Identificador
            /// </summary>
            private int id;
            /// <summary>
            /// Radio in which the enemy can detect a player
            /// </summary>
            private float visionRadio;
            /// <summary>
            /// State of the enemy
            /// </summary>
            private State state;
            /// <summary>
            /// Strategy to use
            /// </summary>
            private MoveStrategy strategy;
            /// <summary>
            /// Previous done action
            /// </summary>
            private Action previousAction;
            /// <summary>
            /// Enemy group
            /// </summary>
            private EnemyGroup group;
            private int stun;
            public const int STUNTIME = 15;
            static Random rnd = new Random(DateTime.Now.Millisecond);
            private bool isBerserkerMultiplied = false;
        #endregion

        #region Properties
                public float VisionRadio
                {
                    get
                    {
                        return this.visionRadio;
                    }
                    set
                    {
                        visionRadio = value;
                    }
                }

                public State State
                {
                    get
                    {
                        return state;
                    }
                    set
                    {
                        state = value;
                    }
                }

                public MoveStrategy Strategy
                {
                    get
                    {
                        return strategy;
                    }
                    set
                    {
                        strategy = value;
                    }
                }

                public Action PreviousAction
                {
                    get
                    {
                        return previousAction;
                    }
                    set
                    {
                        previousAction = value;
                    }
                }

                public int ID
                {
                    get
                    {
                        return id;
                    }
                }

                public EnemyGroup enemyGroup
                {
                    get
                    {
                        return group;
                    }
                    set
                    {
                        group = value;
                    }
                }

                public int Stun
                {
                    get
                    {
                        return stun;
                    }
                    set
                    {
                        stun = value;
                    }
                }

                public bool IsBerserkerMultiplied
                {
                    get
                    {
                        return isBerserkerMultiplied;
                    }
                    set
                    {
                        isBerserkerMultiplied = value;
                    }
                }
            #endregion

        #region Constructor
                /// <summary>
                /// Constructor for an enemy
                /// </summary>
                /// <param name="hp">Health points</param>
                /// <param name="spriteSheet">Sprite sheet</param>
                /// <param name="collidables">Colliders for the character</param>
                /// <param name="position">Initial position</param>
                /// <param name="drawOffset"></param>
                /// <param name="visionRadio">Radio in which the enemy can detect a player</param>
                public Enemy(int id, float hp, float dps, float regeneration, float speed, float visionRadio, MobileObject mobile, String spriteSheet) :
                    base(hp, dps, regeneration, speed, mobile, spriteSheet)
                {
                    this.id = id;
                    this.visionRadio = visionRadio;
                    this.state = State.IDLE;
                    this.previousAction = Action.IDLE;
                    this.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouth";
                    this.stun = 0;
                }
            #endregion

        #region GameMethods
                public override void UpdateOnWin(GameTime gameTime)
                {
                    Vector3 direction;
                    this.PreviousMove = IsoInfo.South;
                    this.PreviousAction = Action.IDLE;
                    String animationName = getAction(this.PreviousAction, gameTime, out direction);
                    this.Mobile.Update(gameTime, animationName);
                }
                
        #endregion

        #region Methods

                
                public override void die()
                {
                    CharacterManager.Instance.DeathList.Add(this.id);
                }
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
                public string getAction(Action action, GameTime gameTime, out Vector3 direction)
                {
                    String animationName;

                    if (action == Action.IDLE)
                    {
                        animationName = "idle";
                    }
                    else if (action == Action.ATTACK)
                    {
                        animationName = "attacking";
                    }
                    else
                    {
                        animationName = "moving";
                    }

                    if (animationName != "moving")
                    {
                        direction = this.PreviousMove;
                    }
                    else
                    {
                        if (action == Action.MOVENORTH || action == Action.HEADNORTH)
                            direction = IsoInfo.North;
                        else if (action == Action.MOVENORTHEAST || action == Action.HEADNORTHEAST)
                            direction = IsoInfo.NorthEast;
                        else if (action == Action.MOVEEAST || action == Action.HEADEAST)
                            direction = IsoInfo.East;
                        else if (action == Action.MOVESOUTHEAST || action == Action.HEADSOUTHEAST)
                            direction = IsoInfo.SouthEast;
                        else if (action == Action.MOVESOUTH || action == Action.HEADSOUTH)
                            direction = IsoInfo.South;
                        else if (action == Action.MOVESOUTHWEST || action == Action.HEADSOUTHWEST)
                            direction = IsoInfo.SouthWest;
                        else if (action == Action.MOVEWEST || action == Action.HEADWEST)
                            direction = IsoInfo.West;
                        else
                            direction = IsoInfo.NorthWest;
                    }

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

                public override void drainHP(float hpDrain)
                {
                    base.drainHP(hpDrain);
                    if (rnd.NextDouble() >= 0.75)
                    {
                        stun = 1;
                    }
                }

                public void drainHPBerserker()
                {
                    base.drainHP(1.0f / 60.0f);
                }
        #endregion

                #region ActionMethods
                                   
                    /// <summary>
                    /// Executes the dash
                    /// </summary>
                    public override void dash()
                    {
                    }

                    /// <summary>
                    /// Moves the character
                    /// </summary>
                    /// <param name="direction">Movement direction</param>
                    public override void move(Vector3 direction)
                    {
                        Mobile.move(id, direction, Speed);
                        PreviousMove = direction;
                    }
                #endregion
    }
}
