using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.MobileObject;
using Prototype.Characters.CharacterIO;

namespace Prototype.Characters
{
    public abstract class EnemyFactory
    {
        CharModel warriorModel;
        CharModel archerModel;
        CharModel bossModel;

        public EnemyFactory()
        {
            warriorModel = ModelReader.readFromXML("Characters/XML/warrior.xml");
            archerModel = ModelReader.readFromXML("Characters/XML/archer.xml");
            bossModel = ModelReader.readFromXML("Characters/XML/boss.xml");
        }

        /// <summary>
        /// Creates a warrior type enemy
        /// </summary>
        /// <param name="initialPosition">Initial position for the warrior</param>
        /// <param name="initialOrientation">Indicates in which direction the warrior looks when created</param>
        /// <returns></returns>
        public virtual EnemyWarrior createEnemyWarrior(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {

            MobileObject mobile = new MobileObject(warriorModel.collidables, initialPosition, warriorModel.drawOffset, warriorModel.attackCollider);
            mobile.AnimatedSprite.nSpritesRow = warriorModel.nFramesRow;
            foreach (AnimationReader reader in warriorModel.animations)
            {
                mobile.addAnimation(reader.name, reader.x, reader.y, reader.width, reader.height, reader.nFrames, reader.frameDuration);
            }
            EnemyWarrior warrior = new EnemyWarrior(id, warriorModel.hp, warriorModel.dps, warriorModel.regeneration, warriorModel.speed, warriorModel.visionRadio, mobile, warriorModel.spriteSheet);
            warrior.PreviousMove = initialOrientation;
            if (warriorModel.interrumptibleAttackFrames != null)
                warrior.InterrumptibleAttackFrames.AddRange(warriorModel.interrumptibleAttackFrames);
            if (warriorModel.interrumptibleDashFrames != null)
                warrior.InterrumptibleDashFrames.AddRange(warriorModel.interrumptibleDashFrames);
            return warrior;
        }

        /// <summary>
        /// Creates an archer type enemy
        /// </summary>
        /// <param name="initialPosition">Initial position for the archer</param>
        /// <param name="initialOrientation">Indicates in which direction the archer looks when created</param>
        /// <returns></returns>
        public virtual EnemyArcher createEnemyArcher(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {
            MobileObject mobile = new MobileObject(archerModel.collidables, initialPosition, archerModel.drawOffset, archerModel.attackCollider);
            mobile.AnimatedSprite.nSpritesRow = archerModel.nFramesRow;
            foreach (AnimationReader reader in archerModel.animations)
            {
                mobile.addAnimation(reader.name, reader.x, reader.y, reader.width, reader.height, reader.nFrames, reader.frameDuration);
            }
            EnemyArcher archer = new EnemyArcher(id, archerModel.hp, archerModel.dps, archerModel.regeneration, archerModel.speed, archerModel.visionRadio, archerModel.shotRadio, archerModel.escapeRadio, mobile, archerModel.spriteSheet);
            archer.PreviousMove = initialOrientation;
            if (archerModel.interrumptibleAttackFrames != null)
                archer.InterrumptibleAttackFrames.AddRange(archerModel.interrumptibleAttackFrames);
            if (archerModel.interrumptibleDashFrames != null)
                archer.InterrumptibleDashFrames.AddRange(archerModel.interrumptibleDashFrames);
            return archer;
        }

        /// <summary>
        /// Creates a boss type enemy
        /// </summary>
        /// <param name="initialPosition">Initial position for the boss</param>
        /// <param name="initialOrientation">Indicates in which direction the boss looks when created</param>
        /// <returns></returns>
        public virtual EnemyBoss createEnemyBoss(int id, Vector3 initialPosition, Vector3 initialOrientation)
        {
            MobileObject mobile = new MobileObject(bossModel.collidables, initialPosition, bossModel.drawOffset, bossModel.attackCollider);
            mobile.AnimatedSprite.nSpritesRow = bossModel.nFramesRow;
            foreach (AnimationReader reader in bossModel.animations)
            {
                mobile.addAnimation(reader.name, reader.x, reader.y, reader.width, reader.height, reader.nFrames, reader.frameDuration);
            }

            EnemyBoss boss = new EnemyBoss(id, bossModel.hp, bossModel.dps, bossModel.regeneration, bossModel.speed, bossModel.visionRadio, mobile, bossModel.spriteSheet);
            boss.PreviousMove = initialOrientation;
            if (bossModel.interrumptibleAttackFrames != null)
                boss.InterrumptibleAttackFrames.AddRange(bossModel.interrumptibleAttackFrames);
            if (bossModel.interrumptibleDashFrames != null)
                boss.InterrumptibleDashFrames.AddRange(bossModel.interrumptibleDashFrames);
            return boss;
        }
    }
}
