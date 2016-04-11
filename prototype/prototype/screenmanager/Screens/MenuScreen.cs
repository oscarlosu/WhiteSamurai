using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prototype.ScreenManager.Screens
{
    abstract class MenuScreen : Screen
    {
        #region Fields
        /// <summary>
        /// List of menu entries
        /// </summary>
        protected List<MenuEntry> menuEntries;
        /// <summary>
        /// Entry that has focus
        /// </summary>
        protected int highlightedEntry;
        /// <summary>
        /// Font to be used at drawing
        /// </summary>
        protected SpriteFont menuFont;
        /// <summary>
        /// Starting vertical position for the menu
        /// </summary>
        protected int minHeight;
        /// <summary>
        /// Final vertical position for the menu
        /// </summary>
        protected int maxHeight;
        /// <summary>
        /// Screen that calls this menu
        /// </summary>
        protected Screen previousScreen;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for menu screen
        /// </summary>
        /// <param name="name">Screen name</param>
        /// <param name="minHeight">Starting vertical position for the menu</param>
        /// <param name="maxHeight">Final vertical position for the menu</param>
        public MenuScreen(String name, int minHeight, int maxHeight)
            : this(name, minHeight, maxHeight, null)
        {
        }

        /// <summary>
        /// Constructor for the menu screen
        /// </summary>
        /// <param name="name">Screen name</param>
        /// <param name="minHeight">Starting vertical position for the menu</param>
        /// <param name="maxHeight">Final vertical position for the menu</param>
        /// <param name="previous">Screen that calls this menu</param>
        public MenuScreen(String name, int minHeight, int maxHeight, Screen previous)
            : base(name)
        {
            this.highlightedEntry = 0;
            menuEntries = new List<MenuEntry>();
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.previousScreen = previous;
        }

        #endregion

        #region methods
        /// <summary>
        /// Loads content for the menu
        /// </summary>
        /// <param name="content">Content Manager</param>
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            menuFont = content.Load<SpriteFont>("Fonts/menuFont");
        }

        /// <summary>
        /// Updates the menu
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Highlighting the option
            if (menuEntries.Count() > highlightedEntry)
            {
                menuEntries.ElementAt(highlightedEntry).highlight();

                //Handle the input
                HandleInput(ScreenManager.Input);

                Vector4 pos = this.menuEntries.ElementAt(highlightedEntry).Position;
                //Update positions of the menu entries:
                if (pos.Y + pos.W > this.maxHeight)
                {
                    float pixelsToSubstract = pos.Y + pos.W - this.maxHeight;
                    foreach (MenuEntry menEntr in menuEntries)
                    {
                        Vector4 newPos = menEntr.Position;
                        newPos.Y -= pixelsToSubstract;
                        menEntr.Position = newPos;
                        if (isEntryVisible(menEntr))
                        {
                            menEntr.Show();
                        }
                        else
                        {
                            menEntr.Hide();
                        }
                    }
                }
                else if (pos.Y < this.minHeight)
                {
                    float pixelsToAdd = this.minHeight - pos.Y;
                    foreach (MenuEntry menEntr in menuEntries)
                    {
                        Vector4 newPos = menEntr.Position;
                        newPos.Y += pixelsToAdd;
                        menEntr.Position = newPos;
                        if (isEntryVisible(menEntr))
                        {
                            menEntr.Show();
                        }
                        else
                        {
                            menEntr.Hide();
                        }
                    }
                }
            }


        }
        /// <summary>
        /// Manages the input. Indicates if the higlighted entry has changed, or, in case an entry has been selected,
        /// executes the event handler
        /// </summary>
        /// <param name="input">Input state</param>
        public override void HandleInput(InputState input)
        {
            PlayerIndex? pl;
            if (menuEntries.Count > 0)
            {
                if (input.isKeyNewlyPressed(Keys.Down) || input.isGamePadButtonNewlyPressed(Buttons.DPadDown, out pl))
                {
                    menuEntries.ElementAt(highlightedEntry).unHighlight();
                    if (highlightedEntry + 1 < menuEntries.Count)
                    {
                        highlightedEntry++;
                        menuEntries.ElementAt(highlightedEntry).highlight();
                    }
                    else
                    {
                        highlightedEntry = 0;
                        menuEntries.ElementAt(0).highlight();
                    }
                }
                else if (input.isKeyNewlyPressed(Keys.Up) || input.isGamePadButtonNewlyPressed(Buttons.DPadUp, out pl))
                {
                    menuEntries.ElementAt(highlightedEntry).unHighlight();
                    if (highlightedEntry - 1 >= 0)
                    {
                        highlightedEntry--;
                        menuEntries.ElementAt(highlightedEntry).highlight();
                    }
                    else
                    {
                        highlightedEntry = menuEntries.Count - 1;
                        menuEntries.ElementAt(highlightedEntry).highlight();
                    }
                }
                else if (input.isKeyNewlyPressed(Keys.Enter) || input.isGamePadButtonNewlyPressed(Buttons.A, out pl))
                {
                    Boolean keyboard = false;
                    if (pl == null)
                    {
                        pl = PlayerIndex.One;
                        keyboard = true;
                    }
                    menuEntries.ElementAt(highlightedEntry).OnSelectedEntry((PlayerIndex)pl, keyboard);
                }
                else if (input.isKeyNewlyPressed(Keys.Escape) || input.isGamePadButtonNewlyPressed(Buttons.B, out pl))
                {
                    onCancel();
                }
            }
        }

        /// <summary>
        /// Adds an entry to the menu
        /// </summary>
        /// <param name="entry">New entry</param>
        public void AddEntry(MenuEntry entry)
        {
            entry.LoadContent(ScreenManager.Game.Content);
            this.menuEntries.Add(entry);
        }

        /// <summary>
        /// In case the menu is exited with no action
        /// </summary>
        public virtual void onCancel()
        {
            if(previousScreen != null)
                ScreenManager.setActiveScreen(previousScreen.Name);
            ExitScreen();
        }

        /// <summary>
        /// Draws the different menuEntries
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (
                int i = 0; i < this.menuEntries.Count(); ++i)
            {
                this.menuEntries.ElementAt(i).Draw(spriteBatch, menuFont, this.minHeight, this.maxHeight);
            }
        }

        public override void changeResolution(Utils.Pair<int, int> oldRes, Utils.Pair<int, int> newRes)
        {
            double heightChange = (newRes.Second + 0.0) / (oldRes.Second + 0.0);
            double widthChange = (newRes.First + 0.0) / (oldRes.First + 0.0);

            this.maxHeight = (int)Math.Truncate(this.maxHeight * heightChange + 0.5);
            this.minHeight = (int)Math.Truncate(this.minHeight * heightChange + 0.5);

            foreach (MenuEntry menEntr in menuEntries)
            {
                Vector4 newPos = menEntr.Position;
                newPos.X = (int)Math.Truncate(newPos.X * widthChange + 0.5); ;
                newPos.Y = (int)Math.Truncate(newPos.Y * heightChange + 0.5); ;
                newPos.Z = (int)Math.Truncate(newPos.Z * widthChange + 0.5); ;
                newPos.W = (int)Math.Truncate(newPos.W * heightChange + 0.5); ;
                menEntr.Position = newPos;
                if (isEntryVisible(menEntr))
                    menEntr.Show();
                else
                    menEntr.Hide();
            }
        }

        /// <summary>
        /// Checks if an entry is visible
        /// </summary>
        /// <param name="menEntr">Menu Entry to check</param>
        /// <returns>True if it is visible, false if not</returns>
        public bool isEntryVisible(MenuEntry menEntr)
        {
            Vector4 newPos = menEntr.Position;
            if (newPos.Y > this.maxHeight || newPos.Y + newPos.W < this.minHeight)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

    }
}
