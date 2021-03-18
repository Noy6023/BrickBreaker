﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
    /// <summary>
    /// the adapter of the score list view
    /// </summary>
    class ScoreAdapter : ArrayAdapter<Score>
    {
        readonly Android.Content.Context context;
        readonly List<Score> objects;
        Color background; 

        public ScoreAdapter(Context context, int resorce, int textViewRecourceId, List<Score> objects, Color background) : base(context, resorce, textViewRecourceId, objects)
        {
            this.context = context;
            this.objects = objects;
            this.background = background;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((FireStoreActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.activity_score, parent, false);
            TextView tvName = view.FindViewById<TextView>(Resource.Id.tvScoreName);
            TextView tvValue = view.FindViewById<TextView>(Resource.Id.tvScoreValue);
            view.SetBackgroundColor(background);
            Score temp = objects[position];
            if (temp != null)
            {
                tvName.Text = temp.Name;
                tvValue.Text = temp.HighestValue.ToString();
            }
            return view;
        }

    }
}