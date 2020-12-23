using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    class FireBaseData
    {
        public FirebaseFirestore firestore { get; set; }
        private FirebaseAuth auth;
        private FirebaseApp app;

        public FireBaseData()
        {
            app = FirebaseApp.InitializeApp(Application.Context);
            if (app is null)
            {
                FirebaseOptions options = GetMyOptions();
                app = FirebaseApp.InitializeApp(Application.Context, options);
            }
            firestore = FirebaseFirestore.GetInstance(app);
            auth = FirebaseAuth.Instance;
        }

        private FirebaseOptions GetMyOptions()
        {
            return new FirebaseOptions.Builder()
                    .SetProjectId("brickbreaker-6c644")
                    .SetApplicationId("brickbreaker-6c644")
                    .SetApiKey("AIzaSyABeKz9zLg-OTnfDK6mtyVkUVeIRJTatVM")
                    .SetDatabaseUrl("https://brickbreaker-6c644-default-rtdb.europe-west1.firebasedatabase.app")
                    .SetStorageBucket("brickbreaker-6c644.appspot.com")
                    .Build();
        }

        public Task CreateUser(string email, string password)
        {
            return auth.CreateUserWithEmailAndPassword(email, password);
        }

        public Task SignIn(string email, string password)
        {
            return auth.SignInWithEmailAndPassword(email, password);
        }

        
        public void AddDocumentToCollection(string cName, string dName, HashMap hmFields)
        {
            DocumentReference cr;
            if (dName == null)
                cr = firestore.Collection(cName).Document();
            else
                cr = firestore.Collection(cName).Document(dName);
            cr.Set(hmFields);
        }
        public void DeleteDocumentFromCollection(string cName, string dName)
        {
            
            DocumentReference cr = firestore.Collection(cName).Document(dName);
            cr.Delete();
        }
        public void FindDocumentCollection(string cName, Score score)
        {
            //DocumentReference cr = firestore.Collection(cName);
            
        }
    }
}