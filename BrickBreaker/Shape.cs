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
        public Shape(Vector position, Color color)
        {
            this.position = new Vector(position);
            this.paint = new Paint();
            paint.Color = color;
        }
        public Shape(Vector position)
        {
            this.position = new Vector(position);
            this.paint = new Paint();
        }
        public Shape(Color color)
        {
            position = new Vector(Constants.DEFULT_VECTOR);
            this.paint = new Paint();
            paint.Color = color;
        }
        public Shape()
        {
            paint = new Paint();
            paint.Color = Constants.DEFULT_COLOR;
            position = new Vector(Constants.DEFULT_VECTOR);
        }
        public virtual void Draw(Canvas canvas) { }
    }
}