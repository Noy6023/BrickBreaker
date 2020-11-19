using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Views;
using Android.Content;

namespace BrickBreaker
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Android.Views.View.IOnClickListener
    {
        Button btnStart;
        TextView tvScore;
        int max;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitView();
        }
        private void InitView()
        {
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            tvScore = FindViewById<TextView>(Resource.Id.tvScore);
            btnStart.SetOnClickListener(this);
            max = 0;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnClick(View v)
        {
            if(v.Id == btnStart.Id)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivityForResult(intent, 0);
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.Extras != null)
                    {
                        int lastScore = Int32.Parse(data.GetStringExtra("score"));
                        if (lastScore > max) max = lastScore;
                        tvScore.Text = "Score: " + lastScore.ToString() + "\nHighest Score: " + max.ToString();
                    }
                }
            }
        }
    }
}