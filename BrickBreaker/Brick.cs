using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BrickBreaker
{
    public class Brick : Shape
    {
        public bool IsVisible { get; set; } //true if the brick is visable and can be hit. else false
        public static Vector Size { get; set; }
        public Brick(Vector position, Color color) : base(position, color) //constructor
        {
            this.IsVisible = true;
        }
        public Brick(Brick other):base()
        {
            base.Position = new Vector(other.Position);
            base.Paint = new Paint(other.Paint);
            Paint.Color = other.Paint.Color;
            IsVisible = other.IsVisible;
        }
        /// <summary>
        /// draws the brick
        /// </summary>
        /// <param name="canvas">the canvas</param>
        /// <param name="space">the space that comes after the brick</param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(Position.X,
                Position.Y,
                Position.X + Size.X,
                Position.Y + Size.Y, Paint);
        }
        
        /// <summary>
        /// a check if the brick was hit by the ball. if so then change velocity of the ball and the visiblity of the brick
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="space">the space</param>
        /// <returns>true- the brick was hit by the ball. false- the brick wasn't hit by the ball.</returns>
        public bool IsHit(Ball ball)
        {
            if (!this.IsVisible)
            {
                return false;
            }
            /*
            Rect brickRect = new Rect(this.Position.X, this.Position.Y, this.Position.X + Size.X, this.Position.Y + Size.Y);
            Rect ballRect = new Rect(ball.Position.X - Ball.Radius, ball.Position.Y - Ball.Radius, ball.Position.X + Ball.Radius, ball.Position.Y + Ball.Radius);
            if(brickRect.Intersect(ballRect))
            {
                this.IsVisible = false;
                ball.Velocity.Y = -ball.Velocity.Y;
                Random rand = new Random();
                ball.Velocity.X += rand.Next(-2, 2);
                int sign = 0;
                if (ball.Velocity.X > 0) sign = 1;
                else sign = -1;
                //if velocity gets too slow or too fast - normalize it
                if (Math.Abs(ball.Velocity.X) < Constants.BALL_MIN_VELOCITY.X) ball.Velocity.X = sign * Constants.BALL_MIN_VELOCITY.X;
                if (Math.Abs(ball.Velocity.X) > Constants.BALL_MAX_VELOCITY.X) ball.Velocity.X = sign * Constants.BALL_MAX_VELOCITY.X;
                return true;
            }*/

            float testX = ball.Position.X;
            float testY = ball.Position.Y;

            if (ball.Position.X < this.Position.X) testX = this.Position.X;        // left edge
            else if (ball.Position.X > this.Position.X + Size.X) testX = this.Position.X + Size.X;     // right edge

            if (ball.Position.Y < this.Position.Y) testY = this.Position.Y;        // top edge
            else if (ball.Position.Y > this.Position.Y + Size.Y) testY = this.Position.Y + Size.Y;     // bottom edge

            float distX = ball.Position.X - testX;
            float distY = ball.Position.Y - testY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= Ball.Radius)
            {
                this.IsVisible = false;
                ball.Velocity.Y = -ball.Velocity.Y;
                Random rand = new Random();
                ball.Velocity.X += rand.Next(-2, 2);
                ball.KeepVelocity(); //make sure the velocity isn't too fast or to slow
                return true;
            }

            return false;
        }

    }
}