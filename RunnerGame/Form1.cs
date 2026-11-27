using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RunnerGame
{
    public partial class Form1 : Form
    {
        // Constants for player and obstacle sizes and initial settings
        private const int PlayerWidth = 50;
        private const int PlayerHeight = 50;
        private const int PlayerInitialY = 300;
        private const int PlayerJumpHeight = 200;
        private const int ObstacleWidth = 50;
        private const int ObstacleHeight = 50;
        private const int ObstacleSpeed = 5;

        private PictureBox player;
        private Timer gameTimer;
        private Random random;
        private List<PictureBox> obstacles;

        private bool isJumping;
        private int jumpSpeed;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        // Initialize game elements and settings
        private void InitializeGame()
        {
            // Create player
            player = new PictureBox();
            player.BackColor = System.Drawing.Color.Red;
            player.Size = new System.Drawing.Size(PlayerWidth, PlayerHeight);
            player.Location = new System.Drawing.Point(100, PlayerInitialY);
            this.Controls.Add(player);

            // Initialize game timer
            gameTimer = new Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            // Initialize random generator and obstacle list
            random = new Random();
            obstacles = new List<PictureBox>();

            // Initialize jumping variables
            isJumping = false;
            jumpSpeed = 15;

            // Event handler for spacebar key press to jump
            this.KeyDown += GameForm_KeyDown;
        }

        // Event handler for spacebar key press
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !isJumping)
            {
                isJumping = true;
            }
        }

        // Main game loop
        private void GameLoop(object sender, EventArgs e)
        {
            MovePlayer();
            GenerateObstacles();
            MoveObstacles();
            CheckCollisions();
        }

        // Move the player (jumping logic included)
        private void MovePlayer()
        {
            if (isJumping)
            {
                player.Top -= jumpSpeed;
                jumpSpeed--;

                if (jumpSpeed < 0)
                {
                    isJumping = false;
                    jumpSpeed = 15;
                }
            }
            else if (player.Top < PlayerInitialY)
            {
                player.Top += jumpSpeed;
                jumpSpeed++;
            }
        }

        // Generate obstacles randomly
        private void GenerateObstacles()
        {
            if (random.Next(0, 100) < 10)
            {
                PictureBox obstacle = new PictureBox();
                obstacle.BackColor = System.Drawing.Color.Blue;
                obstacle.Size = new System.Drawing.Size(ObstacleWidth, ObstacleHeight);
                obstacle.Location = new System.Drawing.Point(800, PlayerInitialY);
                this.Controls.Add(obstacle);
                obstacles.Add(obstacle);
            }
        }

        // Move obstacles from right to left
        private void MoveObstacles()
        {
            foreach (PictureBox obstacle in obstacles)
            {
                obstacle.Left -= ObstacleSpeed;

                if (obstacle.Left + ObstacleWidth < 0)
                {
                    obstacles.Remove(obstacle);
                    this.Controls.Remove(obstacle);
                    break;
                }
            }
        }

        // Check for collisions between player and obstacles
        private void CheckCollisions()
        {
            foreach (PictureBox obstacle in obstacles)
            {
                if (player.Bounds.IntersectsWith(obstacle.Bounds))
                {
                    GameOver();
                }
            }
        }

        // Actions to take when the game is over
        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over!");
            Application.Restart(); // Restart the game
        }
    }
}