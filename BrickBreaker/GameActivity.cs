﻿using System;
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
using Java.Lang;

namespace BrickBreaker
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, Android.Hardware.ISensorEventListener
    {
        SensorManager sensMan;
        Board board;
        bool userAskBack = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            board = new Board(this);
            SetContentView(board);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
            board.t.Start();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (board != null)
            {
                board.resume();
            }
        }

        protected override void OnStart()
        {
            base.OnStart(); ;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnPause()
        {
            base.OnPause();
            if (userAskBack)
            {
            }
            else if (board != null)
            {
                board.pause();
            }
        }
        public override void Finish()
        {

            base.Finish();
            userAskBack = true;
            board.threadRunning = false;
            while (true)
            {
                try
                {
                    board.t.Join();
                }
                catch (InterruptedException e)
                {
                    //Toast.MakeText(this,"some problem happened",ToastLength.Long).Show();
                }
                break;
            }
        }
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //we dont want anything to happen here so it's empty
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type.Equals(SensorType.Accelerometer))
            {
                //if a phone movement is detected - move the bat;
                board.MoveBatBySensor((int)e.Values[0]);
                if (board.hasLost)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("score", board.score.ToString());
                    SetResult(Result.Ok, intent);
                    Finish();
                }
                    
            }
        }

    }
}