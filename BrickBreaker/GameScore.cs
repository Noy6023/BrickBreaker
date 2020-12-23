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
        public GameScore(Vector position, Color color) : base(position, color)
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
        }
        public GameScore() : base()
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
            Position = new Vector(0, (int)Paint.TextSize);
        }
        /// <summary>
        /// get score function
        /// </summary>
        /// <returns></returns>
        public int GetScore() { return score; }
        
        /// <summary>
        /// increases the score
        /// </summary>
        public void IncreaseScore()
        {
            score++;
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