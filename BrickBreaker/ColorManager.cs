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
    public sealed class ColorManager
    {
        //using a singelton method so that there is only one instance of color manager
        private static readonly ColorManager instance = new ColorManager();
        public Hashtable Colors;
        public static string[] Keys = { "ball", "bat", "brick", "background" };
        static ColorManager() //static constructor
        {

        }
        private ColorManager() //private constructor
        {
            InitColors();
        }

        public static ColorManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// inits the color hashtable to the defult colors
        /// </summary>
        public void InitColors()
        {
            Colors = new Hashtable();
            Colors.Add("ball", Constants.DEFULT_COLOR);
            Colors.Add("bat", Constants.DEFULT_BAT_COLOR);
            Colors.Add("brick", Constants.DEFULT_BRICK_COLOR);
            Colors.Add("background", Constants.BACKGROUND_COLOR);
        }

        public void GetColorsFromIntent(Intent data)
        {
            Colors = new Hashtable();
            foreach (string key in Keys)
            {
                Colors.Add(key, IntToColorConvertor(data.GetIntExtra(key, 0)));
            }
        }
        public void PutColorsInIntent(Intent data)
        {
            foreach (string key in Keys)
            {
                data.PutExtra(key, ((Color)Colors[key]).ToArgb());
            }
        }

        /// <summary>
        /// sets a color
        /// </summary>
        /// <param name="key">the key of the color</param>
        /// <param name="color">the color int</param>
        public void SetColor(string key, int color)
        {
            if(Colors.ContainsKey(key))
            {
                Colors[key] = IntToColorConvertor(color);
            }
        }

        /// <summary>
        /// get a color
        /// </summary>
        /// <param name="key">the key of the color to get</param>
        /// <returns>the color or a defult color if there's no such key</returns>
        public Color GetColor(string key)
        {
            if (Colors.ContainsKey(key))
            {
                return (Color)(Colors[key]);
            }
            return Constants.DEFULT_COLOR;
        }

        /// <summary>
        /// converts an int representation of a color to a color
        /// </summary>
        /// <param name="color">the given int color</param>
        /// <returns>the color</returns>
        public Color IntToColorConvertor(int color)
        {
            Color result = new Color();
            result.A = Convert.ToByte(0xff & color >> 24);
            result.R = Convert.ToByte(0xff & color >> 16);
            result.G = Convert.ToByte(0xff & color >> 8);
            result.B = Convert.ToByte(0xff & color);
            return result;

        }

        /// <summary>
        /// randomize the colors
        /// </summary>
        public void RandomColors()
        {
            Random rand = new Random();
            foreach (string key in Keys)
            {
                Color color = new Color();
                color.A = Convert.ToByte(255);
                color.R = Convert.ToByte(rand.Next(255));
                color.G = Convert.ToByte(rand.Next(255));
                color.B = Convert.ToByte(rand.Next(255));
                Colors[key] = color;
            }
        }
    }
}