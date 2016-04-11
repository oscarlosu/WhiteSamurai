using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.ScreenManager;
using Prototype.Characters.Enemies;

namespace Prototype.Characters.Strategies
{
    class RandomStrategy : MoveStrategy
    {
        int counter = 0;
        static Random rnd = new Random(DateTime.Now.Millisecond);

        public String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime)
        {
            Action action = gameCharacter.PreviousAction ;
            String animationName;
            Vector3 direction;
            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }
            else
            {
                if (gameCharacter.PreviousAction == Action.ATTACK)
                {
                    counter = 0;
                }

                if (counter == 0)
                {
                
                    action = (Action) rnd.Next(0, 10);
                }

                counter = (counter + 1)%30;

                animationName = gameCharacter.getAction(action, gameTime, out direction);
            }

            if (animationName.Contains("moving"))
            {
                gameCharacter.move(direction);
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }
            gameCharacter.PreviousAction = action;

            return animationName;
        }
        public String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime)
        {
            Action action = gameCharacter.PreviousAction;
            String animationName;
            Vector3 direction;
            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }
            else
            {
                if (gameCharacter.PreviousAction == Action.ATTACK)
                {
                    counter = 0;
                }

                if (counter == 0)
                {

                    action = (Action)rnd.Next(0, 10);
                }

                counter = (counter + 1) % 30;

                animationName = gameCharacter.getAction(action, gameTime, out direction);
            }

            if (animationName.Contains("moving"))
            {
                gameCharacter.move(direction);
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }
            gameCharacter.PreviousAction = action;

            return animationName;
        }
    }
}
