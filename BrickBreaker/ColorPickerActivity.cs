﻿using Android.App;
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
    [Activity(Label = "ColorPicker")]
    public class ColorPickerActivity : Activity, Android.Views.View.IOnClickListener
    {
        private ColorPicker picker;
        private SVBar svBar;
        private OpacityBar opacityBar;
        private Button btnApply;
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
            btnApply = FindViewById<Button>(Resource.Id.btnApply);
            picker.AddSVBar(svBar);
            picker.AddOpacityBar(opacityBar);
            btnApply.SetOnClickListener(this);
        }

        /// <summary>
        /// handles a click event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            if(btnApply == v)
            {
                picker.OldCenterColor = picker.Color;
                Intent intent = new Intent();
                intent.PutExtra("color", picker.Color);
                SetResult(Result.Ok, intent);
                Finish();
            }
        }
    }
}