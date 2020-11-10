using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BrickBreaker
{
    public class Board : View
    {
        public bool lost { get; set; }
        float x, y, r; //ball variables
        float deltax, deltay; //ball movement
        float batX; //bat position
        public float batDeltax { get; set; } //bat movement (by sensor)
        //paints
        Paint pBall;
        Paint pBat;
        Context context; //context
        public Board(Context context) : base(context)
        {
            this.context = context;
            lost = false;
            pBall = new Paint();
            pBat = new Paint();
            pBat.Color = Color.Red;
            pBall.Color = Color.White;
            //start pos
            x = 100;
            y = 100;
            //movement speed
            deltax = 20;
            deltay = 20;
            r = 30; //ball size
            batDeltax = 0; //start bat speed
        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas); //set the canvas to be drawn on
            int batHeight = 20; //set the height of the bat
            int batWidth = 200; //set the width of the bat
            canvas.DrawColor(Color.Black); //set background color to black
            //draw the bat in the correct position
            canvas.DrawRect(batX, canvas.Height - batHeight, batX + batWidth, canvas.Height, pBat);
            canvas.DrawCircle(x, y, r, pBall); //draw the ball in the correct position
            batX = batX - batDeltax * 5; //update the bat movement
            //update the ball movement
            x = x + deltax;
            y = y + deltay;
            //check bounds of the bat
            if (batX < 0)
                batX = 0;
            if (batX >= canvas.Width-batWidth)
                batX = canvas.Width - batWidth;
            //check bounds of the ball - walls
            if (x <= 0 || x >= canvas.Width - r)
                deltax = -deltax;
            if (y < 0 || y >= canvas.Height - r)
                deltay = -deltay;
            if(y >= canvas.Height-r)
            {
                if (x < batX || x > batX + batWidth) lost = true;
            }
            if(!lost) Invalidate(); //keep calling this function and drawing until the app is closed.
        }
    }
}