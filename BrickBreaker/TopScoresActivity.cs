using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using System.Collections.Generic;

namespace BrickBreaker
{
    /// <summary>
    /// the firestore activity - top scores screen
    /// </summary>
    [Activity(Label = "Top Scores")]
    public class TopScoresActivity : AppCompatActivity, View.IOnClickListener, Firebase.Firestore.IEventListener
    {
        private List<Score> scoreList;
        private ScoreAdapter scoreAdapter;
        private ListView lv;
        private Button btnUpload;
        private Score currentScore;
        private TextView tvName, tvHighestScore;
        private LinearLayout llTopScores;
        private Color background;
        private readonly FireBaseData fd = FireBaseData.Instance;
        private string uploadText, removeText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ChangeTheme();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_top_scores);
            InitScoreList();
            InitViews();
        }

        /// <summary>
        /// inits the views in the activity
        /// </summary>
        private void InitViews()
        {
            llTopScores = FindViewById<LinearLayout>(Resource.Id.llTopScores);
            lv = FindViewById<ListView>(Resource.Id.lv);
            llTopScores.SetBackgroundColor(background);
            scoreAdapter = new ScoreAdapter(this, 0, 0, scoreList, background);
            lv.Adapter = scoreAdapter;
            currentScore = GetScore();
            uploadText = "Upload Yours";
            removeText = "Remove Yours";
            tvName = FindViewById<TextView>(Resource.Id.tvName);
            tvName.Text = "Your nickname is: " + currentScore.Name;
            tvHighestScore = FindViewById<TextView>(Resource.Id.tvHighestScore);
            tvHighestScore.Text = "Your highest score is: " + currentScore.HighestValue.ToString();
            btnUpload = FindViewById<Button>(Resource.Id.btnUpload);
            btnUpload.SetOnClickListener(this);
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
        /// gets the index of the score in the score list
        /// </summary>
        /// <param name="score">the score to find</param>
        /// <returns>the index of the score in the score list</returns>
        private int GetIndexOf(Score score)
        {
            int i = 0;
            foreach(Score s in scoreList)
            {
                if (s.Key == score.Key) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// gets the current user score from the intent (from Main Activity)
        /// </summary>
        /// <returns>the score</returns>
        private Score GetScore()
        {
            return new Score(Intent.GetStringExtra(Constants.NAME), Intent.GetIntExtra(Constants.LAST_VALUE, 0), Intent.GetIntExtra(Constants.HIGHEST_VALUE, 0), Intent.GetIntExtra(Constants.KEY, 0));
        }

        /// <summary>
        /// inits the score list from the database
        /// </summary>
        private void InitScoreList()
        {
            scoreList = new List<Score>();
            FetchData(Constants.PLAYERS_COLLECTION);
        }

        /// <summary>
        /// trues to get the collection from the database
        /// </summary>
        /// <param name="cName"></param>
        private void FetchData(string cName)
        {
            fd.AddSnapshotListener(cName, this);
        }

        /// <summary>
        /// Resuests permission
        /// </summary>
        /// <param name="requestCode">request code</param>
        /// <param name="permissions">the permissions</param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// finds a suitable index to put the new value in so that it remains descending
        /// </summary>
        /// <param name="value">the value to insert</param>
        /// <returns>the right index to insert in</returns>
        private int GetInsertIndex(int value)
        {
            int i = 0;
            if(scoreList.Count == 0)
                return i;
            if (value > scoreList[i].HighestValue) return i;
            if (value < scoreList[scoreList.Count - 1].HighestValue) return scoreList.Count;
            foreach (Score s in scoreList)
            {
                if(i + 1 < scoreList.Count && value <= s.HighestValue && value >= scoreList[i+1].HighestValue)
                    return i+1;
                i++;
            }
            return i;
        }

        /// <summary>
        /// Puts fields of score in hashtable, adds the data to the collection and updates the list view
        /// </summary>
        /// <param name="score">the score to add</param>
        private void AddDocument(Score score)
        {
            Hashtable data = new Hashtable();
            if(Intent.Extras != null)
            {
                data.Put(Constants.NAME, score.Name);
                data.Put(Constants.SCORE, score.HighestValue);
                data.Put(Constants.KEY, score.Key);
                fd.AddDocumentToCollection(Constants.PLAYERS_COLLECTION, score.Key.ToString(), data);
                AddToList(score);
                btnUpload.Text = removeText;
            }
        }

        /// <summary>
        /// adds a score to the sorted list in the right place
        /// </summary>
        /// <param name="score">the score to add</param>
        private void AddToList(Score score)
        {
            int i = GetInsertIndex(score.HighestValue);
            scoreList.Insert(i, score);
            scoreAdapter.NotifyDataSetChanged();
        }

        /// <summary>
        /// handles click event
        /// </summary>
        /// <param name="v"></param>
        public void OnClick(View v)
        {
            if(v == btnUpload)
            {
                if(btnUpload.Text == uploadText)
                    AddDocument(currentScore);
                else
                {
                    int index = GetIndexOf(currentScore);
                    if(index >= 0)
                        scoreList.RemoveAt(index);
                    scoreAdapter.NotifyDataSetChanged();
                    btnUpload.Text = uploadText;
                    fd.DeleteDocumentFromCollection(Constants.PLAYERS_COLLECTION, currentScore.Key.ToString());
                }
            }
        }

        /// <summary>
        /// gets the collection and placed the documents data in the score list
        /// </summary>
        /// <param name="value">the snapshot</param>
        /// <param name="error">an error</param>
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)value;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                scoreList.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    Score score = new Score();
                    score.Name = item.Get(Constants.NAME).ToString();
                    score.HighestValue = int.Parse(item.Get(Constants.SCORE).ToString());
                    score.Key = int.Parse(item.Get(Constants.KEY).ToString());
                    if (score.Key == currentScore.Key) btnUpload.Text = removeText;
                    AddToList(score);
                }
            }
        }
    }
}