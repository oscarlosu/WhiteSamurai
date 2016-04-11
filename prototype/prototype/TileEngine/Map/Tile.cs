using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.TileEngine.Map
{
    /// <summary>
    /// Enum that represents a tile in the TileSheet
    /// </summary>
    public enum Tile
    {
        // SWNE
        empty = -1,
        // 4 elevated vertices
        elevated, // 0
        // 1 elevated vertex
        slopeHighSVertex, // 1
        slopeHighWVertex,
        slopeHighNVertex,
        slopeHighEVertex,
        // 2 elevated vertices
        slopeLowSWEdge, // 5
        slopeLowNWEdge,
        slopeLowNEEdge,
        slopeLowSEEdge,
        // 3 elevated vertices
        slopeLowSVertex, // 9
        slopeLowWVertex,
        slopeLowNVertex,
        slopeLowEVertex
    };

    /// <summary>
    /// Enum that represents a tile in the ObjectSheet
    /// </summary>
    public enum TerrainObject
    {
        empty = -1,
        // Trees
        coneTreeLower = 8,
        coneTreeMiddle = 4,
        coneTreeUpper = 0,
        sphereTreeLower = 5,
        sphereTreeUpper = 1,
        // Rocks
        rock1 = 9,
        rock2 = 2,
        rock3 = 3,
        rock4 = 6,
        rock5 = 7
    };
}
