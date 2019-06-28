using System;
using System.Collections.Generic;
using System.Text;
using console_snake.utility;
using System.Numerics;

namespace console_snake {
    class Snake {

        // parent-child system is based on index. It means that
        // [1] is a child of [0], [2] is a child of [1], and vice versa
        private List<Segment> segments { get; set; } = new List<Segment>();

        public Segment this[int i] {
            get {
                return segments[i];
            }
            private set {
                segments[i] = value;
            }
        }

        public int Length { get { return segments.Count; } }

        public Snake (int snakeLength, int boardSize) {
            if (boardSize < snakeLength * 2) {
                throw new Exception("boardSize is less than twice the snakeLength");
            }

            int midIdx = (boardSize + 1) / 2 - 1;

            for (int i = 0; i < snakeLength; i++) {
                Segment newSegment = new Segment(new Vector2(midIdx), Direction.Right);

                if (i != 0) {
                    newSegment = new Segment(new Vector2(midIdx - i, midIdx), Direction.Right);
                }

                segments.Add(newSegment);
            }
        }

        #region Method

        public void ChangeDirection (Vector2 direction) {
            this[0].direction = direction;
        }

        public void Move () { // All methods kept in one method
            AddDirection();
            UpdateDirection();
        }

        private void UpdateDirection () {
            for (int i = Length - 1; i > 0; i--) {
                if (i != 0) { // Change itself according to it's parent
                    if (this[i].position == this[i - 1].positionChanged) {
                        this[i].direction = this[i - 1].direction;
                    }
                }
            }
        }

        public void AddSegment () {
            segments.Add(new Segment(this[Length - 1].lastPosition, this[Length - 1].lastDirection));
        }

        #endregion

        private void AddDirection () {
            for (int i = 0; i < this.Length; i++) {
                this[i].position += this[i].direction;
            }
        }
    }

    // Accesable only inside this file
    internal class Segment {
        #region Variable

        private Vector2 refDirection { get; set; }
        private Vector2 refPosition { get; set; }

        public Vector2 position {
            get {
                return refPosition;
            }
            set {
                lastPosition = refPosition;
                refPosition = value;
            }
        }
        // Position before adding direction; Used in GUI
        public Vector2 lastPosition { get; private set; }
        // Position that the direction changed; Used in segments following the parents
        public Vector2 positionChanged { get; private set; }
        public Vector2 direction {
            get {
                return refDirection;
            }
            set {
                lastDirection = refDirection;
                positionChanged = position;
                refDirection = value;
            }
        }
        public Vector2 lastDirection { get; private set; }

        #endregion

        public Segment (Vector2 position, Vector2 direction) {
            this.position = position;
            this.direction = direction;
        }
    }
}
