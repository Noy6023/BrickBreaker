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
        public int x { get; set; } //the x coordinate
        public int y { get; set; } //the y coordinate
        public Vector(int x, int y) //constructor
        {
            this.x = x;
            this.y = y;
        }
        public Vector(Vector other) //copy constructor
        {
            this.x = other.x;
            this.y = other.y;
        }
    }
}