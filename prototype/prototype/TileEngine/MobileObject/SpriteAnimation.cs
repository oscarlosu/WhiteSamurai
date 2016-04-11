using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Prototype.TileEngine.MobileObject
{
    public class SpriteAnimation
    {
        #region Fields and properties
        public int nSpritesRow;
        /// <summary>
        /// Spritesheet that contains thee images for the animations
        /// </summary>
        Dictionary<String, Texture2D> spriteSheet;
        public Dictionary<String, Texture2D> SpriteSheet
        {
            get { return spriteSheet; }
        }
        /// <summary>
        /// Flag that indicates if animationsn are being played
        /// </summary>
        bool isAnimating = true;
        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }
        /// <summary>
        /// Any value other than Color.White will colorize the images
        /// </summary>
        Color tint = Color.White;
        public Color Tint
        {
            get { return tint; }
            set { tint = value; }
        }
        /// <summary>
        /// Screen position for the sprite
        /// </summary>
        Vector2 position = new Vector2(0, 0);
        public Vector2 Position
        {
            get { return position; }
            set
            {
                lastPosition = position;
                position = value;
            }
        }
        public int X
        {
            get { return (int)position.X; }
            set
            {
                lastPosition.X = position.X;
                position.X = value;
            }
        }
        public int Y
        {
            get { return (int)position.Y; }
            set
            {
                lastPosition.Y = position.Y;
                position.Y = value;
            }
        }
        /*
        public float Depth 
        { 
            get 
            {
                // + new Vector3(0, 0, 128 /* Character spriteHeight)
                return Camera.getDepth(position, DrawableType.MOBILE_OBJECT, SpecialDepthPrecedence.WORLD);
            }
        }
        */

        Vector2 lastPosition = new Vector2(0, 0);
        /// <summary>
        /// Dictionary that allows us to identify different FrameAnimations with names
        /// </summary>
        Dictionary<string, FrameAnimation> frameAnimations = new Dictionary<string, FrameAnimation>(); /**< Dictionary that allows us to identify different FrameAnimations with names */
        
        /// <summary>
        /// Indicates which FrameAnimation is being played
        /// </summary>
        string currentAnimation = null;
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return frameAnimations[currentAnimation];
                else
                    return null;
            }
        }
        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set
            {
                if (frameAnimations.ContainsKey(value))
                {
                    currentAnimation = value;
                    frameAnimations[currentAnimation].CurrentFrame = 0;
                    frameAnimations[currentAnimation].PlayCount = 0;
                }
            }
        }
        /// <summary>
        /// Center of the sprite (calculated)
        /// </summary>
        Vector2 spriteCenter;
        /// <summary>
        /// Width of the sprite (calculated)
        /// </summary>
        int width;
        public int Width
        {
            get { return width; }
        }
        /// <summary>
        /// Height of the sprite (calculated)
        /// </summary>
        int height;
        public int Height
        {
            get { return height; }
        }
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, width, height); }
        }

        public String direction;
        #endregion

        #region Methods


        public SpriteAnimation()
        {
           // spriteSheet = new Dictionary<string, Texture2D>();
        }
        /// <summary>
        /// Adds an animation
        /// </summary>
        /// <param name="name">Name of the animation</param>
        /// <param name="x">Origin of the first frame rectangle X</param>
        /// <param name="y">Origin of the first frame rectangle Y</param>
        /// <param name="width">Frame width</param>
        /// <param name="height">Frame height</param>
        /// <param name="nFrames">Number of frames</param>
        /// <param name="frameDuration">Frame duration</param>
        public void addAnimation(string name, int x, int y, int width, int height, int nFrames, float frameDuration)
        {
            FrameAnimation frame = new FrameAnimation(x, y, width, height, nFrames, frameDuration);
            frame.NumFramesRow = this.nSpritesRow;
            frameAnimations.Add(name, frame);
            this.width = width;
            this.height = height;
            this.spriteCenter = new Vector2(width / 2, height / 2);
        }
        /// <summary>
        /// Adds an animation
        /// </summary>
        /// <param name="name">Name of the animation</param>
        /// <param name="x">Origin of the first frame rectangle X</param>
        /// <param name="y">Origin of the first frame rectangle Y</param>
        /// <param name="width">Frame width</param>
        /// <param name="height">Frame height</param>
        /// <param name="nFrames">Number of frames</param>
        /// <param name="frameDuration">Frame duration</param>
        /// <param name="nextAnimationName">Name of the next animation</param>
        public void addAnimation(string name, int x, int y, int width, int height, int nFrames, float frameDuration, string nextAnimationName)
        {
            FrameAnimation frame = new FrameAnimation(x, y, width, height, nFrames, frameDuration, nextAnimationName);
            frame.NumFramesRow = this.nSpritesRow;
            frameAnimations.Add(name, frame);
            this.width = width;
            this.height = height;
            this.spriteCenter = new Vector2(width / 2, height / 2);
        }
        /// <summary>
        /// Gets an animation by name
        /// </summary>
        /// <param name="name">Name of the animation</param>
        /// <returns>The animation if exists, null if not</returns>
        public FrameAnimation getAnimationByName(string name)
        {
            if (frameAnimations.ContainsKey(name))
            {
                return frameAnimations[name];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Move position
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void moveBy(int x, int y)
        {
            lastPosition = position;
            position.X += x;
            position.Y += y;
        }

        /// <summary>
        /// Loads the sprite Sheet
        /// </summary>
        /// <param name="content">Content manager</param>
        /// <param name="spriteSheetName">SpriteSheet name</param>
        public void LoadContent(ContentManager content, String spriteSheetName)
        {
            spriteSheet = new Dictionary<String, Texture2D>(); 
            Texture2D sheet;
            sheet = content.Load<Texture2D>(spriteSheetName + "North");
            spriteSheet.Add("North", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "NorthEast");
            spriteSheet.Add("NorthEast", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "East");
            spriteSheet.Add("East", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "SouthEast");
            spriteSheet.Add("SouthEast", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "South");
            spriteSheet.Add("South", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "SouthWest");
            spriteSheet.Add("SouthWest", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "West");
            spriteSheet.Add("West", sheet);
            sheet = content.Load<Texture2D>(spriteSheetName + "NorthWest");
            spriteSheet.Add("NorthWest", sheet);
            direction = "North";
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.CurrentAnimationName.Contains("NorthEast"))
            {
                this.direction = "NorthEast";
            }
            else if (this.CurrentAnimationName.Contains("NorthWest"))
            {
                this.direction = "NorthWest";
            }
            else if (this.CurrentAnimationName.Contains("SouthEast"))
            {
                this.direction = "SouthEast";
            }
            else if (this.CurrentAnimationName.Contains("SouthWest"))
            {
                this.direction = "SouthWest";
            }
            else if (this.CurrentAnimationName.Contains("North"))
            {
                this.direction = "North";
            }
            else if (this.CurrentAnimationName.Contains("South"))
            {
                this.direction = "South";
            }
            else if (this.CurrentAnimationName.Contains("East"))
            {
                this.direction = "East";
            }
            else if (this.CurrentAnimationName.Contains("West"))
            {
                this.direction = "West";
            }
            // Animate only if flag is set to true
            if (isAnimating)
            {
                // No active animation
                if (CurrentFrameAnimation == null)
                {
                    // Check if there are animations defined
                    if (frameAnimations.Count > 0)
                    {
                        // Set the first FrameAnimation in the dictionary be the currentAnimation
                        string[] sKeys = new string[frameAnimations.Count];
                        frameAnimations.Keys.CopyTo(sKeys, 0);
                        CurrentAnimationName = sKeys[0];
                    }
                    else
                    {
                        return;
                    }
                }

                // Update current FrameAnimation
                CurrentFrameAnimation.Update(gameTime);

                // Check if there is a nextAnimation defined for the current FrameAnimation
                if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    // Check if the animation loop has been completed
                    if (CurrentFrameAnimation.PlayCount > 0)
                    {
                        // If so, set up the next animation
                        CurrentAnimationName = CurrentFrameAnimation.NextAnimation;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the animation
        /// </summary>
        /// <param name="spriteBatch">batch</param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="depth">Depth of the animation</param>
        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset, float depth)
        {
            //if (isAnimating)
            spriteBatch.Draw(spriteSheet[this.direction], (position + new Vector2(xOffset, yOffset)),
                             CurrentFrameAnimation.CurrentFrameRectangle, tint,
                             0, Vector2.Zero, 1f, SpriteEffects.None,
                             depth);
        }

        #endregion
    }
}
