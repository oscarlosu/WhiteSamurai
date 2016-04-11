using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Characters.Enemies;
using Prototype.ScreenManager;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine;
using Prototype.Utils;

namespace Prototype.Characters.Strategies
{
    /// <summary>
    /// Searches for characters to join in a group
    /// </summary>
    class FlyStrategy : MoveStrategy
    {

        int counter;
        static Random rnd = new Random(DateTime.Now.Millisecond);
        public int[] adjacentCellsWeights = new int[StrategyConstants.NUMDIR];
        int radius = 3; //Number of cells to check in a square

        public FlyStrategy()
        {
            counter = 0;
            for (int i = 0; i < StrategyConstants.NUMDIR; ++i)
            {
                adjacentCellsWeights[i] = 0;
            }
        }

        public String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime)
        {
            Player player = CharacterManager.Instance.Player;
            Action action = gameCharacter.PreviousAction;
            String animationName;
            Vector3 direction;
            int index = 0;

            for (int i = 0; i < StrategyConstants.NUMDIR; ++i)
            {
                adjacentCellsWeights[i] = 0;
            }

            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }
            else
            {

                switch (gameCharacter.State)
                {
                    case State.IDLE:
                        if ((gameCharacter.Position - player.Position).Length() < gameCharacter.VisionRadio)
                        {
                            gameCharacter.State = State.ALERTED;
                        }
                        if (counter == 0)
                        {

                            action = (Action)rnd.Next(1, 9);
                            gameCharacter.PreviousAction = action;
                        }

                        counter = (counter + 1) % 30;
                        break;
                    case State.ALERTED: //Alerted and berserker cases are the same
                    case State.BERSERKER:
                    case State.GROUPALERTED:
                        if ((gameCharacter.Position - player.Position).Length() > gameCharacter.VisionRadio && gameCharacter.State != State.GROUPALERTED)
                        {
                            gameCharacter.State = State.IDLE;
                        }

                        if (gameCharacter.PreviousAction == Action.HEAD)
                        {
                            action = Action.ATTACK;
                            gameCharacter.PreviousAction = Action.ATTACK;
                        }
                        else
                        {
                            if (gameCharacter.PreviousAction == Action.ATTACK || gameCharacter.PreviousAction == Action.HEAD)
                            {
                                counter = 0;
                            }

                            if (counter == 0)
                            {
                                direction = lineOfSightFlyDirection(player.Position, gameCharacter.Position);
                                if (direction == Vector3.Zero)
                                {
                                    action = (Action)rnd.Next(1, 9);
                                    gameCharacter.PreviousAction = action;
                                }
                                else
                                {
                                    Vector2 playerCell = Coordinates.toCell(player.Position);
                                    Vector2 currentCell = Coordinates.toCell(gameCharacter.Position);
                                    Vector2 result = playerCell - currentCell;
                                    result.X = Math.Abs(result.X);
                                    result.Y = Math.Abs(result.Y);

                                    if (direction == IsoInfo.North)
                                    {
                                        action = Action.MOVENORTH;
                                        index = StrategyConstants.NORTH;
                                    }
                                    else if (direction == IsoInfo.NorthEast)
                                    {
                                        action = Action.MOVENORTHEAST;
                                        index = StrategyConstants.NORTHEAST;
                                    }
                                    else if (direction == IsoInfo.East)
                                    {
                                        action = Action.MOVEEAST;
                                        index = StrategyConstants.EAST;
                                    }
                                    else if (direction == IsoInfo.SouthEast)
                                    {
                                        action = Action.MOVESOUTHEAST;
                                        index = StrategyConstants.SOUTHEAST;
                                    }
                                    else if (direction == IsoInfo.South)
                                    {
                                        action = Action.MOVESOUTH;
                                        index = StrategyConstants.SOUTH;
                                    }
                                    else if (direction == IsoInfo.SouthWest)
                                    {
                                        action = Action.MOVESOUTHWEST;
                                        index = StrategyConstants.SOUTHWEST;
                                    }
                                    else if (direction == IsoInfo.West)
                                    {
                                        action = Action.MOVEWEST;
                                        index = StrategyConstants.WEST;
                                    }
                                    else
                                    {
                                        action = Action.MOVENORTHWEST;
                                        index = StrategyConstants.NORTHWEST;
                                    }
                                    gameCharacter.PreviousAction = action;

                                }
                            }

                            counter = (counter + 1) % 30;
                        }

                        break;
                    default:
                        break;
                }
            }
            animationName = gameCharacter.getAction(action, gameTime, out direction);
            if (animationName.Contains("moving"))
            {
                Vector3 previousDirection = gameCharacter.Position;
                gameCharacter.move(direction);
                if (gameCharacter.Position == previousDirection && counter == 1 && gameCharacter.PreviousAction != Action.HEAD) //Action has been decided in this turn and it has been proven useless
                {
                    Vector2 cell = Coordinates.toCell(gameCharacter.Position);
                    Vector2 destiny;
                    int value = StrategyConstants.INITVAL / 2;
                    for (int i = 1; i < StrategyConstants.NUMDIR / 2; ++i)
                    {

                        destiny.X = cell.X + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].First;
                        destiny.Y = cell.Y + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].Second;
                        if (MapManager.Instance.CurrentMap.areConnected(cell, destiny))
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = value;
                        }
                        else
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = 0;
                        }
                        value = value / 2;
                    }

                    for (int i = StrategyConstants.NUMDIR / 2; i < StrategyConstants.NUMDIR; ++i)
                    {
                        destiny.X = cell.X + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].First;
                        destiny.Y = cell.Y + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].Second;
                        if (MapManager.Instance.CurrentMap.areConnected(cell, destiny))
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = value;
                        }
                        else
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = 0;
                        }
                        value = value * 2;
                    }
                    value = 0;

                    if (rnd.NextDouble() > 0.5)
                    {
                        for (int i = 1; i < StrategyConstants.NUMDIR; ++i)
                        {
                            if (adjacentCellsWeights[i] > value)
                            {
                                index = i;
                                value = adjacentCellsWeights[i];
                            }
                        }
                    }
                    else
                    {
                        for (int i = StrategyConstants.NUMDIR - 1; i > 0; --i)
                        {
                            if (adjacentCellsWeights[i] > value)
                            {
                                index = i;
                                value = adjacentCellsWeights[i];
                            }
                        }
                    }


                    if (index == StrategyConstants.NORTH)
                    {
                        action = Action.MOVENORTH;
                        gameCharacter.PreviousAction = Action.MOVENORTH;
                        direction = IsoInfo.North;
                    }
                    else if (index == StrategyConstants.NORTHEAST)
                    {
                        action = Action.MOVENORTHEAST;
                        gameCharacter.PreviousAction = Action.MOVENORTHEAST;
                        direction = IsoInfo.NorthEast;
                    }
                    else if (index == StrategyConstants.EAST)
                    {
                        action = Action.MOVEEAST;
                        gameCharacter.PreviousAction = Action.MOVEEAST;
                        direction = IsoInfo.East;
                    }
                    else if (index == StrategyConstants.SOUTHEAST)
                    {
                        action = Action.MOVESOUTHEAST;
                        gameCharacter.PreviousAction = Action.MOVESOUTHEAST;
                        direction = IsoInfo.SouthEast;
                    }
                    else if (index == StrategyConstants.SOUTH)
                    {
                        action = Action.MOVESOUTH;
                        gameCharacter.PreviousAction = Action.MOVESOUTH;
                        direction = IsoInfo.South;
                    }
                    else if (index == StrategyConstants.SOUTHWEST)
                    {
                        action = Action.MOVESOUTHWEST;
                        gameCharacter.PreviousAction = Action.MOVESOUTHWEST;
                        direction = IsoInfo.SouthWest;
                    }
                    else if (index == StrategyConstants.WEST)
                    {
                        action = Action.MOVEWEST;
                        gameCharacter.PreviousAction = Action.MOVEWEST;
                        direction = IsoInfo.West;
                    }
                    else
                    {
                        action = Action.MOVENORTHWEST;
                        gameCharacter.PreviousAction = Action.MOVENORTHWEST;
                        direction = IsoInfo.NorthWest;
                    }

                    gameCharacter.move(direction);
                    counter = (counter + 1) % 30;
                }
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }
            gameCharacter.PreviousAction = action;

            return animationName;
        }


        public String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime)
        {
            Player player = CharacterManager.Instance.Player;
            Action action = gameCharacter.PreviousAction;
            String animationName;
            Vector3 direction;
            int index = 0;

            for (int i = 0; i < StrategyConstants.NUMDIR; ++i)
            {
                adjacentCellsWeights[i] = 0;
            }

            if (!gameCharacter.canInterrupt(gameTime))
            {
                // Ignore commands
                action = gameCharacter.PreviousAction;
                animationName = gameCharacter.Mobile.AnimatedSprite.CurrentAnimationName;
                direction = gameCharacter.PreviousMove;
            }
            else
            {

                switch (gameCharacter.State)
                {
                    case State.IDLE:
                        if ((gameCharacter.Position - player.Position).Length() < gameCharacter.VisionRadio)
                        {
                            gameCharacter.State = State.ALERTED;
                        }
                        if (counter == 0)
                        {

                            action = (Action)rnd.Next(1, 9);
                            gameCharacter.PreviousAction = action;
                        }

                        counter = (counter + 1) % 30;
                        break;
                    case State.ALERTED: //Alerted and berserker cases are the same
                    case State.BERSERKER:
                    case State.GROUPALERTED:
                        if ((gameCharacter.Position - player.Position).Length() > gameCharacter.VisionRadio && gameCharacter.State != State.GROUPALERTED)
                        {
                            gameCharacter.State = State.IDLE;
                        }

                        if (gameCharacter.PreviousAction == Action.HEAD)
                        {
                            action = Action.ATTACK;
                            gameCharacter.PreviousAction = Action.ATTACK;
                        }
                        else
                        {
                            if (gameCharacter.PreviousAction == Action.ATTACK || gameCharacter.PreviousAction == Action.HEAD)
                            {
                                counter = 0;
                            }

                            if (counter == 0)
                            {
                                direction = lineOfSightFlyDirection(player.Position, gameCharacter.Position);
                                if (direction == Vector3.Zero)
                                {
                                    action = (Action)rnd.Next(1, 9);
                                    gameCharacter.PreviousAction = action;
                                }
                                else
                                {
                                    Vector2 playerCell = Coordinates.toCell(player.Position);
                                    Vector2 currentCell = Coordinates.toCell(gameCharacter.Position);
                                    Vector2 result = playerCell - currentCell;
                                    result.X = Math.Abs(result.X);
                                    result.Y = Math.Abs(result.Y);

                                    if (direction == IsoInfo.North)
                                    {
                                        action = Action.MOVENORTH;
                                        index = StrategyConstants.NORTH;
                                    }
                                    else if (direction == IsoInfo.NorthEast)
                                    {
                                        action = Action.MOVENORTHEAST;
                                        index = StrategyConstants.NORTHEAST;
                                    }
                                    else if (direction == IsoInfo.East)
                                    {
                                        action = Action.MOVEEAST;
                                        index = StrategyConstants.EAST;
                                    }
                                    else if (direction == IsoInfo.SouthEast)
                                    {
                                        action = Action.MOVESOUTHEAST;
                                        index = StrategyConstants.SOUTHEAST;
                                    }
                                    else if (direction == IsoInfo.South)
                                    {
                                        action = Action.MOVESOUTH;
                                        index = StrategyConstants.SOUTH;
                                    }
                                    else if (direction == IsoInfo.SouthWest)
                                    {
                                        action = Action.MOVESOUTHWEST;
                                        index = StrategyConstants.SOUTHWEST;
                                    }
                                    else if (direction == IsoInfo.West)
                                    {
                                        action = Action.MOVEWEST;
                                        index = StrategyConstants.WEST;
                                    }
                                    else
                                    {
                                        action = Action.MOVENORTHWEST;
                                        index = StrategyConstants.NORTHWEST;
                                    }
                                    gameCharacter.PreviousAction = action;

                                }
                            }

                            counter = (counter + 1) % 30;
                        }

                        break;
                    default:
                        break;
                }
            }
            animationName = gameCharacter.getAction(action, gameTime, out direction);
            if (animationName.Contains("moving"))
            {
                Vector3 previousDirection = gameCharacter.Position;
                gameCharacter.move(direction);
                if (gameCharacter.Position == previousDirection && counter == 1 && gameCharacter.PreviousAction != Action.HEAD) //Action has been decided in this turn and it has been proven useless
                {
                    Vector2 cell = Coordinates.toCell(gameCharacter.Position);
                    Vector2 destiny;
                    int value = StrategyConstants.INITVAL / 2;
                    for (int i = 1; i < StrategyConstants.NUMDIR / 2; ++i)
                    {

                        destiny.X = cell.X + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].First;
                        destiny.Y = cell.Y + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].Second;
                        if (MapManager.Instance.CurrentMap.areConnected(cell, destiny))
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = value;
                        }
                        else
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = 0;
                        }
                        value = value / 2;
                    }

                    for (int i = StrategyConstants.NUMDIR / 2; i < StrategyConstants.NUMDIR; ++i)
                    {
                        destiny.X = cell.X + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].First;
                        destiny.Y = cell.Y + StrategyConstants.directions[(i + index) % StrategyConstants.NUMDIR].Second;
                        if (MapManager.Instance.CurrentMap.areConnected(cell, destiny))
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = value;
                        }
                        else
                        {
                            adjacentCellsWeights[(i + index) % StrategyConstants.NUMDIR] = 0;
                        }
                        value = value * 2;
                    }
                    value = 0;

                    if (rnd.NextDouble() > 0.5)
                    {
                        for (int i = 1; i < StrategyConstants.NUMDIR; ++i)
                        {
                            if (adjacentCellsWeights[i] > value)
                            {
                                index = i;
                                value = adjacentCellsWeights[i];
                            }
                        }
                    }
                    else
                    {
                        for (int i = StrategyConstants.NUMDIR - 1; i > 0; --i)
                        {
                            if (adjacentCellsWeights[i] > value)
                            {
                                index = i;
                                value = adjacentCellsWeights[i];
                            }
                        }
                    }


                    if (index == StrategyConstants.NORTH)
                    {
                        action = Action.MOVENORTH;
                        gameCharacter.PreviousAction = Action.MOVENORTH;
                        direction = IsoInfo.North;
                    }
                    else if (index == StrategyConstants.NORTHEAST)
                    {
                        action = Action.MOVENORTHEAST;
                        gameCharacter.PreviousAction = Action.MOVENORTHEAST;
                        direction = IsoInfo.NorthEast;
                    }
                    else if (index == StrategyConstants.EAST)
                    {
                        action = Action.MOVEEAST;
                        gameCharacter.PreviousAction = Action.MOVEEAST;
                        direction = IsoInfo.East;
                    }
                    else if (index == StrategyConstants.SOUTHEAST)
                    {
                        action = Action.MOVESOUTHEAST;
                        gameCharacter.PreviousAction = Action.MOVESOUTHEAST;
                        direction = IsoInfo.SouthEast;
                    }
                    else if (index == StrategyConstants.SOUTH)
                    {
                        action = Action.MOVESOUTH;
                        gameCharacter.PreviousAction = Action.MOVESOUTH;
                        direction = IsoInfo.South;
                    }
                    else if (index == StrategyConstants.SOUTHWEST)
                    {
                        action = Action.MOVESOUTHWEST;
                        gameCharacter.PreviousAction = Action.MOVESOUTHWEST;
                        direction = IsoInfo.SouthWest;
                    }
                    else if (index == StrategyConstants.WEST)
                    {
                        action = Action.MOVEWEST;
                        gameCharacter.PreviousAction = Action.MOVEWEST;
                        direction = IsoInfo.West;
                    }
                    else
                    {
                        action = Action.MOVENORTHWEST;
                        gameCharacter.PreviousAction = Action.MOVENORTHWEST;
                        direction = IsoInfo.NorthWest;
                    }

                    gameCharacter.move(direction);
                    counter = (counter + 1) % 30;
                }
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }
            gameCharacter.PreviousAction = action;

            return animationName;
        }





        /// <summary>
        /// Gets the direction in which to chase the player. If parameters are inversed, the enemy fliess
        /// </summary>
        /// <param name="playerDirection">Player position</param>
        /// <param name="enemyPosition">Enemy position</param>
        /// <returns></returns>
        private Vector3 lineOfSightFlyDirection(Vector3 playerPosition, Vector3 enemyPosition)
        {
            Vector3 relative = enemyPosition - playerPosition;
            relative = new Vector3(relative.X, relative.Y, 0); //We study only the first two coordinates
            if (relative == Vector3.Zero)
                return Vector3.Zero;

            double cosine = Vector3.Dot(relative, Vector3.UnitX) / (relative.Length());
            double angle = Math.Acos(cosine); //Returns a value between 0 and pi
            if (relative.Y < 0)
            {
                angle *= -1; //cos(x) = cos(-x)
            }

            Vector3 direction;
            if (angle >= -Math.PI / 8.0 && angle < Math.PI / 8.0)
                direction = IsoInfo.SouthWest;
            else if (angle >= Math.PI / 8.0 && angle < 3 * Math.PI / 8.0)
                direction = IsoInfo.South;
            else if (angle >= 3 * Math.PI / 8.0 && angle < 5 * Math.PI / 8.0)
                direction = IsoInfo.SouthEast;
            else if (angle >= 5 * Math.PI / 8.0 && angle < 7 * Math.PI / 8.0)
                direction = IsoInfo.East;
            else if (angle >= -3 * Math.PI / 8.0 && angle < -Math.PI / 8.0)
                direction = IsoInfo.West;
            else if (angle >= -5 * Math.PI / 8.0 && angle < -3 * Math.PI / 8.0)
                direction = IsoInfo.NorthWest;
            else if (angle >= -7 * Math.PI / 8.0 && angle < -5 * Math.PI / 8.0)
                direction = IsoInfo.North;
            else
                direction = IsoInfo.NorthEast;

            return direction;
        }

        /// <summary>
        /// Checks for nearby allies
        /// </summary>
        private void alert(Enemy enemy)
        {
            Vector2 cell = Coordinates.toCell(enemy.Position);
            EnemyGroup group = new EnemyGroup();
            group.addEnemy(enemy);
            for (int x = (int)cell.X - radius; x < cell.X + radius; ++x)
            {
                for (int y = (int)cell.Y - radius; y < cell.Y + radius; ++y)
                {
                    if (x >= 0 && x < MapManager.Instance.CurrentMap.NCols && y >= 0 && y < MapManager.Instance.CurrentMap.NRows)
                    {
                        List<int> enemies = CharacterManager.Instance.Map.getCell(y, x).EnemyIds;
                        foreach (int id in enemies)
                        {
                            if (enemy.ID != id && id != -1)
                            {
                                Enemy en = CharacterManager.Instance.getEnemy(id);
                                group.addEnemy(en);
                            }
                        }
                    }
                }

            }

            if (group.Enemies.Count() > 0)
            {
                int chasing = 0;
                int astar = 0;
                foreach (Enemy en in group.Enemies)
                {
                    if (en.Strategy.GetType() != typeof(ChasingStrategy) && en.Strategy.GetType() != typeof(AStarChasingStrategy))
                    {
                        if (rnd.NextDouble() > 0.5)
                        {
                            en.Strategy = new ChasingStrategy();
                            chasing++;
                        }
                        else
                        {
                            en.Strategy = new AStarChasingStrategy();
                            astar++;
                        }
                    }
                    else if (en.Strategy.GetType() == typeof(ChasingStrategy))
                    {
                        chasing++;
                    }
                    else
                    {
                        astar++;
                    }
                    en.Strategy = new ChasingStrategy();
                }

                if (chasing > astar)
                {
                    enemy.Strategy = new ChasingStrategy();
                }
                else
                {
                    enemy.Strategy = new AStarChasingStrategy();
                }
            }
            CharacterManager.Instance.addEnemyGroup(group);
        }
    }
}
