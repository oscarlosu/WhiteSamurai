using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.ScreenManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Prototype.TileEngine;
using Prototype.TileEngine.IsometricView;

namespace Prototype
{
    enum PlayerAction
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        DASH,
        NONE
    }

    static class InputHandler
    {
        static Dictionary<PlayerAction, String> actions;
        static Dictionary<Vector3, String> directions;
        public static void Initialize()
        {
            actions = new Dictionary<PlayerAction,string>();
            actions.Add(PlayerAction.IDLE,"idle");
            actions.Add(PlayerAction.MOVE ,"moving");
            actions.Add(PlayerAction.ATTACK , "attacking");
            actions.Add(PlayerAction.DASH , "dashing");

            directions = new Dictionary<Vector3, string>();
            directions.Add(IsoInfo.North, "North");
            directions.Add(IsoInfo.South, "South");
            directions.Add(IsoInfo.West, "West");
            directions.Add(IsoInfo.East, "East");
            directions.Add(IsoInfo.NorthEast, "NorthEast");
            directions.Add(IsoInfo.NorthWest, "NorthWest");
            directions.Add(IsoInfo.SouthEast, "SouthEast");
            directions.Add(IsoInfo.SouthWest, "SouthWest");
        }

        public static String determinePlayerAction(InputState input, Vector3 previous, out Vector3 direction)
        {
            direction = previous;
            PlayerAction action;
            Vector3 aux = Vector3.Zero;
            KeyboardState state = input.CurrentKeyBoardState;
            GamePadState gpstate = input.CurrentGamePadState[0];
            String response;
            String auxResp;
            // Check for dash
            if (input.isGamePadButtonNewlyPressed(Buttons.A, PlayerIndex.One) || input.isKeyNewlyPressed(Keys.Space))
            {
                action = PlayerAction.DASH;
            }
            else if(input.isGamePadButtonNewlyPressed(Buttons.X, PlayerIndex.One) || input.isKeyNewlyPressed(Keys.X))
            { //Check for attack
                action = PlayerAction.ATTACK;
            }
            else
            {
                action = isMoving(input, out aux);
            }

            if (aux != Vector3.Zero)
            {
                direction = aux;
            }

            //We get the name of the animation
            actions.TryGetValue(action, out response);
            if (directions.TryGetValue(direction, out auxResp) == false)
            {
                direction = IsoInfo.North;
                auxResp = "North";
            }

            response = response + auxResp;
            return response;
        }

        public static PlayerAction isMoving(InputState input, out Vector3 direction)
        {
            GamePadState gpstate = input.CurrentGamePadState[0];
            KeyboardState state = input.CurrentKeyBoardState;
            PlayerAction action = PlayerAction.IDLE;
            direction = Vector3.Zero;

            
            //Check for moving using GamePad
            Vector2 normalizedLeftThumbstick = gpstate.ThumbSticks.Left;
            if (input.IsGamePadConnected[0] && normalizedLeftThumbstick != Vector2.Zero) //normalizedLeftThumbstick == Vector2.Zero iff character is not moving using GamePad
            {
                normalizedLeftThumbstick.Normalize();

            
                double angle = Math.Acos(normalizedLeftThumbstick.X);
                if (normalizedLeftThumbstick.Y < 0)
                {
                    angle = 2.0 * Math.PI - angle;
                }

                if (angle == 0)
                {
                    direction = IsoInfo.East;
                }
                else if (0 < angle && angle < Math.PI / 2.0)
                {
                    direction = IsoInfo.NorthEast;
                }
                else if (angle == Math.PI / 2.0)
                {
                    direction = IsoInfo.North;
                }
                else if (Math.PI / 2.0 < angle && angle < Math.PI)
                {
                    direction = IsoInfo.NorthWest;
                }
                else if (angle == Math.PI)
                {
                    direction = IsoInfo.West;
                }
                else if (Math.PI < angle && angle < 3.0 * Math.PI / 2.0)
                {
                    direction = IsoInfo.SouthWest;
                }
                else if (angle == 3.0 * Math.PI / 2.0)
                {
                    direction = IsoInfo.South;
                }
                else if (angle > 3.0 * Math.PI / 2.0)
                {
                    direction = IsoInfo.SouthEast;
                }

                action = PlayerAction.MOVE;
                

                
            } //Check for moving using keyboard
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.Down))
            {
                if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Up))
                {
                    direction = IsoInfo.NorthWest;
                }
                else if (state.IsKeyDown(Keys.Up) && state.IsKeyDown(Keys.Right))
                {
                    direction = IsoInfo.NorthEast;
                }
                else if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Down))
                {
                    direction = IsoInfo.SouthEast;
                }
                else if (state.IsKeyDown(Keys.Down) && state.IsKeyDown(Keys.Left))
                {
                    direction = IsoInfo.SouthWest;
                }
                else if (state.IsKeyDown(Keys.Left))
                {
                    direction = IsoInfo.West;
                }
                else if (state.IsKeyDown(Keys.Up))
                {
                    direction = IsoInfo.North;
                }
                else if (state.IsKeyDown(Keys.Right))
                {
                    direction = IsoInfo.East;
                }
                else if (state.IsKeyDown(Keys.Down))
                {
                    direction = IsoInfo.South;
                }

                action = PlayerAction.MOVE;
            }

            return action;
        }


    }
}
