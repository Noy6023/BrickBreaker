using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
    [Activity(Label = "CustomizeActivity")]
    public class CustomizeActivity : Activity, Android.Views.View.IOnClickListener
    {
        Button btnBallColor;
        Button btnBatColor;
        Button btnBrickColor;
        Button btnBackgroundColor;
        Button btnApply;
        //Hashtable colors;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_customize);
            // Create your application here
            InitViews();
        }

        private void InitViews()
        {
            //colors = new Hashtable();
            btnBallColor = FindViewById<Button>(Resource.Id.btnBallColor);
            btnBatColor = FindViewById<Button>(Resource.Id.btnBatColor);
            btnBrickColor = FindViewById<Button>(Resource.Id.btnBrickColor);
            btnBackgroundColor = FindViewById<Button>(Resource.Id.btnBackgroundColor);
            btnApply = FindViewById<Button>(Resource.Id.btnApply);
            btnBallColor.SetOnClickListener(this);
            btnBatColor.SetOnClickListener(this);
            btnBrickColor.SetOnClickListener(this);
            btnBackgroundColor.SetOnClickListener(this);
            btnApply.SetOnClickListener(this);
        }
        public void OnClick(View v)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            if (v == btnBallColor)
            {
                StartActivityForResult(intent, 0);
            }
            if (v == btnBatColor)
            {
                StartActivityForResult(intent, 1);
            }
            if (v == btnBrickColor)
            {
                StartActivityForResult(intent, 2);
            }
            if (v == btnBackgroundColor)
            {
                StartActivityForResult(intent, 3);
            }
            if(v == btnApply)
            {
                SetResult(Result.Ok);
                Finish();
            }
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    if(!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBallColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor("ball", color);
                    }
                }
            }
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBatColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor("bat", color);
                    }
                }
            }
            if (requestCode == 2)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBrickColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor("brick", color);
                    }
                }
            }

            if (requestCode == 3)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBackgroundColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor("background", color);
                    }
                }
            }
        }

    }
}