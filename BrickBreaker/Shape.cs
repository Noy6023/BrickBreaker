using Android.App;
using Android.Content;
using Android.Graphics;
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
    public class Shape
    {
        public Vector position { get; set; } //the current positon of the shape
        public Paint paint { get; set; } //the paint of the shape
        public Shape(Vector position, Paint paint)
        {
            this.position = new Vector(position);
            this.paint = new Paint(paint);
        }
        public Shape()
        {
            paint = new Paint();
            position = new Vector(0, 0);
        }
        public virtual void Draw(Canvas canvas)
        {
            
        }
    }
}