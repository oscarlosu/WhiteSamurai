using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Characters.CharacterIO
{
    public enum StrategiesType
    {
        IDLE = 0,
        RANDOM,
        ATTACK,
        CHASING,
        ASTARCHASING,
        FLY,
        ALERT
    }

    public enum EnemyType
    {
        WARRIOR = 0,
        ARCHER,
        BOSS
    }
}
