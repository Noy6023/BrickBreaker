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
    public class Bat
    {
        private readonly Vector SIZE = new Vector(300, 20);
        private readonly Vector SCREEN_SIZE = new Vector(500, 1500);
        public Vector position { get; set; } //the current positon of the bat
        public Vector size { get; set; } //the size of the bat
        public Paint paint { get; set; } //the paint of the bat
        public Vector velocity { get; set; } //the velocity of the bat
        public Bat(Vector position, Vector size, Paint paint, Vector velocity) //costructor
        {
            this.position = new Vector(position);
            this.size = new Vector(size);
            this.paint = new Paint(paint);
            this.velocity = new Vector(velocity);
        }
        public Bat() //defult constructor
        {
            this.position = new Vector(0, SCREEN_SIZE.y - (SCREEN_SIZE.y / 10));
            this.size = new Vector(SIZE.x, SIZE.y);
            this.paint = new Paint();
            paint.Color = Color.Red;
            velocity = new Vector(0, 0);
        }
        /// <summary>
        /// handle the bats bounds
        /// </summary>
        /// <param name="canvas">the current canvas</param>
        public void UpdateBounds(Canvas canvas)
        {
            if (position.x < 0)
                position.x = 0;
            if (position.x >= canvas.Width - size.x)
                position.x = canvas.Width - size.x;
        }
        /// <summary>
        /// update the bat movement by decreasing the velocity times 5 from the position
        /// </summary>
        public void UpdateMovement()
        {
            position.x -= velocity.x * 5;
        }
        /// <summary>
        /// a check if the bat has hit the ball or missed it
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="canvas">the canvas</param>
        /// <returns>1- the bat hit the ball. -1- the bat missed the ball. 0- the ball wasn't near the bat</returns>
        public int IsBallHit(Ball ball, Canvas canvas)
        {
            if (ball.position.y >= canvas.Height - size.y - ball.radius) //if the bat missed the ball
            {
                if (ball.position.x < this.position.x || ball.position.x > this.position.x + this.size.x) return -1;
                else
                {
                    ball.velocity.y = -ball.velocity.y;
                    return 1;
                }
            }
            return 0;
            
        }
        /// <summary>
        /// draws the bat on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public void Draw(Canvas canvas)
        {
            canvas.DrawRect(position.x, canvas.Height - size.y, position.x + size.x, canvas.Height, paint);
        }
    }
}