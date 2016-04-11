using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Characters.Map
{
    public class CharacterMap
    {
        private int nRows;
        private int nCols;
        private List<List<CharCell>> cells;

        public CharacterMap(int nRows, int nCols)
        {
            this.nRows = nRows;
            this.nCols = nCols;
            cells = new List<List<CharCell>>();
            for (int x = 0; x < nRows; ++x)
            {
                cells.Add(new List<CharCell>());
                for (int y = 0; y < nCols; ++y)
                {
                    cells[x].Add(new CharCell());
                }
            }
        }

        public CharCell getCell(int row, int col)
        {
            return cells[row][col];
        }

        public void addToCell(int id, int row, int col)
        {
            if (0 <= row && row < nRows)
            {
                if (0 <= col && col < nCols)
                {
                    this.cells[row][col].addEnemy(id);
                }
            }
        }

        public void deleteFromCell(int id, int row, int col)
        {
            if (0 <= row && row < nRows)
            {
                if (0 <= col && col < nCols)
                {
                    this.cells[row][col].deleteEnemy(id);
                }
            }
        }
    }
}
