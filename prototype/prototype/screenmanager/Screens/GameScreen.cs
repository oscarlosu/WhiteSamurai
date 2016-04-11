using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Prototype.TileEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Prototype.Utils;
using Prototype.TileEngine.IsometricView;
using Prototype.Characters;
using Microsoft.Xna.Framework.Media;
using Prototype.TileEngine.ColorAlgorithm;
using Prototype.ScreenManager.Widget;
using Prototype.TileEngine.MobileObject;
using Prototype.Projectiles;
using Prototype.Characters.Strategies.AStar;


namespace Prototype.ScreenManager.Screens
{
    /// <summary>
    /// Main game screen
    /// </summary>
    class GameScreen : Screen
    {
        Texture2D background;
        HealthBar healthBar;
        float endingTime = 0;
        float LOADENDSCREENTIME = 5000;

        /// <summary>
        /// Constructor for the Game screen
        /// </summary>
        public GameScreen()
            : base("Game Screen")
        {
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        /// <see cref="Prototype.ScreenManager.Screen.LoadContent(ContentManager)"/>
        /// <param name="content">Content manager</param>
        public override void LoadContent(ContentManager content)
        {
            IsoInfo.Initialize();
            Camera.Initialize(ScreenManager.Game.Window);
            MapManager.Instance.LoadContent(content);
            MapManager.Instance.newMap("TileEngine/XML/mapTest.xml");
            CharacterManager.Instance.Initialize("Characters/XML/player.xml", "TileEngine/XML/mapTestCharacters.xml");
            CharacterManager.Instance.LoadContent(content);
            //  font = content.Load<SpriteFont>("Fonts/Kootenay");
            InputHandler.Initialize();
            background = content.Load<Texture2D>("Images/Backgrounds/background");
            
            healthBar = new HealthBar(CharacterManager.Instance.Player.MAXHP);
            healthBar.LoadContent(content);
            ProjectileManager.Instance.LoadContent(content);
            Pathfinder.Initialize();
            MusicManager.changeSong("inGameSong");
        }

        /// <summary>
        /// Unloadds content for the game
        /// </summary>
        public override void UnloadContent()
        {
        }

        /// <summary>
        /// Updates the different elements in the game
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="sm">Screen Manager</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (GameState.GameState.isGameOver() == false && GameState.GameState.isWinning() == false && GameState.GameState.isGameWon() == false)
            {
                CharacterManager.Instance.Update(gameTime, ScreenManager.Input);
                Camera.Update(CharacterManager.Instance.playerPosition);
                MapManager.Instance.Update(gameTime, CharacterManager.Instance.playerPosition);
                ColorManager.updateColor();
                HandleInput(ScreenManager.Input);
                healthBar.Update(CharacterManager.Instance.Player.HP, gameTime);
                ProjectileManager.Instance.Update(gameTime);
            }
            else if (GameState.GameState.isGameOver())
            {
                ScreenManager.addScreen(new GameOverScreen());
                ScreenManager.deleteScreen(this);
                ScreenManager.setActiveScreen("Game Over Screen");

            }
            else if (GameState.GameState.isWinning())
            {
                MusicManager.endSong();
                CharacterManager.Instance.UpdateOnWin(gameTime);
                Camera.Update(CharacterManager.Instance.playerPosition);
                MapManager.Instance.Update(gameTime, CharacterManager.Instance.playerPosition);
                ColorManager.whiten();
                ProjectileManager.Instance.Update(gameTime);
            }
            else //GameState.GameState.isGameWon()
            {
                endingTime += gameTime.ElapsedGameTime.Milliseconds;
                if (endingTime > LOADENDSCREENTIME)
                {
                    ScreenManager.addScreen(new EndGameScreen());
                    ScreenManager.deleteScreen(this);
                    ScreenManager.setActiveScreen("End Game Screen");
                }
                else
                {
                    CharacterManager.Instance.UpdateOnWin(gameTime);
                    Camera.Update(CharacterManager.Instance.playerPosition);
                    MapManager.Instance.Update(gameTime, CharacterManager.Instance.playerPosition);
                    ColorManager.whiten();
                    ProjectileManager.Instance.Update(gameTime);
                }
            }

        }

        /// <summary>
        /// Controls the input in the game.
        /// If StartAction is pressed, then, the game pauses, and the pause menu opens
        /// </summary>
        /// <param name="input">Keyboard, gamepad and mouse current state</param>
        public override void HandleInput(InputState input)
        {
            if (input.isGamePadButtonNewlyPressed(Buttons.Start, PlayerIndex.One) || input.isKeyNewlyPressed(Keys.Escape))
            {
                ScreenManager.addScreen(new PauseMenu(0, ScreenManager.Height, this));
                ScreenManager.setActiveScreen("Pause menu");
            }
        }

        /// <summary>
        /// Draws the different resources of the game
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.Width, ScreenManager.Height), Color.White);
            MapManager.Instance.Draw(spriteBatch);
            CharacterManager.Instance.Draw(spriteBatch);

            healthBar.Draw(spriteBatch, ScreenManager.Height, ScreenManager.Width);
            ProjectileManager.Instance.Draw(spriteBatch);
        }

        /// <summary>
        /// Changes game elements according to the new resolution
        /// </summary>
        public override void changeResolution(Pair<int, int> oldRes, Pair<int, int> newRes)
        {
            Camera.ScreenHeight = ScreenManager.Height;
            Camera.ScreenWidth = ScreenManager.Width;
            MapManager.Instance.adjustResolution();
        }
    }
}
