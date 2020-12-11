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
            if(this.HasHitBall(ball, Size))
            {
                this.IsVisible = false;
                ball.Velocity.Y = -ball.Velocity.Y;
                ball.ChangeVelocity();
                return true;
            }

            return false;
        }

    }
}