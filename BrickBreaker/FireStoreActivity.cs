﻿using Android.App;
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
    [Activity(Label = "Top Scores")]
    public class FireStoreActivity : AppCompatActivity, View.IOnClickListener, IOnSuccessListener
    {
        List<Score> scoreList;
        ScoreAdapter scoreAdapter;
        ListView lv;
        Button btnUpload;
        Score currentScore;
        TextView tvName;
        TextView tvHighestScore;
        FireBaseData fd = new FireBaseData();
        string uploadText;
        string removeText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
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
            lv = FindViewById<ListView>(Resource.Id.lv);
            scoreAdapter = new ScoreAdapter(this, 0, 0, scoreList);
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
            return new Score(Intent.GetStringExtra("Name"), Intent.GetIntExtra("LastValue", 0), Intent.GetIntExtra("HighestValue", 0), Intent.GetIntExtra("Key", 0));
        }

        /// <summary>
        /// inits the score list from the database
        /// </summary>
        private void InitScoreList()
        {
            scoreList = new List<Score>();
            FetchData("Players");
        }
        /// <summary>
        /// trues to get the collection from the database
        /// </summary>
        /// <param name="cName"></param>
        private void FetchData(string cName)
        {
            fd.GetCollection(cName).AddOnSuccessListener(this);
        }

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

        /// <summary>
        /// Puts fields of score in hashtable, adds the data to the collection and updates the list view
        /// </summary>
        /// <param name="score">the score to add</param>
        private void AddDocument(Score score)
        {
            Hashtable data = new Hashtable();
            if(Intent.Extras != null)
            {
                data.Put("Name", score.Name);
                data.Put("Score", score.HighestValue);
                data.Put("Key", score.Key);
                fd.AddDocumentToCollection("Players", score.Key.ToString(), data);
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
                {
                    AddDocument(currentScore);
                }
                else
                {
                    int index = GetIndexOf(currentScore);
                    fd.DeleteDocumentFromCollection("Players", currentScore.Key.ToString());
                    scoreList.RemoveAt(GetIndexOf(currentScore));
                    scoreAdapter.NotifyDataSetChanged();
                    btnUpload.Text = uploadText;
                }
            }
        }

        /// <summary>
        /// updates the existing score if it already exists in the list
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScore(Score score, Score s)
        {
            s.HighestValue = score.HighestValue;
            s.Name = score.Name;
            fd.DeleteDocumentFromCollection("Players", score.Key.ToString());
            AddDocument(score);
            scoreList.RemoveAt(GetIndexOf(currentScore));
            scoreAdapter.NotifyDataSetChanged();
            return;
        }

        /// <summary>
        /// gets the collection and placed the documents data in the score list
        /// </summary>
        /// <param name="result">the snapshot result</param>
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
                    if(score.Key == currentScore.Key)
                    {
                        UpdateScore(currentScore, score);
                    }
                    AddToList(score);

                }
                if (GetIndexOf(currentScore) >= 0) btnUpload.Text = removeText;
            }
        }
    }
}