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
    /// an enum for the difficulty - easy or hard.
    /// used to choose whether the top bet moves in sync with the bottom bat or against it
    /// </summary>
    public enum Difficulty
    {
        Easy,
        Hard
    }
}