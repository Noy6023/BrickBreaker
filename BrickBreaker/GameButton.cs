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
    class GameButton : Shape
    {
        public Bitmap Bitmap { get; set; }
        public Point Size { get; set; }
        public bool Show { get; set; }
        //ctor
        public GameButton(Point position, Bitmap bitmap, Point size, bool show) : base(position)
        {
            this.Position = new Point(position);
            this.Bitmap = bitmap;
            this.Size = new Point(size.X, size.Y);
            this.Show = show;
        }
        /// <summary>
        /// draw function
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas)
        {
            if (Show)
                canvas.DrawBitmap(Bitmap, Position.X, Position.Y, Paint);
        }
        public void UpdateSize(Canvas canvas)
        {
            Size = new Point(Bitmap.GetScaledWidth(canvas), Bitmap.GetScaledHeight(canvas));
        }
        /// <summary>
        /// checks if the button was clicked
        /// </summary>
        /// <param name="position">the position of the touch</param>
        /// <returns></returns>
        public bool IsClicked(Point position)
        {
            float left = Position.X;
            float top = Position.Y;
            float right = Position.X + Size.X;
            float bottom = Position.Y + Size.Y;
            float x = position.X;
            float y = position.Y;
            if (x >= left && x <= right && y >= top && y <= bottom) return true;
            return false;
        }
    }
}