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
        public Vector Size { get; set; }
        public bool Show { get; set; }
        //ctor
        public GameButton(Vector position, Bitmap bitmap, Vector size, bool show) : base(position)
        {
            this.Bitmap = bitmap;
            this.Size = size;
            this.Show = show;
        }
        /// <summary>
        /// draw function
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas)
        {
            if(Show)
                canvas.DrawBitmap(Bitmap, Position.X, Position.Y, Paint);
        }
    }
}