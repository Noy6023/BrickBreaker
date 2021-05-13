using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Java.Lang;
using static Android.OS.PowerManager;

namespace BrickBreaker
{
    /// <summary>
    /// the game screen activity
    /// </summary>
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, Android.Hardware.ISensorEventListener
    {
        private SensorManager sensMan;
        private Board board;
        private Difficulty difficulty;
        private WakeLock wakeLock;

        /// <summary>
        /// the on create function
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            difficulty = Difficulty.Easy;
            if (Intent.Extras != null)
                difficulty = (Difficulty)Intent.GetIntExtra("difficulty", 1);
            board = new Board(this, difficulty);
            SetContentView(board);
            sensMan = (SensorManager)GetSystemService(Context.SensorService);
            Sensor sen = sensMan.GetDefaultSensor(SensorType.Accelerometer);
            sensMan.RegisterListener(this, sen, Android.Hardware.SensorDelay.Game);
            KeepScreenOn();
            //board.T.Start();
        }

        /// <summary>
        /// Keeps the screen on while in the game
        /// </summary>
        private void KeepScreenOn()
        {
            PowerManager powerManager = (PowerManager)this.GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full, "My Lock");
            wakeLock.Acquire();
        }

        /// <summary>
        /// on resume function - handles resume
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            wakeLock.Acquire();
            if (board != null)
                board.Resume();
        }

        /// <summary>
        /// on start function - handles start
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
            wakeLock.Acquire();
            if (board != null)
                board.StartGame();
        }

        /// <summary>
        /// on destroy function - handles destroy
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            wakeLock.Release();
        }

        /// <summary>
        /// on stop function - handles stop
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            wakeLock.Release();
        }

        /// <summary>
        /// on pause function - handle pause
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            wakeLock.Release();
            if (board != null)
                board.Pause();
        }
        
        /// <summary>
        /// called when the game is finished
        /// </summary>
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
                }
                break;
            }
        }

        /// <summary>
        /// handle sensor accuracy change event
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="accuracy"></param>
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        { }

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