using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BrickBreaker
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, Android.Hardware.ISensorEventListener
    {
        TextView tvLevel;
        ImageView ivBall, ivBat;
        SensorManager sensMan;
        private float deltax;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_game);
            InitViews();
            // Create your application here
        }

        private void InitViews()
        {
            tvLevel = FindViewById<TextView>(Resource.Id.tvLevel);
            ivBall = FindViewById<ImageView>(Resource.Id.ivBall);
            ivBat = FindViewById<ImageView>(Resource.Id.ivBat);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
        }
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type.Equals(SensorType.Accelerometer))
            {
                deltax = e.Values[0];
                if (ivBat != null)
                {
                    ivBat.SetX(ivBat.GetX() - deltax * 3);
                }
                Android.Util.Log.Debug("sh", "x=" + deltax);
            }
        }
    }
}