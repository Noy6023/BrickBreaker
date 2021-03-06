﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;
using Android.Gms.Tasks;
using Firebase.Firestore;

namespace BrickBreaker
{
    /// <summary>
    /// the main screen activity - home screen
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Android.Views.View.IOnClickListener, RadioGroup.IOnCheckedChangeListener, IOnSuccessListener, IOnFailureListener
    {
        private const int GAME_CODE = 0, TOP_SCORES_CODE = 1, CUSTOMIZE_CODE = 2;
        private Button btnStart, btnSaveSettings,btnBackSettings, btnSaveName, btnBackName, btnName;
        private LinearLayout llMain, llSettings, llHelp, llName;
        private EditText etName;
        private CheckBox cbMuteSound, cbMuteMusic;
        private TextView tvLastScore, tvMaxScore;
        private ImageView ivTiltPhone;
        private Color background;
        private Dialog settingsDialog, nameDialog, helpDialog;
        private ISharedPreferences sp;
        private RadioGroup rgBallSize, rgBrickSize, rgDifficulty;
        private RadioButton rbBallSmall, rbBallMedium, rbBallBig;
        private RadioButton rbBrickSmall, rbBrickMedium, rbBrickBig;
        private RadioButton rbEasy, rbHard;
        private Size lastBallChecked, lastBrickChecked;
        private Difficulty lastDifficultyChecked;
        private Score score;
        private bool isLightTheme;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            sp = this.GetSharedPreferences(Constants.SP_NAME, FileCreationMode.Private);
            ChangeTheme();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitViews();
        }

        /// <summary>
        /// inits the views in the main screen and loads the info from the previous runs
        /// </summary>
        private void InitViews()
        {
            llMain = FindViewById<LinearLayout>(Resource.Id.llMain);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            tvLastScore = FindViewById<TextView>(Resource.Id.tvLastScore);
            tvMaxScore = FindViewById<TextView>(Resource.Id.tvMaxScore);
            btnName = FindViewById<Button>(Resource.Id.btnName);
            btnName.SetOnClickListener(this);
            btnStart.SetOnClickListener(this);
            score = new Score();
            llMain.SetBackgroundColor(background);
            score.SetInfo(FileManager.Instance.LoadInfo(this));
            SetScoreInfo();
            AudioManager.IsSoundMuted = sp.GetBoolean(Constants.SOUND, false);
            AudioManager.IsMusicMuted = sp.GetBoolean(Constants.MUSIC, false);
            lastBallChecked = Size.Medium;
            lastBrickChecked = Size.Medium;
            lastDifficultyChecked = Difficulty.Easy;
            SetSizes();
            SetDifficulty();
            GetInfoFromFirestore();
        }

        /// <summary>
        /// changes the theme according to the background color
        /// </summary>
        private void ChangeTheme()
        {
            ColorManager.Instance.LoadColors(sp);
            background = (Color)ColorManager.Instance.Colors[ColorKey.Background];
            if (ColorManager.Instance.IsColorLight(background))
            {
                isLightTheme = true;
                SetTheme(Resource.Style.AppTheme);
            }
            else
            {
                isLightTheme = false;
                SetTheme(Resource.Style.AppThemeDark);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// sets the difficulty according to the last run or what the player chose
        /// </summary>
        private void SetDifficulty()
        {
            Difficulty difficulty = (Difficulty)sp.GetInt(Constants.DIFFICULTY, 0);
            lastDifficultyChecked = difficulty;
            if(difficulty == Difficulty.Hard)
                if (rbHard != null) rbHard.Checked = true;
            if(difficulty == Difficulty.Easy)
                if (rbEasy != null) rbEasy.Checked = true;
        }

        /// <summary>
        /// sets the sizes according to the last run or what the player chose
        /// </summary>
        private void SetSizes()
        {
            //get the size or get medium size by defult
            Size ballSize = (Size)sp.GetInt(Constants.BALL_SIZE, 1);
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
            Size brickSize = (Size)sp.GetInt(Constants.BRICK_SIZE, 1);
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

        /// <summary>
        /// creats a menu
        /// </summary>
        /// <param name="menu">the menu</param>
        /// <returns>true</returns>
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_menu, menu);
            return true;
        }

        /// <summary>
        /// handles menu actions
        /// </summary>
        /// <param name="item">the item of the menu that was selected</param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if(item.ItemId == Resource.Id.topScores)
            {
                Intent intent = new Intent(this, typeof(TopScoresActivity));
                intent = score.SetScoreInIntent(intent);
                StartActivityForResult(intent, TOP_SCORES_CODE);
                return true;
            }
            if (item.ItemId == Resource.Id.settings)
            {
                CreateSettingsDialog();
                return true;
            }
            if (item.ItemId == Resource.Id.customize)
            {
                Intent intent = new Intent(this, typeof(CustomizeActivity));
                StartActivityForResult(intent, CUSTOMIZE_CODE);
                return true;
            }
            if(item.ItemId == Resource.Id.help)
            {
                CreateHelpDialog();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// creates the setting dialog
        /// </summary>
        public void CreateSettingsDialog()
        {
            settingsDialog = new Dialog(this);
            settingsDialog.SetContentView(Resource.Layout.activity_settings);
            settingsDialog.SetTitle("Settings");
            settingsDialog.SetCancelable(true);
            llSettings = settingsDialog.FindViewById<LinearLayout>(Resource.Id.llSettings);
            llSettings.SetBackgroundColor(background);
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

        /// <summary>
        /// created the help dialog
        /// </summary>
        public void CreateHelpDialog()
        {
            helpDialog = new Dialog(this);
            helpDialog.SetContentView(Resource.Layout.activity_help);
            helpDialog.SetCancelable(true);
            llHelp = helpDialog.FindViewById<LinearLayout>(Resource.Id.llHelp);
            ivTiltPhone = helpDialog.FindViewById<ImageView>(Resource.Id.ivTiltPhone);
            llHelp.SetBackgroundColor(background);
            if(isLightTheme)
                ivTiltPhone.SetBackgroundResource(Resource.Drawable.tilt_phone);
            else
                ivTiltPhone.SetBackgroundResource(Resource.Drawable.white_tilt_phone);
            helpDialog.Show();
        }

        /// <summary>
        /// creates the name dialog
        /// </summary>
        public void CreateNameDialog()
        {
            nameDialog = new Dialog(this);
            nameDialog.SetContentView(Resource.Layout.activity_name);
            nameDialog.SetCancelable(true);
            llName = nameDialog.FindViewById<LinearLayout>(Resource.Id.llName);
            llName.SetBackgroundColor(background);
            etName = nameDialog.FindViewById<EditText>(Resource.Id.etName);
            btnSaveName = nameDialog.FindViewById<Button>(Resource.Id.btnSave);
            btnBackName = nameDialog.FindViewById<Button>(Resource.Id.btnBack);
            btnBackName.SetOnClickListener(this);
            btnSaveName.SetOnClickListener(this);
            nameDialog.Show();
        }

        /// <summary>
        /// handles a click event
        /// </summary>
        /// <param name="v">the view that was clicked</param>
        public void OnClick(View v)
        {
            if(v == btnStart)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra(Constants.DIFFICULTY, (int)lastDifficultyChecked);
                StartActivityForResult(intent, GAME_CODE);
            }
            if(v == btnName)
                CreateNameDialog();
            if(v == btnBackName)
                nameDialog.Dismiss();
            if(v == btnSaveName)
            {   
                score.Name = etName.Text;
                btnName.Text = score.Name;
                FireBaseData.Instance.SaveScoreToCollection(Constants.PLAYERS_COLLECTION, score);
                FileManager.Instance.SaveInfo('\n', score.GetInfo(), this);
                nameDialog.Dismiss();
            }
            if (v == btnBackSettings)
                if (btnBackSettings != null)
                    settingsDialog.Dismiss();
            if (v == btnSaveSettings)
            {
                var editor = sp.Edit();
                editor.PutBoolean(Constants.SOUND, cbMuteSound.Checked);
                editor.PutBoolean(Constants.MUSIC, cbMuteMusic.Checked);
                editor.Commit();
                AudioManager.IsSoundMuted = sp.GetBoolean(Constants.SOUND, false);
                AudioManager.IsMusicMuted = sp.GetBoolean(Constants.MUSIC, false);
                settingsDialog.Dismiss();
            }
        }

        /// <summary>
        /// the function that is being called after the game ended.
        /// it sets the last score and max score and saves them.
        /// </summary>
        /// <param name="requestCode">the screen we came from</param>
        /// <param name="resultCode">the result - ok/cancled</param>
        /// <param name="data">the intent with the information that was sent from the game activity</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == GAME_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.Extras != null)
                    {
                        score.LastValue = data.GetIntExtra(Constants.LAST_SCORE, 0);
                        score.ChangedScore();
                        SetScoreInfo();
                        FireBaseData.Instance.SaveScoreToCollection(Constants.PLAYERS_COLLECTION, score);
                        FileManager.Instance.SaveInfo('\n', score.GetInfo(), this);
                    }
                }
            }
            if(requestCode == CUSTOMIZE_CODE)
            {
                ColorManager.Instance.SaveColors(sp);
                Recreate();
            }
        }

        /// <summary>
        /// sets the text of the max score and last score according to the given parametrs.
        /// </summary>
        /// <param name="max">the max score to set</param>
        /// <param name="lastScore">the last score to set</param>
        public void SetScoreInfo()
        {
            btnName.Text = score.Name;
            tvLastScore.Text = "Last Score: " + score.LastValue.ToString();
            tvMaxScore.Text = "Highest Score: " + score.HighestValue.ToString();
        }

        /// <summary>
        /// handles a check change in the radio group
        /// </summary>
        /// <param name="group">the radio group</param>
        /// <param name="checkedId">the radio button the was checked</param>
        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            //create an editor
            var editor = sp.Edit();
            //check if the radio group is the ball size
            if (group == rgBallSize)
            {
                Size ballSize = Size.Medium; //set defult to medium
                //check which size was clicked and change it
                if(checkedId == Resource.Id.rbBallSmall)
                    ballSize = Size.Small;
                if (checkedId == Resource.Id.rbBallMedium)
                    ballSize = Size.Medium;
                if (checkedId == Resource.Id.rbBallBig)
                    ballSize = Size.Big;
                //insert the size to the editor
                editor.PutInt(Constants.BALL_SIZE, (int)ballSize);
            }
            //check if the radio group is the brick size
            if (group == rgBrickSize)
            {
                Size brickSize = Size.Medium; //set defult to medium
                //check which size was clicked and change it
                if (checkedId == Resource.Id.rbBrickSmall)
                    brickSize = Size.Small;
                if (checkedId == Resource.Id.rbBrickMedium)
                    brickSize = Size.Medium;
                if (checkedId == Resource.Id.rbBrickBig)
                    brickSize = Size.Big;
                //insert the size to the editor
                editor.PutInt(Constants.BRICK_SIZE, (int)brickSize);
            }
            //check if the radio group is the difficulty
            if (group == rgDifficulty)
            {
                Difficulty difficulty = Difficulty.Easy; //set defult to easy
                //check which difficulty was clicked and change it
                if (checkedId == Resource.Id.rbEasy)
                    difficulty = Difficulty.Easy;
                if (checkedId == Resource.Id.rbHard)
                    difficulty = Difficulty.Hard;
                //insert the difficulty to the editor
                editor.PutInt(Constants.DIFFICULTY, (int)difficulty);
            }
            //commit the changes and set them
            editor.Commit();
            SetSizes();
            SetDifficulty();
        }

        /// <summary>
        /// gets the score info from firestore
        /// </summary>
        private void GetInfoFromFirestore()
        {
            FireBaseData.Instance.GetDocument(Constants.PLAYERS_COLLECTION, score.Key.ToString()).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        /// <summary>
        /// sets the score if the document was found
        /// </summary>
        /// <param name="result">the result document that was found</param>
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            string name = snapshot.Exists() ? snapshot.Get(Constants.NAME).ToString() : score.Name;
            int highestScore = snapshot.Exists() ? int.Parse(snapshot.Get(Constants.SCORE).ToString()) : score.HighestValue;
            score = new Score(name, score.LastValue, highestScore, score.Key);
            SetScoreInfo();
        }

        /// <summary>
        /// handles a case when the document wasn't found
        /// </summary>
        /// <param name="e">the exception</param>
        public void OnFailure(Java.Lang.Exception e)
        {
            score.SetInfo(FileManager.Instance.LoadInfo(this));
            SetScoreInfo();
        }
    }
}