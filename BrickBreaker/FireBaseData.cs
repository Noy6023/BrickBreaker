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
    /// <summary>
    /// fire base data class
    /// </summary>
    public sealed class FireBaseData
    {
        private static readonly FireBaseData instance = new FireBaseData();
        private FirebaseFirestore firestore;
        private FirebaseAuth auth;
        private FirebaseApp app;
        static FireBaseData() //static constructor
        {

        }
        public static FireBaseData Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// constructor
        /// </summary>
        private FireBaseData()
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
        /// <summary>
        /// links the project to the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// adds a document to the given collection
        /// </summary>
        /// <param name="cName">the collection to add to</param>
        /// <param name="dName">the name of the document to add</param>
        /// <param name="hmFields">the fiels in the document</param>
        public void AddDocumentToCollection(string cName, string dName, Hashtable hmFields)
        {
            DocumentReference cr;
            if (dName == null)
                cr = firestore.Collection(cName).Document();
            else
                cr = firestore.Collection(cName).Document(dName);
            cr.Set(hmFields);
        }
        /// <summary>
        /// deletes a document from a collection
        /// </summary>
        /// <param name="cName">the collection name</param>
        /// <param name="dName">the document name</param>
        public void DeleteDocumentFromCollection(string cName, string dName)
        {
            DocumentReference cr = firestore.Collection(cName).Document(dName);
            cr.Delete();
        }
        /// <summary>
        /// gets the collection
        /// </summary>
        /// <param name="cName">the collection to get</param>
        /// <returns>the task to listen to</returns>
        public Task GetCollection(string cName)
        {
            return firestore.Collection(cName).Get();
        }
        public void SaveScoreToCollection(string cName, Score score)
        {
            Hashtable data = new Hashtable();
            data.Put("Name", score.Name);
            data.Put("Score", score.HighestValue);
            data.Put("Key", score.Key);
            DocumentReference cr = firestore.Collection(cName).Document(score.Key.ToString());
            cr.Set(data);
        }
        /// <summary>
        /// gets a document from the collection
        /// </summary>
        /// <param name="cName">the collection name</param>
        /// <param name="dName">the document name</param>
        /// <returns>a task</returns>
        public Task GetDocument(string cName, string dName)
        {
            return firestore.Collection(cName).Document(dName).Get();
        }
    }
}