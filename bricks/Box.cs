using System.Drawing;
using System.Windows.Forms;

namespace bricks {

    public enum Score {
        low    = 100,
        medium = 200,
        high   = 300
    }

    public class Box : Control {

        Form1 parent;
        public Score score    { get; private set; }
        public bool destroyed { get; private set; }

        public Box(Form1 parent, Score score) {
            this.parent = parent;
            this.score  = score;

            destroyed = false;

            if (score == Score.low) {
                BackColor = Color.MediumSeaGreen;

            } else if (score == Score.medium) {
                BackColor = Color.LightYellow;

            } else {
                BackColor = Color.IndianRed;
            }
        }

        public void destroy() {
            // subtract the time taken from the score of the box
            parent.score += (int)((int)score - parent.time);
            destroyed = true;
            BackColor = parent.background.BackColor; // 'hide' the box
        }
    }
}
