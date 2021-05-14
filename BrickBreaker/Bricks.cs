using Android.App;
using Android.Content;
using Android.Graphics;
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
    class Bricks
    {
        private Brick[,] bricks; //the bricks array
        private int visibleBricks; //the number of visible bricks
        public Bricks(Canvas canvas)
        {
            InitBricks(canvas); //create the bricks array according to the size of the screen
            visibleBricks = bricks.Length; //all the bricks are visible when created
        }

        /// <summary>
        /// inits the brick array by creating the bricks and positioning them
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void InitBricks(Canvas canvas)
        {
            Color brickColor = ColorManager.Instance.GetColor(ColorKey.Brick);
            float space = canvas.Width / 20;
            float x = space * 1.5f;
            float y = canvas.Height / 3;
            Brick.SetSize(canvas);
            bricks = new Brick[(int)((canvas.Height / 3) / (Brick.Size.Y + space)), (int)((canvas.Width - x) / (Brick.Size.X + space))];

            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = new Brick(new Vector(x, y), brickColor);
                    x += Brick.Size.X + space;
                }
                y += Brick.Size.Y + space;
                x = space * 1.5f;
            }
        }

        /// <summary>
        /// draw the bricks
        /// </summary>
        /// <param name="canvas">the canvas to draw on</param>
        public void Draw(Canvas canvas)
        {
            //draw the bricks
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].IsVisible)
                        bricks[i, j].Draw(canvas);
                }
            }
        }

        /// <summary>
        /// were the bricks hit by the ball or not. if so then make the brick invisible.
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <returns>whether the bricks were hit or not</returns>
        public bool HasBallHit(Ball ball)
        {
            if (IsBallNearBricks(ball))
            {
                //if the ball is near the bricks - check hit on every brick
                for (int i = 0; i < bricks.GetLength(0); i++)
                {
                    for (int j = 0; j < bricks.GetLength(1); j++)
                    {
                        if (bricks[i, j].IsHit(ball))
                        {
                            bricks[i, j].IsVisible = false;
                            ball.Velocity.Y = -ball.Velocity.Y;
                            ball.ChangeVelocity();
                            visibleBricks--;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// checks if there are any bricks left
        /// </summary>
        /// <returns></returns>
        public bool AreOver()
        {
            if (visibleBricks == 0)
                return true;
            return false;
        }

        /// <summary>
        /// makes every brick in the given array visible
        /// </summary>

        public void MakeVisible()
        {
            visibleBricks = bricks.Length;
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j].IsVisible = true;
                }
            }
        }

        /// <summary>
        /// checks if the ball is near the bricks
        /// </summary>
        /// <param name="ball">the ball to check</param>
        /// <returns>true- the ball is lower than the first brick and higher than the last brick. else - false.</returns>
        private bool IsBallNearBricks(Ball ball)
        {
            if ((ball.Position.Y >= bricks[0, 0].Position.Y - Ball.Radius && ball.Position.Y <= bricks[bricks.GetLength(0) - 1, 0].Position.Y + 2 * Ball.Radius))
                return true;
            return false;
        }
    }
}