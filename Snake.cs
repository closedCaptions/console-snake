using System.Collections.Generic;
using System.Numerics;

namespace console_snake {

    internal class Snake {

        // parent-child thingy is based on index. It means that
        // [1] is a child of [0], [2] is a child of [1], and vice versa
        private List<Segment> segments { get; set; } = new List<Segment>();

        // Indexer
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
            int midIdx = (boardSize + 1) / 2 - 1;

            for (int i = 0; i < snakeLength; i++) {
                Segment newSegment = new Segment(new Vector2(midIdx - i, midIdx), Direction.Right);

                segments.Add(newSegment);
            }
        }

        #region Method

        public void ChangeDirection (Vector2 direction) {
            // Just change the direction of the head
            this[0].direction = direction;
        }

        public void UpdateDirection () {
            for (int i = segments.Count - 1; i > 0; i--) {
                if (i != 0) { // Change itself according to it's parent
                    // EX: Child of 0 is 1, Parent of 2 is 1, and vice versa.
                    if (this[i].position == this[i - 1].positionChanged) {
                        this[i].direction = this[i - 1].direction;
                    }
                }
            }
        }

        public void MoveSegments () {
            for (int i = 0; i < segments.Count; i++) {
                this[i].position += this[i].direction;
            }
        }

        public void AddSegment (Game game) {
            // Easy access
            Segment lastSegment = this[Length - 1];

            Vector2 position = lastSegment.position - lastSegment.direction;
            Vector2 direction = lastSegment.direction;

            // If the position assigned is out of bounds then..
            if ((lastSegment.position - lastSegment.direction).X >= game.boardSize ||
                (lastSegment.position - lastSegment.direction).X < 0 ||
                (lastSegment.position - lastSegment.direction).Y >= game.boardSize ||
                (lastSegment.position - lastSegment.direction).Y < 0) {
                position = lastSegment.lastPosition;
                direction = lastSegment.lastDirection;
            }

            // Add a new segment
            segments.Add(new Segment(position, direction));
        }

        #endregion Method
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

        // Position before change
        public Vector2 lastPosition { get; private set; }

        // Position of the snake when it's direction changed
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

        #endregion Variable

        public Segment (Vector2 position, Vector2 direction) {
            this.position = position;
            this.direction = direction;
        }
    }
}