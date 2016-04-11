using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine;
using Prototype.TileEngine.Map;
using Microsoft.Xna.Framework;

namespace Prototype.Characters.Strategies.AStar
{
    public static class Pathfinder
    {
        private static SearchNode[,] searchNodes;
        private static int nCols;
        private static int nRows;
        private static List<SearchNode> openList;
        private static List<SearchNode> closedList;

        public static void Initialize()
        {
            StrategyConstants.Initialize();
            nCols = MapManager.Instance.CurrentMap.NCols;
            nRows = MapManager.Instance.CurrentMap.NRows;
            openList = new List<SearchNode>();
            closedList = new List<SearchNode>();
            searchNodes = new SearchNode[nRows, nCols];
            initializeSearchNodes();

        }

        public static float heuristic(Vector2 cell, Vector2 destinyCell)
        {
            return Math.Abs(cell.X - destinyCell.X) + Math.Abs(cell.Y - destinyCell.Y);
        }

        /// <summary>
        /// Initializes the search network
        /// </summary>
        public static void initializeSearchNodes()
        {
            TileMap map = MapManager.Instance.CurrentMap;

            // Initializing the nodes
            for(int x = 0; x < nRows; ++x)
            {
                for(int y = 0; y < nCols; ++y)
                {
                    if(map.Cells[x][y].TerrainObjectTileCount == 0) //If the cell has objects, then it can't be walked by NPCs
                    {
                        searchNodes[x,y] = new SearchNode(x,y);
                    }
                }
            }

            for (int x = 0; x < nCols; ++x)
            {
                for (int y = 0; y < nRows; ++y)
                {
                    Vector2 currentCell = new Vector2(x,y);
                    for (int i = 0; i < StrategyConstants.NUMDIR; ++i)
                    {
                        Vector2 destinyCell = new Vector2(x + StrategyConstants.directions[i].First, y + StrategyConstants.directions[i].Second);
                        if (destinyCell.X < 0 || destinyCell.X >= nCols || destinyCell.Y < 0 || destinyCell.Y >= nRows)
                        {
                            continue;
                        }
                        else if (searchNodes[y, x] == null)
                            continue;
                        if (!map.areConnected(currentCell, destinyCell))
                            continue;
                        searchNodes[y, x].neighbours[i] = searchNodes[y + StrategyConstants.directions[i].Second, x + StrategyConstants.directions[i].First];
                    }
                }
            }

        }

        /// <summary>
        /// Finds the best node in the open list
        /// </summary>
        /// <returns>The node with minimum distance to goal</returns>
        private static SearchNode findBestNode()
        {
            SearchNode currentTile = null;
            float smallestDistanceToGoal = float.MaxValue;
            for (int i = 0; i < openList.Count; ++i)
            {
                if (openList[i].distanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.distanceToGoal;
                }
            }
            return currentTile;
        }

        private static List<Vector2> findFinalPath(SearchNode startNode, SearchNode endNode)
        {
            List<SearchNode> aux = new List<SearchNode>();
            aux.Add(endNode);
            SearchNode parentTile = endNode.parent;

            while (parentTile != startNode)
            {
                aux.Add(parentTile);
                parentTile = parentTile.parent;
            }

            List<Vector2> path = new List<Vector2>();
            for (int i = aux.Count - 1; i >= 0; --i)
            {
                path.Add(new Vector2(aux[i].position.Second, aux[i].position.First));
            }

            return path;
        }

        private static void clearLists()
        {
            foreach (SearchNode node in openList)
            {
                node.clear();
            }
            foreach (SearchNode node in closedList)
            {
                node.clear();
            }
            openList.Clear();
            closedList.Clear();
        }

        public static List<Vector2> findPath(Vector2 startCell, Vector2 endCell)
        {
            if (startCell == endCell)
            {
                return new List<Vector2>();
            }

            SearchNode startNode = searchNodes[(int)startCell.Y, (int)startCell.X];
            SearchNode endNode = searchNodes[(int) endCell.Y, (int) endCell.X];
            startNode.isInOpenList = true;
            startNode.distanceToGoal = heuristic(startCell, endCell);
            startNode.distanceTravelled = 0;
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                SearchNode currentNode = findBestNode();
                if (closedList.Count > StrategyConstants.MAXCLOSEDLIST)
                {
                    break;
                }
                if (currentNode == null) //There is no path
                {
                    break;
                }
                else if (currentNode == endNode) // A path has been found
                {
                    List<Vector2> path = findFinalPath(startNode, currentNode);
                    clearLists();
                    return path;
                }
                else //General case
                {
                    for (int i = 0; i < StrategyConstants.NUMDIR; ++i)
                    {
                        SearchNode neighbour = currentNode.neighbours[i];
                        if (neighbour == null)
                            continue;

                        float distanceTravelled = currentNode.distanceTravelled + 1;
                        float heuristicCost = heuristic(new Vector2(neighbour.position.Second, neighbour.position.First), endCell);

                        if (neighbour.isInOpenList == false && neighbour.isInClosedList == false)
                        {
                            neighbour.distanceTravelled = distanceTravelled;
                            neighbour.distanceToGoal = distanceTravelled + heuristicCost;
                            neighbour.parent = currentNode;
                            neighbour.isInOpenList = true;
                            openList.Add(neighbour);
                        }
                        else
                        {
                            if (neighbour.distanceTravelled > distanceTravelled)
                            {
                                neighbour.distanceTravelled = distanceTravelled;
                                neighbour.distanceToGoal = distanceTravelled + heuristicCost;
                                neighbour.parent = currentNode;
                            }
                        }
                    }

                    openList.Remove(currentNode);
                    currentNode.isInOpenList = false;
                    currentNode.isInClosedList = true;
                    closedList.Add(currentNode);
                }
            }

            clearLists();
            return null;
        }
    }
}
