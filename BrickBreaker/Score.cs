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
            base.paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
        }
        public Score() : base()
        {
            base.paint.TextSize = Constants.DEFULT_SCORE_SIZE;
            this.score = 0;
            position = new Vector(0, (int)paint.TextSize);
        }
        public int GetScore() { return score; }
        public void IncreaseScore()
        {
            score++;
        }
        public override void Draw(Canvas canvas)
        {
            canvas.DrawText("Score: " + score.ToString(), position.x, position.y, paint);
        }
    }
}