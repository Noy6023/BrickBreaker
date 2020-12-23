using Android.App;
using Android.Content;
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
    class Score
    {
        public string Name { get; set; }
        public int LastValue { get; set; }
        public int HighestValue { get; set; }
        public int Key { get; set; }
        public Score()
        {
            Name = "Player";
            LastValue = 0;
            HighestValue = 0;
            Key = new Random().Next(10000, 90000);
        }

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
        /// <param name="info"></param>
        public void SetInfo(string[] info)
        {
            if (info != null)
            {
                LastValue = Int32.Parse(info[0]);
                HighestValue = Int32.Parse(info[1]);
                Key = Int32.Parse(info[2]);
                Name = info[3];
            }
        }

        /// <summary>
        /// gets the current info and places it in an info array the will be saved 
        /// </summary>
        /// <returns></returns>
        public string[] GetInfo()
        {
            string[] info = new string[4];
            info[0] = LastValue.ToString();
            info[1] = HighestValue.ToString();
            info[2] = Key.ToString();
            info[3] = Name;
            return info;
        }
        public void ChangedScore()
        {
            if (LastValue > HighestValue)
            {
                HighestValue = LastValue;
            }
        }
        public void SetScoreInIntent(Intent intent)
        {
            intent.PutExtra("Name", Name);
            intent.PutExtra("LastValue", LastValue);
            intent.PutExtra("HighestValue", HighestValue);
            intent.PutExtra("Key", Key);
        }
    }
}