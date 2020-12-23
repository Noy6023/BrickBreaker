using Android.App;
using Android.Content;
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
    class ScoreAdapter : ArrayAdapter<Score>
    {
        readonly Android.Content.Context context;
        readonly List<Score> objects;

        public ScoreAdapter(Context context, int resorce, int textViewRecourceId, List<Score> objects) : base(context, resorce, textViewRecourceId, objects)
        {
            this.context = context;
            this.objects = objects;
        }

        public List<Score> GetList()
        {
            return this.objects;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return this.objects.Count; }
        }

        public Score this[int position]
        {
            get { return this.objects[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((FireStoreActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.activity_score, parent, false);
            TextView tvName = view.FindViewById<TextView>(Resource.Id.tvScoreName);
            TextView tvValue = view.FindViewById<TextView>(Resource.Id.tvScoreValue);
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