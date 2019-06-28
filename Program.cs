using System;
using System.Diagnostics;
using System.Numerics;

namespace console_snake {
    class Program {

        public static int Time = 0;

        static void Main (string[] args) {
            Game game = new Game(17, 3);
            Console.WindowHeight = (int) game.boardSize * 2;
            Console.WindowHeight = (int) game.boardSize * 2;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            game.UpdateGUI();
            game.DrawGUI();

            // Actual game loop
            while (!game.Finished) {

                if (Console.KeyAvailable) { // Listen for keys
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
                    }
                }

                // Every 100 ms..
                // change direction of the snake, move the snake, update GUI, and draw GUI. 
                if (sw.ElapsedMilliseconds % 100 == 0) {

                    game.snake.Move();
                    Console.Clear();

                    if (game.Finished)
                        break;

                    game.UpdateGUI();
                    game.DrawGUI();
                }   
            }

            // Don't UpdateGUI() or else there would be an OutOfBounds exception/error
            game.DrawGUI();

            Console.WriteLine(game.Score);
        }
    }
}
