using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.Map;
using Prototype.Utils;

namespace Prototype.TileEngine.ColorAlgorithm
{
    public static class ColorManager
    {
        public const int maxInitialColor = 50;
        public const int minValueColor = 50;
        public const float intensifyProb = 0.2f;
        public const int minIntensifyStep = 1;
        public const int maxIntensifyStep = 3;
        public const float horizontalPropagationProb = 0.0001f;
        public const float verticalPropagationProb = 0.0005f;
        public const float whitePropagationProb = 0.000001f;
        public const float whitePropagationProbWinning = 0.05f;
        private static Random rnd = new Random();

        /// <summary>
        /// Creates color cells starting from dead enemies
        /// </summary>
        /// <param name="cells">Cells where the enemies have dead</param>
        public static void createColor(List<Vector2> cells)
        {
            // Create color
            if (cells != null)
            {
                foreach (Vector2 cell in cells)
                {
                    MapCell mapCell = MapManager.Instance.CurrentMap.Cells[(int)cell.Y][(int)cell.X];
                    createColor(mapCell);
                }
            }
        }

        
        /// <summary>
        /// Updates the color
        /// </summary>
        public static void updateColor()
        {
            Vector3 initialCell = MapManager.Instance.FirstCell;
            Vector3 lastCell = MapManager.Instance.LastCell;

            
            // Update color
            for (int row = (int)Math.Floor(initialCell.Y); row < (int)Math.Floor(lastCell.Y); ++row)
            {
                for (int col = (int)Math.Floor(initialCell.X); col < (int)Math.Floor(lastCell.X); ++col)
                {
                    MapCell mapCell = MapManager.Instance.CurrentMap.Cells[row][col];
                    for (int i = 0; i < mapCell.TerrainTileCount; ++i)
                    {
                        if (mapCell.TerrainColor[i] != Color.White)
                        {
                            // Step 1: Horizontal Propagation
                            horizontalPropagation(row, col, i, mapCell.TerrainColor[i]);
                            // Step 2: Vertical Propagation
                            verticalPropagation(mapCell, i);
                            // Step 3: Intensify
                            //intensify(mapCell, i);
                            if (i == mapCell.TerrainTileCount - 1 && mapCell.TerrainObjectTileCount > 0)
                            {
                                propagateToObject(mapCell, mapCell.TerrainColor[i]);
                            }
                        }
                        else
                        {
                            whiteHorizontalPropagation(row, col, i);
                            whiteVerticalPropagation(mapCell, i);
                        }
                    }

                    for (int i = 0; i < mapCell.TerrainObjectTileCount; ++i)
                    {
                        if(mapCell.TerrainObjectColor[i] != Color.White)
                        {
                            horizontalObjectPropagation(row, col, i, mapCell.TerrainObjectColor[i]);
                            verticalObjectPropagation(mapCell, i);
                            if (i == 0)
                            {
                                propagateToTile(mapCell, mapCell.TerrainObjectColor[i]);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Propagates a color horizontally
        /// </summary>
        /// <param name="row">Map row</param>
        /// <param name="col">Map col</param>
        /// <param name="level">Elevation level</param>
        /// <param name="color">Color of the propagating cell</param>
        public static void horizontalPropagation(int row, int col, int level, Color color)
        {
            for (int i = row - 1; i <= row + 1; ++i)
            {
                for (int j = col - 1; j <= col + 1; ++j)
                {
                    if (i != row || j != col)
                    {
                        if (rnd.NextDouble() < horizontalPropagationProb)
                        {
                            if (i < MapManager.Instance.CurrentMap.NRows && i >= 0 && j < MapManager.Instance.CurrentMap.NCols && j >= 0)
                            {
                                MapCell mapCell = MapManager.Instance.CurrentMap.Cells[i][j];
                                if (mapCell.TerrainTileCount > level)
                                {

                                    if (mapCell.TerrainColor[level] == Color.White)
                                    {
                                        mapCell.TerrainColor[level] = color;
                                    }
                                    else
                                    {
                                        Color mapCellColor = mapCell.TerrainColor[level];
                                        mapCell.TerrainColor[level] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Propagates a color horizontally
        /// </summary>
        /// <param name="row">Map row</param>
        /// <param name="col">Map col</param>
        /// <param name="level">Elevation level</param>
        /// <param name="color">Color of the propagating cell</param>
        public static void whiteHorizontalPropagation(int row, int col, int level)
        {
            for (int i = row - 1; i <= row + 1; ++i)
            {
                for (int j = col - 1; j <= col + 1; ++j)
                {
                    if (i != row || j != col)
                    {
                        if (rnd.NextDouble() < whitePropagationProb)
                        {
                            if (i < MapManager.Instance.CurrentMap.NRows && i >= 0 && j < MapManager.Instance.CurrentMap.NCols && j >= 0)
                            {
                                MapCell mapCell = MapManager.Instance.CurrentMap.Cells[i][j];
                                if (mapCell.TerrainTileCount > level)
                                {

                                    if (mapCell.TerrainColor[level] != Color.White)
                                    {
                                        Color mapCellColor = mapCell.TerrainColor[level];
                                        mapCell.TerrainColor[level] = new Color((Color.White.R + mapCellColor.R) / 2, (Color.White.G + mapCellColor.G) / 2, (Color.White.B + mapCellColor.B) / 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Propagates color vertically
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="level">Level of the studied cell</param>
        public static void verticalPropagation(MapCell cell, int level)
        {
            Color color = cell.TerrainColor[level];
            for (int i = level - 1; i <= level + 1; ++i)
            {
                if (i >= 0 && i < cell.TerrainTileCount && i != level)
                {
                    if (rnd.NextDouble() < verticalPropagationProb)
                    {
                        if (cell.TerrainColor[i] == Color.White)
                        {
                            cell.TerrainColor[i] = color;
                        }
                        else
                        {

                            Color mapCellColor = cell.TerrainColor[i];
                            cell.TerrainColor[i] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagates color vertically
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="level">Level of the studied cell</param>
        public static void whiteVerticalPropagation(MapCell cell, int level)
        {
            
            for (int i = level - 1; i <= level + 1; ++i)
            {
                if (i >= 0 && i < cell.TerrainTileCount && i != level)
                {
                    if (rnd.NextDouble() < verticalPropagationProb)
                    {
                        if (cell.TerrainColor[i] != Color.White)
                        {
                            Color mapCellColor = cell.TerrainColor[i];
                            cell.TerrainColor[i] = new Color((Color.White.R + mapCellColor.R) / 2, (Color.White.G + mapCellColor.G) / 2, (Color.White.B + mapCellColor.B) / 2);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Intensifies a color
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="level">Elevation Level</param>
        public static void intensify(MapCell cell, int level)
        {
            Color color = cell.TerrainColor[level];
            if (rnd.NextDouble() < intensifyProb)
            {
                Vector3 intensify = RandomVector.colorRandomVector(minIntensifyStep, maxIntensifyStep);
                if (color.R - intensify.X < minValueColor)
                {
                    color.R = minValueColor;
                }
                else
                {
                    color.R -= (byte)intensify.X;
                }

                if (color.G - intensify.Y < minValueColor)
                {
                    color.G = minValueColor;
                }
                else
                {
                    color.G -= (byte)intensify.Y;
                }

                if (color.B - intensify.Z < minValueColor)
                {
                    color.B = minValueColor;
                }
                else
                {
                    color.B -= (byte)intensify.Z;
                }
            }
        }

        /// <summary>
        /// Colours a cell
        /// </summary>
        /// <param name="cell">Cell</param>
        public static void createColor(MapCell cell)
        {
            if (cell.TerrainColor[cell.TerrainTileCount - 1] == Color.White)
            {
                Vector3 color = RandomVector.colorRandomVector(maxInitialColor);
                cell.TerrainColor[cell.TerrainTileCount - 1] = new Color((int) color.X, (int) color.Y, (int) color.Z);
            }
            else
            {
                Vector3 colorV = RandomVector.colorRandomVector(maxInitialColor);
                Color color = new Color((int)colorV.X, (int)colorV.Y, (int)colorV.Z);
                Color mapCellColor = cell.TerrainColor[cell.TerrainTileCount - 1];
                cell.TerrainColor[cell.TerrainTileCount - 1] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                        
            }
        }

        /// <summary>
        /// Propagates color from a terrain tile to an object
        /// </summary>
        /// <param name="mapCell">Map cell</param>
        /// <param name="color">Color to propagate</param>
        public static void propagateToObject(MapCell mapCell, Color color)
        {
            if (rnd.NextDouble() < verticalPropagationProb)
            {
                if (mapCell.TerrainObjectColor[0] == Color.White)
                {
                    mapCell.TerrainObjectColor[0] = color;
                }
                else
                {
                    Color mapCellColor = mapCell.TerrainObjectColor[0];
                    mapCell.TerrainColor[0] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                }
            }
        }

        /// <summary>
        /// Propagates color from an object tile to a terrain tile
        /// </summary>
        /// <param name="mapCell">Cell</param>
        /// <param name="color">Color to propagate</param>
        public static void propagateToTile(MapCell mapCell, Color color)
        {
            if (rnd.NextDouble() < verticalPropagationProb)
            {
                if (mapCell.TerrainColor[mapCell.TerrainTileCount - 1] == Color.White)
                {
                    mapCell.TerrainColor[mapCell.TerrainTileCount - 1] = color;
                }
            }
            else
            {
                Color mapCellColor = mapCell.TerrainObjectColor[0];
                mapCell.TerrainColor[mapCell.TerrainTileCount - 1] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
            }
        }


        /// <summary>
        /// Propagates a color horizontally (in objects)
        /// </summary>
        /// <param name="row">Map row</param>
        /// <param name="col">Map col</param>
        /// <param name="level">Elevation level</param>
        /// <param name="color">Color of the propagating cell</param>
        public static void horizontalObjectPropagation(int row, int col, int level, Color color)
        {
            for (int i = row - 1; i <= row + 1; ++i)
            {
                for (int j = col - 1; j <= col + 1; ++j)
                {
                    if (i != row || j != col)
                    {
                        if (rnd.NextDouble() < horizontalPropagationProb)
                        {
                            if (i < MapManager.Instance.CurrentMap.NRows && i >= 0 && j < MapManager.Instance.CurrentMap.NCols && j >= 0)
                            {
                                MapCell mapCell = MapManager.Instance.CurrentMap.Cells[i][j];
                                if (mapCell.TerrainObjectTileCount > level)
                                {

                                    if (mapCell.TerrainObjectColor[level] == Color.White)
                                    {
                                        mapCell.TerrainObjectColor[level] = color;
                                    }
                                    else
                                    {
                                        Color mapCellColor = mapCell.TerrainObjectColor[level];
                                        mapCell.TerrainObjectColor[level] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propagates color vertically (in objects)
        /// </summary>
        /// <param name="cell">Cell</param>
        /// <param name="level">Level of the studied cell</param>
        public static void verticalObjectPropagation(MapCell cell, int level)
        {
            Color color = cell.TerrainObjectColor[level];
            for (int i = level - 1; i <= level + 1; ++i)
            {
                if (i >= 0 && i < cell.TerrainObjectTileCount && i != level)
                {
                    if (rnd.NextDouble() < verticalPropagationProb)
                    {
                        if (cell.TerrainObjectColor[i] == Color.White)
                        {
                            cell.TerrainObjectColor[i] = color;
                        }
                        else
                        {
                            Color mapCellColor = cell.TerrainObjectColor[i];
                            cell.TerrainObjectColor[i] = new Color((color.R + mapCellColor.R) / 2, (color.G + mapCellColor.G) / 2, (color.B + mapCellColor.B) / 2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gradually recovers the map original color (white)
        /// </summary>
        public static void whiten()
        {
            Vector3 initialCell = MapManager.Instance.FirstCell;
            Vector3 lastCell = MapManager.Instance.LastCell;


            // Update color
            for (int row = (int)Math.Floor(initialCell.Y); row < (int)Math.Floor(lastCell.Y); ++row)
            {
                for (int col = (int)Math.Floor(initialCell.X); col < (int)Math.Floor(lastCell.X); ++col)
                {
                    MapCell mapCell = MapManager.Instance.CurrentMap.Cells[row][col];
                    for (int i = 0; i < mapCell.TerrainTileCount; ++i)
                    {
                        if (rnd.NextDouble() < whitePropagationProbWinning)
                        {
                            Color color = mapCell.TerrainColor[i];
                            color = new Color((color.R + Color.White.R) / 2, (color.G + Color.White.G) / 2, (color.B + Color.White.B) / 2);
                            mapCell.TerrainColor[i] = color;
                        }
                    }

                    for (int i = 0; i < mapCell.TerrainObjectTileCount; ++i)
                    {
                        if (rnd.NextDouble() < whitePropagationProbWinning)
                        {
                            Color color = mapCell.TerrainObjectColor[i];
                            color = new Color((color.R + Color.White.R) / 2, (color.G + Color.White.G) / 2, (color.B + Color.White.B) / 2);
                            mapCell.TerrainObjectColor[i] = color;
                        }
                    }
                }
            }
        }

    }
}
