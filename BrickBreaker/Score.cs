﻿using Android.Content;
using System;

namespace BrickBreaker
{
    /// <summary>
    /// a score class that holds the name of the player, last score, highest score and special key
    /// </summary>
    public class Score
    {
        public string Name { get; set; }
        public int LastValue { get; set; }
        public int HighestValue { get; set; }
        public int Key { get; set; }
        /// <summary>
        /// the defult constructor
        /// </summary>
        public Score()
        {
            Name = Constants.DEFULT_NAME;
            LastValue = 0;
            HighestValue = 0;
            Key = new Random().Next(10000, 100000);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="lastValue">the value</param>
        /// <param name="highestValue">the highest value</param>
        /// <param name="key">the key</param>
        public Score(string name, int lastValue, int highestValue, int key)
        {
            this.Name = name;
            this.LastValue = lastValue;
            this.HighestValue = highestValue;
            this.Key = key;
        }

        /// <summary>
        /// sets the init info from the info array that contains the info from previous runs
        /// </summary>
        /// <param name="info">the info array to take the data from</param>
        public void SetInfo(string[] info)
        {
            if (info != null)
            {
                LastValue = Int32.Parse(info[0]);
                HighestValue = Int32.Parse(info[1]);
                Key = int.Parse(info[2]);
                Name = info[3];
            }
        }

        /// <summary>
        /// gets the current info and places it in an info array the will be saved 
        /// </summary>
        /// <returns>the info array</returns>
        public string[] GetInfo()
        {
            string[] info = new string[4];
            info[0] = LastValue.ToString();
            info[1] = HighestValue.ToString();
            info[2] = Key.ToString();
            info[3] = Name;
            return info;
        }

        /// <summary>
        /// check if the last score is greater than the highest and update to highest incase it is
        /// </summary>
        public void ChangedScore()
        {
            if (LastValue > HighestValue)
                HighestValue = LastValue;
        }

        /// <summary>
        /// sets to score fields in a given intent
        /// </summary>
        /// <param name="intent">the intent to update</param>
        /// <returns>the intent</returns>
        public Intent SetScoreInIntent(Intent intent)
        {
            intent.PutExtra(Constants.NAME, Name);
            intent.PutExtra(Constants.LAST_VALUE, LastValue);
            intent.PutExtra(Constants.HIGHEST_VALUE, HighestValue);
            intent.PutExtra(Constants.KEY, Key);
            return intent;
        }
    }
}