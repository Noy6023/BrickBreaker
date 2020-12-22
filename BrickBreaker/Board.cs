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
        //the context of the game
        Context context; //the context of the game

        //the objects of the game
        Ball ball; // the ball
        Brick[,] bricks; //the bricks array
        public Score Score { get; set; }//the score that is increased by hitting a brick
        public Bat TopBat { get; set; } //the top bat
        public Bat BottomBat { get; set; } //the bottom bat
        GameButton pause; //the pause button
        GameButton resume; //the resume button
        Vector screenSize; //holds the screen size

        //flags
        public bool HasLost { get; set; } //keeps the result whether the bat ha missed and lost or not
        bool isFirstCall = true; //check if its the first time running to init the game

        //the threads
        public bool ThreadRunning = true; //a flag that holds whether the thread is running or not
        public bool IsRunning = true; //a flag that holds whether the game is running or not
        public Thread T; //the thread
        ThreadStart ts; //the thread start
        
        /// <summary>
        /// the constructor of the board class
        /// </summary>
        /// <param name="context">the context of the game</param>
        public Board(Context context) : base(context)
        {
            this.context = context; //set this context to the given context
            HasLost = false; //init the result
            
            //create the objects
            ball = new Ball(ColorManager.Instance.GetColor("ball")); //create the ball
            BottomBat = new Bat(ColorManager.Instance.GetColor("bat"));  //create the bottom bat
            TopBat = new Bat(ColorManager.Instance.GetColor("bat")); //create the top bat
            Score = new Score(); //create the score
            screenSize = new Vector(Constants.DEFULT_SCREEN_SIZE); //init the screen size to defult 
            
            //create the pause button
            pause = new GameButton(Constants.DEFULT_VECTOR, 
                BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pausebutton),
                new Vector(Constants.PAUSE_BUTTON_SIZE, Constants.PAUSE_BUTTON_SIZE));
            
            //create the resume button
            resume = new GameButton(Constants.DEFULT_VECTOR,
               BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.resumebtn),
               new Vector(Constants.RESUME_BUTTON_SIZE, Constants.RESUME_BUTTON_SIZE));

            //init the players
            AudioManager.Instance.InitPlayers(context);
            
            //start the thread - game
            ts = new ThreadStart(Run);
            T = new Thread(ts);
        }

        /// <summary>
        /// the function that is being called when the game is destroyed - finished.
        /// returns the player to the homescreen.
        /// </summary>
        public void Destroy()
        {
            IsRunning = false;
            ((GameActivity)context).Finish();
        }

        /// <summary>
        /// the function that is being called when the game is paused (clicked or minimized).
        /// pauses the music
        /// </summary>
        public void Pause()
        {
            IsRunning = false;
            if(!HasLost)
                AudioManager.Instance.Pause(Sound.music);
        }

        /// <summary>
        /// the function that is being called when the game is resumed (clicked or minimized).
        /// resumes the music
        /// </summary>
        public void Resume()
        {
            IsRunning = true;
            AudioManager.Instance.ResumeSound(Sound.music);
        }

        /// <summary>
        /// the function that is being called when the game is started.
        /// starts the music
        /// </summary>
        public void StartGame()
        {
            IsRunning = true;
            AudioManager.Instance.PlayMusicLoop(Sound.music);
        }

        /// <summary>
        /// the game loop function that handles the threads
        /// </summary>
        public void Run()
        {
            while (ThreadRunning)
            {
                if (IsRunning)
                {
                    //get the canvas to draw on
                    if (!this.Holder.Surface.IsValid)
                        continue;
                    Canvas canvas = null;
                    try
                    {
                        canvas = this.Holder.LockCanvas();
                        if (isFirstCall)
                        {
                            //init the game objects positions and sizes according to the specific phone screen size - canvas
                            InitBricks(canvas);
                            screenSize = new Vector(canvas.Width, canvas.Height);
                            pause.Position = new Vector(screenSize.X - pause.Size.X, 0);
                            resume.Position = new Vector(screenSize.X/2 - resume.Size.X, screenSize.Y / 2 - resume.Size.Y);
                            BottomBat.Position = new Vector(BatStartPositionGenerator(canvas));
                            TopBat.Position = new Vector(BottomBat.Position.X, (int)(Score.Paint.TextSize) + Bat.Size.Y);
                            ball.Position = new Vector(BottomBat.Position.X + Bat.Size.X / 2, BottomBat.Position.Y - 2*Ball.Radius);
                            isFirstCall = false;
                        }

                        //update the game
                        Update(canvas);

                        //draw everything on the canvas
                        Draw(canvas);

                        if (HasLost)
                        {
                            //if the game had ended and the player lost
                            AudioManager.Instance.Stop(Sound.music); //stop the music
                            AudioManager.Instance.PlaySound(Sound.lost); //play lost sound
                            Thread.Sleep(1000);
                            AudioManager.Instance.Release(); //release the sound
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
        /// generates a start position for the bottom bat
        /// </summary>
        /// <param name="canvas">the canvas</param>
        /// <returns>the random vector position</returns>
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
            Color brickColor = ColorManager.Instance.GetColor("brick");
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

        /// <summary>
        /// handles touch events - button clicks
        /// </summary>
        /// <param name="e">the touch event</param>
        /// <returns>true</returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            Vector position = new Vector((int)e.GetX(), (int)e.GetY());
            if(HasTouchedButton(pause, position))
                Pause();
            if (HasTouchedButton(resume, position))
                Resume();
            Invalidate();
            return true;
        }

        /// <summary>
        /// checks if the touch was on a button
        /// </summary>
        /// <param name="btn">the button to check</param>
        /// <param name="position">the position of the touch</param>
        /// <returns></returns>
        private bool HasTouchedButton(GameButton btn, Vector position)
        {
            int left = btn.Position.X;
            int top = btn.Position.Y;
            int right = btn.Position.X + btn.Size.X;
            int bottom = btn.Position.Y + btn.Size.Y;
            int x = position.X;
            int y = position.Y;
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

        /// <summary>
        /// counts the visible bricks
        /// </summary>
        /// <param name="bricks">the bricks array</param>
        /// <returns>the number of visible bricks</returns>
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
            canvas.DrawColor(ColorManager.Instance.GetColor("background")); //draws the background color
            Score.Draw(canvas); //draws the score
            pause.Draw(canvas); //draws the pause
            BottomBat.Draw(canvas); //draws the bottom bat
            TopBat.Draw(canvas); //draws the top bat
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
                //if the game is paused - draw the resume button
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
                //if the ball is near the bricks - check hit on every brick
                for (int i = 0; i < bricks.GetLength(0); i++)
                {
                    for (int j = 0; j < bricks.GetLength(1); j++)
                    {
                        if (bricks[i, j].IsHit(ball))
                        {
                            //if there was a hit - play hit sound and increase score
                            AudioManager.Instance.PlaySound(Sound.brick_hit);
                            Score.IncreaseScore();
                        }
                    }
                }
            }
            if (CountVisible(bricks) == 0)
            {
                //if there aren't any bricks left - play sound and refill the bricks
                AudioManager.Instance.PlaySound(Sound.finished_bricks);
                Thread.Sleep(1000);
                MakeVisible(bricks);
            }
            BottomBat.UpdateMovement(); //update the bottom bat movement
            TopBat.UpdateMovement(); //update the bottom bat movement
            ball.UpdateMovement(canvas); //update the ball movement
            BottomBat.UpdateBounds(canvas); //check bounds of the bat
            TopBat.UpdateBounds(canvas); //check bounds of the bat
            ball.UpdateWallHit(new Vector(canvas.Width, canvas.Height)); //check bounds of the ball - walls
            
            if (BottomBat.IsBallHit(ball, canvas, 'b') == 1 || TopBat.IsBallHit(ball, canvas, 't') == 1)
            {
                // if the bat hit the ball - play sound
                AudioManager.Instance.PlaySound(Sound.bat_hit);
            }
            else if (BottomBat.IsBallHit(ball, canvas, 'b') == -1 || TopBat.IsBallHit(ball, canvas, 't') == -1)
            {
                // if the bat missed the ball - the player lost
                HasLost = true;
            }
        }

        /// <summary>
        /// checks if the ball is near the bricks
        /// </summary>
        /// <returns>true- the ball is lower than the first brick and higher than the last brick. else - false.</returns>
        private bool IsBallNearBricks()
        {
            if ((ball.Position.Y >= bricks[0, 0].Position.Y - Ball.Radius && ball.Position.Y <= bricks[bricks.GetLength(0) - 1, 0].Position.Y + 2* Ball.Radius))
                return true;
            return false;
        }
    }
}