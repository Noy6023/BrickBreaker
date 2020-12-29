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
    static class Constants
    {
        //general constants
        public static Vector DEFULT_SCREEN_SIZE = new Vector(1000, 2000);
        public static Vector DEFULT_VECTOR = new Vector(0, 0);
        public static Color DEFULT_COLOR = Color.Argb(255, 23, 30, 31);
        public static Color BACKGROUND_COLOR = Color.Argb(255, 181, 245, 255);
        public const int PAUSE_BUTTON_SIZE = 100;
        public const int RESUME_BUTTON_SIZE = 200;
        public static Vector START_TEXT_SIZE = new Vector(400, 50);
        //bat constants
        public static Vector BAT_SIZE = new Vector(300, 20);
        public static Color DEFULT_BAT_COLOR = Color.Black;


        //brick constants
        public static Vector BRICK_SMALL_SIZE = new Vector(30, 20);
        public static Vector BRICK_BIG_SIZE = new Vector(70, 40);
        public static Vector BRICK_MEDIUM_SIZE = new Vector(50, 30);
        public const int SPACE = 60;
        public static Color DEFULT_BRICK_COLOR = Color.Black;

        //ball constants
        public static Vector BALL_START_VELOCITY = new Vector(7, 13);
        public const int MEDIUM_BALL_RADIUS = 25;
        public const int SMALL_BALL_RADIUS = 20;
        public const int BIG_BALL_RADIUS = 35;
        public const int EDGE = 50;
        public static Vector BALL_MIN_VELOCITY = new Vector(3, 13);
        public static Vector BALL_MAX_VELOCITY = new Vector(15, 13);

        //Player manager constants
        public const int NUM_OF_PLAYERS = 5;

        //score constants
        public const int DEFULT_SCORE_SIZE = 70;


    }
}