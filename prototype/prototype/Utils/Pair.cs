using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Utils
{
    /// <summary>
    /// Pair of objects
    /// </summary>
    /// <typeparam name="K">Class of the first object of the Pair</typeparam>
    /// <typeparam name="T">Class of the second object of the Pair</typeparam>
    public class Pair<K, T>
    {
        /// <summary>
        /// First element
        /// </summary>
        private K first;
        /// <summary>
        /// Second element
        /// </summary>
        private T second;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        public Pair(K first, T second)
        {
            this.first = first;
            this.second = second;
        }

        public Pair()
        {
            this.first = default(K);
            this.second = default(T);
        }

        /// <summary>
        /// <see cref="first"/>
        /// </summary>
        public K First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }

        /// <summary>
        /// <see cref="second"/>
        /// </summary>
        public T Second
        {
            get
            {
                return this.second;
            }
            set
            {
                this.second = value;
            }
        }
    }
}

