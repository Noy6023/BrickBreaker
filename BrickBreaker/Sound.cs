using Android.App;
using Android.Content;
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
    /// <summary>
    /// an enum for the sounds.
    /// </summary>
    public enum Sound
    {
        brick_hit,
        bat_hit,
        music,
        finished_bricks,
        lost
    }
}