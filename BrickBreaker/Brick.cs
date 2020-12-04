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
        public bool isVisible { get; set; } //true if the brick is visable and can be hit. else false
        public static Vector size { get; set; }
        public Brick(Vector position, Color color) : base(position, color) //constructor
        {
            this.isVisible = true;
        }
        public Brick(Brick other):base()
        {
            base.position = new Vector(other.position);
            base.paint = new Paint(other.paint);
            paint.Color = other.paint.Color;
            isVisible = other.isVisible;
        }
        /// <summary>
        /// draws the brick
        /// </summary>
        /// <param name="canvas">the canvas</param>
        /// <param name="space">the space that comes after the brick</param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(position.x,
                position.y,
                position.x + size.x,
                position.y + size.y, paint);
        }
        
        /// <summary>
        /// a check if the brick was hit by the ball. if so then change velocity of the ball and the visiblity of the brick
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="space">the space</param>
        /// <returns>true- the brick was hit by the ball. false- the brick wasn't hit by the ball.</returns>
        public bool IsHit(Ball ball)
        {
            if (!this.isVisible)
            {
                return false;
            }
            Rect brickRect = new Rect(this.position.x, this.position.y, this.position.x + size.x, this.position.y + size.y);
            Rect ballRect = new Rect(ball.position.x - Ball.radius, ball.position.y - Ball.radius, ball.position.x + Ball.radius, ball.position.y + Ball.radius);
            if(brickRect.Intersect(ballRect))
            {
                this.isVisible = false;
                ball.velocity.y = -ball.velocity.y;
                Random rand = new Random();
                ball.velocity.x += rand.Next(-2, 2);
                int sign = 0;
                if (ball.velocity.x > 0) sign = 1;
                else sign = -1;
                //if velocity gets too slow or too fast - normalize it
                if (Math.Abs(ball.velocity.x) < Constants.BALL_MIN_VELOCITY.x) ball.velocity.x = sign * Constants.BALL_MIN_VELOCITY.x;
                if (Math.Abs(ball.velocity.x) > Constants.BALL_MAX_VELOCITY.x) ball.velocity.x = sign * Constants.BALL_MAX_VELOCITY.x;
                return true;
            }
            return false;
        }

    }
}