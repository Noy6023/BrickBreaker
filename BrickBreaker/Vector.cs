using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    public class Vector
    {
        public int X { get; set; } //the x coordinate
        public int Y { get; set; } //the y coordinate
        public Vector(int x, int y) //constructor
        {
            this.X = x;
            this.Y = y;
        }
        public Vector(Vector other) //copy constructor
        {
            this.X = other.X;
            this.Y = other.Y;
        }
        public double Distance(Vector other)
        {
            return Math.Sqrt(Math.Pow((double)(this.X - other.X), 2.0) + Math.Pow((double)(this.Y - other.Y), 2.0));
        }
    }
}