using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();
            //set settings
            new Settings();

            //set game speed and start timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //start new game
            StartGame();
        }

        private void StartGame()
        {
            lbgameOver.Visible = false;
            new Settings();

            //Create new player object
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            int maxXPos = pictureBox1.Size.Width / Settings.Width;
            int maxYPos = pictureBox1.Size.Height / Settings.Height;
            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    Application.Restart();
                    StartGame();

                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pictureBox1.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if(!Settings.GameOver)
            {
                Brush snakeColour;
                //draw snake
                for(int i=0; i<Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColour = Brushes.Black;
                    else
                        snakeColour = Brushes.Green;
                    //draw snake
                    canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    //draw food
                    canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height,Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOVer = "Game Over \n Seu Score é: " + Settings.Score + "\n aperte enter para jogar de novo";
                lbgameOver.Text = gameOVer;
                lbgameOver.Visible = true;
            }
        }
        private void MovePlayer()
        {
            for(int i = Snake.Count -1; i>=0; i--)
            {
                //move head
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                        break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    int maxXPos = pictureBox1.Size.Width / Settings.Width;
                    int maxYPos = pictureBox1.Size.Height / Settings.Height;

                    //colisao borda
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    for (int j = 1; j <Snake.Count; j++)
                    {
                        if (Snake[i].X==Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    if(Snake[0].X==food.X && Snake[0].Y ==food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }
        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            GenerateFood();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
