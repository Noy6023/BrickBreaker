using System;
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
        public Bat topBat { get; set; } //the top bat
        public Bat bottomBat { get; set; } //the bottom bat
        public bool hasLost { get; set; } //keeps the result whether the bat ha missed and lost or not
        Context context; //the context
        bool isFirstCall = true;
        private PlayerManager playerManager;
        //private MediaPlayer[] players;
        public int score { get; set; } //the score that is increased by hitting a brick

        public Thread t;
        ThreadStart ts;
        public Board(Context context) : base(context) //constructor
        {
            this.context = context;
            hasLost = false; //init the result
            ball = new Ball(); //init the ball
            ball.position = new Vector(BallStartPosintionGenerator()); //set the ball on a random place
            bottomBat = new Bat(); //init the bat
            topBat = new Bat(); //init the bat
            score = 0; //init the score
            playerManager = new PlayerManager(context);
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

        public void destroy()
        {
            isRunning = false;
            ((GameActivity)context).Finish();
        }


        public void pause()
        {
            isRunning = false;
        }


        public void resume()
        {
            isRunning = true;
        }
        public void startGame()
        {
            isRunning = true;
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
                            bottomBat.position = new Vector(canvas.Width / 2, canvas.Height - bottomBat.size.y);
                            topBat.position = new Vector(canvas.Width / 2, topBat.size.y);
                            isFirstCall = false;
                        }
                        Update(canvas);
                        Draw(canvas);
                        if (hasLost)
                        {
                            playerManager.Release();
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

        /// <summary>
        /// generates a random position for the ball to start from
        /// </summary>
        /// <returns></returns>
        private Vector BallStartPosintionGenerator()
        {
            Random r = new Random();
            //return new Vector(r.Next(Constants.DEFULT_SCREEN_SIZE.x), r.Next(Constants.DEFULT_SCREEN_SIZE.y / 2, Constants.DEFULT_SCREEN_SIZE.y - 200));
            return new Vector(r.Next(Constants.DEFULT_SCREEN_SIZE.x), r.Next(200, Constants.DEFULT_SCREEN_SIZE.y/4));

        }
        /// <summary>
        /// inits the brick array by creating the brickes and positioning them
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void InitBricks(Canvas canvas)
        {
            int x = Constants.SPACE *2;
            int y = canvas.Height / 3;
            bricks = new Brick[(int)((canvas.Height / 3) / (Constants.BRICK_SIZE.y + Constants.SPACE)), (int)((canvas.Width - x) / (Constants.BRICK_SIZE.x + Constants.SPACE))];

            for(int i = 0; i < bricks.GetLength(0); i++)
            {
                for(int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = new Brick(new Vector(x, y));
                    x += Constants.BRICK_SIZE.x + Constants.SPACE;
                }
                y += Constants.BRICK_SIZE.y + Constants.SPACE;
                x = Constants.SPACE*2;
            }
            
        }
        public void MoveBatBySensor(int newVelocity)
        {
            bottomBat.velocity.x = newVelocity;
            topBat.velocity.x = bottomBat.velocity.x;
            //topBat.velocity.x = -bottomBat.velocity.x;
        }
        /// <summary>
        /// draws all the objects on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public new void Draw(Canvas canvas)
        {
            canvas.DrawColor(Color.Black); //set background color to black
            Paint pscore = new Paint();
            pscore.Color = Color.White;
            pscore.TextSize = 70;
            canvas.DrawText("Score: " + score.ToString(), 0, pscore.TextSize, pscore);
            base.OnDraw(canvas); //set the canvas to be drawn on
            //draw the bat in the correct position
            bottomBat.Draw(canvas);
            canvas.DrawRect(topBat.position.x, topBat.position.y, topBat.position.x + topBat.size.x, topBat.position.y + topBat.size.y, topBat.paint);
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
        }
        /// <summary>
        /// updates the positions and events
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void Update(Canvas canvas)
        {
            //brick hit
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].IsHit(ball))
                    {
                        playerManager.PlaySound(3);
                        score++;
                    }
                }
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
                playerManager.PlaySound(4);
            }
            else if (bottomBat.IsBallHit(ball, canvas, 'b') == -1 || topBat.IsBallHit(ball, canvas, 't') == -1)
                hasLost = true;
            
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