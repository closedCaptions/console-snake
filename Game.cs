using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Linq;
using System.Text;

enum DrawState { Blank, Segment, Food };
public static class Direction {
    public static Vector2 Up = new Vector2(0, -1);
    public static Vector2 Down = new Vector2(0, 1);
    public static Vector2 Right = new Vector2(1, 0);
    public static Vector2 Left = new Vector2(-1, 0);
}

namespace console_snake {

    class Game {

        #region Variable

        private DrawState[,] drawBoard { get; set; }
        public Snake snake { get; }

        public int boardSize { get; }

        public bool Finished {
            get {
                return Collided() || ExceededCount();
            }
        }

        public int Score {
            get {
                if (Collided()) {
                    return -1;
                }else if (ExceededCount()) {
                    return 1;
                }else {
                    return 0;
                }
            }
        }

        #endregion

        public Game (int boardSize, int snakeLength) {
            if (boardSize * 2 > Console.LargestWindowWidth || boardSize * 2 > Console.LargestWindowHeight) {
                throw new Exception("Set a boardSize smaller than " + (int) Console.LargestWindowHeight / 2);
            }

            drawBoard = new DrawState[boardSize, boardSize];
            snake = new Snake(snakeLength, boardSize);
            this.boardSize = boardSize;
        }

        #region Method

        public void DrawGUI () {
            for (int y = 0; y < boardSize; y++) {
                Console.WriteLine(string.Join("--- ", new string[boardSize + 1]));
                for (int x = 0; x < boardSize; x++) {
                    string toWrite = "■";

                    switch (drawBoard[y, x]) {
                        case DrawState.Food:
                            toWrite = "O";
                            break;
                        case DrawState.Blank:
                            toWrite = " ";
                            break;
                    }

                    Console.Write($" {toWrite} |");
                }
                Console.WriteLine();
            }
        }

        public void UpdateGUI () {
            for (int i = 0; i < snake.Length; i++) {
                if (snake[i].position != snake[i].lastPosition) {
                    drawBoard[(int) snake[i].lastPosition.Y, (int) snake[i].lastPosition.X] = DrawState.Blank;
                }

                drawBoard[(int) snake[i].position.Y, (int) snake[i].position.X] = DrawState.Segment;
            }
        }

        #region Winning Methods

        public bool Collided () {
            for (int i = 0; i < snake.Length; i++) {
                // Collided into the wall
                try {
                    if (CollidedWall(snake[0]) || CollidedSelf(snake[0], snake[i])) {
                        return true;
                    }
                }catch {
                    if (CollidedWall(snake[0])) {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CollidedWall (Segment segment) {
            return segment.position.Y >= boardSize || segment.position.Y < 0 ||
                   segment.position.X >= boardSize || segment.position.X < 0;
        }

        private bool CollidedSelf (Segment target, Segment toCompare) {
            if (target == toCompare) {
                return false;
            }

            return target.position == toCompare.position;
        }

        public bool ExceededCount () {
            return snake.Length > Math.Pow(boardSize, 2);
        }

        #endregion

        #endregion

    }
}
