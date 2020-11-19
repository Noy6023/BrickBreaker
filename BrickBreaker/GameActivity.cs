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
        SensorManager sensMan;
        Board board;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            board = new Board(this);
            SetContentView(board);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
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
                board.bat.velocity.x = (int)e.Values[0];
                Android.Util.Log.Debug("sh", "x=" + board.bat.velocity.x);
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