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
    public class GameScore : Shape
    {
        private int score;
        public GameScore(Point position, Color color) : base(position, color)
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
        }
        public GameScore() : base()
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
            Position = new Point(0, (int)Paint.TextSize);
        }
        /// <summary>
        /// get score function
        /// </summary>
        /// <returns></returns>
        public int GetScore() { return score; }
        
        /// <summary>
        /// increases the score according to the difficulty
        /// </summary>
        /// <param name="difficulty">the difficulty</param>
        public void IncreaseScore(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Easy)
            {
                score++;
            }
            else
            {
                score += 2;
            }
        }

        /// <summary>
        /// draws the score on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawText("Score: " + score.ToString(), Position.X, Position.Y, Paint);
        }
    }
}