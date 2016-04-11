using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prototype.ScreenManager;
using Prototype.TileEngine;
using Prototype.TileEngine.IsometricView;
using Prototype.Characters.Enemies;
using Prototype.Characters.CharacterIO;
using Prototype.Characters.Factory;
using Prototype.Utils;
using Prototype.Characters.Map;
using Prototype.TileEngine.ColorAlgorithm;
using Prototype.TileEngine.MobileObject;

namespace Prototype.Characters
{
    public class CharacterManager
    {
        #region Fields
            /// <summary>
            /// Instance of the character manager
            /// </summary>
            private static CharacterManager instance = null;
            /// <summary>
            /// Player
            /// </summary>
            private Player player;
            /// <summary>
            /// List of the current enemies
            /// </summary>
            private List<EnemyGroup> enemyGroups;
            /// <summary>
            /// Enemy list, indexed by enemy ID
            /// </summary>
            private Dictionary<int, Enemy> enemies;
            /// <summary>
            /// Number of created enemies
            /// </summary>
            private int createdEnemies = 0;
            /// <summary>
            /// Map that indicates where the different characters are
            /// </summary>
            private CharacterMap map;
            /// <summary>
            /// List of dead characters
            /// </summary>
            private List<int> deathList;
        #endregion

        #region Properties
            /// <summary>
            /// Instance of the character manager
            /// </summary>
            public static CharacterManager Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new CharacterManager();
                    }
                    return instance;
                }
            }

            /// <summary>
            /// <see cref="player"/>
            /// </summary>
            public Player Player
            {
                get
                {
                    return player;
                }
            }

            /// <summary>
            /// <see cref="enemies"/>
            /// </summary>
            public List<EnemyGroup> EnemyGroups
            {
                get
                {
                    return enemyGroups;
                }
            }

            public Vector3 playerPosition
            {
                get
                {
                    return this.player.Mobile.Position;
                }
            }

            public CharacterMap Map
            {
                get
                {
                    return this.map;
                }
            }

            public List<int> DeathList
            {
                get
                {
                    return this.deathList;
                }
            }
        #endregion Properties

        #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            private CharacterManager()
            {
                this.enemyGroups = new List<EnemyGroup>();
                this.enemies = new Dictionary<int, Enemy>();
                this.deathList = new List<int>();
            }

            /// <summary>
            /// Creates the player
            /// </summary>
            /// <param name="modelFile">File that contains the model for the character</param>
            private void createPlayer(String modelFile, Pair<int,int> charCell, Vector3 initialOrientation)
            {
                CharModel model = ModelReader.readFromXML(modelFile);

                
                Vector3 middleCell = new Vector3(charCell.First * IsoInfo.TileSide + IsoInfo.TileCenter.X, charCell.Second * IsoInfo.TileSide + IsoInfo.TileCenter.Y, 0);
                float elevation = MapManager.Instance.CurrentMap.Cells[charCell.First][charCell.Second].getTerrainElevation(middleCell);
                Vector3 pos = new Vector3(middleCell.X, middleCell.Y, elevation);
                MobileObject mobile = new MobileObject(model.collidables, pos, model.drawOffset, model.attackCollider);
                mobile.AnimatedSprite.nSpritesRow = model.nFramesRow;
                foreach (AnimationReader reader in model.animations)
                {
                    mobile.addAnimation(reader.name, reader.x, reader.y, reader.width, reader.height, reader.nFrames, reader.frameDuration);
                }
                this.player = new Player(model.hp, model.dps, model.regeneration, model.speed, mobile, model.spriteSheet);
                this.player.PreviousMove = initialOrientation;
                if(model.interrumptibleAttackFrames != null)
                    this.player.InterrumptibleAttackFrames.AddRange(model.interrumptibleAttackFrames);
                if(model.interrumptibleDashFrames != null)
                    this.player.InterrumptibleDashFrames.AddRange(model.interrumptibleDashFrames);
            }

            private Enemy createEnemy(int id, EnemyType type, StrategiesType strategy, Pair<int, int> cell, Vector3 initialOrientation)
            {
                Enemy enemy;
                IdleFactory idleFactory = new IdleFactory();
                RandomFactory randomFactory = new RandomFactory();
                AttackFactory attackFactory = new AttackFactory();
                ChasingFactory chasingFactory = new ChasingFactory();
                AStarChasingFactory interceptionFactory = new AStarChasingFactory();
                AlertFactory alertFactory = new AlertFactory();
                FlyFactory flyFactory = new FlyFactory();

                Vector3 initialPosition = new Vector3(cell.Second * IsoInfo.TileSide + IsoInfo.TileCenter.X, cell.First * IsoInfo.TileSide + IsoInfo.TileCenter.Y, 0);
                float elevation = MapManager.Instance.CurrentMap.Cells[cell.First][cell.Second].getTerrainElevation(initialPosition);
                initialPosition = new Vector3(initialPosition.X, initialPosition.Y, elevation);
                if (strategy == StrategiesType.IDLE)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = idleFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = idleFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = idleFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                else if (strategy == StrategiesType.RANDOM)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = randomFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = randomFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = randomFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                else if (strategy == StrategiesType.ATTACK)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = attackFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = attackFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = attackFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }

                }
                else if (strategy == StrategiesType.CHASING)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = chasingFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = chasingFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = chasingFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                else if (strategy == StrategiesType.ASTARCHASING)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = interceptionFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = interceptionFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = interceptionFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                else if (strategy == StrategiesType.ALERT)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = alertFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = alertFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = alertFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                else // if (strategy == StrategiesType.FLY)
                {
                    if (type == EnemyType.WARRIOR)
                    {
                        enemy = flyFactory.createEnemyWarrior(id, initialPosition, initialOrientation);
                    }
                    else if (type == EnemyType.ARCHER)
                    {
                        enemy = flyFactory.createEnemyArcher(id, initialPosition, initialOrientation);
                    }
                    else //type == EnemyType.BOSS
                    {
                        enemy = flyFactory.createEnemyBoss(id, initialPosition, initialOrientation);
                    }
                }
                return enemy;
            }

        #endregion

        #region GameMethods
            /// <summary>
            /// Initializes the manager
            /// </summary>
            /// <param name="playerModel"></param>
            public void Initialize(String playerModel, String enemyList)
            {
                this.map = new CharacterMap(MapManager.Instance.CurrentMap.NRows, MapManager.Instance.CurrentMap.NCols);
                
                this.enemies.Clear();
                this.enemyGroups.Clear();
                
                CharacterList el = CharacterList.readFromXML(enemyList);
                createPlayer(playerModel, el.playerReader.initialPosition, el.playerReader.initialOrientation);
                Vector2 cell = Coordinates.toCell(playerPosition);
                this.map.addToCell(-1, (int)cell.Y, (int)cell.X);
                
                foreach (EnemyReader er in el.enemyList)
                {
                    Enemy enemy = createEnemy(this.createdEnemies, er.type, er.strategy, er.initialPosition, er.initialOrientation);
                    this.enemies.Add(this.createdEnemies, enemy);
                    cell = Coordinates.toCell(enemy.Position);
                    this.map.addToCell(this.createdEnemies, (int)cell.Y, (int)cell.X);
                    this.createdEnemies++;
                }
            }

            /// <summary>
            /// Loads content
            /// </summary>
            /// <param name="content">Content manager</param>
            public void LoadContent(ContentManager content)
            {
                this.player.LoadContent(content);
                foreach (Enemy enemy in this.enemies.Values)
                {
                    enemy.LoadContent(content);
                }
            }

            /// <summary>
            /// Unloads content
            /// </summary>
            public void UnloadContent()
            {
            }

            /// <summary>
            /// Updates the characters
            /// </summary>
            /// <param name="gameTime">Game time</param>
            /// <param name="input">Input state</param>
            public void Update(GameTime gameTime, InputState input)
            {
                Vector2 previousCell;
                Vector2 nextCell;
                List<Vector2> deathPositions = new List<Vector2>();
                List<EnemyGroup> disbandedGroups = new List<EnemyGroup>();
                previousCell = Coordinates.toCell(playerPosition);
                this.player.Update(input, gameTime);
                nextCell = Coordinates.toCell(playerPosition);
                if(previousCell != nextCell)
                {
                    this.map.deleteFromCell(-1, (int) previousCell.Y, (int) previousCell.X);
                    this.map.addToCell(-1, (int)nextCell.Y, (int)nextCell.X);
                }
                foreach (EnemyGroup enemyGroup in enemyGroups)
                {
                    enemyGroup.Update(gameTime, this.playerPosition, input);
                    if (enemyGroup.Disbanded == true)
                    {
                        disbandedGroups.Add(enemyGroup);
                    }
                }
                foreach (Enemy enemy in this.enemies.Values)
                {
                    previousCell = Coordinates.toCell(enemy.Position);
                    enemy.Update(input, gameTime);
                    nextCell = Coordinates.toCell(enemy.Position);
                    if (previousCell != nextCell)
                    {
                        this.map.deleteFromCell(enemy.ID, (int) previousCell.Y, (int) previousCell.X);
                        this.map.addToCell(enemy.ID, (int)nextCell.Y, (int)nextCell.X);
                    }
                }
                foreach (int id in deathList)
                {
                    GameCharacter gc = getEnemy(id);
                    Vector2 cell = Coordinates.toCell(gc.Position);
                    deathPositions.Add(cell);
                    map.deleteFromCell(id, (int)cell.Y, (int)cell.X);
                    this.deleteEnemy(id);
                }
                foreach(EnemyGroup eg in disbandedGroups)
                {
                    deleteEnemyGroup(eg);
                }
                this.deathList.Clear();
                ColorManager.createColor(deathPositions);
            }

            public void UpdateOnWin(GameTime gameTime)
            {
                this.player.UpdateOnWin(gameTime);
                foreach (Enemy enemy in this.enemies.Values)
                {
                    enemy.UpdateOnWin(gameTime);
                }
            }
            /// <summary>
            /// Draws the characters
            /// </summary>
            /// <param name="spriteBatch"></param>
            public void Draw(SpriteBatch spriteBatch)
            {
                this.player.Draw(spriteBatch);

                foreach (Enemy enemy in this.enemies.Values)
                {
                    enemy.Draw(spriteBatch);
                }
            }
        #endregion

        #region Methods
            /// <summary>
            /// Adds an enemy to the list
            /// </summary>
            /// <param name="enemy"></param>
            public void addEnemyGroup(EnemyGroup enemyG)
            {
                if (enemyG != null)
                {
                    this.enemyGroups.Add(enemyG);
                }
            }

            /// <summary>
            /// Deletes an enemy from the list
            /// </summary>
            /// <param name="enemy"></param>
            public void deleteEnemyGroup(EnemyGroup enemyG)
            {
                if (enemyG != null && this.enemyGroups.Contains(enemyG))
                {
                    this.enemyGroups.Remove(enemyG);
                }
            }

            public void addEnemy(Enemy enemy)
            {
                this.enemies.Add(enemy.ID, enemy);
            }

            public void deleteEnemy(int id)
            {
                
                this.enemies.Remove(id);
                
            }
            public Enemy getEnemy(int id)
            {
                Enemy en;
                this.enemies.TryGetValue(id, out en);
                return en;
            }
        #endregion Methods



            
    }
}
