using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    public sealed class AudioManager
    {
        //using a singelton method so that there is only one instance of audio manager
        private static readonly AudioManager instance = new AudioManager();
        public static string[] sounds = { "brick_hit", "bat_hit", "music", "finished_bricks", "lost" };
        public static Context context { get; set; }
        private Hashtable players;
        public static bool IsSoundMuted;
        public static bool IsMusicMuted;
        static AudioManager() //static constructor
        {
            IsSoundMuted = false;
            IsMusicMuted = false;
        }
        private AudioManager() //private constructor
        {
            players = new Hashtable();
        }
        public static AudioManager Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// init the players
        /// </summary>
        /// <param name="ctx">the context</param>
        public void InitPlayers(Context ctx)
        {
            context = ctx;
            players = new Hashtable();
            foreach(string sound in sounds)
            {
                players.Add(sound, new MediaPlayer());
                MediaPlayer player = (MediaPlayer)players[sound];
                player.Reset();
                players[sound] = MediaPlayer.Create(context, context.Resources.GetIdentifier(sound, "raw", context.PackageName));
            }
        }

        /// <summary>
        /// play a sound
        /// </summary>
        /// <param name="sound">the name of the sound to play</param>
        public void PlaySound(string sound)
        {
            if (players.ContainsKey(sound) && !IsSoundMuted)
            {
                ((MediaPlayer)players[sound]).Start();
            }
        }

        /// <summary>
        /// play a sound in a loop - the music
        /// </summary>
        /// <param name="sound">the sound to play</param>
        public void PlayMusicLoop(string sound)
        {
            if (players.ContainsKey(sound) && !IsMusicMuted)
            {
                MediaPlayer music = (MediaPlayer)players[sound];
                music.Looping = true;
                music.Start();
            }
        }

        /// <summary>
        /// pause a sound
        /// </summary>
        /// <param name="sound">the sound to pause</param>
        public void Pause(string sound)
        {
            if (players.ContainsKey(sound))
            {
                ((MediaPlayer)players[sound]).Pause();
            }
        }

        /// <summary>
        /// resume a sound
        /// </summary>
        /// <param name="sound">the sound to resume</param>
        public void ResumeSound(string sound)
        {
            if(players.ContainsKey(sound) && !IsMusicMuted)
            {
                ((MediaPlayer)players[sound]).Start();
            }
        }

        /// <summary>
        /// stop a sound
        /// </summary>
        /// <param name="sound">the sound to stop</param>
        public void Stop(string sound)
        {
            if (players.ContainsKey(sound))
            {
                ((MediaPlayer)players[sound]).Stop();
            }
        }

        /// <summary>
        /// release all the players
        /// </summary>
        public void Release()
        {
            foreach (string sound in sounds)
            {
                ((MediaPlayer)players[sound]).Release();
            }
        }
    }
}