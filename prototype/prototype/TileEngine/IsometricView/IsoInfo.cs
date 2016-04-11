using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Prototype.TileEngine.IsometricView
{

    /// <summary>
    /// Stores information about the tileset and the isometric view definition
    /// </summary>
    static class IsoInfo
    {
        #region Fields and Properties
        /// <summary>
        /// Angle from horizontal line to base tile side (in radians)
        /// </summary>
        private static float angle;
        /// <summary>
        /// Height of the tile (in pixels)
        /// </summary>
        private static float tileHeight;
        /// <see cref="tileHeight"/>
        public static float TileHeight
        {
            get
            {
                return tileHeight;
            }
        }
        /// <summary>
        /// Width of the tile (in pixels)
        /// </summary>
        private static float tileWidth;
        /// <see cref="tileWidth" />
        public static float TileWidth
        {
            get
            {
                return tileWidth;
            }
        }
        /// <summary>
        /// Size of the tile elevation (in pixels)
        /// </summary>
        private static float tileElevation;
        /// <see cref="tileElevation" />
        public static float TileElevation
        {
            get
            {
                return tileElevation;
            }
        }
        /// <summary>
        /// Length of the tile side (in pixels)
        /// </summary>
        private static float tileSide;
        /// <see cref="tileSide" />
        public static float TileSide
        {
            get
            {
                return tileSide;
            }
        }

        /// <summary>
        /// Offset to tile base top corner from sprite top left corner (in pixels)
        /// </summary>
        private static Vector2 tileOrigin;
        /// <see cref="tileOrigin" />
        public static Vector2 TileOrigin
        {
            get
            {
                return tileOrigin;
            }
        }
        /// <summary>
        /// Center of the tile
        /// </summary>
        private static Vector2 tileCenter;
        /// <see cref="tileCenter" />
        public static Vector2 TileCenter { get { return tileCenter; } set { tileCenter = value; } }

        /// <summary>
        /// Padding between sprites in the same row in a sprite sheet
        /// </summary>
        private static float spriteSheetRowPadding;
        /// <see cref="spriteSheetRowPadding" />
        public static float SpriteSheetRowPadding
        {
            get
            {
                return spriteSheetRowPadding;
            }
        }

        /// <summary>
        /// Padding between sprites in the same column in a sprite sheet
        /// </summary>
        private static float spriteSheetColPadding;
        /// <see cref="spriteSheetColPadding" />
        public static float SpriteSheetColPadding
        {
            get
            {
                return spriteSheetColPadding;
            }
        }

        /// <summary>
        /// Cosine of the isometric angle
        /// </summary>
        private static float cosine;
        /// <see cref="cosine" />
        public static float Cosine
        {
            get
            {
                return cosine;
            }
        }
        /// <summary>
        /// Sine of the isometric angle
        /// </summary>
        private static float sine;
        /// <see cref="cosine" />
        public static float Sine
        {
            get
            {
                return sine;
            }
        }

        // Direction vectors
        /// <summary>
        /// Vector that points Nort-West
        /// </summary>
        private static Vector3 northWest;
        /// <summary>
        /// <see cref="northWest"/>
        /// </summary>
        public static Vector3 NorthWest
        {
            get
            {
                return northWest;
            }
        }
        /// <summary>
        /// Vector that points northeast
        /// </summary>
        private static Vector3 northEast;
        /// <summary>
        /// <see cref="northEast"/>
        /// </summary>
        public static Vector3 NorthEast
        {
            get
            {
                return northEast;
            }
        }
        /// <summary>
        /// Vector that points South East
        /// </summary>
        private static Vector3 southEast;
        /// <summary>
        /// <see cref="southEast"/>
        /// </summary>
        public static Vector3 SouthEast
        {
            get
            {
                return southEast;
            }
        }
        /// <summary>
        /// Vector that points South West
        /// </summary>
        private static Vector3 southWest;
        /// <summary>
        /// <see cref="southWest"/>
        /// </summary>
        public static Vector3 SouthWest
        {
            get
            {
                return southWest;
            }
        }
        /// <summary>
        /// Vector that points West
        /// </summary>
        private static Vector3 west;
        /// <summary>
        /// <see cref="west"/>
        /// </summary>
        public static Vector3 West
        {
            get
            {
                return west;
            }
        }

        /// <summary>
        /// Vector that points North
        /// </summary>
        private static Vector3 north;
        /// <summary>
        /// <see cref="north"/>
        /// </summary>
        public static Vector3 North
        {
            get
            {
                return north;
            }
        }

        /// <summary>
        /// Vector that points East
        /// </summary>
        private static Vector3 east;
        /// <summary>
        /// <see cref="east"/>
        /// </summary>
        public static Vector3 East
        {
            get
            {
                return east;
            }
        }
        /// <summary>
        /// Vector that points South
        /// </summary>
        private static Vector3 south;
        /// <summary>
        /// <see cref="south"/>
        /// </summary>
        public static Vector3 South
        {
            get
            {
                return south;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes params to default values
        /// angle = 26.565, 
        /// tileHeight = tileWidth = 64
        /// tileSide = 36
        /// tileElevation = 16
        /// </summary>
        public static void Initialize()
        {
            //TODO: Mejorar la definicion de los parametros
            XmlDocument xmlDoc = null;
            double tangent;
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            // We read the parameters from a file
            using (XmlReader reader = XmlReader.Create("TileEngine/XML/isoinfo.xml", readerSettings))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                XmlNodeList ls = xmlDoc.GetElementsByTagName("tileHeight");
                tileHeight = Convert.ToInt32(xmlDoc.GetElementsByTagName("tileHeight").Item(0).InnerXml);
                tileWidth = Convert.ToInt32(xmlDoc.GetElementsByTagName("tileWidth").Item(0).InnerXml);
                tileElevation = Convert.ToInt32(xmlDoc.GetElementsByTagName("tileElevation").Item(0).InnerXml);
                tangent = Convert.ToDouble(xmlDoc.GetElementsByTagName("tangent").Item(0).InnerXml);
                reader.Close();
            }

            // angle shoud be 26.565
            //angle = 26.565f;
            angle = (float)Math.Atan(tangent);
            // tileHeight should be 48
            //            tileHeight = 48;
            // tileHeight should be 64
            //            tileWidth = 64;

            // Calculate other parameters

            // tileSide = sqrt(A^2 + B^2) should be 35.78 ~ 36
            float sideA = tileWidth / 2;
            float sideB = tileWidth / 4;
            tileSide = (float)Math.Sqrt(sideA * sideA + sideB * sideB);

            // tileElevation should be 16
            //tileElevation = 16;

            // tileOrigin should be (32, 16) -> the lower north corner of the tile
            tileOrigin = new Vector2(TileWidth / 2, tileElevation);
            tileCenter = new Vector2(TileWidth / 2, TileWidth / 4);
            // Precalculates some measures to save processing time
            // Angle must be in radians. 0.0174 radians = 1 degree
            cosine = (float)Math.Cos(angle);
            sine = (float)Math.Sin(angle);

            spriteSheetRowPadding = tileWidth / 2;
            spriteSheetColPadding = tileElevation;
            // Precalculate Isometric direction vectors
            Vector3 v;
            // northWest
            v = new Vector3(0, -1, 0);
            v.Normalize();
            northWest = v;
            // northEast
            v = new Vector3(-1, 0, 0);
            v.Normalize();
            northEast = v;
            // southEast
            v = new Vector3(0, 1, 0);
            v.Normalize();
            southEast = v;
            // southWest
            v = new Vector3(1, 0, 0);
            v.Normalize();
            southWest = v;
            // west
            v = new Vector3(IsoInfo.TileSide / IsoInfo.TileWidth, -IsoInfo.TileSide / IsoInfo.TileWidth, 0);
            v.Normalize();
            west = v;
            // north
            v = new Vector3(-IsoInfo.TileSide / (IsoInfo.TileWidth / 2), -IsoInfo.TileSide / (IsoInfo.TileWidth / 2), 0);
            v.Normalize();
            north = v;
            // east
            v = new Vector3(-IsoInfo.TileSide / IsoInfo.TileWidth, IsoInfo.TileSide / IsoInfo.TileWidth, 0);
            v.Normalize();
            east = v;
            // south
            v = new Vector3(IsoInfo.TileSide / (IsoInfo.TileWidth / 2), IsoInfo.TileSide / (IsoInfo.TileWidth / 2), 0);
            v.Normalize();
            south = v;
        }

        #endregion
    }
}
