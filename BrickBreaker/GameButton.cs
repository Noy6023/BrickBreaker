﻿using Android.App;
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
        public Bitmap bitmap { get; set; }
        public Vector size { get; set; }
        public GameButton(Vector position, Bitmap bitmap, Vector size) : base(position)
        {
            this.bitmap = bitmap;
            this.size = size;
        }
        public override void Draw(Canvas canvas)
        {
            canvas.DrawBitmap(bitmap, position.x, position.y, paint);
        }
    }
}