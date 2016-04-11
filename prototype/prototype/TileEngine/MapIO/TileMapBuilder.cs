using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.Map;
using Prototype.Utils;
using System.Xml.Serialization;
using System.IO;
using Prototype.TileEngine.MapIO;

namespace Prototype.TileEngine.MapIO
{

    static class TileMapBuilder
    {
        /// <summary>
        /// Reads a map from a file
        /// </summary>
        /// <param name="mapFile">Name of the file containing the map</param>
        public static TileMap createMap(String mapFile)
        {
            TileMapReader reader = TileMapReader.readFromXMLFile(mapFile);
            //Initializing the map
            TileMap map = new TileMap(reader.nRows, reader.nCols);

            // First, we read the intervals
            foreach (Interval interv in reader.intervals)
            {
                for (int x = interv.start.First; x <= interv.end.First; ++x)
                {
                    if (x >= 0 && x < reader.nRows)
                    {
                        for (int y = interv.start.Second; y <= interv.end.Second; ++y)
                        {
                            if (y >= 0 && y < reader.nCols)
                            {
                                map.Cells[x][y].addTerrainTileList(interv.tiles);
                                map.Cells[x][y].addTerrainObjectTileList(interv.objects);
                            }
                        }
                    }
                }
            }


            // Next, we read the lists
            foreach (CellList cellL in reader.cellLists)
            {
                foreach (Pair<int, int> cell in cellL.list)
                {
                    if (cell.First >= 0 && cell.First < reader.nRows)
                    {
                        if (cell.Second >= 0 && cell.First < reader.nCols)
                        {
                            map.Cells[cell.First][cell.Second].addTerrainTileList(cellL.tiles);
                            map.Cells[cell.First][cell.Second].addTerrainObjectTileList(cellL.objects);
                        }
                    }
                }
            }

            return map;

        }

        /// <summary>
        /// Writes a map to a file
        /// </summary>
        /// <param name="mapFile">Name of the objective file</param>
        /// <param name="map">Tile map to save</param>
        public static void saveMap(TileMap map, String mapFile)
        {
            TileMapReader reader = new TileMapReader();
            reader.nCols = map.NCols;
            reader.nRows = map.NRows;

            for (int i = 0; i < map.NRows; ++i)
            {
                for (int j = 0; i < map.NCols; ++j)
                {
                    CellList cellL = new CellList();
                    List<Tile> listaTiles = map.Cells[i][j].Terrain;
                    cellL.tiles.AddRange(listaTiles);
                    List<TerrainObject> listaObjects = map.Cells[i][j].TerrainObjects;
                    cellL.objects.AddRange(listaObjects);
                    reader.cellLists.Add(cellL);
                }
            }

            TileMapReader.writeToXMLFile(reader, mapFile);

            return;

        }

    }
}
