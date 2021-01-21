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
    /// <summary>
    /// a game button class
    /// </summary>
    class GameButton : Shape
    {
        public Bitmap Bitmap { get; set; }
        public Vector Size { get; set; }
        public bool Show { get; set; }
        //ctor
        public GameButton(Vector position, Bitmap bitmap, Vector size, bool show) : base(position)
        {
            this.Position = new Vector(position);
            this.Bitmap = bitmap;
            this.Size = new Vector(size.X, size.Y);
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
            Size = new Vector(Bitmap.GetScaledWidth(canvas), Bitmap.GetScaledHeight(canvas));
        }
        /// <summary>
        /// checks if the button was clicked
        /// </summary>
        /// <param name="position">the position of the touch</param>
        /// <returns></returns>
        public bool IsClicked(Vector position)
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