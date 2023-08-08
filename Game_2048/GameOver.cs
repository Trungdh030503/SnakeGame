using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_2048
{
    public partial class GameOver : Form
    {
        public event EventHandler ReplayClicked;
        public event EventHandler QuitClicked;
        public GameOver()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public Button ReplayButton => Replay;
        private void buttonReplay_Click(object sender, EventArgs e)
        {
            ReplayClicked?.Invoke(this, EventArgs.Empty);
        }
        public Button QuitButton => button2;
        private void button2_Click(object sender, EventArgs e)
        {
            QuitClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
