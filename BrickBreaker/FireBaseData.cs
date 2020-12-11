using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    class FireBaseData
    {
        private FirebaseFirestore firestore;
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
                    .SetProjectId("brickbreaker-95b6c")
                    .SetApplicationId("brickbreaker-95b6c")
                    .SetApiKey("AIzaSyD3b1eF2D5MG7CPBNJ0ckzy0Jzb0XSvIxQ")
                    .SetDatabaseUrl("https://console.firebase.google.com/u/0/project/brickbreaker-95b6c/firestore/data~2FPlayers~2F1bedyfhl3mdB5CfGRlhJ")
                    .SetStorageBucket("https://console.firebase.google.com/u/0/project/brickbreaker-95b6c/storage/brickbreaker-95b6c.appspot.com/files")
                    .Build();
        }

        public Android.Gms.Tasks.Task CreateUser(string email, string password)
        {
            return auth.CreateUserWithEmailAndPassword(email, password);
        }

        public Android.Gms.Tasks.Task SignIn(string email, string password)
        {
            return auth.SignInWithEmailAndPassword(email, password);
        }
    }
}