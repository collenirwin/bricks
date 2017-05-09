using System.Drawing;
using System.Windows.Forms;

namespace bricks {
    public class Ball : Control {

        public const int SUBTRACT_AMOUNT = 100;

        int _speed;
        Form1 parent;
        Point direction = new Point(-1, -1);

        public int speed {
            get { return _speed; }
            set {
                direction.X = direction.X * value;
                direction.Y = direction.Y * value;
                _speed = value;
            }
        }

        public Ball(Form1 parent, int speed) {
            this.parent = parent;
            this.speed  = speed;

            BackColor = Color.White;
            Anchor = AnchorStyles.Bottom;
            Height = 16;
            Width  = 16;

            reset(false);
        }

        public void reset(bool subtract) {
            if (subtract) {
                parent.score -= SUBTRACT_AMOUNT;
            }

            direction = new Point(-speed, -speed);

            // directly above paddle
            Location = new Point(
                parent.paddle.Left + (parent.paddle.Width / 2) - (Width / 2),
                parent.paddle.Top - 10 - Height
            ); 
        }

        public void move() {

            // check if we're hitting a side wall
            if (Left <= 0 || (Left + Width) > parent.background.Width) {
                direction.X = -direction.X;
            }

            // check if we're hitting the top wall or if we've hit the paddle
            if (Top <= 0 || Bounds.IntersectsWith(parent.paddle.Bounds)) {
                direction.Y = -direction.Y;
            }

            // check if we're at the bottom
            if (Top + Height >= parent.background.Height) {
                reset(true);
            }

            bool reversedY = false;

            // check if we're colliding with a box
            foreach (var box in parent.boxes) {
                if (Bounds.IntersectsWith(box.Bounds) && !box.destroyed) {
                    box.destroy();

                    // if we haven't already reversed the Y direction
                    if (!reversedY) {
                        direction.Y = -direction.Y;
                        reversedY = true;
                    }
                }
            }

            // apply the change to the ball's location
            Left += direction.X;
            Top  += direction.Y;
        }
    }
}
