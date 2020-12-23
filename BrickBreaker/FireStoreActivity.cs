using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    [Activity(Label = "FireStoreActivity")]
    public class FireStoreActivity : AppCompatActivity, IOnCompleteListener, View.IOnClickListener, IOnSuccessListener
    {
        List<Score> scoreList;
        ScoreAdapter scoreAdapter;
        ListView lv;
        Button btnUpload;
        Score currentScore;
        TextView tvHighestScore;
        FireBaseData fd = new FireBaseData();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_top_scores);
            InitScoreList();
            InitViews();
            // Create your application here
        }

        private void InitViews()
        {
            lv = FindViewById<ListView>(Resource.Id.lv);
            scoreAdapter = new ScoreAdapter(this, 0, 0, scoreList);
            lv.Adapter = scoreAdapter;
            currentScore = GetScore();
            tvHighestScore = FindViewById<TextView>(Resource.Id.tvHighestScore);
            tvHighestScore.Text = "Your highest score is: " + currentScore.HighestValue.ToString();
            btnUpload = FindViewById<Button>(Resource.Id.btnUpload);
            btnUpload.SetOnClickListener(this);
        }
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
        private Score GetScore()
        {
            return new Score(Intent.GetStringExtra("Name"), Intent.GetIntExtra("LastValue", 0), Intent.GetIntExtra("HighestValue", 0), Intent.GetIntExtra("Key", 0));
        }

        private void InitScoreList()
        {
            scoreList = new List<Score>();
            FetchData("Players");
        }
        private void FetchData(string cName)
        {
            fd.firestore.Collection(cName).Get().AddOnSuccessListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private int GetInsertIndex(int value)
        {
            int i = 0;
            if(scoreList.Count == 0)
            {
                return i;
            }
            if (value > scoreList[i].HighestValue) return i;
            if (value < scoreList[scoreList.Count - 1].HighestValue) return scoreList.Count;
            foreach (Score s in scoreList)
            {
                if(i + 1 < scoreList.Count && value <= s.HighestValue && value >= scoreList[i+1].HighestValue)
                {
                    return i+1;
                }
                i++;
            }
            return i;
        }

        public void OnComplete(Task task)
        {
            string msg = "supposed to work";
            
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }
        private void AddDocument(Score score)
        {
            HashMap data = new HashMap();
            if(Intent.Extras != null)
            {
                data.Put("Name", score.Name);
                data.Put("Score", score.HighestValue);
                data.Put("Key", score.Key);
                fd.AddDocumentToCollection("Players", score.Key.ToString(), data);
                AddToList(score);
                btnUpload.Text = "Remove Yours";
            }
        }

        private void AddToList(Score score)
        {
            int i = GetInsertIndex(score.HighestValue);
            scoreList.Insert(i, score);
            scoreAdapter.NotifyDataSetChanged();
        }

        public void OnClick(View v)
        {
            if(v == btnUpload)
            {
                if(btnUpload.Text == "Upload Yours")
                {
                    AddDocument(currentScore);
                }
                else
                {
                    int index = GetIndexOf(currentScore);
                    fd.DeleteDocumentFromCollection("Players", currentScore.Key.ToString());
                    scoreList.RemoveAt(GetIndexOf(currentScore));
                    scoreAdapter.NotifyDataSetChanged();
                    btnUpload.Text = "Upload Yours";
                }
            }
        }
        private void UpdateScore(Score score)
        {
            foreach (Score s in scoreList)
            {
                if (s.Key == score.Key)
                {
                    s.HighestValue = score.HighestValue;
                    s.Name = score.Name;
                    fd.DeleteDocumentFromCollection("Players", score.Key.ToString());
                    AddDocument(score);
                    scoreList.RemoveAt(GetIndexOf(currentScore));
                    scoreAdapter.NotifyDataSetChanged();
                    return;
                }
            }
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            if(!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach(DocumentSnapshot item in documents)
                {
                    Score score = new Score();
                    score.Name = item.Get("Name").ToString();
                    score.HighestValue = int.Parse(item.Get("Score").ToString());
                    score.Key = int.Parse(item.Get("Key").ToString());
                    AddToList(score);

                }
                if (GetIndexOf(currentScore) >= 0) btnUpload.Text = "Remove Yours";
                UpdateScore(currentScore);
            }
        }
    }
}