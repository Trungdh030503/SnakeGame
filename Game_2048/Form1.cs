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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    MiniGame1.Form1 miniGameForm = new MiniGame1.Form1();
        //    this.Hide();
        //    miniGameForm.ShowDialog();
        //    this.Show();
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            MiniGame2.SnakeGame snakeGame = new MiniGame2.SnakeGame();
            this.Hide();
            snakeGame.ShowDialog();
            this.Show();
        }




    }
}
