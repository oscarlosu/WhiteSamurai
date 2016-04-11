using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Characters.CharacterIO
{
    public class GroupReader
    {
        public PlayerReader playerReader;
        public List<EnemyReader> group;
        
        public GroupReader()
        {
            group = new List<EnemyReader>();
        }
    }
}
