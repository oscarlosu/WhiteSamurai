using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.TileEngine.Map
{
    public class MapProperties
    {
        //We read this properties from an XML file. They will be the same for every possible map
        #region fields
        /// <summary>
        /// Maximum elevation for the tiles in a cell
        /// </summary>
        public int maxTileStack;
        /// <summary>
        /// Name of the tileSheet
        /// </summary>
        public string tileSheetName;
        /// <summary>
        /// Number of rows of the tileSheet
        /// </summary>
        public int tileSheetRows;
        /// <summary>
        /// Number of columns of the tileSheet
        /// </summary>
        public int tileSheetCols;
        /// <summary>
        /// Name of the objectSheet
        /// </summary>
        public string objectSheetName;
        /// <summary>
        /// Number of rows of the objectSheet
        /// </summary>
        public int objectSheetRows;
        /// <summary>
        /// Number of columns of the objectSheet
        /// </summary>
        public int objectSheetCols;
        #endregion
    }

}
