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
    public static class AudioManager
    {
        public static string[] sounds = { "brick_hit", "bat_hit", "music", "finished_bricks", "lost" };
        public static Context context;
        public static Hashtable players;
        public static bool IsSoundMuted = false;
        public static bool IsMusicMuted = false;
        public static void InitPlayers(Context ctx)
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
        public static void PlaySound(string sound)
        {
            if (players.ContainsKey(sound) && !IsSoundMuted)
            {
                ((MediaPlayer)players[sound]).Start();
            }
        }
        public static void PlayMusicLoop(string sound)
        {
            if (players.ContainsKey(sound) && !IsMusicMuted)
            {
                MediaPlayer music = (MediaPlayer)players[sound];
                music.Looping = true;
                music.Start();
            }
        }
        public static void Pause(string sound)
        {
            if (players.ContainsKey(sound))
            {
                ((MediaPlayer)players[sound]).Pause();
            }
        }
        public static void ResumeSound(string sound)
        {
            if(players.ContainsKey(sound) && !IsMusicMuted)
            {
                ((MediaPlayer)players[sound]).Start();
            }
        }
        public static void Stop(string sound)
        {
            if (players.ContainsKey(sound))
            {
                ((MediaPlayer)players[sound]).Stop();
            }
        }
        public static void Release()
        {
            foreach (string sound in sounds)
            {
                ((MediaPlayer)players[sound]).Release();
            }
        }
    }
}