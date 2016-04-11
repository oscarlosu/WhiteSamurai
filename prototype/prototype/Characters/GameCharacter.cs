using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.MobileObject;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Prototype.ScreenManager;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype.Characters
{
    public abstract class GameCharacter
    {
        #region Fields
            #region NumericFields
                /// <summary>
                /// Character's health points
                /// </summary>
                private float hp;
                /// <summary>
                /// Character's maximum health points
                /// </summary>
                private float maxHP;
                /// <summary>
                /// Damage per second
                /// </summary>
                private float dps;
                /// <summary>
                /// Movement speed
                /// </summary>
                private float speed;
                /// <summary>
                /// Health regeneration per second
                /// </summary>
                private float regeneration;
            #endregion

            #region MovementFields
                /// <summary>
                /// Mobile object
                /// </summary>
                private MobileObject mobile;
                /// <summary>
                /// Direction the character is heading
                /// </summary>
                private Vector3 previousMove;
                /// <summary>
                /// Interrumptible frames during attack
                /// </summary>
                private List<int> interrumptibleAttackFrames;
                /// <summary>
                /// Interrumptible frames during dash
                /// </summary>
                private List<int> interrumptibleDashFrames;
            #endregion

            #region RegenerationFields
                public const float SECONDSMULTIPLIER = 3;
                public float seconds = 0;
                public int multiplier = 0;
                public bool regen = true;
            #endregion
                /// <summary>
                /// Sprite sheet for drawing the character
                /// </summary>
                private String spriteSheet;
        #endregion

        #region Properties
        /// <summary>
        /// <see cref="hp"/>
        /// </summary>
        public float HP
        {
            get
            {
                return hp;
            }
        }
        /// <summary>
        /// <see cref="maxHP"/>
        /// </summary>
        public float MAXHP
        {
            get
            {
                return maxHP;
            }
        }
        /// <summary>
        /// <see cref="dps"/>
        /// </summary>
        public float DPS
        {
            get
            {
                return dps;
            }
            set
            {
                dps = value;
            }
        }
        /// <summary>
        /// <see cref="speed"/>
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        /// <summary>
        /// <see cref="regeneration"/>
        /// </summary>
        public float Regeneration
        {
            get
            {
                return regeneration;
            }
        }
        /// <summary>
        /// <see cref="mobile"/>
        /// </summary>
        public MobileObject Mobile
        {
            get
            {
                return mobile;
            }
        }
        /// <summary>
        /// <see cref="spriteSheet"/>
        /// </summary>
        public String SpriteSheet
        {
            get
            {
                return spriteSheet;
            }
        }
        /// <summary>
        /// <see cref="previousMove"/>
        /// </summary>
        public Vector3 PreviousMove
        {
            get
            {
                return previousMove;
            }

            set
            {
                previousMove = value;
            }
        }
        /// <summary>
        /// <see cref="interrumptibleAttackFrames"/>
        /// </summary>
        public List<int> InterrumptibleAttackFrames
        {
            get
            {
                return interrumptibleAttackFrames;
            }
        }

        /// <summary>
        /// <see cref="interrumptibleDashFrames"/>
        /// </summary>
        public List<int> InterrumptibleDashFrames
        {
            get
            {
                return interrumptibleDashFrames;
            }
        }
        /// <summary>
        /// Character's current position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return Mobile.Position;
            }
        }
        #endregion

        #region Constructor
            /// <summary>
            /// Constructor for a character
            /// </summary>
            /// <param name="hp">Health points</param>
            /// <param name="spriteSheet">Sprite sheet for the character</param>
            /// <param name="collidables">Dictionary of collidables</param>
            /// <param name="position">Initial position for the character</param>
            /// <param name="drawOffset">Offset that will be used when drawing the sprites</param>
            public GameCharacter(float hp, float dps, float regeneration, float speed, MobileObject mobile, String spriteSheet)
            {
                this.maxHP = hp;
                this.hp = hp;
                this.spriteSheet = spriteSheet;
                this.mobile = mobile;
                this.interrumptibleAttackFrames = new List<int>();
                this.interrumptibleDashFrames = new List<int>();
                this.dps = dps;
                this.regeneration = regeneration;
                this.speed = speed;
            }
        #endregion

        #region GameMethods

            /// <summary>
            /// Loads the spriteSheet for the character
            /// </summary>
            /// <param name="content">Content manager</param>
            public virtual void LoadContent(ContentManager content)
            {
                this.mobile.LoadContent(content, spriteSheet);
            }

            /// <summary>
            /// Updates the character
            /// </summary>
            /// <param name="input">Player input</param>
            /// <param name="gameTime">Game time</param>
            public abstract void Update(InputState input, GameTime gameTime);
            /// <summary>
            /// Updates the character when the player has won the game
            /// </summary>
            /// <param name="gameTime">Game Time</param>
            public abstract void UpdateOnWin(GameTime gameTime);
            /// <summary>
            /// Draws the character in the screen
            /// </summary>
            /// <param name="spriteBatch">Sprite batch</param>
            public virtual void Draw(SpriteBatch spriteBatch)
            {
                this.Mobile.Draw(spriteBatch);
            }
        #endregion

        #region Methods
            /// <summary>
            /// Removes a certain quantity of health points
            /// </summary>
            /// <param name="hpDrain">Health points lost</param>
            public virtual void drainHP(float hpDrain)
            {
                regen = false;
                seconds = 0;
                multiplier = 0;

                if (hpDrain >= 0)
                {
                    this.hp -= hpDrain;
                }
                if (this.hp <= 0)
                {
                    this.hp = 0;
                }
            }
            /// <summary>
            /// Checks if an animation can be interrumpted or not
            /// </summary>
            /// <param name="gameTime">gameTime</param>
            /// <returns></returns>
            public virtual bool canInterrupt(GameTime gameTime)
            {
                if (!Mobile.AnimatedSprite.CurrentAnimationName.Contains("dashing") ||
                    (InterrumptibleDashFrames.Contains(Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame) &&
                       Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration))
                {
                    return !Mobile.AnimatedSprite.CurrentAnimationName.Contains("attacking") ||
                       (InterrumptibleAttackFrames.Contains(Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame) &&
                       Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration);
                }
                return false;
                // It's interruptible if it's an interruptible frame that has finished playing or if it's not an attack

            }

            /// <summary>
            /// Makes the character die
            /// </summary>
            public abstract void die();
            /// <summary>
            /// Executes the character attack
            /// </summary>
            /// <param name="gameTime">Game time</param>
            public abstract void attack(GameTime gameTime);
            /// <summary>
            /// Executes the character dash
            /// </summary>
            public abstract void dash();
            /// <summary>
            /// Executes the character move
            /// </summary>
            /// <param name="direction">Direction in which to move the character</param>
            public abstract void move(Vector3 direction);

            /// <summary>
            /// Obtains the death animation
            /// </summary>
            /// <returns></returns>
            public abstract String getDeathAnimation();
            /// <summary>
            /// Regenerates life
            /// </summary>
            /// <param name="gameTime">Game time</param>
            /// <param name="update">Indicates if life can be regenerated or not</param>
            public void regenerate(GameTime gameTime, Boolean update)
            {
                if (update == true)
                {
                    this.seconds += 0.001f * gameTime.ElapsedGameTime.Milliseconds;
                    if (this.seconds > SECONDSMULTIPLIER)
                    {
                        multiplier = 1;
                    }
                }
                else
                {
                    this.seconds = 0;
                    this.multiplier = 0;
                }

                this.hp = MathHelper.Clamp(hp + multiplier * regeneration / 60.0f, 0, maxHP);
            }

        

        
        #endregion
    }
}
