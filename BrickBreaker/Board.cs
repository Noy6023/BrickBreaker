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
        public bool threadRunning = true;
        public bool isRunning = true;

        Ball ball; // the ball
        Brick[,] bricks; //the bricks array
        public Score score { get; set; }//the score that is increased by hitting a brick
        public Bat topBat { get; set; } //the top bat
        public Bat bottomBat { get; set; } //the bottom bat
        public bool hasLost { get; set; } //keeps the result whether the bat ha missed and lost or not
        Context context; //the context
        bool isFirstCall = true;
        Hashtable colors;
        Vector screenSize;
        GameButton pause;
        GameButton resume;
        public Thread t;
        ThreadStart ts;
        public Board(Context context, Hashtable colors) : base(context) //constructor
        {
            this.context = context;
            this.colors = new Hashtable(colors);
            hasLost = false; //init the result
            ball = new Ball((Color)colors["ball"]); //init the ball
            bottomBat = new Bat((Color)colors["bottomBat"]); //init the bat
            topBat = new Bat((Color)colors["topBat"]); //init the bat
            score = new Score(); //init the score
            screenSize = new Vector(Constants.DEFULT_SCREEN_SIZE);
            pause = new GameButton(Constants.DEFULT_VECTOR,
                BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pausebutton),
                new Vector(Constants.PAUSE_BUTTON_SIZE, Constants.PAUSE_BUTTON_SIZE));
            resume = new GameButton(Constants.DEFULT_VECTOR,
               BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.resumebtn),
               new Vector(Constants.RESUME_BUTTON_SIZE, Constants.RESUME_BUTTON_SIZE));
            AudioManager.InitPlayers(context);
            ts = new ThreadStart(Run);
            t = new Thread(ts);
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
            isRunning = false;
            ((GameActivity)context).Finish();
        }


        public void Pause()
        {
            isRunning = false;
            if(!hasLost)
                AudioManager.Pause("music");
        }


        public void Resume()
        {
            isRunning = true;
            AudioManager.ResumeSound("music");
        }
        public void StartGame()
        {
            isRunning = true;
            AudioManager.PlayMusicLoop("music");
        }
        public void Run()
        {
            while (threadRunning)
            {
                if (isRunning)
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
                            pause.position = new Vector(screenSize.x - pause.size.x, 0);
                            resume.position = new Vector(screenSize.x/2 - resume.size.x, screenSize.y / 2 - resume.size.y);
                            //ball.position = new Vector(BallStartPosintionGenerator(canvas)); //set the ball on a random place
                            bottomBat.position = new Vector(BatStartPositionGenerator(canvas));
                            //bottomBat.position = new Vector(canvas.Width / 2, canvas.Height - bottomBat.size.y);
                            topBat.position = new Vector(bottomBat.position.x, (int)(score.paint.TextSize) + Bat.size.y);
                            ball.position = new Vector(bottomBat.position.x + Bat.size.x / 2, bottomBat.position.y - 2*Ball.radius);
                            isFirstCall = false;
                        }
                        Update(canvas);
                        Draw(canvas);
                        if (hasLost)
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
            Vector randPos = new Vector(r.Next(canvas.Width - Bat.size.x),canvas.Height- 2*Bat.size.y);
            return randPos;
        }

        /// <summary>
        /// generates a random position for the ball to start from
        /// </summary>
        /// <returns></returns>
        private Vector BallStartPosintionGenerator(Canvas canvas)
        {
            Random r = new Random();
            Vector randPos = new Vector(r.Next(canvas.Width), r.Next(canvas.Height));
            //if the position is in the brick range or to close to the edges, generate a diffrent position
            while((randPos.y >= bricks[0,0].position.y && randPos.y <= bricks[bricks.GetLength(0) - 1, 0].position.y) || randPos.y < Constants.EDGE || randPos.y > canvas.Height - Constants.EDGE)
            {
                randPos = new Vector(r.Next(canvas.Width), r.Next(canvas.Height));
            }
            if (randPos.y > canvas.Height - Constants.EDGE) ball.velocity.y = -ball.velocity.y;
            return randPos;
            //return new Vector(r.Next(Constants.DEFULT_SCREEN_SIZE.x), r.Next(Constants.DEFULT_SCREEN_SIZE.y / 2, Constants.DEFULT_SCREEN_SIZE.y - 200));
            //return new Vector(r.Next(Constants.DEFULT_SCREEN_SIZE.x), r.Next(200, Constants.DEFULT_SCREEN_SIZE.y/4));

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
            bricks = new Brick[(int)((canvas.Height / 3) / (Brick.size.y + Constants.SPACE)), (int)((canvas.Width - x) / (Brick.size.x + Constants.SPACE))];

            for(int i = 0; i < bricks.GetLength(0); i++)
            {
                for(int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = new Brick(new Vector(x, y), brickColor);
                    x += Brick.size.x + Constants.SPACE;
                }
                y += Brick.size.y + Constants.SPACE;
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
            int left = btn.position.x;
            int top = btn.position.y;
            int right = btn.position.x + btn.size.x;
            int bottom = btn.position.y + btn.size.y;
            int x = (int)e.GetX();
            int y = (int)e.GetY();
            if (x > left && x < right && y > top && y < bottom) return true;
            return false;
        }
        public void MoveBatBySensor(int newVelocity)
        {
            bottomBat.velocity.x = newVelocity;
            //topBat.velocity.x = bottomBat.velocity.x; //moves with the bottom bat
            topBat.velocity.x = -bottomBat.velocity.x; //moves against the bottom bat
        }
        public void MakeVisible(Brick[,] bricks)
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j].isVisible = true;
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
                    if (bricks[i, j].isVisible) count++;
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
            canvas.DrawColor(Constants.BACKGROUND_COLOR); //set background color to black
            score.Draw(canvas);
            pause.Draw(canvas);
            //canvas.DrawBitmap(bmPause, canvas.Width - Constants.PAUSE_BUTTON_SIZE, 0, new Paint());
            //draw the bat in the correct position
            bottomBat.Draw(canvas);
            topBat.Draw(canvas);
            //canvas.DrawRect(topBat.position.x, topBat.position.y, topBat.position.x + topBat.size.x, topBat.position.y + topBat.size.y, topBat.paint);
            //draw the bricks
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].isVisible)
                    {
                        bricks[i, j].Draw(canvas);
                    }
                }
            }
            //draw the ball
            ball.Draw(canvas);
            if(!isRunning)
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
                            score.IncreaseScore();
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
            bottomBat.UpdateMovement(); //update the bat movement
            topBat.UpdateMovement();
            ball.UpdateMovement(); //update the ball movement
            bottomBat.UpdateBounds(canvas); //check bounds of the bat
            topBat.UpdateBounds(canvas); //check bounds of the bat
            ball.UpdateWallHit(new Vector(canvas.Width, canvas.Height)); //check bounds of the ball - walls
            //check if the bat missed the ball and lost
            if (bottomBat.IsBallHit(ball, canvas, 'b') == 1 || topBat.IsBallHit(ball, canvas, 't') == 1)
            {
                AudioManager.PlaySound("bat_hit");
            }
            else if (bottomBat.IsBallHit(ball, canvas, 'b') == -1 || topBat.IsBallHit(ball, canvas, 't') == -1)
            {
                hasLost = true;
            }
        }
        private bool IsBallNearBricks()
        {
            if ((ball.position.y >= bricks[0, 0].position.y - Ball.radius && ball.position.y <= bricks[bricks.GetLength(0) - 1, 0].position.y + 2* Ball.radius))// || ball.position.y < Constants.EDGE || ball.position.y > screenSize.y - Constants.EDGE)
                return true;
            return false;
        }
        /// <summary>
        /// game loop that goes on and on until the bat has missed the ball
        /// </summary>
        /// <param name="canvas"></param>
        /*protected override void OnDraw(Canvas canvas)
        {
            //draw the objects on the canvas
            Draw(canvas);
            //update them
            Update(canvas);
            if (!hasLost) Invalidate(); //keep calling this function and drawing until the player lost.
            else player.Release();
        }*/
    }
}