using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Characters.Map
{
    public class CharCell
    {
        #region Fields
            /// <summary>
            /// List of enemies in this cell of the map
            /// </summary>
            private List<int> enemyIds;
        #endregion
        
        #region Properties
            public List<int> EnemyIds
            {
                get
                {
                    return enemyIds;
                }
            }
        #endregion

        public CharCell()
        {
            enemyIds = new List<int>();
        }

        public Boolean isCellEmpty()
        {
            return (enemyIds.Count == 0);
        }

        public void addEnemy(int id)
        {
            this.enemyIds.Add(id);
        }

        public void deleteEnemy(int id)
        {
            this.enemyIds.Remove(id);
        }
    }
}
