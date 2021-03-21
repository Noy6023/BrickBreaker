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
    /// <summary>
    /// manages the colors of the objects in the game
    /// </summary>
    public sealed class ColorManager
    {
        //using a singelton method so that there is only one instance of color manager
        private static readonly ColorManager instance = new ColorManager();
        public Hashtable Colors;
        private int length = Enum.GetNames(typeof(ColorKey)).Length;

        /// <summary>
        /// static constructor
        /// </summary>
        static ColorManager() //static constructor
        { }

        /// <summary>
        /// private constructor that inits the colors hashtable
        /// </summary>
        private ColorManager() //private constructor
        {
            InitColors();
        }

        /// <summary>
        /// gets the instance
        /// </summary>
        public static ColorManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// inits the color hashtable to the defult colors
        /// </summary>
        public void InitColors()
        {
            Colors = new Hashtable();
            Colors.Add(ColorKey.Ball, Constants.DEFULT_COLOR);
            Colors.Add(ColorKey.Bat, Constants.DEFULT_BAT_COLOR);
            Colors.Add(ColorKey.Brick, Constants.DEFULT_BRICK_COLOR);
            Colors.Add(ColorKey.Background, Constants.BACKGROUND_COLOR);
            Colors.Add(ColorKey.Text, Constants.DEFULT_COLOR);
        }

        /// <summary>
        /// sets a color
        /// </summary>
        /// <param name="key">the key of the color</param>
        /// <param name="color">the color int</param>
        public void SetColor(ColorKey key, int color)
        {
            if(Colors.ContainsKey(key))
                Colors[key] = IntToColorConvertor(color);
        }

        /// <summary>
        /// get a color
        /// </summary>
        /// <param name="key">the key of the color to get</param>
        /// <returns>the color or a defult color if there's no such key</returns>
        public Color GetColor(ColorKey key)
        {
            if (Colors.ContainsKey(key))
                return (Color)(Colors[key]);
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
            for (int i = 0; i < length; i++)
            {
                ColorKey key = (ColorKey)i;
                Color color = new Color();
                color.A = Convert.ToByte(255);
                color.R = Convert.ToByte(rand.Next(255));
                color.G = Convert.ToByte(rand.Next(255));
                color.B = Convert.ToByte(rand.Next(255));
                Colors[key] = color;
            }
        }

        /// <summary>
        /// saves the colors of the game to the shared preference
        /// </summary>
        /// <param name="sp">the shared preference</param>
        public void SaveColors(ISharedPreferences sp)
        {
            var editor = sp.Edit();
            for (int i = 0; i < length; i++)
            {
                ColorKey key = (ColorKey)i;
                editor.PutInt(key.ToString(), ((Color)Colors[key]).ToArgb());
            }
            editor.Commit();
        }

        /// <summary>
        /// loads the colors of the game from the shared preference
        /// </summary>
        /// <param name="sp">the shared preference</param>
        public void LoadColors(ISharedPreferences sp)
        {
            int current = 0;
            for (int i = 0; i < length; i++)
            {
                ColorKey key = (ColorKey)i;
                current = sp.GetInt(key.ToString(), ((Color)Colors[key]).ToArgb());
                Colors[key] = IntToColorConvertor(current);
            }
        }

        /// <summary>
        /// checks if the color is light or not
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true- the color is light.
        ///          false - the color is dark.</returns>
        public bool IsColorLight(Color color)
        {
            if (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722 > 255 / 2) return true;
            return false;
        }
    }
}