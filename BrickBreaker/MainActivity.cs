using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Views;
using Android.Content;
using Android.Graphics;
using Java.IO;
using System.IO;
using System.Text;
using System.Collections;

namespace BrickBreaker
{
    [Activity(Label = "", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Android.Views.View.IOnClickListener, RadioGroup.IOnCheckedChangeListener
    {
        Button btnStart;
        Button btnSaveSettings;
        Button btnBackSettings;
        Button btnSaveName;
        Button btnBackName;
        Button btnName;
        EditText etName;
        CheckBox cbMuteSound, cbMuteMusic;
        TextView tvLastScore;
        TextView tvMaxScore;
        int max, lastScore;
        Dialog settingsDialog, nameDialog, helpDialog;
        ISharedPreferences sp;
        RadioGroup rgBallSize, rgBrickSize, rgDifficulty;
        RadioButton rbBallSmall, rbBallMedium, rbBallBig;
        RadioButton rbBrickSmall, rbBrickMedium, rbBrickBig;
        RadioButton rbEasy, rbHard;
        Size lastBallChecked, lastBrickChecked;
        Difficulty lastDifficultyChecked;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitView();
        }
        private void InitView()
        {
            sp = this.GetSharedPreferences("Settings", FileCreationMode.Private);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            tvLastScore = FindViewById<TextView>(Resource.Id.tvLastScore);
            tvMaxScore = FindViewById<TextView>(Resource.Id.tvMaxScore);
            btnName = FindViewById<Button>(Resource.Id.btnName);
            btnName.SetOnClickListener(this);
            btnStart.SetOnClickListener(this);
            max = 0;
            lastScore = 0;
            AudioManager.IsSoundMuted = sp.GetBoolean("sound", false);
            AudioManager.IsMusicMuted = sp.GetBoolean("music", false);
            lastBallChecked = Size.Medium;
            lastBrickChecked = Size.Medium;
            lastDifficultyChecked = Difficulty.Easy;
            SetSizes();
            SetDifficulty();
            SetInfo(FileManager.Instance.LoadInfo(this));
        }
        private void SetInfo(string[] info)
        {
            if(info != null)
            {
                lastScore = Int32.Parse(info[0]);
                max = Int32.Parse(info[1]);
                btnName.Text = info[2];
                SetScoreInfo(max, lastScore);
            }
        }
        private string[] GetInfo()
        {
            string[] info = new string[3];
            info[0] = lastScore.ToString();
            info[1] = max.ToString();
            info[2] = btnName.Text;
            return info;
        }
        private void SetDifficulty()
        {
            Difficulty difficulty = (Difficulty)sp.GetInt("difficulty", 0);
            lastDifficultyChecked = difficulty;
            if(difficulty == Difficulty.Hard)
            {
                if (rbHard != null) rbHard.Checked = true;
            }
            else
            {
                if (rbEasy != null) rbEasy.Checked = true;
            }
        }
        private void SetSizes()
        {
            Size ballSize = (Size)sp.GetInt("ball size", 1);
            lastBallChecked = ballSize;
            if (ballSize == Size.Big)
            {
                Ball.Radius = Constants.BIG_BALL_RADIUS;
                if (rbBallBig != null) rbBallBig.Checked = true;
            }
            else if (ballSize == Size.Small)
            {
                Ball.Radius = Constants.SMALL_BALL_RADIUS;
                if(rbBallSmall != null) rbBallSmall.Checked = true;
            }
            else
            {
                Ball.Radius = Constants.MEDIUM_BALL_RADIUS;
                if (rbBallMedium != null) rbBallMedium.Checked = true;

            }
            Size brickSize = (Size)sp.GetInt("brick size", 1);
            lastBrickChecked = brickSize;
            if (brickSize == Size.Big)
            {
                Brick.Size = Constants.BRICK_BIG_SIZE;
                if (rbBrickBig != null) rbBrickBig.Checked = true;
            }
            else if (brickSize == Size.Small)
            {
                Brick.Size = Constants.BRICK_SMALL_SIZE;
                if (rbBrickSmall != null) rbBrickSmall.Checked = true;
            }
            else
            {
                Brick.Size = Constants.BRICK_MEDIUM_SIZE;
                if (rbBrickMedium != null) rbBrickMedium.Checked = true;
            }
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
            if (item.ItemId == Resource.Id.settings)
            {
                CreateSettingsDialog();
                return true;
            }
            if (item.ItemId == Resource.Id.customize)
            {
                Intent intent = new Intent(this, typeof(CustomizeActivity));
                StartActivityForResult(intent, 1);
                return true;
            }
            if(item.ItemId == Resource.Id.help)
            {
                CreateHelpDialog();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        public void CreateSettingsDialog()
        {
            settingsDialog = new Dialog(this);
            settingsDialog.SetContentView(Resource.Layout.activity_settings);
            settingsDialog.SetTitle("Settings");
            settingsDialog.SetCancelable(true);
            cbMuteSound = settingsDialog.FindViewById<CheckBox>(Resource.Id.cbMuteSound);
            cbMuteMusic = settingsDialog.FindViewById<CheckBox>(Resource.Id.cbMuteMusic);
            cbMuteSound.Checked = AudioManager.IsSoundMuted;
            cbMuteMusic.Checked = AudioManager.IsMusicMuted;
            rgBallSize = settingsDialog.FindViewById<RadioGroup>(Resource.Id.rgBallSize);
            rgBrickSize = settingsDialog.FindViewById<RadioGroup>(Resource.Id.rgBrickSize);
            rgDifficulty = settingsDialog.FindViewById<RadioGroup>(Resource.Id.rgDifficulty);
            rgBallSize.SetOnCheckedChangeListener(this);
            rgBrickSize.SetOnCheckedChangeListener(this);
            rgDifficulty.SetOnCheckedChangeListener(this);
            rbBallSmall = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBallSmall);
            rbBallMedium = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBallMedium);
            rbBallBig = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBallBig);
            rbBrickSmall = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBrickSmall);
            rbBrickMedium = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBrickMedium);
            rbBrickBig = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbBrickBig);
            rbEasy = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbEasy);
            rbHard = settingsDialog.FindViewById<RadioButton>(Resource.Id.rbHard);
            SetSizes();
            SetDifficulty();
            btnSaveSettings = settingsDialog.FindViewById<Button>(Resource.Id.btnSave);
            btnBackSettings = settingsDialog.FindViewById<Button>(Resource.Id.btnBack);
            btnBackSettings.SetOnClickListener(this);
            btnSaveSettings.SetOnClickListener(this);
            settingsDialog.Show();
        }
        public void CreateHelpDialog()
        {
            helpDialog = new Dialog(this);
            helpDialog.SetContentView(Resource.Layout.activity_help);
            helpDialog.SetCancelable(true);
            helpDialog.Show();
        }
        public void CreateNameDialog()
        {
            nameDialog = new Dialog(this);
            nameDialog.SetContentView(Resource.Layout.activity_name);
            nameDialog.SetCancelable(true);
            etName = nameDialog.FindViewById<EditText>(Resource.Id.etName);
            btnSaveName = nameDialog.FindViewById<Button>(Resource.Id.btnSave);
            btnBackName = nameDialog.FindViewById<Button>(Resource.Id.btnBack);
            btnBackName.SetOnClickListener(this);
            btnSaveName.SetOnClickListener(this);
            nameDialog.Show();
        }
        public void OnClick(View v)
        {
            if(v == btnStart)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("difficulty", (int)lastDifficultyChecked);
                StartActivityForResult(intent, 0);
            }
            if(v == btnName)
            {
                CreateNameDialog();
            }
            if(v == btnBackName)
            {
                nameDialog.Dismiss();
            }
            if(v == btnSaveName)
            {
                btnName.Text = etName.Text;
                FileManager.Instance.SaveInfo('\n', GetInfo(), this);
                nameDialog.Dismiss();
            }
            if(v == btnBackSettings)
            {
                if (btnBackSettings != null) settingsDialog.Dismiss();
            }
            if (v == btnSaveSettings)
            {
                var editor = sp.Edit();
                editor.PutBoolean("sound", cbMuteSound.Checked);
                editor.PutBoolean("music", cbMuteMusic.Checked);
                editor.Commit();
                AudioManager.IsSoundMuted = sp.GetBoolean("sound", false);
                AudioManager.IsMusicMuted = sp.GetBoolean("music", false);
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
                        lastScore = data.GetIntExtra("score", 0);
                        if (lastScore > max) max = lastScore;
                        SetScoreInfo(max, lastScore);
                        FileManager.Instance.SaveInfo('\n', GetInfo(), this);
                    }
                }
            }
        }
        public void SetScoreInfo(int max, int lastScore)
        {
            tvLastScore.Text = "Last Score: " + lastScore.ToString();
            tvMaxScore.Text = "Highest Score: " + max.ToString();
        }
        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            var editor = sp.Edit();
            if (group == rgBallSize)
            {
                Size ballSize = Size.Medium;
                if(checkedId == Resource.Id.rbBallSmall)
                {
                    ballSize = Size.Small;
                }
                if (checkedId == Resource.Id.rbBallMedium)
                {
                    ballSize = Size.Medium;
                }
                if (checkedId == Resource.Id.rbBallBig)
                {
                    ballSize = Size.Big;
                }
                editor.PutInt("ball size", (int)ballSize);
            }
            if (group == rgBrickSize)
            {
                Size brickSize = Size.Medium;
                if (checkedId == Resource.Id.rbBrickSmall)
                {
                    brickSize = Size.Small;
                }
                if (checkedId == Resource.Id.rbBrickMedium)
                {
                    brickSize = Size.Medium;
                }
                if (checkedId == Resource.Id.rbBrickBig)
                {
                    brickSize = Size.Big;
                }
                editor.PutInt("brick size", (int)brickSize);
            }
            if(group == rgDifficulty)
            {
                Difficulty difficulty = Difficulty.Easy;
                if (checkedId == Resource.Id.rbEasy)
                {
                    difficulty = Difficulty.Easy;
                }
                if (checkedId == Resource.Id.rbHard)
                {
                    difficulty = Difficulty.Hard;
                }
                editor.PutInt("difficulty", (int)difficulty);
            }
            editor.Commit();
            SetSizes();
            SetDifficulty();
        }
    }
}