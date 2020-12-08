﻿using Android.App;
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
    public class Bat : Shape
    {
        public static Vector Size { get; set; } //the size of the bat
        public Vector Velocity { get; set; } //the velocity of the bat
        public Bat(Vector position, Color color, Vector velocity) : base(position, color) //costructor
        {
            this.Velocity = new Vector(velocity);
        }
        public Bat(Color color):base(color) //defult constructor
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
                if (ball.Position.Y >= this.Position.Y - Ball.Radius) //if the bottom bat missed the ball
                {
                    if (ball.Position.X < this.Position.X - Ball.Radius || ball.Position.X > this.Position.X + Size.X + Ball.Radius) return -1;
                    else
                    {
                        ball.Velocity.Y = -ball.Velocity.Y;
                        return 1;
                    }
                }
                return 0;
            }
            if (ball.Position.Y <= this.Position.Y + Ball.Radius) //if the bat missed the ball
            {
                if (ball.Position.X < this.Position.X || ball.Position.X > this.Position.X + Size.X) return -1;
                else
                {
                    ball.Velocity.Y = -ball.Velocity.Y;
                    return 1;
                }
            }
            return 0;
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