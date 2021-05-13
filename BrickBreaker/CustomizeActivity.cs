using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace BrickBreaker
{
    /// <summary>
    /// the customize screen activity
    /// </summary>
    [Activity(Label = "Customize")]
    public class CustomizeActivity : AppCompatActivity, Android.Views.View.IOnClickListener
    {
        private const int BALL_REQUEST_CODE = 0, BAT_REQUEST_CODE = 1, BRICK_REQUEST_CODE = 2, BACKGROUND_REQUEST_CODE = 3;
        private Button btnBallColor, btnBatColor, btnBrickColor, btnBackgroundColor, btnRandom, btnDefult, btnBack;
        private TableLayout tlCustomize;
        private Color background;

        /// <summary>
        /// the on create function
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ChangeTheme();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_customize);
            InitViews();
        }

        /// <summary>
        /// inits the views
        /// </summary>
        private void InitViews()
        {
            tlCustomize = FindViewById<TableLayout>(Resource.Id.tlCustomize);
            tlCustomize.SetBackgroundColor(background);
            btnBallColor = FindViewById<Button>(Resource.Id.btnBallColor);
            btnBatColor = FindViewById<Button>(Resource.Id.btnBatColor);
            btnBrickColor = FindViewById<Button>(Resource.Id.btnBrickColor);
            btnBackgroundColor = FindViewById<Button>(Resource.Id.btnBackgroundColor);
            btnRandom = FindViewById<Button>(Resource.Id.btnRandom);
            btnDefult = FindViewById<Button>(Resource.Id.btnDefult);
            btnBack = FindViewById<Button>(Resource.Id.btnBack);
            ChangeButtonsColors();
            btnBallColor.SetOnClickListener(this);
            btnBatColor.SetOnClickListener(this);
            btnBrickColor.SetOnClickListener(this);
            btnBackgroundColor.SetOnClickListener(this);
            btnDefult.SetOnClickListener(this);
            btnRandom.SetOnClickListener(this);
            btnBack.SetOnClickListener(this);
        }

        /// <summary>
        /// changes the theme according to the background color
        /// </summary>
        public void ChangeTheme()
        {
            background = (Color)ColorManager.Instance.Colors[ColorKey.Background];

            if (ColorManager.Instance.IsColorLight(background))
                SetTheme(Resource.Style.AppTheme);
            else
                SetTheme(Resource.Style.AppThemeDark);
        }

        /// <summary>
        /// handles event click
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            if (v == btnBallColor)
                StartActivityForResult(intent, BALL_REQUEST_CODE);
            if (v == btnBatColor)
                StartActivityForResult(intent, BAT_REQUEST_CODE);
            if (v == btnBrickColor)
                StartActivityForResult(intent, BRICK_REQUEST_CODE);
            if (v == btnBackgroundColor)
                StartActivityForResult(intent, BACKGROUND_REQUEST_CODE);
            if(v == btnDefult)
            {
                ColorManager.Instance.InitColors();
                Recreate();
            }
            if (v == btnRandom)
            {
                ColorManager.Instance.RandomColors();
                Recreate();
            }
            if(v == btnBack)
                Finish();
        }

        /// <summary>
        /// changes the color of the buttons
        /// </summary>
        private void ChangeButtonsColors()
        {
            ColorDrawable ball = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Ball));
            ColorDrawable brick = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Brick));
            ColorDrawable bat = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Bat));
            ColorDrawable background = new ColorDrawable(ColorManager.Instance.GetColor(ColorKey.Background));
            btnBallColor.Background = ball;
            btnBrickColor.Background = brick;
            btnBatColor.Background = bat;
            btnBackgroundColor.Background = background;
        }

        /// <summary>
        /// gets the color that was chosen from the color picker
        /// </summary>
        /// <param name="requestCode">the view to change</param>
        /// <param name="resultCode">the result code - ok/cancled</param>
        /// <param name="data">the data that was sent in an intent</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == BALL_REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if(!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0); //get the color int that was chosen
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBallColor.Background = colorDrawable; //set the button color to it
                        ColorManager.Instance.SetColor(ColorKey.Ball, color); //update the color in the color manager
                    }
                }
            }
            if (requestCode == BAT_REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBatColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor(ColorKey.Bat, color);
                    }
                }
            }
            if (requestCode == BRICK_REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBrickColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor(ColorKey.Brick, color);
                    }
                }
            }

            if (requestCode == BACKGROUND_REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if (!data.Extras.IsEmpty)
                    {
                        int color = data.GetIntExtra("color", 0);
                        ColorDrawable colorDrawable = new ColorDrawable(ColorManager.Instance.IntToColorConvertor(color));
                        btnBackgroundColor.Background = colorDrawable;
                        ColorManager.Instance.SetColor(ColorKey.Background, color);
                        Recreate();
                    }
                }
            }
        }
    }
}