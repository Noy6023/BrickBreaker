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
        public int radius { get;  set; } //the radius of the ball
        public Vector velocity { get; set; } //the velocity of the ball
        public Ball(Vector position, Paint paint, Vector velocity, int radius) : base(position, paint) //constructor
        {
            this.velocity = new Vector(velocity);
            this.radius = radius;
        }
        public Ball() : base() //defult constructor
        {
            base.position = new Vector(300, 1000);
            base.paint = new Paint();
            paint.Color = Color.White;
            this.velocity = new Vector(7, 15);
            this.radius = 30;
        }
        /// <summary>
        /// updates the movement of the ball by adding the velocity to the position
        /// </summary>
        public void UpdateMovement()
        {
            position.x += velocity.x;
            position.y += velocity.y;
        }
        /// <summary>
        /// handles a hit with the wall by negating the velocity
        /// </summary>
        /// <param name="screenSize">the screen size</param>
        public void UpdateWallHit(Vector screenSize) //handle hits with the ball
        {
            if (position.x - radius <= 0 || position.x >= screenSize.x - radius)
                velocity.x = -velocity.x;
            if (position.y < 0 )//position.y >= screenSize.y - bat.size.y - 2 * radius)
                velocity.y = -velocity.y;
        }
        /// <summary>
        /// draws the ball on the canvas
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas) //draw the ball in the correct position
        {
            canvas.DrawCircle(position.x, position.y, radius, paint); 
        }
    }
}