using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Utils;
using Microsoft.Xna.Framework;

namespace Prototype.Characters.CharacterIO
{
    public class PlayerReader
    {
        /// <summary>
        /// Initial cell of the character
        /// </summary>
        public Pair<int, int> initialPosition;
        /// <summary>
        /// Initial orientation
        /// </summary>
        public Vector3 initialOrientation;
    }
}
