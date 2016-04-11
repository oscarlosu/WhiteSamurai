using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine.Map;
using Prototype.TileEngine.MapIO;
using Prototype.TileEngine.Collisions;

namespace Prototype.TileEngine
{
    class MapManager
    {
        #region Fields
        /// <summary>
        /// Instance of mapManager
        /// </summary
        private static MapManager instance = null;
        /// <summary>
        ///  Name of the file from where we read the map properties
        /// </summary>
        private string propertiesFile = "TileEngine/XML/mapProperties.xml";
        /// <summary>
        /// Current map
        /// </summary>
        private TileMap currentMap;
        /// <summary>
        /// Name of the map file
        /// </summary>
        private String mapFile;
        /// <summary>
        /// Common properties to every map
        /// </summary>
        public MapProperties properties;
        /// <summary>
        /// Collidable terrain objects
        /// </summary>
        public CollidableTerrainObjects collidables;
        /// <summary>
        /// Map tiles
        /// </summary>
        public Texture2D tileSheet;
        /// <summary>
        /// Object tiles
        /// </summary>
        public Texture2D objectSheet;
        /// <summary>
        /// number of rows to draw
        /// </summary>
        public int screenRowWidth;
        /// <summary>
        /// number of columns to draw
        /// </summary>
        public int screenColWidth;
        /// <summary>
        /// Margin for drawing
        /// </summary>
        public const int MARGIN = 0;
        /// <summary>
        /// Center cell to draw
        /// </summary>
        private Vector3 center;
        


        #endregion

        #region Properties

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }

        public Vector3 Center { get { return center; } set { center = value; } }
        public Vector2 CenterCell { get { return Coordinates.toCell(center); } }
        public Vector3 FirstCell
        {
            get { return new Vector3(MathHelper.Max(CenterCell.X - this.screenColWidth, 0), MathHelper.Max(CenterCell.Y - this.screenRowWidth, 0), 0); }
        }
        public Vector3 LastCell
        {
            get { return new Vector3(MathHelper.Min(CenterCell.X + this.screenColWidth, currentMap.NCols), MathHelper.Min(CenterCell.Y + this.screenRowWidth, currentMap.NRows), 0); }
        }

        public TileMap CurrentMap
        {
            get { return currentMap; }
        }
        #endregion

        private MapManager()
        {
            //We obtain the Map Properties
            XmlSerializer serializer = new XmlSerializer(typeof(MapProperties));
            TextReader reader = new StreamReader(propertiesFile);
            object obj = serializer.Deserialize(reader);
            properties = (MapProperties)obj;
            reader.Close();

            collidables = CollidableTerrainObjects.loadFromXML("TileEngine/XML/collidables.xml");
        }

        /// <summary>
        /// Loads the tilesheets
        /// </summary>
        /// <param name="content">Content manager</param>
        public void LoadContent(ContentManager content)
        {
            tileSheet = content.Load<Texture2D>(properties.tileSheetName);
            objectSheet = content.Load<Texture2D>(properties.objectSheetName);
            adjustResolution();
        }

        public void Update(GameTime gameTime, Vector3 center)
        {
            this.center = center;
            this.currentMap.Update(gameTime);
        }
        /// <summary>
        /// Loads a new map from a file
        /// </summary>
        /// <param name="mapFile">Name of the map</param>
        public void newMap(string mapFile)
        {
            this.mapFile = mapFile;
            currentMap = TileMapBuilder.createMap(mapFile);
            Camera.MapSize = currentMap.NCols + currentMap.NRows;
        }

        /// <summary>
        /// Adjusts the number of rows and columns to draw in the map
        /// </summary>
        public void adjustResolution()
        {
            int width = Camera.ScreenWidth;
            int height = Camera.ScreenHeight;
            double cos = IsoInfo.Cosine;
            double sin = IsoInfo.Sine;
            double tileWidth = IsoInfo.TileWidth;
            double tileHeight = IsoInfo.TileHeight;
            double n = Math.Ceiling(((Camera.ScreenWidth * IsoInfo.Cosine) / (IsoInfo.TileWidth * IsoInfo.Sine)) + Camera.ScreenHeight / (IsoInfo.TileHeight - IsoInfo.TileElevation));
            this.screenColWidth = (int)(n / 2 * IsoInfo.Cosine) + MARGIN;
            this.screenRowWidth = this.screenColWidth;
        }

        /// <summary>
        /// Draws the current map
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.currentMap.Draw(spriteBatch);
        }
    }
}
