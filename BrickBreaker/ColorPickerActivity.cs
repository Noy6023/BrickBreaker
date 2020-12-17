using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LarsWerkman.HoloColorPicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    [Activity(Label = "ColorPickerActivity")]
    public class ColorPickerActivity : Activity, Android.Views.View.IOnClickListener
    {
        private ColorPicker picker;
        private SVBar svBar;
        private OpacityBar opacityBar;
        private Button button;
        private Button btnApply;
        private TextView text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_color_picker);
            InitViews();
            // Create your application here
        }

        /// <summary>
        /// inits the views
        /// </summary>
        private void InitViews()
        {
            picker = FindViewById<ColorPicker>(Resource.Id.picker);
            svBar = FindViewById<SVBar>(Resource.Id.svbar);
            opacityBar = FindViewById<OpacityBar>(Resource.Id.opacitybar);
            button = FindViewById<Button>(Resource.Id.button1);
            text = FindViewById<TextView>(Resource.Id.textView1);
            btnApply = FindViewById<Button>(Resource.Id.btnApply);
            picker.AddSVBar(svBar);
            picker.AddOpacityBar(opacityBar);
            button.SetOnClickListener(this);
            btnApply.SetOnClickListener(this);
        }

        /// <summary>
        /// handles a click event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            if(button == v)
            {
                text.Text = picker.Color.ToString();
                picker.OldCenterColor = picker.Color;
            }
            if(btnApply == v)
            {
                text.Text = picker.Color.ToString();
                picker.OldCenterColor = picker.Color;
                Intent intent = new Intent();
                intent.PutExtra("color", picker.Color);
                SetResult(Result.Ok, intent);
                Finish();
            }
        }
    }
}