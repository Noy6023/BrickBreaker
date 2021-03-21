using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using Java.IO;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using System.Collections;

namespace BrickBreaker
{
    /// <summary>
    /// manages the file data
    /// </summary>
    public sealed class FileManager
    {
        //using a singelton method so that there is only one instance of audio manager
        private static readonly FileManager instance = new FileManager();

        /// <summary>
        /// constructor
        /// </summary>
        private FileManager() //private constructor
        { }

        /// <summary>
        /// gets the instance
        /// </summary>
        public static FileManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// save the info to the file
        /// </summary>
        /// <param name="seperator">the separator</param>
        /// <param name="info">the info</param>
        /// <param name="context">the context</param>
        public void SaveInfo(char seperator, string[] info, Context context)
        {
            //save: score, highest score, name
            string str = "";
            try
            {
                for(int i = 0; i < info.Length; i++)
                {
                    str += info[i] + seperator;
                }
                using (Stream stream = context.OpenFileOutput("userinfo.txt", FileCreationMode.Private))
                {
                    if (str != null)
                    {
                        try
                        {
                            stream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                            stream.Close();
                        }
                        catch (Java.IO.IOException e)
                        {
                            e.PrintStackTrace();
                        }
                    }
                }
            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// load the info from the file
        /// </summary>
        /// <param name="context">the context</param>
        /// <returns>an info array</returns>
        public string[] LoadInfo(Context context)
        {
            string[] seperators = new[] { "\n" };
            string str;
            try
            {
                using (Stream inTo = context.OpenFileInput("userinfo.txt"))
                {
                    try
                    {
                        byte[] buffer = new byte[4096];
                        inTo.Read(buffer, 0, buffer.Length);
                        str = System.Text.Encoding.Default.GetString(buffer);
                        inTo.Close();
                        if (str != null)
                        {
                            string[] info = str.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            return info;
                        }
                    }
                    catch (Java.IO.IOException e)
                    {
                        e.PrintStackTrace();
                    }
                }
            }
            catch (Java.IO.FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            return null;
        }

    }
}