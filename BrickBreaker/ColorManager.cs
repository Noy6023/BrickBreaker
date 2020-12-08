using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    static class ColorManager
    {
        public static Hashtable Colors;
        public static string[] keys = { "ball", "bat", "brick", "background" };
        public static void InitColors()
        {
            Colors = new Hashtable();
            Colors.Add("ball", Constants.DEFULT_COLOR);
            Colors.Add("bat", Constants.DEFULT_BAT_COLOR);
            Colors.Add("brick", Constants.DEFULT_BRICK_COLOR);
            Colors.Add("background", Constants.BACKGROUND_COLOR);
        }
        public static void GetColorsFromIntent(Intent data)
        {
            Colors = new Hashtable();
            foreach (string key in Colors.Keys)
            {
                Colors.Add(key, IntToColorConvertor(data.GetIntExtra(key, 0)));
            }
        }
        public static void PutColorsInIntent(Intent data)
        {
            foreach (string key in Colors.Keys)
            {
                data.PutExtra(key, ((Color)Colors[key]).ToArgb());
            }
        }
        public static void SetColor(string key, int color)
        {
            if(Colors.ContainsKey(key))
            {
                Colors[key] = IntToColorConvertor(color);
            }
        }
        public static Color GetColor(string key)
        {
            if (Colors.ContainsKey(key))
            {
                return (Color)(Colors[key]);
            }
            return Constants.DEFULT_COLOR;
        }
        public static Color IntToColorConvertor(int color)
        {
            Color result = new Color();
            result.A = Convert.ToByte(0xff & color >> 24);
            result.R = Convert.ToByte(0xff & color >> 16);
            result.G = Convert.ToByte(0xff & color >> 8);
            result.B = Convert.ToByte(0xff & color);
            return result;

        }
    }
}