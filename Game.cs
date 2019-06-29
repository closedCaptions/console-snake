using System;
using System.Numerics;

// Guides the GUI on what to draw
internal enum DrawState { Blank, Segment, Food };

// For easy access to directions
public static class Direction {
    public static Vector2 Up = new Vector2(0, -1);
    public static Vector2 Down = new Vector2(0, 1);
    public static Vector2 Right = new Vector2(1, 0);
    public static Vector2 Left = new Vector2(-1, 0);
}

namespace console_snake {

    internal class Game {

        #region Variable

        private DrawState[,] drawBoard { get; set; }

        // Indxer
        public DrawState this[int y, int x] {
            get {
                return drawBoard[y, x];
            }
            set {
                drawBoard[y, x] = value;
            }
        }

        public int boardSize { get; }
        public Snake snake { get; }

        public bool Finished {
            get {
                // Dosen't care if you win or lose
                return Collided() || ExceededCount();
            }
        }

        public int Won {
            get {
                if (Collided()) {
                    return -1; // Report lose
                } else if (ExceededCount()) {
                    return 1; // Report win
                } else {
                    return 0; // Nothing happened..?
                }
            }
        }

        #endregion Variable

        public Game (int boardSize, int snakeLength) {
            if (boardSize > Console.LargestWindowWidth / 2 || boardSize > Console.LargestWindowHeight / 2) {
                // Might go beyond the max size
                throw new Exception("Set a boardSize smaller than " + (int) Console.LargestWindowWidth / 2 +
                    " or smaller than " + Console.LargestWindowHeight / 2);
            }

            drawBoard = new DrawState[boardSize, boardSize];
            snake = new Snake(snakeLength, boardSize);
            this.boardSize = boardSize;
        }

        #region Method

        #region GUI

        public void DrawGUI () {
            for (int y = 0; y < boardSize; y++) {
                // Will add a "--- " in between the elements of the string array we created
                Console.WriteLine(string.Join("--- ", new string[boardSize + 1]));
                for (int x = 0; x < boardSize; x++) {
                    // Default string for toWrite is the string for DrawState.Segment
                    string toWrite = "■";

                    switch (drawBoard[y, x]) {
                        case DrawState.Food:
                            // O reperesents the food
                            toWrite = "O";
                            break;

                        case DrawState.Blank:
                            toWrite = " ";
                            break;
                    }

                    Console.Write($" {toWrite} |");
                }
                // Skip a line
                Console.WriteLine();
            }
        }

        public void UpdateGUI () {
            for (int i = 0; i < snake.Length; i++) {
                if (snake[i].position != snake[i].lastPosition) {
                    // Set previous positions as blank
                    drawBoard[(int) snake[i].lastPosition.Y, (int) snake[i].lastPosition.X] = DrawState.Blank;
                }

                // Set current position as the segment ()
                drawBoard[(int) snake[i].position.Y, (int) snake[i].position.X] = DrawState.Segment;
            }
        }

        #endregion

        #region Ending

        private bool Collided () {
            for (int i = 0; i < snake.Length; i++) {
                // If the head (snake[0]) collides with the body (snake[i])
                // or
                // If the head (snake[0]) is colliding with the wall (Which is outside the array bounds?!)
                if (CollidedSelf(snake[0], snake[i]) || CollidedWalls(snake[0])) {
                    return true;
                }
            }

            return false;
        }

        private bool CollidedWalls (Segment segment) {
            return segment.position.Y >= boardSize || segment.position.Y < 0 ||
                   segment.position.X >= boardSize || segment.position.X < 0;
        }

        private bool CollidedSelf (Segment head, Segment body) {
            // Prevents colliding with the same thing
            if (head == body) {
                // Which dosen't make sense so it will always collide with itself
                return false;
            }

            return head.position == body.position;
        }

        public bool ExceededCount () {
            // A = l * w
            return snake.Length > boardSize * boardSize;
        }

        #endregion Ending Methods

        #endregion Method
    }
}