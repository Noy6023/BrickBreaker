using Android.Graphics;

namespace BrickBreaker
{
    /// <summary>
    /// a game button class
    /// </summary>
    class GameButton : Shape
    {
        public Bitmap Bitmap { get; set; }
        public Vector Size { get; set; }
        public bool IsVisible { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="position">the position</param>
        /// <param name="bitmap">the bitmap</param>
        /// <param name="size">the size</param>
        /// <param name="show">whether it should be shown </param>
        public GameButton(Vector position, Bitmap bitmap, Vector size, bool isVisible) : base(position)
        {
            this.Position = new Vector(position);
            this.Bitmap = bitmap;
            this.Size = new Vector(size.X, size.Y);
            this.IsVisible = isVisible;
        }

        /// <summary>
        /// draw function
        /// </summary>
        /// <param name="canvas"></param>
        public override void Draw(Canvas canvas)
        {
            if (IsVisible)
                canvas.DrawBitmap(Bitmap, Position.X, Position.Y, Paint);
        }

        /// <summary>
        /// updates the button's side according to the real time scale that the canvas have done
        /// </summary>
        /// <param name="canvas">the canvas which scaled the bitmap</param>
        public void UpdateSize(Canvas canvas)
        {
            Size = new Vector(Bitmap.GetScaledWidth(canvas), Bitmap.GetScaledHeight(canvas));
        }

        /// <summary>
        /// checks if the button was clicked
        /// </summary>
        /// <param name="position">the position of the touch</param>
        /// <returns></returns>
        public bool IsClicked(Vector position)
        {
            float left = Position.X;
            float top = Position.Y;
            float right = Position.X + Size.X;
            float bottom = Position.Y + Size.Y;
            float x = position.X;
            float y = position.Y;
            if (x >= left && x <= right && y >= top && y <= bottom) return true;
            return false;
        }
    }
}