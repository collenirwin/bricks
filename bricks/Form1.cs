using System;
using System.Drawing;
using System.Windows.Forms;

namespace bricks {

    enum Direction {
        left,
        right,
        none
    }

    public partial class Form1 : Form {

        const int PADDLE_SPEED = 5;
        const int BALL_SPEED   = 3;

        Direction direction = Direction.none;
        DateTime start;

        public int score     { get; set; }
        public double time   { get; private set; }
        public Box[] boxes   { get; private set; }
        public Paddle paddle { get; private set; }
        public Ball ball     { get; private set; }

        public Form1() {
            InitializeComponent();

            // this is for graphics redrawing optimization
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, 
                true
            );

            // set all vars to defaults
            reset();
        }

        public void reset() {
            tmrMain.Stop();

            score = 0;
            time  = 0;
            start = DateTime.Now;

            direction = Direction.none;

            // get rid of all controls in background
            foreach (Control control in background.Controls) {
                control.Dispose();
            }

            paddle = new Paddle(this, PADDLE_SPEED);
            background.Controls.Add(paddle);

            ball = new Ball(this, BALL_SPEED);
            background.Controls.Add(ball);

            boxes = new Box[30];

            // populate our box array, 3 rows of 10
            // high
            for (int x = 0; x < 10; x++) {
                boxes[x] = new Box(this, Score.high);
            }

            // medium
            for (int x = 10; x < 20; x++) {
                boxes[x] = new Box(this, Score.medium);
            }

            // low
            for (int x = 20; x < 30; x++) {
                boxes[x] = new Box(this, Score.low);
            }

            // add the boxes as children of the backgound picturebox
            background.Controls.AddRange(boxes);

            // start the 'game loop' timer
            tmrMain.Start();
        }

        private void tmrMain_Tick(object sender, EventArgs e) {
            background.Invalidate();

            // get # of seconds since start of the game
            time = (DateTime.Now - start).TotalSeconds;

            // set scoreboard (score, time passed)
            lblScoreTime.Text = string.Format(
                lblScoreTime.Tag.ToString(), 
                score.ToString(), 
                time.ToString("n2")
            );

            ball.move();

            if (direction == Direction.left) {
                paddle.moveLeft();

            } else if (direction == Direction.right) {
                paddle.moveRight();
            }

            if (gameFinished()) {
                MessageBox.Show(string.Format(
                    "You won!\nTotal Time: {0}\nScore: {1}", 
                    time.ToString("n2"), 
                    score.ToString()
                ));

                reset();
            }
        }

        /// <summary>
        /// Checks to see if all boxes have been destroyed
        /// </summary>
        private bool gameFinished() {
            foreach (var box in boxes) {
                if (!box.destroyed) {
                    return false;
                }
            }

            return true;
        }

        private void background_Paint(object sender, PaintEventArgs e) {
            int width  = background.Width / 10;
            int height = background.Height / 10;
            int x = 0;
            int y = 0;
            int rowCounter = 0;

            foreach (var box in boxes) {
                box.Width  = width;
                box.Height = height;
                box.Left = x;
                box.Top  = y;

                x += width;

                if (++rowCounter == 10) {
                    rowCounter = 0;
                    x = 0;
                    y += height;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) {
                direction = Direction.left;

            } else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) {
                direction = Direction.right;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            direction = Direction.none;
        }
    }
}
