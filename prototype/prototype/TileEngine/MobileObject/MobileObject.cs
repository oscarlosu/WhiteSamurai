using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Prototype.TileEngine.Collisions;
using Prototype.TileEngine.IsometricView;
using Prototype.TileEngine.Map;
using Prototype.Characters.Map;
using Prototype.Characters;

namespace Prototype.TileEngine.MobileObject
{
    public class MobileObject
    {
        #region Fields and Properties
            /// <summary>
            /// Collidables indexed by elevation level
            /// </summary>
            protected Dictionary<string, Dictionary<float, Collidable>> collidables; /**< Collidables indexed by elevation level. */
            public Dictionary<string, Dictionary<float, Collidable>> Collidables { get { return collidables; } }

            /// <summary>
            /// The Sprite Animation
            /// </summary>
            protected SpriteAnimation animatedSprite;
            public SpriteAnimation AnimatedSprite { get { return animatedSprite; } }
            /// <summary>
            /// Offset that will be used when drawing the sprites. Eg so that the feet of the character
            /// are drawn in the character's position instead of the sprite's top left corner
            /// </summary>
            private Vector2 drawOffset;
            /// <summary>
            /// Position
            /// </summary>
            protected Vector3 position;
            public Vector3 Position { get { return position; } }

            private Collidable attackCollision;
            public Collidable AttackCollision { get { return attackCollision; } }

        #endregion

        #region Constructor
            public MobileObject(Dictionary<string, Dictionary<float, Collidable>> collidables, Vector3 position, Vector2 drawOffset, Collidable attackCollider)
            {
                animatedSprite = new SpriteAnimation();
                this.collidables = collidables;
                this.position = position;
                this.drawOffset = drawOffset;
                this.attackCollision = attackCollider;
            
            }
        #endregion

        #region Methods
            /// <summary>
            /// Load content
            /// </summary>
            /// <param name="content">Content manager</param>
            /// <param name="spriteSheetName">Character spriteSheet</param>
            public void LoadContent(ContentManager content, string spriteSheetName)
            {
                animatedSprite.LoadContent(content, spriteSheetName);
            }
            /// <summary>
            /// Updates the animation
            /// </summary>
            /// <param name="gameTime">Game time</param>
            /// <param name="animationName">Animation name</param>
            public void Update(GameTime gameTime, string animationName)
            {    
                // Update SpriteAnimation's position (it's a 2D screen position)
                animatedSprite.Position = Coordinates.toScreen(position) - drawOffset - Camera.Position;
                // Set SpriteAnimation's CurrentAnimationName to animationName and update SpriteAnimation
                if(animationName != null && animatedSprite.CurrentAnimationName.CompareTo(animationName) != 0)
                {
                    animatedSprite.CurrentAnimationName = animationName;              
                }

                animatedSprite.Update(gameTime);
            
            }

            /// <summary>
            /// Draws the animated sprite
            /// </summary>
            /// <param name="spriteBatch">Sprite batch</param>
            public void Draw(SpriteBatch spriteBatch) 
            {
                float depth = Camera.getDepth(position, DrawableType.MOBILE_OBJECT);
                animatedSprite.Draw(spriteBatch, 0, 0, depth); 
            }

            /// <summary>
            /// Adds an animation
            /// </summary>
            /// <param name="name">Name of the animation</param>
            /// <param name="x">Start position of the first frame rectangle X</param>
            /// <param name="y">Start position of the first frame rectangle Y</param>
            /// <param name="width">Frame width</param>
            /// <param name="height">Frame height</param>
            /// <param name="nFrames">Number of frames</param>
            /// <param name="frameDuration">Frame duration</param>
            public void addAnimation(string name, int x, int y, int width, int height, int nFrames, float frameDuration)
            {
                animatedSprite.addAnimation(name, x, y, width, height, nFrames, frameDuration);
            }

            /// <summary>
            /// Adds an animation
            /// </summary>
            /// <param name="name">Name of the animation</param>
            /// <param name="x">Start position of the first frame rectangle X</param>
            /// <param name="y">Start position of the first frame rectangle Y</param>
            /// <param name="width">Frame width</param>
            /// <param name="height">Frame height</param>
            /// <param name="nFrames">Number of frames</param>
            /// <param name="frameDuration">Name of the next animation</param>
            public void addAnimation(string name, int x, int y, int width, int height, int nFrames, float frameDuration, string nextAnimationName)
            {
                animatedSprite.addAnimation(name, x, y, width, height, nFrames, frameDuration, nextAnimationName);
            }

            public void move(int id, Vector3 direction, float speed)
            {
                Vector2 currentCell = Coordinates.toCell(position);
                float currentElevation = MapManager.Instance.CurrentMap.Cells[(int)currentCell.Y][(int)currentCell.X].getTerrainElevation(position);
                Vector3 destPosition = position + speed * direction;
                Vector2 destCell = Coordinates.toCell(destPosition);
                Dictionary<float, Collidable> colliders;
                Collidable collider;
                if(collidables.TryGetValue(animatedSprite.CurrentAnimationName, out colliders))
                {
                    if(colliders.TryGetValue(0, out collider)) //We get the zero level collider
                    {
                        for (int row = -1; row <= 1; ++row) //row = y
                        {
                            for (int col = -1; col <= 1; ++col) // col = x
                            {
                                Vector2 inspectedCell = new Vector2((destCell.X + col), (destCell.Y + row));
                                Vector2 inspectedCellOrigin = new Vector2(inspectedCell.X * IsoInfo.TileSide, inspectedCell.Y * IsoInfo.TileSide);
                                Vector2 destPositionOnXY = new Vector2(destPosition.X, destPosition.Y);

                                bool isThereCollisionWithCell = collider.collide(Coordinates.to2D(destPositionOnXY), Coordinates.to2D(inspectedCellOrigin));
                                if (isThereCollisionWithCell)
                                {
                                    if (inspectedCell.X >= MapManager.Instance.CurrentMap.NCols || inspectedCell.Y >= MapManager.Instance.CurrentMap.NRows ||
                                        inspectedCell.X < 0 || inspectedCell.Y < 0)
                                    {
                                        return;
                                    }

                                    if (inspectedCell.X < MapManager.Instance.CurrentMap.NCols && inspectedCell.Y < MapManager.Instance.CurrentMap.NRows && inspectedCell.X >= 0 && inspectedCell.Y >= 0)
                                    {
                                        int currentLevel = (int)(destPosition.Z / IsoInfo.TileElevation);
                                        MapCell inspectedMapCell = MapManager.Instance.CurrentMap.Cells[(int)inspectedCell.Y][(int)inspectedCell.X];

                                        int nObjectsInCell = inspectedMapCell.TerrainObjectTileCount;
                                        if (nObjectsInCell > 0)
                                        {
                                            int objectTerrainLevel = (int)(inspectedMapCell.getTerrainElevation() / IsoInfo.TileElevation);
                                            int elevationToTileFactor = (int)(IsoInfo.TileHeight / IsoInfo.TileElevation);
                                            int objectIndex = (int)((currentLevel - objectTerrainLevel) / 2);
                                            if (objectIndex < nObjectsInCell && objectIndex >= 0)
                                            {
                                                TerrainObject terrainObject = inspectedMapCell.getTerrainObjectTile(objectIndex);
                                                Collidable other;
                                                if(MapManager.Instance.collidables.collidables.TryGetValue(terrainObject, out other))
                                                {
                                                    if(collider.collide(Coordinates.to2D(destPositionOnXY), other, Coordinates.to2D(inspectedCellOrigin)))
                                                    {
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                        }

                                        if(inspectedMapCell.TerrainTileCount >= currentLevel)
                                        {
                                            if(inspectedMapCell.getTerrainElevation() >= currentElevation + IsoInfo.TileElevation)
                                            {
                                                return;
                                            }
                                        }

                                    }
                                }
                            }
                        }


                        float destElevation = MapManager.Instance.CurrentMap.Cells[(int) destCell.Y][(int) destCell.X].getTerrainElevation(destPosition);
                        if(Math.Abs(currentElevation-destElevation) < IsoInfo.TileElevation/2)
                        {
                            position += speed*direction;
                            position.Z = destElevation;
                        }
                    }
                }
            }

            /// <summary>
            /// Checks the attacked enemies in the same cell as the character
            /// </summary>
            /// <param name="id">character id</param>
            /// <param name="directionHeading">direction in which the character looks</param>
            /// <returns></returns>
            private List<int> checkAttackedEnemiesInCurrentCell(int id, Vector3 directionHeading)
            {
                List<int> collidedEnemies = new List<int>();
                Vector2 currentCell = Coordinates.toCell(position);
                float currentElevation = MapManager.Instance.CurrentMap.Cells[(int)currentCell.Y][(int)currentCell.X].getTerrainElevation(position);


                if (directionHeading == IsoInfo.North) //If the character is heading NORTH
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X <= this.position.X && character.Position.Y <= character.Position.Y) // If the enemy character is at North
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.NorthEast) //If the character is heading NORTHEAST
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X <= this.position.X) // If the enemy character is at NorthEast
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.East)
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X <= this.position.X && character.Position.Y >= character.Position.Y) // If the enemy character is at East
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.SouthEast)
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.Y >= character.Position.Y) // If the enemy character is at SouthEast
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.South)
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X >= this.position.X && character.Position.Y >= character.Position.Y) // If the enemy character is at East
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.SouthWest)
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X >= this.position.X) // If the enemy character is at SouthWest
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else if (directionHeading == IsoInfo.West)
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.X >= this.position.X && character.Position.Y <= this.position.Y) // If the enemy character is at SouthWest
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                else //directionHeading == IsoInfo.NorthWest
                {
                    //Check the same cell
                    List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)currentCell.Y, (int)currentCell.X).EnemyIds;
                    foreach (int charId in charsInCell)
                    {
                        GameCharacter character;
                        if (charId == -1)
                        {
                            character = CharacterManager.Instance.Player;
                        }
                        else
                        {
                            character = CharacterManager.Instance.getEnemy(charId);
                        }

                        if (character.Position.Y <= this.position.X) // If the enemy character is at SouthWest
                        {
                            // If the character is in the same cell, it's enough to check attacks in levels 1 and 2 with the level 1 and 2 collisions
                            if (checkAttack(character, currentCell, currentCell, currentElevation))
                            {
                                collidedEnemies.Add(charId);
                            }
                        }
                    }
                }
                return collidedEnemies;

            }


            /// <summary>
            /// Checks the attacked characters in a cell list
            /// </summary>
            /// <param name="id"></param>
            /// <param name="cellList"></param>
            /// <param name="directionHeading"></param>
            /// <returns></returns>
            public List<int> checkAttackEnemiesInCellList(int id, List<Vector2> cellList, Vector3 directionHeading)
            {
                List<int> collidedEnemies = new List<int>();
                Vector2 currentCell = Coordinates.toCell(position);
                float currentElevation = MapManager.Instance.CurrentMap.Cells[(int)currentCell.Y][(int)currentCell.X].getTerrainElevation(position);

                foreach (Vector2 cell in cellList)
                {
                    // If the cell is inside the map
                    if (cell.X < MapManager.Instance.CurrentMap.NCols && cell.Y < MapManager.Instance.CurrentMap.NRows && cell.X > 0 && cell.Y > 0)
                    {
                        if (cell == currentCell) //If the cell we check is the current cell of the mobile object
                        {
                            collidedEnemies.AddRange(checkAttackedEnemiesInCurrentCell(id, directionHeading));
                        }
                        else
                        {
                            List<int> charsInCell = CharacterManager.Instance.Map.getCell((int)cell.Y, (int)cell.X).EnemyIds;
                            foreach (int charId in charsInCell)
                            {
                                GameCharacter character;
                                if (charId == -1)
                                {
                                    character = CharacterManager.Instance.Player;
                                }
                                else
                                {
                                    character = CharacterManager.Instance.getEnemy(charId);
                                }
                                if (checkAttack(character, currentCell, cell, currentElevation))
                                {
                                    collidedEnemies.Add(charId);
                                }
                            }
                        }
                    }
                }
                return collidedEnemies;
            }

            /// <summary>
            /// Checks if there is a collision between a character and an attack collision
            /// </summary>
            /// <param name="character">Enemy</param>
            /// <param name="currentCell">Current cell of the character</param>
            /// <param name="enemyCell">Current cell of the enemy</param>
            /// <param name="currentElevation">Current elevation of the character</param>
            /// <returns></returns>
            public Boolean checkAttack(GameCharacter character, Vector2 currentCell, Vector2 enemyCell, float currentElevation)
            {
                int terrainLevel = (int)(currentElevation / (IsoInfo.TileElevation));
                float enemyElevation = MapManager.Instance.CurrentMap.Cells[(int)enemyCell.Y][(int)enemyCell.X].getTerrainElevation(character.Position);
                int enemyLevel = (int)(enemyElevation / (IsoInfo.TileElevation));
                if (terrainLevel == enemyLevel) //Check the collision in levels 1 and 2 if both characters are at the same level
                {
                    Dictionary<float, Collidable> dict;
                    character.Mobile.Collidables.TryGetValue(character.Mobile.AnimatedSprite.CurrentAnimationName, out dict);
                    if (dict != null)
                    {
                        //Level 1:
                        Collidable collider = null;
                        dict.TryGetValue(1.0f, out collider);
                        if (collider != null)
                        {
                            if (this.AttackCollision.collide(new Vector2(position.X, position.Y), collider, new Vector2(character.Position.X, character.Position.Y)))
                            {
                                return true;
                            }
                        }
                        
                        //Level 2:
                        dict.TryGetValue(2.0f, out collider);
                        if (collider != null)
                        {
                            if (this.AttackCollision.collide(new Vector2(position.X, position.Y), collider, new Vector2(character.Position.X, character.Position.Y)))
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (terrainLevel == enemyLevel + 1) //If the character is one level over the enemy, we only check Level 2
                {
                    Dictionary<float, Collidable> dict;
                    character.Mobile.Collidables.TryGetValue(character.Mobile.AnimatedSprite.CurrentAnimationName, out dict);
                    if (dict != null)
                    {
                        Collidable collider = null;
                        //Level 2:
                        dict.TryGetValue(2.0f, out collider);
                        if (collider != null)
                        {
                            if (this.AttackCollision.collide(new Vector2(position.X, position.Y), collider, new Vector2(character.Position.X, character.Position.Y)))
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (terrainLevel == enemyLevel - 1) //If the character is one level under the enemy, we only check Level 1
                {
                    Dictionary<float, Collidable> dict;
                    character.Mobile.Collidables.TryGetValue(character.Mobile.AnimatedSprite.CurrentAnimationName, out dict);
                    if (dict != null)
                    {
                        Collidable collider = null;
                        //Level 2:
                        dict.TryGetValue(1.0f, out collider);
                        if (collider != null)
                        {
                            if (this.AttackCollision.collide(new Vector2(position.X, position.Y), collider, new Vector2(character.Position.X, character.Position.Y)))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false; //If the character and the enemy are distanced by two levels or more, the collision is imposible
            }
        #endregion
    }
}
