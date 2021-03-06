﻿using System;

namespace BrickBreaker
{
    /// <summary>
    /// a point representation with an x and y
    /// </summary>
    public class Vector
    {
        public float X { get; set; } //the x coordinate
        public float Y { get; set; } //the y coordinate

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        public Vector(float x, float y) //constructor
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="other">the other vector to copy</param>
        public Vector(Vector other) //copy constructor
        {
            this.X = other.X;
            this.Y = other.Y;
        }

        /// <summary>
        /// calaculates the distance between the current vector to the other given one
        /// </summary>
        /// <param name="other">the other vector</param>
        /// <returns>the distance</returns>
        public double Distance(Vector other)
        {
            return Math.Sqrt(Math.Pow((double)(this.X - other.X), 2.0) + Math.Pow((double)(this.Y - other.Y), 2.0));
        }
    }
}