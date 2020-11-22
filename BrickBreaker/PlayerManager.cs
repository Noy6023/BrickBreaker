using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickBreaker
{
    public class PlayerManager
    {
        private Context context;
        private MediaPlayer[] players;
        public static bool IsSoundMuted = false;
        public static bool IsMusicMuted = false;
        public PlayerManager(Context context)
        {
            this.context = context;
            players = new MediaPlayer[Constants.NUM_OF_PLAYERS];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new MediaPlayer();
                InitPlayers(i);
            }
        }
        private void InitPlayers(int sound)
        {
            if (players[sound] == null)
            {
                players[sound] = new MediaPlayer();
            }
            else
            {
                players[sound].Reset();
                if (sound == 0)
                {
                    players[sound] = MediaPlayer.Create(context, Resource.Raw.sound1);
                }
                if (sound == 1)
                {
                    players[sound] = MediaPlayer.Create(context, Resource.Raw.sound2);
                }
                if (sound == 2)
                {
                    players[sound - 1] = MediaPlayer.Create(context, Resource.Raw.sound3);
                }
                if (sound == 3)
                {
                    players[sound] = MediaPlayer.Create(context, Resource.Raw.sound4);
                }
                if (sound == 4)
                {
                    players[sound] = MediaPlayer.Create(context, Resource.Raw.sound5);
                }
            }
        }
        public void PlaySound(int sound)
        {
            if(sound < players.Length && !IsSoundMuted)
            {
                players[sound].Start();
            }
        }
        public void Release()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Release();
            }
        }
    }
}