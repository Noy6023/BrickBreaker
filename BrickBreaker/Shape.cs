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
    /// <summary>
    /// an abstract shape class of everything that can be drawn on the game screen
    /// </summary>
    public abstract class Shape
    {
        public Vector Position { get; set; } //the current positon of the shape
        public Paint Paint { get; set; } //the paint of the shape
        ///ctors
        public Shape(Vector position, Color color)
        {
            this.Position = new Vector(position);
            this.Paint = new Paint();
            Paint.Color = color;
        }
        public Shape(Vector position)
        {
            this.Position = new Vector(position);
            this.Paint = new Paint();
        }
        public Shape(Color color)
        {
            Position = new Vector(Constants.DEFULT_VECTOR);
            this.Paint = new Paint();
            Paint.Color = color;
        }
        public Shape()
        {
            Paint = new Paint();
            Paint.Color = Constants.DEFULT_COLOR;
            Position = new Vector(Constants.DEFULT_VECTOR);
        }
        /// <summary>
        /// an abstract draw function that will be different in every sub class
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void Draw(Canvas canvas);

        /// <summary>
        /// check hit of the current shape with ball
        /// </summary>
        /// <param name="ball">the ball to check</param>
        /// <param name="size">the size of the shape</param>
        /// <returns>true - there was a hit. else - false.</returns>
        public bool HasHitBall(Ball ball, Vector size)
        {
            float testX = ball.Position.X;
            float testY = ball.Position.Y;

            if (ball.Position.X < this.Position.X) testX = this.Position.X;        // left edge
            else if (ball.Position.X > this.Position.X + size.X) testX = this.Position.X + size.X;     // right edge

            if (ball.Position.Y < this.Position.Y) testY = this.Position.Y;        // top edge
            else if (ball.Position.Y > this.Position.Y + size.Y) testY = this.Position.Y + size.Y;     // bottom edge

            float distX = ball.Position.X - testX;
            float distY = ball.Position.Y - testY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= Ball.Radius)
            {
                return true;
            }
            return false;
        }
    }
}