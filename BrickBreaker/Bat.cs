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
    /// <summary>
    /// the bat class
    /// </summary>
    public class Bat : Shape
    {
        public static Vector Size { get; set; } //the size of the bat
        public Vector Velocity { get; set; } //the velocity of the bat

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="position">the position</param>
        /// <param name="color">the color</param>
        /// <param name="velocity">the velocity</param>
        public Bat(Vector position, Color color, Vector velocity) : base(position, color)
        {
            this.Velocity = new Vector(velocity);
        }

        /// <summary>
        /// defult constructor
        /// </summary>
        /// <param name="color">the color</param>
        public Bat(Color color):base(color)
        {
            base.Position = new Vector(0, Constants.DEFULT_SCREEN_SIZE.Y - (Constants.DEFULT_SCREEN_SIZE.Y / 10));
            Size = new Vector(Constants.BAT_SIZE.X, Constants.BAT_SIZE.Y);
            base.Paint = new Paint();
            Paint.Color = color;
            Velocity = new Vector(Constants.DEFULT_VECTOR);
        }

        /// <summary>
        /// handle the bats bounds
        /// </summary>
        /// <param name="canvas">the current canvas</param>
        public void UpdateBounds(Canvas canvas)
        {
            if (Position.X < 0)
                Position.X = 0;
            if (Position.X >= canvas.Width - Size.X)
                Position.X = canvas.Width - Size.X;
        }

        /// <summary>
        /// update the bat movement by decreasing the velocity times 5 from the position
        /// </summary>
        public void UpdateMovement()
        {
            Position.X -= Velocity.X * 5;
        }

        /// <summary>
        /// a check if the bat has hit the ball or missed it
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="canvas">the canvas</param>
        /// <param name="bat">'t' if its the top bat. 'b' if its the bottom bat</param>
        /// <returns>1- the bat hit the ball. -1- the bat missed the ball. 0- the ball wasn't near the bat</returns>
        public int IsBallHit(Ball ball, Canvas canvas, char bat)
        {
            if (bat == 'b')
            {
                if (ball.Position.Y - Ball.Radius * 2 > canvas.Height)
                    return -1;
            }
            else
            {
                if (ball.Position.Y + Ball.Radius * 2 < 0)
                    return -1;
            }
            if(HasHitBall(ball, Size))
            {
                if(bat == 'b') ball.Position.Y = this.Position.Y - Ball.Radius;
                else ball.Position.Y = this.Position.Y + Size.Y + Ball.Radius;
                ball.ChangeVelocity();
                ball.Velocity.Y = -ball.Velocity.Y;
                if (ball.Position.X > this.Position.X + Size.X / 2)
                    ball.Velocity.X = Math.Abs(ball.Velocity.X);
                else
                    ball.Velocity.X = -1 * Math.Abs(ball.Velocity.X);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// sets the size of the bat according to the canvas size
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public void SetSize(Canvas canvas)
        {
            Size.X = canvas.Width / 3.5f;
            Size.Y = Size.X / Constants.BAT_SIZE.Y;
        }

        /// <summary>
        /// draws the bat on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y, Paint);
        }
    }
}