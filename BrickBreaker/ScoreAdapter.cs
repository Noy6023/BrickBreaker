using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context">the context</param>
        /// <param name="resorce">the resource</param>
        /// <param name="textViewRecourceId">the id</param>
        /// <param name="objects">the objects list</param>
        /// <param name="background">the background color</param>
        public ScoreAdapter(Context context, int resorce, int textViewRecourceId, List<Score> objects, Color background) : base(context, resorce, textViewRecourceId, objects)
        {
            this.context = context;
            this.objects = objects;
            this.background = background;
        }

        /// <summary>
        /// gets the length of the list
        /// </summary>
        public override int Count
        {
            get { return this.objects.Count; }
        }

        /// <summary>
        /// gets the view
        /// </summary>
        /// <param name="position">the position in the list</param>
        /// <param name="convertView">the view</param>
        /// <param name="parent">the parent</param>
        /// <returns>the view</returns>
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

