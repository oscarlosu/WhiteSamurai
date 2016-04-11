using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Prototype.Utils
{
    public static class RandomVector
    {

        public static Random rand = new Random();
        public static Vector3 colorRandomVector()
        {
            double factor = rand.NextDouble();
            factor *= 255;
            int min = (int) Math.Floor(255 - factor);
            return colorRandomVector(min);
        }
        /// <summary>
        /// Returns a random vector with numbers between 0 and 255
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <returns>The vector</returns>
        public static Vector3 colorRandomVector(int min)
        {
            min = Math.Max(0, min);
            return colorRandomVector(min, 255);
        }
        /// <summary>
        /// Returns a random vector with numbers between 0 and 255
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>The vector</returns>
        public static Vector3 colorRandomVector(int min, int max)
        {
            min = Math.Max(0, min);
            max = Math.Min(max, 255);
            int x = rand.Next(min, max);
            int y = rand.Next(min, max);
            int z = rand.Next(min, max);
            Vector3 v = new Vector3(x, y, z);
            return v;
        }
    }
}
