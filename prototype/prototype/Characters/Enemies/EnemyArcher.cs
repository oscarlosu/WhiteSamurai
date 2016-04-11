using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.MobileObject;
using Prototype.Projectiles;
using Prototype.Characters.Enemies;

namespace Prototype.Characters
{
    public class EnemyArcher : EnemyRanged
    {
        public EnemyArcher(int id, float hp, float dps, float regeneration, float speed, float visionRadio, float shotRadio, float escapeRadio, MobileObject mobile, String spriteSheet) :
            base(id, hp, dps, regeneration, speed, visionRadio, shotRadio, escapeRadio, mobile, spriteSheet)
        {
            Mobile.AnimatedSprite.Tint = Color.LightSlateGray;
        }

        public override void attack(GameTime gameTime)
        {

            if (Mobile.AnimatedSprite.CurrentFrameAnimation.CurrentFrame == Mobile.AnimatedSprite.CurrentFrameAnimation.NFrames - 3 &&
                Mobile.AnimatedSprite.CurrentFrameAnimation.Timer + (float)gameTime.ElapsedGameTime.TotalSeconds >= Mobile.AnimatedSprite.CurrentFrameAnimation.FrameDuration)
            {
                ProjectileManager.Instance.addProjectile(Position + 20*PreviousMove, PreviousMove);
            }
        }

    }
}
