using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.Utils;

namespace Prototype.Characters.Strategies.AStar
{
    class SearchNode
    {
        public Pair<int,int> position;
        public SearchNode[] neighbours;
        public SearchNode parent;
        public bool isInOpenList;
        public bool isInClosedList;
        public float distanceToGoal;
        public float distanceTravelled;

        public SearchNode(int x, int y)
        {
            position = new Pair<int, int>(x, y);
            neighbours = new SearchNode[StrategyConstants.NUMDIR];
            isInOpenList = false;
            isInClosedList = false;
            distanceToGoal = 0;
            distanceTravelled = 0;
            parent = null;
        }
        public void clear()
        {
            isInOpenList = false;
            isInClosedList = false;
            parent = null;
            distanceTravelled = 0;
            distanceToGoal = 0;
        }
    }
}
