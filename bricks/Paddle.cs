using System.Drawing;
using System.Windows.Forms;

namespace bricks {
    public class Paddle : Control {

        Form1 parent;
        public int speed { get; set; }

        public Paddle(Form1 parent, int speed) {
            this.parent = parent;
            this.speed  = speed;

            BackColor = Color.White;
            Anchor = AnchorStyles.Bottom;
            Height = 16;
            Width  = 54;

            // put the paddle in the appropriate place (centered)
            Location = new Point(
                (parent.background.Width / 2) - (Width / 2),
                parent.background.Height - 10 - Height
            );
        }

        /// <summary>
        /// Moves left an amount based on speed if there's room within parent.background
        /// </summary>
        public void moveLeft() {
            if (Left - speed >= 0) {
                Left -= speed;
            }
        }

        /// <summary>
        /// Moves right an amount based on speed if there's room within parent.background
        /// </summary>
        public void moveRight() {
            if (Left + Width + speed <= parent.background.Width) {
                Left += speed;
            }
        }
    }
}
