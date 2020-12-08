using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Apache.Http.Impl.Client;

namespace BrickBreaker
{
    public class Board : SurfaceView
    {
        public bool ThreadRunning = true;
        public bool IsRunning = true;

        Ball ball; // the ball
        Brick[,] bricks; //the bricks array
        public Score Score { get; set; }//the score that is increased by hitting a brick
        public Bat TopBat { get; set; } //the top bat
        public Bat BottomBat { get; set; } //the bottom bat
        public bool HasLost { get; set; } //keeps the result whether the bat ha missed and lost or not
        Context context; //the context
        bool isFirstCall = true;
        Hashtable colors;
        Vector screenSize;
        GameButton pause;
        GameButton resume;
        public Thread T;
        ThreadStart ts;
        public Board(Context context, Hashtable colors) : base(context) //constructor
        {
            this.context = context;
            this.colors = new Hashtable(colors);
            HasLost = false; //init the result
            ball = new Ball((Color)colors["ball"]); //init the ball
            BottomBat = new Bat((Color)colors["bat"]); //init the bat
            TopBat = new Bat((Color)colors["bat"]); //init the bat
            Score = new Score(); //init the score
            screenSize = new Vector(Constants.DEFULT_SCREEN_SIZE);
            pause = new GameButton(Constants.DEFULT_VECTOR,
                BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pausebutton),
                new Vector(Constants.PAUSE_BUTTON_SIZE, Constants.PAUSE_BUTTON_SIZE));
            resume = new GameButton(Constants.DEFULT_VECTOR,
               BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.resumebtn),
               new Vector(Constants.RESUME_BUTTON_SIZE, Constants.RESUME_BUTTON_SIZE));
            AudioManager.InitPlayers(context);
            ts = new ThreadStart(Run);
            T = new Thread(ts);
        }
        public void SurfaceCreated(ISurfaceHolder holder)
        {

        }
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {


        }
        public void Destroy()
        {
            IsRunning = false;
            ((GameActivity)context).Finish();
        }
        public void Pause()
        {
            IsRunning = false;
            if(!HasLost)
                AudioManager.Pause("music");
        }
        public void Resume()
        {
            IsRunning = true;
            AudioManager.ResumeSound("music");
        }
        public void StartGame()
        {
            IsRunning = true;
            AudioManager.PlayMusicLoop("music");
        }
        public void Run()
        {
            while (ThreadRunning)
            {
                if (IsRunning)
                {
                    if (!this.Holder.Surface.IsValid)
                        continue;
                    Canvas canvas = null;
                    try
                    {
                        canvas = this.Holder.LockCanvas();
                        if (isFirstCall)
                        {
                            InitBricks(canvas);
                            screenSize = new Vector(canvas.Width, canvas.Height);
                            pause.Position = new Vector(screenSize.X - pause.Size.X, 0);
                            resume.Position = new Vector(screenSize.X/2 - resume.Size.X, screenSize.Y / 2 - resume.Size.Y);
                            BottomBat.Position = new Vector(BatStartPositionGenerator(canvas));
                            TopBat.Position = new Vector(BottomBat.Position.X, (int)(Score.Paint.TextSize) + Bat.Size.Y);
                            ball.Position = new Vector(BottomBat.Position.X + Bat.Size.X / 2, BottomBat.Position.Y - 2*Ball.Radius);
                            isFirstCall = false;
                        }
                        Update(canvas);
                        Draw(canvas);
                        if (HasLost)
                        {
                            AudioManager.Stop("music");
                            AudioManager.PlaySound("lost");
                            Thread.Sleep(1000);

                            AudioManager.Release();
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        if (canvas != null)
                        {
                            this.Holder.UnlockCanvasAndPost(canvas);
                        }
                    }
                }
            }
        }
        private Vector BatStartPositionGenerator(Canvas canvas)
        {
            Random r = new Random();
            Vector randPos = new Vector(r.Next(canvas.Width - Bat.Size.X),canvas.Height- 2*Bat.Size.Y);
            return randPos;
        }
        /// <summary>
        /// inits the brick array by creating the brickes and positioning them
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void InitBricks(Canvas canvas)
        {
            Color brickColor = (Color)colors["brick"];
            int x = Constants.SPACE *2;
            int y = canvas.Height / 3;
            bricks = new Brick[(int)((canvas.Height / 3) / (Brick.Size.Y + Constants.SPACE)), (int)((canvas.Width - x) / (Brick.Size.X + Constants.SPACE))];

            for(int i = 0; i < bricks.GetLength(0); i++)
            {
                for(int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = new Brick(new Vector(x, y), brickColor);
                    x += Brick.Size.X + Constants.SPACE;
                }
                y += Brick.Size.Y + Constants.SPACE;
                x = Constants.SPACE*2;
            }
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            if(HasTouchedButton(pause, e))
                Pause();
            if (HasTouchedButton(resume, e))
                Resume();
            Invalidate();
            return true;
        }
        private bool HasTouchedButton(GameButton btn, MotionEvent e)
        {
            int left = btn.Position.X;
            int top = btn.Position.Y;
            int right = btn.Position.X + btn.Size.X;
            int bottom = btn.Position.Y + btn.Size.Y;
            int x = (int)e.GetX();
            int y = (int)e.GetY();
            if (x > left && x < right && y > top && y < bottom) return true;
            return false;
        }
        /// <summary>
        /// handles movement of the bat.
        /// </summary>
        /// <param name="newVelocity">the new velocity</param>
        public void MoveBatBySensor(int newVelocity, Difficulty difficulty)
        {
            BottomBat.Velocity.X = newVelocity;
            if(difficulty == Difficulty.Easy)
                TopBat.Velocity.X = BottomBat.Velocity.X; //moves with the bottom bat - easy mode
            else 
                TopBat.Velocity.X = -BottomBat.Velocity.X; //moves against the bottom bat - hard mode
        }
        /// <summary>
        /// makes every brick in the given array visible
        /// </summary>
        /// <param name="bricks">the bricks array</param>
        public void MakeVisible(Brick[,] bricks)
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j].IsVisible = true;
                }
            }
        }
        public int CountVisible(Brick[,] bricks)
        {
            int count = 0;
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].IsVisible) count++;
                }
            }
            return count;
        }
        /// <summary>
        /// draws all the objects on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public new void Draw(Canvas canvas)
        {
            base.OnDraw(canvas); //set the canvas to be drawn on
            canvas.DrawColor((Color)colors["background"]); //set background color to black
            Score.Draw(canvas);
            pause.Draw(canvas);
            //canvas.DrawBitmap(bmPause, canvas.Width - Constants.PAUSE_BUTTON_SIZE, 0, new Paint());
            //draw the bat in the correct position
            BottomBat.Draw(canvas);
            TopBat.Draw(canvas);
            //canvas.DrawRect(topBat.position.x, topBat.position.y, topBat.position.x + topBat.size.x, topBat.position.y + topBat.size.y, topBat.paint);
            //draw the bricks
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].IsVisible)
                    {
                        bricks[i, j].Draw(canvas);
                    }
                }
            }
            //draw the ball
            ball.Draw(canvas);
            if(!IsRunning)
            {
                resume.Draw(canvas);
            }
        }
        /// <summary>
        /// updates the positions and events
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void Update(Canvas canvas)
        {
            if(IsBallNearBricks())
            {
                //brick hit
                for (int i = 0; i < bricks.GetLength(0); i++)
                {
                    for (int j = 0; j < bricks.GetLength(1); j++)
                    {
                        if (bricks[i, j].IsHit(ball))
                        {
                            AudioManager.PlaySound("brick_hit");
                            Score.IncreaseScore();
                        }
                    }
                }
            }
            if (CountVisible(bricks) == 0)
            {
                AudioManager.PlaySound("finished_bricks");
                Thread.Sleep(1000);
                MakeVisible(bricks);
            }
            BottomBat.UpdateMovement(); //update the bat movement
            TopBat.UpdateMovement();
            ball.UpdateMovement(); //update the ball movement
            BottomBat.UpdateBounds(canvas); //check bounds of the bat
            TopBat.UpdateBounds(canvas); //check bounds of the bat
            ball.UpdateWallHit(new Vector(canvas.Width, canvas.Height)); //check bounds of the ball - walls
            //check if the bat missed the ball and lost
            if (BottomBat.IsBallHit(ball, canvas, 'b') == 1 || TopBat.IsBallHit(ball, canvas, 't') == 1)
            {
                AudioManager.PlaySound("bat_hit");
            }
            else if (BottomBat.IsBallHit(ball, canvas, 'b') == -1 || TopBat.IsBallHit(ball, canvas, 't') == -1)
            {
                HasLost = true;
            }
        }
        private bool IsBallNearBricks()
        {
            if ((ball.Position.Y >= bricks[0, 0].Position.Y - Ball.Radius && ball.Position.Y <= bricks[bricks.GetLength(0) - 1, 0].Position.Y + 2* Ball.Radius))
                return true;
            return false;
        }
    }
}