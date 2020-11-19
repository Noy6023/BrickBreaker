using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    public class Board : View
    {
        private readonly Vector SCREEN_SIZE = new Vector(1000, 2000); //defult screen size
        private const int SPACE = 25;
        Ball ball; // the ball
        Brick[,] bricks; //the bricks array
        public Bat bat { get; set; } //the bat
        public bool hasLost { get; set; } //keeps the result whether the bat ha missed and lost or not
        Context context; //the context
        bool isFirstCall = true;
        protected MediaPlayer player;
        public int score { get; set; } //the score that is increased by hitting a brick
        public Board(Context context) : base(context) //constructor
        {
            this.context = context;
            hasLost = false; //init the result
            ball = new Ball(); //init the ball
            ball.position = new Vector(BallStartPosintionGenerator()); //set the ball on a random place
            bat = new Bat(); //init the bat
            score = 0; //init the score
            player = new MediaPlayer(); //Init the media player
        }

        /// <summary>
        /// generates a random position for the ball to start from
        /// </summary>
        /// <returns></returns>
        private Vector BallStartPosintionGenerator()
        {
            Random r = new Random();
            return new Vector(r.Next(SCREEN_SIZE.x), r.Next(SCREEN_SIZE.y / 3, SCREEN_SIZE.y - 200));
        }
        /// <summary>
        /// inits the brick array by creating the brickes and positioning them
        /// </summary>
        /// <param name="canvas">the canvas</param>
        private void InitBricks(Canvas canvas)
        {
            Brick brick = new Brick(new Vector(0,0));
            bricks = new Brick[(int)((canvas.Height / 3) / (brick.SIZE.y + SPACE)), (int)(canvas.Width / (brick.SIZE.x + SPACE))];
            int x = SPACE/2;
            int y = SPACE * 3;
            for(int i = 0; i < bricks.GetLength(0); i++)
            {
                for(int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = new Brick(new Vector(x, y));
                    x += brick.SIZE.x + SPACE;
                }
                y += brick.SIZE.y + SPACE;
                x = SPACE/2;
            }
            
        }
        /// <summary>
        /// draws all the objects on the canvas
        /// </summary>
        /// <param name="canvas">the canvas</param>
        public new void Draw(Canvas canvas)
        {
            if (isFirstCall)
            {
                InitBricks(canvas);
                bat.position = new Vector(canvas.Width / 2, canvas.Height - canvas.Height / 10);
                isFirstCall = false;
            }
            canvas.DrawColor(Color.Black); //set background color to black
            Paint pscore = new Paint();
            pscore.Color = Color.White;
            pscore.TextSize = 70;
            canvas.DrawText("Score: " + score.ToString(), 0, pscore.TextSize, pscore);
            base.OnDraw(canvas); //set the canvas to be drawn on
            //draw the bat in the correct position
            bat.Draw(canvas);
            //draw the bricks
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].isVisible)
                    {
                        bricks[i, j].Draw(canvas, SPACE);
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
            bat.UpdateMovement(); //update the bat movement
            ball.UpdateMovement(); //update the ball movement
            bat.UpdateBounds(canvas); //check bounds of the bat
            ball.UpdateWallHit(new Vector(canvas.Width, canvas.Height), bat); //check bounds of the ball - walls
            hasLost = !bat.IsBallHit(ball, canvas); //check if the bat missed the ball and lost
            //brick hit
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j].IsHit(ball, SPACE))
                    {
                        score++;
                    }
                }
            }
        }
        /// <summary>
        /// game loop that goes on and on until the bat has missed the ball
        /// </summary>
        /// <param name="canvas"></param>
        protected override void OnDraw(Canvas canvas)
        {
            //draw the objects on the canvas
            Draw(canvas);
            //update them
            Update(canvas);
            if (!hasLost) Invalidate(); //keep calling this function and drawing until the player lost.
            else player.Release();
        }
    }
}