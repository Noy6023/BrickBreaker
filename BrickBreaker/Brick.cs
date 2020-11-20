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
        public readonly Vector SIZE = new Vector(70, 40);
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
        /// <summary>
        /// draws the brick
        /// </summary>
        /// <param name="canvas">the canvas</param>
        /// <param name="space">the space that comes after the brick</param>
        public void Draw(Canvas canvas, int space)
        {
            canvas.DrawRect(position.x + space, position.y + space, position.x + SIZE.x + space, position.y + SIZE.y + space, paint);
        }
        /// <summary>
        /// a check if the brick was hit by the ball. if so then change velocity of the ball and the visiblity of the brick
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="space">the space</param>
        /// <returns>true- the brick was hit by the ball. false- the brick wasn't hit by the ball.</returns>
        public bool IsHit(Ball ball, int space)
        {
            if (!this.isVisible) return false;
            if (ball.position.x + ball.velocity.x > this.position.x - space && ball.position.x + ball.velocity.x < this.position.x + SIZE.x + space)
            {
                if (ball.position.y + ball.velocity.y < this.position.y + SIZE.y + space  && ball.position.y + ball.velocity.y > this.position.y - space)
                {
                    if (this.isVisible)
                    {
                        this.isVisible = false;
                        ball.velocity.y = -ball.velocity.y;
                        Random rand = new Random();
                        ball.velocity.x += rand.Next(-5, 5);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}