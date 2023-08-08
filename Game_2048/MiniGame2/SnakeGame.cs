using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;


namespace Game_2048.MiniGame2
{
    public partial class SnakeGame : Form
    {
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label scoreLable;
        private Label maxScoreLable;
        private PictureBox gameover;
        private PictureBox head;
        private PictureBox food;
        Button button1 = new Button();
        Button button2 = new Button();
        private List<Point> tailPositions;
        List<PictureBox> tails = new List<PictureBox>();
        List<PictureBox> brinks = new List<PictureBox>();

        string direction = "none";
        int score;
        int[][] headPos;
        int length;
        bool dead = false;
        int maxscore;

        private string assetsFolderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "MiniGame2\\Assets"));
        public SnakeGame()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Set the FormBorderStyle to FixedSingle
            this.ClientSize = new Size(1000, 900); // Set your desired fixed size here
            LoadMaxScore();
            CreateControls();
            LoadWall();
        }

        private void SetUpGame()
        {
            score = 0;
            direction = "right";
            length = 3;
            button1.Enabled = false;
            button2.Enabled = false;
            timer1.Start();
        }

        private void SnakeGame_Load(object sender, EventArgs e)
        {
        }

        private void LoadWall()
        {
            string filePath = Path.Combine(assetsFolderPath, "TextFile1.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                int cellSize = 20;
                int startX = 30;
                int startY = 210;

                for (int row = 0; row < lines.Length; row++)
                {
                    string line = lines[row];
                    for (int col = 0; col < line.Length; col++)
                    {
                        if (line[col] == '#')
                        {
                            PictureBox pic = new PictureBox();
                            pic.Location = new Point(col * cellSize + startX, row * cellSize + startY);
                            pic.BorderStyle = BorderStyle.FixedSingle;
                            pic.Size = new Size(cellSize, cellSize);
                            pic.BackColor = Color.Black; // Set the background color for the wall PictureBox
                            this.Controls.Add(pic);
                            pic.BringToFront();
                            brinks.Add(pic);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Wall file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void CreateControls()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Calculate positions and sizes based on percentages of form size
            int pictureBox1Width = (int)(900);
            int pictureBox1Height = (int)(500);
            int pictureBox1X = 30;
            int pictureBox1Y = 210;

            int pictureBox2Width = 160;
            int pictureBox2Height = 160;
            int pictureBox2X = 30;
            int pictureBox2Y = 30; // Distance between pictureBox1 and pictureBox2 is 20

            int pictureBox3Width = 200;
            int pictureBox3Height = 90;
            int pictureBox3X = pictureBox2X + pictureBox2Width + 50;
            int pictureBox3Y = 30; // Distance between pictureBox2 and pictureBox3 is 20

            int pictureBox4Width = 200;
            int pictureBox4Height = 90;
            int pictureBox4X = pictureBox3X + pictureBox3Width + 50;
            int pictureBox4Y = 30;

            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonX = pictureBox3X;
            int buttonY = pictureBox3Height + pictureBox3Y + 20;

            int button2X = pictureBox4X;
            int button2Y = buttonY;

            


            // Create PictureBoxes
            pictureBox1 = new PictureBox();
            pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            pictureBox1.Location = new System.Drawing.Point(pictureBox1X, pictureBox1Y);
            pictureBox1.Size = new System.Drawing.Size(pictureBox1Width, pictureBox1Height);
            pictureBox1.TabStop = false;
            this.Controls.Add(pictureBox1);

            pictureBox2 = new PictureBox();
            pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            pictureBox2.Location = new System.Drawing.Point(pictureBox2X, pictureBox2Y);
            pictureBox2.Size = new System.Drawing.Size(pictureBox2Width, pictureBox2Height);
            pictureBox2.TabStop = false;
            this.Controls.Add(pictureBox2);

            scoreLable = new Label();
            scoreLable.BackColor = System.Drawing.SystemColors.ControlDark;
            scoreLable.Location = new System.Drawing.Point(pictureBox3X, pictureBox3Y);
            scoreLable.Size = new System.Drawing.Size(pictureBox3Width, pictureBox3Height);
            scoreLable.Font = new Font("Myanmar Text", 18, FontStyle.Bold);
            scoreLable.TabStop = false;
            scoreLable.Text = "Score:\n0"; // Initial score is 0
            scoreLable.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(scoreLable);

            maxScoreLable = new Label();
            maxScoreLable.BackColor = System.Drawing.SystemColors.ControlDark;
            maxScoreLable.Location = new System.Drawing.Point(pictureBox4X, pictureBox4Y);
            maxScoreLable.Size = new System.Drawing.Size(pictureBox4Width, pictureBox4Height);
            maxScoreLable.AutoSize = false;
            maxScoreLable.Font = new Font("Myanmar Text", 18, FontStyle.Bold);
            maxScoreLable.TabStop = false;
            maxScoreLable.Text = "Max Score:\n" + maxscore; // Initial score is 0
            maxScoreLable.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(maxScoreLable);

            // Create Buttons
            button1.Location = new System.Drawing.Point(buttonX, buttonY);
            button1.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            button1.Text = "Play";
            button1.TabStop = false;
            this.Controls.Add(button1);

            button2.Location = new System.Drawing.Point(button2X, button2Y);
            button2.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            button2.Text = "Menu";
            button2.TabStop = false;
            this.Controls.Add(button2);

            head = new PictureBox();
            head.BackColor = Color.FromArgb(124, 252, 0);
            head.Location = new System.Drawing.Point(pictureBox1X + pictureBox1Width/2 + 10, pictureBox1Y + pictureBox1Height/2 + 10);
            head.Size = new System.Drawing.Size(20, 20);
            this.Controls.Add(head);
            head.BringToFront();

            PictureBox tail1 = new PictureBox();
            tail1.BackColor = Color.FromArgb(255, 160, 122);
            tail1.Location = new System.Drawing.Point(pictureBox1X + pictureBox1Width / 2 - 10, pictureBox1Y + pictureBox1Height / 2 + 10);
            tail1.Size = new System.Drawing.Size(20, 20);
            this.Controls.Add(tail1);
            tail1.BringToFront();

            PictureBox tail2 = new PictureBox();
            tail2.BackColor = Color.FromArgb(255, 160, 122);
            tail2.Location = new System.Drawing.Point(pictureBox1X + pictureBox1Width / 2 - 30, pictureBox1Y + pictureBox1Height / 2 + 10);
            tail2.Size = new System.Drawing.Size(20, 20);
            this.Controls.Add(tail2);
            tail2.BringToFront();

            PictureBox tail3 = new PictureBox();
            tail3.BackColor = Color.FromArgb(255, 160, 122);
            tail3.Location = new System.Drawing.Point(pictureBox1X + pictureBox1Width / 2 - 50, pictureBox1Y + pictureBox1Height / 2 + 10);
            tail3.Size = new System.Drawing.Size(20, 20);
            this.Controls.Add(tail3);
            tail3.BringToFront();

            food = new PictureBox();
            food.BackColor = Color.Yellow;
            food.Location = GenerateRandomFoodLocation();
            food.Size = new Size(20, 20);
            this.Controls.Add(food);
            food.BringToFront();
            tails.Add(tail1);
            tails.Add(tail2);
            tails.Add(tail3);
        }

        private void gameTimer(object sender, EventArgs e)
        {
          
        }

        private void SnakeGame_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyboardPressed(object sender, KeyEventArgs e)
        {


            switch (e.KeyCode.ToString())
            {
                case "Right":
                    if (!direction.Equals("left"))
                        direction = "right";
                    break;
                case "Left":
                    if (!direction.Equals("right"))
                        direction = "left";
                    break;
                case "Down":
                    if (!direction.Equals("up"))
                        direction = "down";
                    break;
                case "Up":
                    if (!direction.Equals("down"))
                        direction = "up";
                    break;
            }

        }

        private void RestartGame()
        {
            // Unsubscribe from event handlers to prevent multiple registrations
            this.KeyDown -= OnKeyboardPressed;
            timer1.Tick -= timer1_Tick;

            dead = false; // Reset the "dead" flag
            this.Controls.Clear();
            tails.Clear();
            brinks.Clear();
            CreateControls();
            LoadWall();
            button1.Text = "Play"; // Set the button text back to "Play"
            button1.Enabled = false; // Disable the button again

            // Re-register event handlers after the game is set up again
            this.KeyDown += OnKeyboardPressed;
            timer1.Tick += timer1_Tick;
        }
        private void SnakeGame_Load_1(object sender, EventArgs e)
        {
            this.KeyDown -= new KeyEventHandler(OnKeyboardPressed);
            this.KeyDown += new KeyEventHandler(OnKeyboardPressed);
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (dead)
            {
                RestartGame();
            }
            
            SetUpGame();
            
        }

        private void SaveMaxScoreToFile()
        {
            string filePath = Path.Combine(assetsFolderPath, "score.txt");
            File.WriteAllText(filePath, maxscore.ToString());
        }

        private void LoadMaxScore()
        {
            string filePath = Path.Combine(assetsFolderPath, "score.txt");
            if (File.Exists(filePath))
            {
                string maxScoreString = File.ReadAllText(filePath);

                // Try parsing the maxScoreString to an integer
                if (int.TryParse(maxScoreString, out int loadedMaxScore))
                {
                    maxscore = loadedMaxScore;
                }
                else
                {
                    // If parsing fails, set maxscore to 0 (or any other default value you prefer)
                    maxscore = 0;
                }
            }
            else
            {
                // If the score.txt file doesn't exist, set maxscore to 0 (or any other default value you prefer)
                maxscore = 0;
            }
        }

        private void updateScore()
        {
            score += 5;
            scoreLable.Text = "Score:\n" + score;
            if (score > maxscore)
            {
                
                maxscore = score;
                maxScoreLable.Text = "Max Score:\n" + score;
                SaveMaxScoreToFile();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkHit())
            {
                dead = true;
                timer1.Stop();
                pictureBox1.Image = Image.FromFile(assetsFolderPath + "\\gameover.jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.BringToFront();
                button1.Text = "Replay";
                button1.Enabled = true;
                button2.Enabled = true;
                return;
            }
            updateTail();
            Point headPos = head.Location;
            int moveAmount = 20; // You can adjust this value to change the amount the head moves
            if (direction.Equals("right"))
            {
                head.Location = new Point(headPos.X + moveAmount, headPos.Y);
            }
            if (direction.Equals("left"))
            {
                head.Location = new Point(headPos.X - moveAmount, headPos.Y);
            }
            if (direction.Equals("down"))
            {
                head.Location = new Point(headPos.X, headPos.Y + moveAmount);
            }
            if (direction.Equals("up"))
            {
                head.Location = new Point(headPos.X, headPos.Y - moveAmount);
            }
        }


        private void updateTail()
        {

            if (checkColision())
            {

                AddNewTail();
                updateScore();

                for (int i = length - 1; i > 0; i--)
                {
                    // Move each tail towards the position of the tail before it
                    tails[i].Location = tails[i - 1].Location;
                }

                
            }
            for (int i = length - 1; i > 0; i--)
            {
                // Move each tail towards the position of the tail before it
                tails[i].Location = tails[i - 1].Location;
            }

            // Move the first tail towards the head's previous position
            tails[0].Location = head.Location;

            
        }

        private void AddNewTail()
        {
            PictureBox newtail = new PictureBox();
            newtail.BackColor = Color.FromArgb(255, 160, 122);
            newtail.Location = tails[length - 1].Location;
            newtail.Size = new System.Drawing.Size(20, 20);
            this.Controls.Add(newtail);
            newtail.BringToFront();
            tails.Add(newtail);
            length++;
        }
        private bool checkHit()
        {
            Rectangle headArea = new Rectangle(head.Location, head.Size);
            for (int i=0; i < length; i++)
            {
                Rectangle tailArea = new Rectangle(tails[i].Location, tails[i].Size);
                if (headArea.IntersectsWith(tailArea))
                {
                    return true;
                }
            }
            for (int i = 0; i < brinks.Count; i++)
            {
                Rectangle brickArea = new Rectangle(brinks[i].Location, brinks[i].Size);
                if (headArea.IntersectsWith(brickArea))
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkColision()
        {
            Rectangle headArea = new Rectangle(head.Location, head.Size);
            Rectangle foodArea = new Rectangle(food.Location, food.Size);
            if (headArea.IntersectsWith(foodArea))
            {
                food.Location = GenerateRandomFoodLocation();
                return true;
            }
            else 
                return false;
        }

        private Point GenerateRandomFoodLocation()
        {
            // Get the dimensions of the game area
            int gameAreaWidth = pictureBox1.Width;
            int gameAreaHeight = pictureBox1.Height;

            // Calculate the maximum X and Y coordinates for the food
            int maxX = gameAreaWidth / 20 - 2; // Assuming the size of each cell is 20x20 pixels
            int maxY = gameAreaHeight / 20 - 2;

            // Generate random X and Y coordinates for the food
            Random random = new Random();
            int foodX = random.Next(1, maxX) * 20 + 30; // Multiply by 20 to get the actual position in pixels
            int foodY = random.Next(1, maxY) * 20 + 210;

            // Check if the generated food position overlaps with the snake's tails or walls
            Point foodLocation = new Point(foodX, foodY);
            while (tails.Any(tail => tail.Location == foodLocation) || brinks.Any(brick => brick.Location == foodLocation) || head.Location == foodLocation)
            {
                foodX = random.Next(1, maxX) * 20 + 30;
                foodY = random.Next(1, maxY) * 20 + 210;
                foodLocation = new Point(foodX, foodY);
            }

            // Return the random food location as a Point
            return foodLocation;
        }

        private void renderTail()
        {

        }
    }
}
