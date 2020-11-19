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
    public class Ball
    {
        public Vector position { get; set; } //the current positon of the ball
        public Paint paint { get; set; } //the paint of the ball
        public int radius { get;  set; } //the radius of the ball
        public Vector velocity { get; set; } //the velocity of the ball
        public Ball(Vector position, Paint paint, Vector velocity, int radius) //constructor
        {
            this.position = new Vector(position);
            this.paint = new Paint(paint);
            this.velocity = new Vector(velocity);
            this.radius = radius;
        }
        public Ball() //defult constructor
        {
            this.position = new Vector(300, 1000);
            this.paint = new Paint();
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
        /// <param name="bat">the bat</param>
        public void UpdateWallHit(Vector screenSize, Bat bat) //handle hits with the ball
        {
            if (position.x <= 0 || position.x >= screenSize.x - 2 * radius)
                velocity.x = -velocity.x;
            if (position.y < 0 )//|| position.y >= screenSize.y - bat.size.y - 2 * radius)
                velocity.y = -velocity.y;
        }
        /// <summary>
        /// draws the ball on the canvas
        /// </summary>
        /// <param name="canvas"></param>
        public void Draw(Canvas canvas) //draw the ball in the correct position
        {
            canvas.DrawCircle(position.x + radius, position.y + radius, radius, paint); 
        }
    }
}