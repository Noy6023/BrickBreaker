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
    public sealed class FileManager
    {
        private static readonly FileManager instance = new FileManager();
        private FileManager()
        {

        }
        public static FileManager Instance
        {
            get
            {
                return instance;
            }
        }
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