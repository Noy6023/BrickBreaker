using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
namespace BrickBreaker
{
    /// <summary>
    /// a constants class
    /// </summary>
    static class Constants
    {
        //general constants
        public static Vector DEFULT_SCREEN_SIZE = new Vector(1000, 2000);
        public static Vector DEFULT_VECTOR = new Vector(0, 0);
        public static Color DEFULT_COLOR = Color.Argb(255, 0, 0, 0);
        public static Color BACKGROUND_COLOR = Color.Argb(255, 181, 245, 255);
        public const int PAUSE_BUTTON_SIZE = 100;
        public const int RESUME_BUTTON_SIZE = 150;
        public static Vector START_TEXT_SIZE = new Vector(400, 50);
        //bat constants
        public static Vector BAT_SIZE = new Vector(300, 20);
        public static Color DEFULT_BAT_COLOR = Color.Black;

        //brick constants
        public static Vector BRICK_SMALL_SIZE = new Vector(30, 30);
        public static Vector BRICK_BIG_SIZE = new Vector(20, 20);
        public static Vector BRICK_MEDIUM_SIZE = new Vector(25, 25);
        public const int SPACE = 60;
        public static Color DEFULT_BRICK_COLOR = Color.Black;

        //ball constants
        public static Vector BALL_START_VELOCITY = new Vector(7, 13);
        public const int MEDIUM_BALL_RADIUS = 40;
        public const int SMALL_BALL_RADIUS = 50;
        public const int BIG_BALL_RADIUS = 30;
        public const int EDGE = 50;
        public static Vector BALL_MIN_VELOCITY = new Vector(3, 13);
        public static Vector BALL_MAX_VELOCITY = new Vector(15, 13);

        //Player manager constants
        public const int NUM_OF_PLAYERS = 5;

        //score constants
        public const int DEFULT_SCORE_SIZE = 70;

        //firestore constants
        public const string NAME = "Name";
        public const string HIGHEST_VALUE = "HighestValue";
        public const string LAST_VALUE = "LastValue";
        public const string KEY = "Key";
        public const string PLAYERS_COLLECTION = "Players";
        public const string SCORE = "Score";
        public const string DEFULT_NAME = "Player";

        //shared prefrences constants
        public const string SP_NAME = "Settings";
        public const string MUSIC = "music";
        public const string LAST_SCORE = "score";
        public const string SOUND = "sound";
        public const string BALL_SIZE = "ball size";
        public const string BRICK_SIZE = "brick size";
        public const string DIFFICULTY = "difficulty";
    }
}