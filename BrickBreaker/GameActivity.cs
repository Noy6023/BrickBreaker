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
using static Android.OS.PowerManager;

namespace BrickBreaker
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, Android.Hardware.ISensorEventListener
    {
        SensorManager sensMan;
        Board board;
        Difficulty difficulty;
        private WakeLock wakeLock;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            difficulty = Difficulty.Easy;
            if (Intent.Extras != null)
            {
                difficulty = (Difficulty)Intent.GetIntExtra("difficulty", 1);
            }
            board = new Board(this, difficulty);
            SetContentView(board);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
            KeepScreenOn();
            board.T.Start();
        }
        private void KeepScreenOn()
        {
            PowerManager powerManager = (PowerManager)this.GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full, "My Lock");
            wakeLock.Acquire();
        }
        protected override void OnResume()
        {
            base.OnResume();
            wakeLock.Acquire();
            if (board != null)
            {
                board.Resume();
            }

        }
        protected override void OnStart()
        {
            base.OnStart();
            wakeLock.Acquire();
            if (board != null)
            {
                board.StartGame();
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            wakeLock.Release();
        }
        protected override void OnStop()
        {
            base.OnStop();
            wakeLock.Release();
        }
        protected override void OnPause()
        {
            base.OnPause();
            wakeLock.Release();
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
        }

        /// <summary>
        /// handle sensor change event
        /// </summary>
        /// <param name="e">the event</param>
        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type.Equals(SensorType.Accelerometer))
            {
                //if a phone movement is detected - move the bat
                board.MoveBatBySensor((int)e.Values[0]);
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