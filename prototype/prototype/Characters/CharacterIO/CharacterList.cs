using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.IsometricView;
using Prototype.Utils;
using System.Xml.Serialization;
using System.IO;

namespace Prototype.Characters.CharacterIO
{
    /// <summary>
    /// Reads a list of enemies
    /// </summary>
    public class CharacterList
    {
        public PlayerReader playerReader;
        /// <summary>
        /// list of enemy groups
        /// </summary>
        public List<EnemyReader> enemyList;

       /* public static void writeToXML(String fileName)
        {
            EnemyList el = new EnemyList();
            GroupReader gr = new GroupReader();
            EnemyReader er = new EnemyReader();
            er.type = EnemyType.WARRIOR;
            er.strategy = StrategiesType.IDLE;
            er.initialPosition = new Pair<int, int>(2,2);
            er.initialOrientation = IsoInfo.North;
            gr.group.Add(er);
            el.groupList.Add(gr);

            XmlSerializer serializer = new XmlSerializer(typeof(EnemyList));
            TextWriter writer = new StreamWriter(fileName);

            serializer.Serialize(writer, el);
            writer.Close();
        } */

        public static CharacterList readFromXML(String fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CharacterList));
            TextReader reader = new StreamReader(fileName);
            object obj = serializer.Deserialize(reader);
            CharacterList el = (CharacterList)obj;
            reader.Close();
            return el;
        }

        public CharacterList()
        {
            enemyList = new List<EnemyReader>();
        }
    }
}
