using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.Characters.Enemies;
using Prototype.ScreenManager;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine;
using Prototype.Characters.Strategies.AStar;
using Prototype.Utils;

namespace Prototype.Characters.Strategies
{
    
    class AStarChasingStrategy : MoveStrategy
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);
        List<Vector2> path = null;
        Vector2 playerCell = new Vector2(-1,-1);

        static int AStarUpdateInterval = 30; // A* updates the path every AStarUpdateInterval cycles
        int cycleCounter = 0;
        int executionCycle = rnd.Next() % AStarUpdateInterval; // This member is used to distribute A* path updates over AStarUpdateInterval cycles to reduce execution time
        

        public String getNextActionMelee(EnemyMelee gameCharacter, InputState input, GameTime gameTime)
        {
            Player player = CharacterManager.Instance.Player;
            Action action = gameCharacter.PreviousAction;
            String animationName;
            Vector3 direction = gameCharacter.PreviousMove;

            // Update cycle counter
            cycleCounter = (cycleCounter + 1) % AStarUpdateInterval;

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
                        

                            action = Action.IDLE;
                            gameCharacter.PreviousAction = action;


                        playerCell = new Vector2(-1,-1);
                        path = null;
                        break;
                    case State.ALERTED: //Alerted and berserker cases are the same
                    case State.BERSERKER:
                        if ((gameCharacter.Position - player.Position).Length() > gameCharacter.VisionRadio)
                        {
                            gameCharacter.State = State.IDLE;
                        }

                        if (gameCharacter.PreviousAction == Action.HEAD)
                        {
                            action = Action.ATTACK;
                            gameCharacter.PreviousAction = Action.ATTACK;
                            path = null;
                        }
                        else
                        {
                            bool move = false;
                            Vector2 charCell = Coordinates.toCell(player.Position);
                            Vector2 currentCell = Coordinates.toCell(gameCharacter.Position);
                            Vector2 result = charCell - currentCell;
                            result.X = Math.Abs(result.X);
                            result.Y = Math.Abs(result.Y);

                            // If the player is near, the character attacks
                            if (result.X <= 1 && result.Y <= 1)
                            {
                                direction = getDirection(currentCell, charCell);
                                if (direction == gameCharacter.PreviousMove)
                                {
                                    action = Action.ATTACK;
                                    gameCharacter.PreviousAction = Action.ATTACK;
                                    path = null;
                                }
                                else
                                {
                                    if (direction == IsoInfo.North)
                                        action = Action.HEADNORTH;
                                    else if (direction == IsoInfo.NorthEast)
                                        action = Action.HEADNORTHEAST;
                                    else if (direction == IsoInfo.East)
                                        action = Action.HEADEAST;
                                    else if (direction == IsoInfo.SouthEast)
                                        action = Action.HEADSOUTHEAST;
                                    else if (direction == IsoInfo.South)
                                        action = Action.HEADSOUTH;
                                    else if (direction == IsoInfo.SouthWest)
                                        action = Action.HEADSOUTHWEST;
                                    else if (direction == IsoInfo.West)
                                        action = Action.HEADWEST;
                                    else
                                        action = Action.HEADNORTHWEST;
                                    path = null;
                                    gameCharacter.PreviousAction = Action.HEAD;
                                }
                            }
                            else
                            {
                                if(path == null || path.Count == 0)
                                {
                                    path = Pathfinder.findPath(currentCell, charCell);
                                    playerCell = charCell;
                                    if(path == null || path.Count == 0) //This might be an error
                                    {
                                        action = Action.IDLE;
                                        direction = gameCharacter.PreviousMove;
                                    }
                                    else
                                    {
                                        direction = getDirection(currentCell, path.First());
                                        if (direction != Vector3.Zero)
                                            move = true;
                                    }
                                }
                                else
                                {
                                        if(playerCell == charCell && currentCell == path.First()) //Player is still in the same position
                                        {
                                            
                                            path.Remove(path.First()); //Remove the first cell
                                            if(path == null || path.Count == 0) //This might be an error
                                            {
                                                action = Action.IDLE;
                                                direction = gameCharacter.PreviousMove;
                                            }
                                            else
                                            {
                                                direction = getDirection(currentCell, path.First());
                                                if (direction != Vector3.Zero)
                                                    move = true;
                                            }
                                        }
                                        else
                                        {
                                            // Update path every AStarUpdateInterval instead of every cycle
                                            if (cycleCounter == executionCycle)
                                            {
                                                path = Pathfinder.findPath(currentCell, charCell);
                                                playerCell = charCell;
                                            }
                                            
                                            if(path == null || path.Count == 0) //This might be an error
                                            {
                                                action = Action.IDLE;
                                                direction = gameCharacter.PreviousMove;
                                            }
                                            else
                                            {
                                                direction = getDirection(currentCell, path.First());
                                                if (direction != Vector3.Zero)
                                                    move = true;
                                            }
                                        }
                                    
                                }
                            
                                if(move)
                                {
                                    if (direction == IsoInfo.North)
                                        action = Action.MOVENORTH;
                                    else if (direction == IsoInfo.NorthEast)
                                        action = Action.MOVENORTHEAST;
                                    else if (direction == IsoInfo.East)
                                        action = Action.MOVEEAST;
                                    else if (direction == IsoInfo.SouthEast)
                                        action = Action.MOVESOUTHEAST;
                                    else if (direction == IsoInfo.South)
                                        action = Action.MOVESOUTH;
                                    else if (direction == IsoInfo.SouthWest)
                                        action = Action.MOVESOUTHWEST;
                                    else if (direction == IsoInfo.West)
                                        action = Action.MOVEWEST;
                                    else
                                        action = Action.MOVENORTHWEST;

                                    gameCharacter.PreviousAction = action;
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            animationName = gameCharacter.getAction(action, gameTime, out direction);
            if (animationName.Contains("moving"))
            {
                gameCharacter.move(direction);
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }
            

            return animationName;
        }


        public String getNextActionRanged(EnemyRanged gameCharacter, InputState input, GameTime gameTime)
        {
            Player player = CharacterManager.Instance.Player;
            Action action = gameCharacter.PreviousAction;
            String animationName;
            Vector3 direction = gameCharacter.PreviousMove;

            // Update cycle counter
            cycleCounter = (cycleCounter + 1) % AStarUpdateInterval;

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


                        action = Action.IDLE;
                        gameCharacter.PreviousAction = action;

                        playerCell = new Vector2(-1, -1);
                        path = null;
                        break;
                    case State.ALERTED: //Alerted and berserker cases are the same
                    case State.BERSERKER:
                        if ((gameCharacter.Position - player.Position).Length() > gameCharacter.VisionRadio)
                        {
                            gameCharacter.State = State.IDLE;
                        }

                        if (gameCharacter.PreviousAction == Action.HEAD)
                        {
                            action = Action.ATTACK;
                            gameCharacter.PreviousAction = Action.ATTACK;
                            path = null;
                        }
                        else
                        {
                            bool move = false;
                            Vector2 charCell = Coordinates.toCell(player.Position);
                            Vector2 currentCell = Coordinates.toCell(gameCharacter.Position);
                            Vector2 result = charCell - currentCell;
                            result.X = Math.Abs(result.X);
                            result.Y = Math.Abs(result.Y);

                            // If the player is near, the character attacks
                            if ((gameCharacter.Position - player.Position).Length() < gameCharacter.ShotRadio)
                            {
                                direction = getDirectionArchers(player.Position, gameCharacter.Position);
                                if (direction == gameCharacter.PreviousMove)
                                {
                                    action = Action.ATTACK;
                                    gameCharacter.PreviousAction = Action.ATTACK;
                                    path = null;
                                }
                                else
                                {
                                    if (direction == IsoInfo.North)
                                        action = Action.HEADNORTH;
                                    else if (direction == IsoInfo.NorthEast)
                                        action = Action.HEADNORTHEAST;
                                    else if (direction == IsoInfo.East)
                                        action = Action.HEADEAST;
                                    else if (direction == IsoInfo.SouthEast)
                                        action = Action.HEADSOUTHEAST;
                                    else if (direction == IsoInfo.South)
                                        action = Action.HEADSOUTH;
                                    else if (direction == IsoInfo.SouthWest)
                                        action = Action.HEADSOUTHWEST;
                                    else if (direction == IsoInfo.West)
                                        action = Action.HEADWEST;
                                    else
                                        action = Action.HEADNORTHWEST;
                                    path = null;
                                    gameCharacter.PreviousAction = Action.HEAD;
                                }
                            }
                            else
                            {
                                if (path == null || path.Count == 0)
                                {
                                    path = Pathfinder.findPath(currentCell, charCell);
                                    playerCell = charCell;
                                    if (path == null || path.Count == 0) //This might be an error
                                    {
                                        action = Action.IDLE;
                                        direction = gameCharacter.PreviousMove;
                                    }
                                    else
                                    {
                                        direction = getDirection(currentCell, path.First());
                                        if (direction != Vector3.Zero)
                                            move = true;
                                    }
                                }
                                else
                                {
                                    if (playerCell == charCell && currentCell == path.First()) //Player is still in the same position
                                    {

                                        path.Remove(path.First()); //Remove the first cell
                                        if (path == null || path.Count == 0) //This might be an error
                                        {
                                            action = Action.IDLE;
                                            direction = gameCharacter.PreviousMove;
                                        }
                                        else
                                        {
                                            direction = getDirection(currentCell, path.First());
                                            if (direction != Vector3.Zero)
                                                move = true;
                                        }
                                    }
                                    else
                                    {
                                        // Update path every AStarUpdateInterval instead of every cycle
                                        if (cycleCounter == executionCycle)
                                        {
                                            path = Pathfinder.findPath(currentCell, charCell);
                                            playerCell = charCell;
                                        }
                                        if (path == null || path.Count == 0) //This might be an error
                                        {
                                            action = Action.IDLE;
                                            direction = gameCharacter.PreviousMove;
                                        }
                                        else
                                        {
                                            direction = getDirection(currentCell, path.First());
                                            if (direction != Vector3.Zero)
                                                move = true;
                                        }
                                    }

                                }

                                if (move)
                                {
                                    if (direction == IsoInfo.North)
                                        action = Action.MOVENORTH;
                                    else if (direction == IsoInfo.NorthEast)
                                        action = Action.MOVENORTHEAST;
                                    else if (direction == IsoInfo.East)
                                        action = Action.MOVEEAST;
                                    else if (direction == IsoInfo.SouthEast)
                                        action = Action.MOVESOUTHEAST;
                                    else if (direction == IsoInfo.South)
                                        action = Action.MOVESOUTH;
                                    else if (direction == IsoInfo.SouthWest)
                                        action = Action.MOVESOUTHWEST;
                                    else if (direction == IsoInfo.West)
                                        action = Action.MOVEWEST;
                                    else
                                        action = Action.MOVENORTHWEST;

                                    gameCharacter.PreviousAction = action;
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            animationName = gameCharacter.getAction(action, gameTime, out direction);
            if (animationName.Contains("moving"))
            {
                gameCharacter.move(direction);
            }
            else if (animationName.Contains("attacking"))
            {
                gameCharacter.attack(gameTime);
            }


            return animationName;
        }

        private Vector3 getDirection(Vector2 currentCell, Vector2 playerCell)
        {
            Pair<int, int> value = new Pair<int,int>((int) (playerCell.X - currentCell.X), (int) (playerCell.Y - currentCell.Y));
            int index = -1;

            for(int i = 0; i < StrategyConstants.NUMDIR; ++i)
            {
                Console.Out.WriteLine(StrategyConstants.directions[i].First + " " + StrategyConstants.directions[i].Second);
                if(StrategyConstants.directions[i].First == value.First && StrategyConstants.directions[i].Second == value.Second)
                {
                    
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                return Vector3.Zero;
            }
            else
            {
                if(index == StrategyConstants.NORTH)
                {
                    return IsoInfo.North;
                }
                else if (index == StrategyConstants.NORTHEAST)
                {
                    return IsoInfo.NorthEast;
                }
                else if (index == StrategyConstants.EAST)
                {
                    return IsoInfo.East;
                }
                else if (index == StrategyConstants.SOUTHEAST)
                {
                    return IsoInfo.SouthEast;
                }
                else if (index == StrategyConstants.SOUTH)
                {
                    return IsoInfo.South;
                }
                else if (index == StrategyConstants.SOUTHWEST)
                {
                    return IsoInfo.SouthWest;
                }
                else if (index == StrategyConstants.WEST)
                {
                    return IsoInfo.West;
                }
                else
                {
                    return IsoInfo.NorthWest;
                }
            }
        }

        /// <summary>
        /// Gets the direction in which to chase the player. If parameters are inversed, the enemy fliess
        /// </summary>
        /// <param name="playerDirection">Player position</param>
        /// <param name="enemyPosition">Enemy position</param>
        /// <returns></returns>
        private Vector3 getDirectionArchers(Vector3 playerPosition, Vector3 enemyPosition)
        {
            Vector3 relative = playerPosition - enemyPosition;
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
    }
}
