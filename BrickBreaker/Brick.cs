using Android.Graphics;

namespace BrickBreaker
{
    /// <summary>
    /// a brick in the game
    /// </summary>
    public class Brick : Shape
    {
        public bool IsVisible { get; set; } //true if the brick is visable and can be hit. else false
        public static Vector Size { get; set; } //the size of the brick

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="position">the position</param>
        /// <param name="color">the color</param>
        public Brick(Vector position, Color color) : base(position, color) //constructor
        {
            this.IsVisible = true;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="other">the other brick to copy</param>
        public Brick(Brick other):base()
        {
            base.Position = new Vector(other.Position);
            base.Paint = new Paint(other.Paint);
            Paint.Color = other.Paint.Color;
            IsVisible = other.IsVisible;
        }

        /// <summary>
        /// draws the brick
        /// </summary>
        /// <param name="canvas">the canvas</param>
        /// <param name="space">the space that comes after the brick</param>
        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(Position.X,
                Position.Y,
                Position.X + Size.X,
                Position.Y + Size.Y, Paint);
        }
        
        /// <summary>
        /// a check if the brick was hit by the ball. if so then change velocity of the ball and the visiblity of the brick
        /// </summary>
        /// <param name="ball">the ball</param>
        /// <param name="space">the space</param>
        /// <returns>true- the brick was hit by the ball. false- the brick wasn't hit by the ball.</returns>
        public bool IsHit(Ball ball)
        {
            if (!this.IsVisible)
                return false;
            if(this.HasHitBall(ball, Size))
            {
                
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the size of the bricks according to the canvas size
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public static void SetSize(Canvas canvas)
        {
            if (Size == Constants.BRICK_SMALL_SIZE)
                Size = new Vector(canvas.Width / Constants.BRICK_SMALL_SIZE.X, (canvas.Width / Constants.BRICK_SMALL_SIZE.Y) / 1.5f);
            if (Size == Constants.BRICK_MEDIUM_SIZE)
                Size = new Vector(canvas.Width / Constants.BRICK_MEDIUM_SIZE.X, (canvas.Width / Constants.BRICK_MEDIUM_SIZE.Y) / 1.5f);
            if (Size == Constants.BRICK_BIG_SIZE)
                Size = new Vector(canvas.Width / Constants.BRICK_BIG_SIZE.X, (canvas.Width / Constants.BRICK_BIG_SIZE.Y) / 1.5f);
        }
    }
}