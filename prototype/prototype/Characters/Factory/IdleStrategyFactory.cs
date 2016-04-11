using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Characters.Strategies;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.IsometricView;

namespace Prototype.Characters.Factory
{
    class IdleStrategyFactory : EnemyFactory
    {
        public override EnemyWarrior createEnemyWarrior(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {
            EnemyWarrior warrior = base.createEnemyWarrior(id, initialPosition, initialOrientation);
            warrior.Strategy = new IdleStrategy();
            if (initialOrientation == IsoInfo.North)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorth";
            }
            else if (initialOrientation == IsoInfo.NorthEast)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthEast";
            }
            else if (initialOrientation == IsoInfo.East)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleEast";
            }
            else if (initialOrientation == IsoInfo.SouthEast)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthEast";
            }
            else if (initialOrientation == IsoInfo.South)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouth";
            }
            else if (initialOrientation == IsoInfo.SouthWest)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthWest";
            }
            else if (initialOrientation == IsoInfo.West)
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleWest";
            }
            else
            {
                warrior.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthWest";
            }


            return warrior;
        }

        public override EnemyArcher createEnemyArcher(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {
            EnemyArcher archer = base.createEnemyArcher(id, initialPosition, initialOrientation);
            archer.Strategy = new IdleStrategy();
            if (initialOrientation == IsoInfo.North)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorth";
            }
            else if (initialOrientation == IsoInfo.NorthEast)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthEast";
            }
            else if (initialOrientation == IsoInfo.East)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleEast";
            }
            else if (initialOrientation == IsoInfo.SouthEast)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthEast";
            }
            else if (initialOrientation == IsoInfo.South)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouth";
            }
            else if (initialOrientation == IsoInfo.SouthWest)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthWest";
            }
            else if (initialOrientation == IsoInfo.West)
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleWest";
            }
            else
            {
                archer.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthWest";
            }

            return archer;
        }

        public override EnemyBoss createEnemyBoss(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {
            EnemyBoss boss = base.createEnemyBoss(id, initialPosition, initialOrientation);
            boss.Strategy = new IdleStrategy();

            if (initialOrientation == IsoInfo.North)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorth";
            }
            else if (initialOrientation == IsoInfo.NorthEast)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthEast";
            }
            else if (initialOrientation == IsoInfo.East)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleEast";
            }
            else if (initialOrientation == IsoInfo.SouthEast)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthEast";
            }
            else if (initialOrientation == IsoInfo.South)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouth";
            }
            else if (initialOrientation == IsoInfo.SouthWest)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleSouthWest";
            }
            else if (initialOrientation == IsoInfo.West)
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleWest";
            }
            else
            {
                boss.Mobile.AnimatedSprite.CurrentAnimationName = "idleNorthWest";
            }

            return boss;
        }
    }
}
