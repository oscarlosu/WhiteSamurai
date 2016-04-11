using System.Threading;
using Prototype.ScreenManager;
using Microsoft.Xna.Framework;
using Prototype.ScreenManager.Screens;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
namespace Prototype.ScreenManager.Screens
{
    class LoadingScreen : Screen  
    {
        #region Fields  
 
        Thread backgroundThread;  
        EventWaitHandle backgroundThreadExit;  
        bool loadingIsSlow;  
        SpriteFont font;
        Screen screenToLoad;  
        GameTime loadStartTime;
        bool initialized = false;
 
        string[] letters = { "L", "o", "a", "d", "i", "n", "g", ".", ".", "." };  
        int[] bounce = new int[10];  
        bool[] bounceBool = new bool[10];  
      
        #endregion  
 
        #region Initialization  
 
 
        /// <summary>  
        /// The constructor is private: loading screens should  
        /// be activated via the static Load method instead.  
        /// </summary>  
        public LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,  
                              Screen screenToLoad)  : base("loading screen")
        {  
            this.loadingIsSlow = loadingIsSlow;  
            this.screenToLoad = screenToLoad;  
              
            if (loadingIsSlow)  
            {  
                backgroundThread = new Thread(BackgroundWorkerThread);  
                backgroundThreadExit = new ManualResetEvent(false);  
            }  
 
        }  

        #endregion  
 
        #region Update and Draw  
 
 
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/menuFont");
        }
        /// <summary>  
        /// Updates the loading screen.  
        /// </summary>  
        public override void Update(GameTime gameTime)  
        {      
            // If all the previous screens have finished transitioning  
            // off, it is time to actually perform the load.  
            
                if (backgroundThread != null)  
                {  initialized = true;
                    loadStartTime = gameTime;  
                    backgroundThread.Start();  
                }  
                ScreenManager.deleteScreen(this);  
                
                ScreenManager.addScreen(screenToLoad);
                ScreenManager.setActiveScreen(screenToLoad.Name);
 
                // Signal the background thread to exit, then wait for it to do so.  
                if (backgroundThread != null)  
                {  
                    backgroundThreadExit.Set();  
                    backgroundThread.Join();  
                }  
                // Once the load has finished, we use ResetElapsedTime to tell  
                // the  game timing mechanism that we have just finished a very  
                // long frame, and that it should not try to catch up.  
                ScreenManager.Game.ResetElapsedTime();  
        }  
 
        /// <summary>  
        /// Worker thread draws the loading animation and updates the network  
        /// session while the load is taking place.  
        /// </summary>  
        void BackgroundWorkerThread()  
        {  
            long lastTime = Stopwatch.GetTimestamp();  
 
            // EventWaitHandle.WaitOne will return true if the exit signal has  
            // been triggered, or false if the timeout has expired. We use the  
            // timeout to update at regular intervals, then break out of the  
            // loop when we are signalled to exit.  
            while (!backgroundThreadExit.WaitOne(1000 / 30))  
            {  
                GameTime gameTime = GetGameTime(ref lastTime);  
 
                DrawLoadAnimation(gameTime);  
 
                //UpdateNetworkSession();  
            }  
        }  
 
 
        /// <summary>  
        /// Works out how long it has been since the last background thread update.  
        /// </summary>  
        GameTime GetGameTime(ref long lastTime)  
        {  
            long currentTime = Stopwatch.GetTimestamp();  
            long elapsedTicks = currentTime - lastTime;  
            lastTime = currentTime;  
 
            TimeSpan elapsedTime = TimeSpan.FromTicks(elapsedTicks *  
                                                      TimeSpan.TicksPerSecond /  
                                                      Stopwatch.Frequency);  
 
            return new GameTime(loadStartTime.TotalGameTime + elapsedTime, elapsedTime);  
        }  
 
 
        /// <summary>  
        /// Calls directly into our Draw method from the background worker thread,  
        /// so as to update the load animation in parallel with the actual loading.  
        /// </summary>  
        void DrawLoadAnimation(GameTime gameTime)  
        {  
            if ((ScreenManager.GraphicsDevice == null) || ScreenManager.GraphicsDevice.IsDisposed)  
                return;  
 
            try 
            {  
                ScreenManager.GraphicsDevice.Clear(Color.Black);  
 
                // Draw the loading screen.  
                Draw(ScreenManager.SpriteBatch);  
 
                // If we have a message display component, we want to display  
                // that over the top of the loading screen, too.  
                //if (messageDisplay != null)  
                //{  
                //    messageDisplay.Update(gameTime);  
                //    messageDisplay.Draw(gameTime);  
                //}  
 
                ScreenManager.GraphicsDevice.Present();  
            }  
            catch 
            {  
                // If anything went wrong (for instance the graphics device was lost  
                // or reset) we don't have any good way to recover while running on a  
                // background thread. Setting the device to null will stop us from  
                // rendering, so the main game can deal with the problem later on.  
                //ScreenManager.GraphicsDevice = null;  
            }  
        }  
 
        /// <summary>  
        /// Draws the loading screen.  
        /// </summary>  
        public override void Draw(SpriteBatch spriteBatch)  
        {   
            // The gameplay screen takes a while to load, so we display a loading  
            // message while that is going on, but the menus load very quickly, and  
            // it would look silly if we flashed this up for just a fraction of a  
            // second while returning from the game to the menus. This parameter  
            // tells us how long the loading is going to take, so we know whether  
            // to bother drawing the message.  
            if (loadingIsSlow)  
            {                 
 
                const string message = "Loading...";  
   
               
 
                // Center the text in the viewport.  
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;  

                Color color = Color.White;

                if (initialized == true)
                {
                    spriteBatch.Begin();
                    // Draw the text.  
                    // spriteBatch.Begin();  



                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (i > 0)
                        {
                            if (bounceBool[i] == false)
                            {
                                if (bounce[i - 1] > 10)
                                {
                                    if (bounce[i] < 20) bounce[i]++;
                                    else
                                        bounceBool[i] = true;
                                }
                            }
                            else
                            {
                                if (bounce[i - 1] < 10)
                                {
                                    if (bounce[i] > 0) bounce[i]--;
                                    else bounceBool[i] = false;
                                }
                            }
                        }
                        else
                        {
                            if (bounceBool[i] == false && bounceBool[i + 1] == false)
                            {
                                if (bounce[i] < 20) bounce[i]++;
                                else if (bounce[9] == 20)
                                    bounceBool[i] = true;
                            }
                            else
                            {
                                if (bounce[i] > 0) bounce[i]--;
                                else bounceBool[i] = false;
                            }
                        }

                        spriteBatch.DrawString(font, letters[i], (textPosition + new Vector2(32 * i, -bounce[i] * 2)), color);
                    }
                    spriteBatch.End();
                }
            }  
        }

        public override void HandleInput(InputState input)
        {
            
        }

        public override void UnloadContent()
        {
            
        }

        public override void changeResolution(Utils.Pair<int, int> oldRes, Utils.Pair<int, int> newRes)
        {
            
        }
        #endregion  
    }  
}  