using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.GameState
{
    public static class GameState
    {
        private static Boolean gameOver = false;
        private static Boolean gameWon = false;
        private static Boolean winning = false;

        public static Boolean GameOver
        {
            set
            {
                gameOver = value;
            }
        }

        public static Boolean GameWon
        {
            set
            {
                gameWon = value;
            }
        }

        public static Boolean Winning
        {
            set
            {
                winning = value;
            }
        }

        public static Boolean isGameOver()
        {
            return gameOver;
        }

        public static Boolean isGameWon()
        {
            return gameWon;
        }

        public static Boolean isWinning()
        {
            return winning;
        }
        
    }
}
