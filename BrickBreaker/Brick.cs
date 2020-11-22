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
        public Brick(Vector position, Paint paint) : base(position, paint) //constructor
        {
            this.isVisible = true;
        }
        public Brick(Vector position) : base() //constructor
        {
            base.position = new Vector(position);
            base.paint = new Paint();
            paint.Color = Color.Green;
            isVisible = true;
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
                position.x + Constants.BRICK_SIZE.x,
                position.y + Constants.BRICK_SIZE.y, paint);
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
            Rect brickRect = new Rect(this.position.x, this.position.y, this.position.x + Constants.BRICK_SIZE.x, this.position.y + Constants.BRICK_SIZE.y);
            Rect ballRect = new Rect(ball.position.x - ball.radius, ball.position.y - ball.radius, ball.position.x + ball.radius, ball.position.y + ball.radius);
            if(brickRect.Intersect(ballRect))
            {
                this.isVisible = false;
                ball.velocity.y = -ball.velocity.y;
                Random rand = new Random();
                ball.velocity.x += rand.Next(-2, 2);
                return true;
            }
            return false;
        }
    }
}