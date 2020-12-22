using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    [Activity(Label = "FireStoreActivity")]
    public class FireStoreActivity : AppCompatActivity, IOnCompleteListener
    {
        bool signInRequest;
        FireBaseData fd = new FireBaseData();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            AddDocument();
            // Create your application here
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnComplete(Task task)
        {
            string msg = "";
            
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }
        private void AddDocument()
        {
            Hashtable data = new Hashtable();
            data.Put("Name", "Noy");
            data.Put("Score", 88);
            fd.AddDocumentToCollection("Players", "", data);

        }
    }
}