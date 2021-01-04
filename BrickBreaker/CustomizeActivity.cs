using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
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
    /// the customize screen activity
    /// </summary>
    [Activity(Label = "Customize")]
    public class CustomizeActivity : AppCompatActivity, Android.Views.View.IOnClickListener
    {
        Button btnBallColor;
        Button btnBatColor;
        Button btnBrickColor;
        Button btnBackgroundColor;
        Button btnRandom;
        Button btnDefult;
        Button btnBack;
        //Hashtable colors;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_customize);
            // Create your application here
            InitViews();
        }

        /// <summary>
        /// inits the views
        /// </summary>
        private void InitViews()
        {
            btnBallColor = FindViewById<Button>(Resource.Id.btnBallColor);
            btnBatColor = FindViewById<Button>(Resource.Id.btnBatColor);
            btnBrickColor = FindViewById<Button>(Resource.Id.btnBrickColor);
            btnBackgroundColor = FindViewById<Button>(Resource.Id.btnBackgroundColor);
            btnRandom = FindViewById<Button>(Resource.Id.btnRandom);
            btnDefult = FindViewById<Button>(Resource.Id.btnDefult);
            btnBack = FindViewById<Button>(Resource.Id.btnBack);
            ChangeButtonsColors();
            btnBallColor.SetOnClickListener(this);
            btnBatColor.SetOnClickListener(this);
            btnBrickColor.SetOnClickListener(this);
            btnBackgroundColor.SetOnClickListener(this);
            btnDefult.SetOnClickListener(this);
            btnRandom.SetOnClickListener(this);
            btnBack.SetOnClickListener(this);
        }

        /// <summary>
        /// handles event click
        /// </summary>
        /// <param name="v"></param>
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
            if(v == btnDefult)
            {
                ColorManager.Instance.InitColors();
                ChangeButtonsColors();
            }
            if(v == btnRandom)
            {
                ColorManager.Instance.RandomColors();
                ChangeButtonsColors();
            }
            if(v == btnBack)
            {
                Finish();
            }
        }
        /// <summary>
        /// changes the color of the buttons
        /// </summary>
        private void ChangeButtonsColors()
        {
            ColorDrawable ball = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Ball));
            ColorDrawable brick = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Brick));
            ColorDrawable bat = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Bat));
            ColorDrawable background = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Background));
            btnBallColor.Background = ball;
            btnBrickColor.Background = brick;
            btnBatColor.Background = bat;
            btnBackgroundColor.Background = background;
        }

        /// <summary>
        /// gets the color that was chosen from the color picker
        /// </summary>
        /// <param name="requestCode">the view to change</param>
        /// <param name="resultCode">the result code - ok/cancled</param>
        /// <param name="data">the data that was sent in an intent</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    if(!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0); //get the color int that was chosen
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBallColor.Background = colorDrawable; //set the button color to it
                        ColorManager.Instance.SetColor(ColorKey.Ball, color); //update the color in the color manager
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
                        ColorManager.Instance.SetColor(ColorKey.Bat, color);
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
                        ColorManager.Instance.SetColor(ColorKey.Brick, color);
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
                        ColorManager.Instance.SetColor(ColorKey.Background, color);
                    }
                }
            }
        }

    }
}