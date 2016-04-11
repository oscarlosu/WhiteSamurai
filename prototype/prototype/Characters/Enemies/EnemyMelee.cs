using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.MobileObject;
using Prototype.ScreenManager;
using Microsoft.Xna.Framework;

namespace Prototype.Characters.Enemies
{
    public abstract class EnemyMelee : Enemy
    {
        #region Constructor
        /// <summary>
        /// Ranged enemy constructor
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="hp">Health points</param>
        /// <param name="dps">Damage per second</param>
        /// <param name="regeneration">Regeneration</param>
        /// <param name="speed">Movement Speed</param>
        /// <param name="visionRadio">Vision radio</param>
        /// <param name="mobile">Mobile object</param>
        /// <param name="spriteSheet">sprite sheet</param>
        public EnemyMelee(int id, float hp, float dps, float regeneration, float speed, float visionRadio, MobileObject mobile, String spriteSheet) :
                    base(id, hp, dps, regeneration, speed, visionRadio, mobile, spriteSheet)
                {

                }
        #endregion

        #region Methods
            public override void Update(InputState input, GameTime gameTime)
            {
                Vector3 playerPosition = CharacterManager.Instance.playerPosition;
                Update(gameTime, playerPosition, input);
            }

            public virtual void Update(GameTime gameTime, Vector3 playerPosition, InputState input)
            {
                String animationName;
                Action action;
                Vector3 direction;
                State state = State;
                if (HP > 0)
                {
                    if (Stun == 0)
                    {
                        animationName = Strategy.getNextActionMelee(this, input, gameTime);
                    }
                    else
                    {
                        
                        Stun = (Stun + 1) % Enemy.STUNTIME;
                        action = Action.IDLE;
                        animationName = getAction(action, gameTime, out direction);
                        
                    }

                    if (IsBerserkerMultiplied == true && State != State.BERSERKER) //If the character stops being berserker
                    {
                        DPS = DPS / 2.0f;
                        Speed = Speed / 2.0f;
                        VisionRadio = VisionRadio / 2.0f;
                        IsBerserkerMultiplied = false;
                    }
                    else if (IsBerserkerMultiplied == false && State == State.BERSERKER) //If the character becomes berserker
                    {
                        DPS = 2 * DPS;
                        Speed = 2 * Speed;
                        VisionRadio = 2 * VisionRadio;
                        IsBerserkerMultiplied = true;
                    }

                    if (State == State.BERSERKER)
                    {
                        drainHPBerserker();
                    }
                }
                else
                {
                    State = State.DEAD;
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
                this.Mobile.Update(gameTime, animationName);
            }
            
        #endregion
    }
}
