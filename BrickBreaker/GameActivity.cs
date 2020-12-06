using System;
using System.Collections;
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
        public static Hashtable Colors;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            board = new Board(this, Colors);
            SetContentView(board);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
            board.T.Start();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (board != null)
            {
                board.Resume();
            }
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (board != null)
            {
                board.StartGame();
            }
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
            if (board != null)
            {
                board.Pause();
            }
        }
        public override void Finish()
        {

            base.Finish();
            board.ThreadRunning = false;
            while (true)
            {
                try
                {
                    board.T.Join();
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
                Difficulty difficulty = Difficulty.Easy;
                if (Intent.Extras != null)
                {
                    difficulty = (Difficulty)Intent.GetIntExtra("difficulty", 1);
                }
                board.MoveBatBySensor((int)e.Values[0], difficulty);
                if (board.HasLost)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("score", board.Score.GetScore());
                    SetResult(Result.Ok, intent);
                    Finish();
                }
                    
            }
        }

    }
}