using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.ScreenManager;
using Prototype.Characters.Enemies;

namespace Prototype.Characters.Strategies
{
    public interface MoveStrategy
    {
        /// <summary>
        /// Obtains the next action that an enemy has to do
        /// </summary>
        /// <param name="previousAction">Previous action performed</param>
        /// <param name="position">Position of the enemy</param>
        /// <param name="playerPosition">Position of the player</param>
        /// <param name="orientation">Orientation of the enemy</param>
        /// <param name="state">State of the enemy</param>
        /// <returns>The next action</returns>
        String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime);
        String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime);
    }
}
