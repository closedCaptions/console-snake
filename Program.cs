using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace console_snake {

    internal class Program {

        [DllImport("user32.dll")]
        public static extern bool ShowWindow (IntPtr hWnd, int cmdShow);

        private static void Maximize () {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        public static Game game = new Game(15, 3);
        public static Random rnd = new Random();

        private static void Main (string[] args) {
            Maximize();

            game.UpdateGUI();
            game.DrawGUI();

            // Game loop
            while (!game.Finished) {
                if (DateTime.Now.Millisecond % 1000 == 0) { // Every 1 second, generate food
                    game[rnd.Next(0, game.boardSize), rnd.Next(0, game.boardSize)] = DrawState.Food;
                }

                if (DateTime.Now.Millisecond % 125 == 0) {
                    Console.Clear();

                    if (Console.KeyAvailable) { // Listen for key pressed
                        switch (Console.ReadKey(true).Key) {
                            case ConsoleKey.UpArrow:
                                if (game.snake[0].direction != Direction.Down)
                                    game.snake.ChangeDirection(Direction.Up);
                                break;

                            case ConsoleKey.DownArrow:
                                if (game.snake[0].direction != Direction.Up)
                                    game.snake.ChangeDirection(Direction.Down);
                                break;

                            case ConsoleKey.LeftArrow:
                                if (game.snake[0].direction != Direction.Right)
                                    game.snake.ChangeDirection(Direction.Left);
                                break;

                            case ConsoleKey.RightArrow:
                                if (game.snake[0].direction != Direction.Left)
                                    game.snake.ChangeDirection(Direction.Right);
                                break;

                            case ConsoleKey.Escape:
                                Environment.Exit(0);
                                break;
                        }
                    }

                    game.snake.MoveSegments();
                    game.snake.UpdateDirection();

                    // Maybe after the move the user lost or won
                    if (game.Finished) {
                        game.DrawGUI();
                        break;
                    }

                    // If snake has eaten the food
                    if (game[(int) game.snake[0].position.Y, (int) game.snake[0].position.X] == DrawState.Food) {
                        game.snake.AddSegment(game);
                        game[(int) game.snake[0].position.Y, (int) game.snake[0].position.X] = DrawState.Blank;
                    }

                    game.UpdateGUI();
                    game.DrawGUI();
                }
            }

            Console.WriteLine("SCORE: " + (game.snake.Length - 3) + "\n");
            Console.WriteLine("Will end in 3 seconds..");

            int startTime = DateTime.Now.Second;
            while (DateTime.Now.Second - startTime != 3) { }
        }
    }
}