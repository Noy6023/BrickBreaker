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
        Button btnSave;
        Button btnBack;
        CheckBox cbMuteSound, cbMuteMusic;
        TextView tvScore;
        int max;
        Dialog settingsDialog;
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
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (item.ItemId == Resource.Id.home)
            {
                Toast.MakeText(this, "you are in home", ToastLength.Long).Show();
                return true;
            }
            else if (item.ItemId == Resource.Id.settings)
            {
                createSettingsDialog();
                return true;
            }
            else if (item.ItemId == Resource.Id.play)
            {
                OnClick(btnStart);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        public void createSettingsDialog()
        {
            settingsDialog = new Dialog(this);
            settingsDialog.SetContentView(Resource.Layout.activity_settings);
            settingsDialog.SetTitle("Settings");
            settingsDialog.SetCancelable(true);
            cbMuteSound = settingsDialog.FindViewById<CheckBox>(Resource.Id.cbMuteSound);
            cbMuteMusic = settingsDialog.FindViewById<CheckBox>(Resource.Id.cbMuteMusic);
            btnSave = settingsDialog.FindViewById<Button>(Resource.Id.btnSave);
            btnBack = settingsDialog.FindViewById<Button>(Resource.Id.btnBack);
            btnBack.SetOnClickListener(this);
            btnSave.SetOnClickListener(this);
            settingsDialog.Show();
        }
        public void OnClick(View v)
        {
            if(v.Id == btnStart.Id)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivityForResult(intent, 0);
            }
            if(v == btnBack)
            {
                if (btnBack != null) settingsDialog.Dismiss();
            }
            if (v == btnSave)
            {
                PlayerManager.IsMusicMuted = cbMuteMusic.Checked;
                PlayerManager.IsSoundMuted = cbMuteSound.Checked;
                settingsDialog.Dismiss();
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