using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Prototype.TileEngine.IsometricView;

namespace Prototype.TileEngine.Map
{
    class TileMap
    {

        #region Fields
        /// <summary>
        /// Cell matrix
        /// </summary>
        private List<List<MapCell>> cells;
        /// <summary>
        /// Number of rows in the map
        /// </summary>
        private int nRows;
        /// <summary>
        /// Number of columns in the map
        /// </summary>
        private int nCols;
       
        /******************** DEBUG ***********************/
        //private string axis;
        //private Texture2D axisSprite;

        private const int MARGIN = 0;

        #endregion

        #region Properties
            /// <summary>
            /// <see cref="cells"/>
            /// </summary>
            public List<List<MapCell>> Cells
            {
                get
                {
                    return cells;
                }
                set
                {
                    cells = value;
                }
            }


            public int NRows /**< @see numRows */
            {
                get
                {
                    return nRows;
                }
                set
                {
                    nRows = value;
                }
            }
        
            public int NCols /**< @see numCols */
            {
                get
                {
                    return nCols;
                }
                set
                {
                    nCols = value;
                }
            } 
         
     /*       public Texture2D AxisSprite /**< @see axisSprite */
      /*      {
                get
                {
                    return axisSprite;
                }
            }*/
        #endregion

        #region Constructor
        
            public TileMap(int nRows, int nCols)
            {
                cells = new List<List<MapCell>>();
                this.nRows = nRows;
                this.nCols = nCols;
                for (int x = 0; x < nRows; ++x)
                {
                    cells.Add(new List<MapCell>());
                    for (int y = 0; y < nCols; ++y)
                    {
                        cells[x].Add(new MapCell());
                    }
                }
            }

        #endregion

        #region Methods

            public void Update(GameTime gameTime)
            {
                Vector3 firstCell = MapManager.Instance.FirstCell;
                Vector3 lastCell = MapManager.Instance.LastCell;

                for (int row = (int)Math.Floor(firstCell.Y); row < (int)Math.Floor(lastCell.Y); ++row)
                {
                    for (int col = (int)Math.Floor(firstCell.X); col < (int)Math.Floor(lastCell.X); ++col)
                    {
                        Vector2 currentCell = new Vector2(col, row);
                        Cells[row][col].Update(gameTime, currentCell);
                    }
                }
            }

            public void Draw(SpriteBatch batch)
            {
                Vector3 firstCell = MapManager.Instance.FirstCell;
                Vector3 lastCell = MapManager.Instance.LastCell;
                // Update center
                for (int row = (int)Math.Floor(firstCell.Y); row < (int)Math.Floor(lastCell.Y); ++row)
                {
                    for (int col = (int)Math.Floor(firstCell.X); col < (int)Math.Floor(lastCell.X); ++col)
                    {
                        Vector2 currentCell = new Vector2(col, row);
                        //TODO: Cambiar a no static
                        Cells[row][col].Draw(batch, currentCell);
                        //TODO: Controlar la parte no visible de los tiles
                    }
                }

                // Debug Info
               /* Vector2 offset = Coordinates.to2D(new Vector2(Coordinates.toInsideCell(center).X, Coordinates.toInsideCell(center).Y));
                batch.DrawString(Game1.font, "# Tiles drawn = " + drawCount, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 20), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                batch.DrawString(Game1.font, "centerCell = " + MapManager.Instance.CenterCell, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 35), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                batch.DrawString(Game1.font, "" + center, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 50), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                batch.DrawString(Game1.font, "" + offset, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 65), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                batch.DrawString(Game1.font, "firstCell = " + firstCell, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 80), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                batch.DrawString(Game1.font, "lastCell = " + lastCell, new Vector2(3 * Camera.ScreenWidth / 4 - 70, 95), Color.Green, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
                */
                // Draw Axis
             /*   Vector2 axisCenter = new Vector2(320, 320);
                Vector2 centerPos = Coordinates.toScreen(center) - Camera.Position;
                Vector2 pos = centerPos - axisCenter;
                batch.Draw(
                    axisSprite, pos, new Rectangle(0, 0, 640 , 480), Color.White, 
                    0f, Vector2.Zero, 1f, SpriteEffects.None, // default/unused
                    Camera.getDepth(Vector3.Zero, DrawableType.TILE, SpecialDepthPrecedence.WORLD_INFO) // Depth: the axis should always be drawn in the front
                    );
                */
            }

            /// <summary>
            /// Checks if a computer controlled character can advance from a cell to another
            /// </summary>
            /// <param name="originCell">First cell</param>
            /// <param name="destinyCell">Second cell</param>
            /// <returns>True if the NPC can advance, false if not</returns>
            public Boolean areConnected(Vector2 originCell, Vector2 destinyCell)
            {

                
                // If the cells are separated by more than one cell
                if (originCell.X < 0 || originCell.X >= nCols || originCell.Y < 0 || originCell.Y >= nRows)
                {
                    return false;
                }
                else if (destinyCell.X < 0 || destinyCell.X >= nCols || destinyCell.Y < 0 || destinyCell.Y >= nRows)
                {
                    return false;
                }

                if (originCell == destinyCell)
                {
                    return true;
                }
                else if (Math.Abs(originCell.X - destinyCell.X) > 1 || Math.Abs(originCell.Y - destinyCell.Y) > 1)
                {
                    return false;
                }

                MapCell origin = this.Cells[(int)originCell.Y][(int)originCell.X];
                MapCell destiny = this.Cells[(int)destinyCell.Y][(int)destinyCell.X];                

                if (destiny.TerrainObjectTileCount > 0) //Characters can't advance if there is an object in a cell
                {
                    return false;
                }

                //Characters can't advance if the diference of height between cells is higher than 1 (two or more).
                //If the diference is one, it might exist a slope
                if (Math.Abs(destiny.TerrainTileCount - origin.TerrainTileCount) > 1)
                {
                    return false;
                }
                else
                {
                    Vector3 checkPosOrigin;
                    Vector3 checkPosDestiny;
                    if (destinyCell.X < originCell.X && destinyCell.Y < originCell.Y) // DIRECTION : NORTH
                    {
                        if (!areConnected(originCell, new Vector2(originCell.X, originCell.Y - 1)) || !areConnected(originCell, new Vector2(originCell.X - 1, originCell.Y)))
                        {
                            return false;
                        }

                        checkPosOrigin = new Vector3(originCell.X * IsoInfo.TileSide + 1, originCell.Y * IsoInfo.TileSide + 1, 0);
                        checkPosDestiny = new Vector3(originCell.X * IsoInfo.TileSide - 1, originCell.Y * IsoInfo.TileSide - 1, 0);
                    }
                    else if (destinyCell.X > originCell.X && destinyCell.Y > originCell.Y) //DIRECTION : SOUTH
                    {
                        if(!areConnected(originCell, new Vector2(originCell.X + 1, originCell.Y)) || !areConnected(originCell, new Vector2(originCell.X, originCell.Y+1)))
                        {
                            return false;
                        }
                        checkPosOrigin = new Vector3((originCell.X + 1) * IsoInfo.TileSide - 1, (originCell.Y + 1) * IsoInfo.TileSide - 1, 0);
                        checkPosDestiny = new Vector3((originCell.X + 1) * IsoInfo.TileSide + 1, (originCell.Y + 1) * IsoInfo.TileSide + 1, 0);
                    }
                    else if (destinyCell.X < originCell.X && destinyCell.Y > originCell.Y) //DIRECTION : EAST
                    {
                        if(!areConnected(originCell, new Vector2(originCell.X, originCell.Y + 1)) || !areConnected(originCell, new Vector2(originCell.X - 1, originCell.Y)))
                        {
                            return false;
                        }
                        checkPosOrigin = new Vector3(originCell.X * IsoInfo.TileSide + 1, (originCell.Y + 1) * IsoInfo.TileSide - 1, 0);
                        checkPosDestiny = new Vector3(originCell.X * IsoInfo.TileSide - 1, (originCell.Y + 1) * IsoInfo.TileSide + 1, 0);
                    }
                    else if (destinyCell.X > originCell.X && destinyCell.Y < originCell.Y) //DIRECTION : WEST
                    {
                        if(!areConnected(originCell, new Vector2(originCell.X, originCell.Y-1)) || !areConnected(originCell, new Vector2(originCell.X + 1, originCell.Y)))
                        {
                            return false;
                        }
                        checkPosOrigin = new Vector3((originCell.X + 1) * IsoInfo.TileSide - 1, originCell.Y * IsoInfo.TileSide + 1, 0);
                        checkPosDestiny = new Vector3((originCell.X + 1) * IsoInfo.TileSide + 1, originCell.Y * IsoInfo.TileSide - 1, 0);
                    }
                    else if (destinyCell.X < originCell.X && destinyCell.Y == originCell.Y) //DIRECTION : NORTHEAST
                    {
                        checkPosOrigin = new Vector3(originCell.X * IsoInfo.TileSide + 1, originCell.Y * IsoInfo.TileSide + IsoInfo.TileSide / 2, 0);
                        checkPosDestiny = new Vector3(originCell.X * IsoInfo.TileSide - 1, originCell.Y * IsoInfo.TileSide + IsoInfo.TileSide / 2, 0);
                    }
                    else if (destinyCell.X == originCell.X && destinyCell.Y < originCell.Y) //DIRECTION : NORTHWEST
                    {
                        checkPosOrigin = new Vector3(originCell.X * IsoInfo.TileSide + IsoInfo.TileSide / 2, originCell.Y * IsoInfo.TileSide + 1, 0);
                        checkPosDestiny = new Vector3(originCell.X * IsoInfo.TileSide + IsoInfo.TileSide / 2, originCell.Y * IsoInfo.TileSide - 1, 0);
                    }
                    else if (destinyCell.X == originCell.X && destinyCell.Y > originCell.Y) // DIRECTION : SOUTHEAST
                    {
                        checkPosOrigin = new Vector3(originCell.X * IsoInfo.TileSide + IsoInfo.TileSide / 2, (originCell.Y + 1) * IsoInfo.TileSide - 1, 0);
                        checkPosDestiny = new Vector3(originCell.X * IsoInfo.TileSide + IsoInfo.TileSide / 2, (originCell.Y + 1) * IsoInfo.TileSide + 1, 0);
                    }
                    else // DIRECTION : SOUTHWEST
                    {
                        checkPosOrigin = new Vector3((originCell.X + 1) * IsoInfo.TileSide - 1, originCell.Y * IsoInfo.TileSide + IsoInfo.TileSide / 2, 0);
                        checkPosDestiny = new Vector3((originCell.X + 1) * IsoInfo.TileSide + 1, originCell.Y * IsoInfo.TileSide + IsoInfo.TileSide / 2, 0);
                    }

                    if (Math.Abs(origin.getTerrainElevation(checkPosOrigin) - destiny.getTerrainElevation(checkPosDestiny)) >= IsoInfo.TileElevation / 3)
                    {
                        return false;
                    }
                }
                return true;
            }
        #endregion
    }
}
