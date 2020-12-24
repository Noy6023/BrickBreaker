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
    /// an enum for the color keys - ball, bat, brick, background.
    /// used to choose the object to paint.
    /// </summary>
    public enum ColorKey
    {
        Ball,
        Bat,
        Brick,
        Background
    }
}