using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.Utils;

namespace Prototype.Characters.CharacterIO
{
    

    public class EnemyReader
    {
        /// <summary>
        /// Enemy type
        /// </summary>
        public EnemyType type;
        /// <summary>
        /// Enemy strategy
        /// </summary>
        public StrategiesType strategy;
        /// <summary>
        /// Initial position
        /// </summary>
        public Pair<int, int> initialPosition;
        /// <summary>
        /// Initial orientation
        /// </summary>
        public Vector3 initialOrientation;
    }
}
