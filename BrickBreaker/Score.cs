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
    public class Score : Shape
    {
        private int score;
        public Score(Vector position, Color color) : base(position, color)
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
        }
        public Score() : base()
        {
            base.Paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
            Position = new Vector(0, (int)Paint.TextSize);
        }
        public int GetScore() { return score; }
        public void IncreaseScore()
        {
            score++;
        }
        public override void Draw(Canvas canvas)
        {
            canvas.DrawText("Score: " + score.ToString(), Position.X, Position.Y, Paint);
        }
    }
}