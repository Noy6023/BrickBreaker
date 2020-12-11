using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
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
    public class Ball : Shape
    {
        public static int Radius { get; set; } //the radius of the ball
        public Vector Velocity { get; set; } //the velocity of the ball
        public Ball(Vector position, Color color, Vector velocity) : base(position, color) //constructor
        {
            this.Velocity = new Vector(velocity);
        }
        public Ball(Color color) : base(color) //defult constructor
        {
            base.Position = new Vector(Constants.DEFULT_VECTOR);
            base.Paint = new Paint();
            Paint.Color = color;
            this.Velocity = new Vector(Constants.BALL_START_VELOCITY);
        }
        /// <summary>
        /// updates the movement of the ball by adding the velocity to the position
        /// </summary>
        public void UpdateMovement()
        {
            KeepVelocity();
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }
        /// <summary>
        /// handles a hit with the wall by negating the velocity
        /// </summary>
        /// <param name="screenSize">the screen size</param>
        public void UpdateWallHit(Vector screenSize) //handle hits with the ball
        {
            if (Position.X - Radius <= 0 || Position.X >= screenSize.X - Radius)
            {
                KeepVelocity();
                Velocity.X = -Velocity.X;
            }
        }
        public void KeepVelocity()
        {
            int sign = Velocity.X / Math.Abs(Velocity.X);
            //if velocity gets too slow or too fast - normalize it
            if (Math.Abs(this.Velocity.X) < Constants.BALL_MIN_VELOCITY.X) this.Velocity.X = sign * Constants.BALL_MIN_VELOCITY.X;
            if (Math.Abs(this.Velocity.X) > Constants.BALL_MAX_VELOCITY.X) this.Velocity.X = sign * Constants.BALL_MAX_VELOCITY.X;
        }

        public void ChangeVelocity()
        {
            Random rand = new Random();
            Velocity.X += rand.Next(-2, 2);
            KeepVelocity(); //make sure the velocity isn't too fast or to slow
        }

        /// <summary>
        /// draws the ball on the canvas
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas) //draw the ball in the correct position
        {
            canvas.DrawCircle(Position.X, Position.Y, Radius, Paint); 
        }
    }
}