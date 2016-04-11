using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.ScreenManager;
using Prototype.Characters.Enemies;

namespace Prototype.Characters.Strategies
{
    class AttackStrategy : MoveStrategy
    {
       
        public String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime)
        {
            Action action = Action.ATTACK;
            String animationName;
            Vector3 direction;
            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }

            animationName = gameCharacter.getAction(Action.ATTACK, gameTime, out direction);
            gameCharacter.attack(gameTime);
            return animationName;
        }

        public String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime)
        {
            Action action = Action.ATTACK;
            String animationName;
            Vector3 direction;
            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }

            animationName = gameCharacter.getAction(Action.ATTACK, gameTime, out direction);
            gameCharacter.attack(gameTime);
            return animationName;
        }
    }
}
