using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Prototype.TileEngine.MobileObject
{
    public class FrameAnimation : ICloneable
    {
        #region Fields and Properties
            private int NUM_FRAMES_ROW;
            /// <summary>
            /// Source rectangle for the initial frame
            /// </summary>
            private Rectangle initialFrameRectangle;
            /// <summary>
            /// Width of the frames
            /// </summary>
            public int FrameWidth
            {
                get { return initialFrameRectangle.Width; }
            }
            /// <summary>
            /// Height of the frames
            /// </summary>
            public int FrameHeight
            {
                get { return initialFrameRectangle.Height; }
            }

            public int NumFramesRow
            {
                set
                {
                    NUM_FRAMES_ROW = value;
                }
            }
            /// <summary>
            /// Source rectangle for the current frame
            /// </summary>
            public Rectangle CurrentFrameRectangle
            {
                get
                {
                    int aux = (initialFrameRectangle.X/initialFrameRectangle.Width) + currentFrame;
                    if (NUM_FRAMES_ROW == 0)
                        NUM_FRAMES_ROW = 1;
                    int initialFrameX = aux % (NUM_FRAMES_ROW);
                    int initialFrameY = aux / (NUM_FRAMES_ROW) + initialFrameRectangle.Y/initialFrameRectangle.Height;
                    return new Rectangle(
                        (initialFrameRectangle.Width * (initialFrameX)), initialFrameY * initialFrameRectangle.Height,
                        initialFrameRectangle.Width, initialFrameRectangle.Height);
                }
            }
            /// <summary>
            /// Number of frames in the animation
            /// </summary>
            private int nFrames = 1; /**< Number of frames in the animation */
            public int NFrames
            {
                get { return nFrames; }
                set { nFrames = value; }
            }
            /// <summary>
            /// Frame currently being displayed. Values range from 0 to nFrames - 1
            /// </summary>
            private int currentFrame = 0;
            public int CurrentFrame
            {
                get { return currentFrame; }
                set { currentFrame = (int)MathHelper.Clamp(value, 0, nFrames - 1); }
            }
            /// <summary>
            /// Amount of time each frame should be displayed (in seconds)
            /// </summary>
            private float frameDuration = 0.2f;
            public float FrameDuration
            {
                get { return frameDuration; }
                set { frameDuration = value; }
            }
            /// <summary>
            /// Amount of time that has passed since last frame change
            /// </summary>
            private float timer = 0.0f;
            public float Timer { get { return timer; } set { timer = value; } }

            /// <summary>
            /// Number of times the animation has been played
            /// </summary>
            private int playCount = 0;
            public int PlayCount
            {
                get { return playCount; }
                set { playCount = value; }
            }
            /// <summary>
            /// Name of the animation that should be played after this animation
            /// </summary>
            private string nextAnimation = null;
            public string NextAnimation
            {
                get { return nextAnimation; }
                set { nextAnimation = value; }
            }
        #endregion

        #region Constructors
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="firstFrame">First frame of the animation</param>
            /// <param name="nFrames">Number of frames</param>
            public FrameAnimation(Rectangle firstFrame, int nFrames)
            {
                initialFrameRectangle = firstFrame;
                this.nFrames = nFrames;
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x">First Frame origin X</param>
            /// <param name="y">First Frame origin Y</param>
            /// <param name="width">Frame Width</param>
            /// <param name="height">Frame Height</param>
            /// <param name="nFrames">Number of frames</param>
            public FrameAnimation(int x, int y, int width, int height, int nFrames)
            {
                initialFrameRectangle = new Rectangle(x, y, width, height);
                this.nFrames = nFrames;
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x">First frame origin X</param>
            /// <param name="y">First frame origin Y</param>
            /// <param name="width">Frame Width</param>
            /// <param name="height">Frame Height</param>
            /// <param name="nFrames">Number of frames</param>
            /// <param name="frameDuration">Duration of the frame</param>
            public FrameAnimation(int x, int y, int width, int height, int nFrames, float frameDuration)
            {
                initialFrameRectangle = new Rectangle(x, y, width, height);
                this.nFrames = nFrames;
                this.frameDuration = frameDuration;
            }
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="x">First frame origin X</param>
            /// <param name="y">First frame origin Y</param>
            /// <param name="width">Frame Width</param>
            /// <param name="height">Frame Height</param>
            /// <param name="nFrames">Number of frames</param>
            /// <param name="frameDuration">Duration of the frame</param>
            /// <param name="nextAnimation">Next animation</param>
            public FrameAnimation(int x, int y, int width, int height, int nFrames, float frameDuration, string nextAnimation)
            {
                initialFrameRectangle = new Rectangle(x, y, width, height);
                this.nFrames = nFrames;
                this.frameDuration = frameDuration;
                this.nextAnimation = nextAnimation;
            }
        #endregion

        #region Methods
            /// <summary>
            /// Update
            /// </summary>
            /// <param name="gameTime">Game time</param>
            public void Update(GameTime gameTime)
            {
                // Update timer with elapsed time
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Wait until frameDuration seconds have passed
                if (timer > frameDuration)
                {
                    // Reset timer
                    timer = 0.0f;
                    // Update currentFrame
                    currentFrame = (currentFrame + 1) % nFrames;
                    // If the an animation cycle has been completed, update playCount
                    if (currentFrame == 0)
                        playCount = (int)MathHelper.Min(playCount + 1, int.MaxValue);
                }
            }
       
            ///<summary>@brief Returns a deep copy of this FrameAnimation
            ///</summary>
            ///<return>Copy of this FrameAnimation</return>
            object ICloneable.Clone()
            {
                return new FrameAnimation(this.initialFrameRectangle.X, this.initialFrameRectangle.Y,
                                          this.initialFrameRectangle.Width, this.initialFrameRectangle.Height,
                                          this.nFrames, this.frameDuration, nextAnimation);
            }

        #endregion
    }
}
