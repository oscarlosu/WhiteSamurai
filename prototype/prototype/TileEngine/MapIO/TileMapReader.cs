using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Prototype.TileEngine.MapIO
{
    [Serializable()]
    public class TileMapReader
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        public int nRows;
        /// <summary>
        /// Number of columns
        /// </summary>
        public int nCols;
        /// <summary>
        /// List of intervals
        /// </summary>
        public List<Interval> intervals;
        /// <summary>
        /// List of cell Lists
        /// </summary>
        public List<CellList> cellLists;

        public TileMapReader()
        {
            intervals = new List<Interval>();
            cellLists = new List<CellList>();
        }


        /// <summary>
        /// Reads a TileMapReader from a file
        /// </summary>
        /// <param name="mapFile">Name of the file</param>
        /// <returns></returns>
        public static TileMapReader readFromXMLFile(string mapFile)
        {
            TileMapReader reader = new TileMapReader();

            XmlSerializer serializer = new XmlSerializer(typeof(TileMapReader));
            TextReader textReader = new StreamReader(mapFile);

            object obj = serializer.Deserialize(textReader);
            reader = (TileMapReader)obj;
            textReader.Close();
            return reader;
        }

        /// <summary>
        /// Writes a TileMapReader to a file
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="mapFile">Name of the file</param>
        public static void writeToXMLFile(TileMapReader reader, string mapFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TileMapReader));
            TextWriter textWriter = new StreamWriter(mapFile);
            serializer.Serialize(textWriter, reader);
            textWriter.Close();
            return;
        }

    }
}
