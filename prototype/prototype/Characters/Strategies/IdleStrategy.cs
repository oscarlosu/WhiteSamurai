using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.ScreenManager;
using Prototype.Characters.Enemies;

namespace Prototype.Characters.Strategies
{
    class IdleStrategy : MoveStrategy
    {
        public String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime)
        {
            Vector3 direction;
            String action = gameCharacter.getAction(Action.IDLE, gameTime, out direction);
            return action;
        }
        public String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime)
        {
            Vector3 direction;
            gameCharacter.PreviousAction = Action.IDLE;
            String action = gameCharacter.getAction(Action.IDLE, gameTime, out direction);
            return action;
        }
    }
}
