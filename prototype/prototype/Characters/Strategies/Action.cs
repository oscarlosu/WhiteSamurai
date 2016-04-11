using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Characters
{
    public enum Action
    {
        IDLE = 0,
        
        MOVENORTH = 1,
        MOVESOUTH = 2,
        MOVEEAST = 3,
        MOVEWEST = 4,
        MOVENORTHEAST = 5,
        MOVESOUTHEAST = 6,
        MOVESOUTHWEST = 7,
        MOVENORTHWEST = 8,

        ATTACK = 9,

        HEADNORTH = 10,
        HEADSOUTH = 11,
        HEADEAST = 12,
        HEADWEST = 13,
        HEADNORTHEAST = 14,
        HEADNORTHWEST = 15,
        HEADSOUTHEAST = 16,
        HEADSOUTHWEST = 17,
        HEAD = 18
    }
}
