using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace games.almost_purrfect.fastcube.level_generation_utils
{
    public static class Generator
    {
        public static Random Random;

        private static int _zChangesCount;

        public static Vector3 PickNextPosition(float stepSize, Vector3 lastPosition)
        {
            var direction = PickDirection();

            Vector3 shift;

            switch (direction)
            {
                case Direction.Up:
                    shift = new Vector3(0f, 0f, stepSize);
                    break;
                case Direction.Left:
                    shift = new Vector3(-stepSize, 0f, 0f);
                    break;
                case Direction.Right:
                    shift = new Vector3(stepSize, 0f, 0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return lastPosition + shift;
        }

        private static Direction PickDirection()
        {
            var allowedDirections = GetAllowedDirections();
            var rand = Random.Next(0, allowedDirections.Count);
            return allowedDirections[rand];
        }

        private static List<Direction> GetAllowedDirections()
        {
            var moveLeftOrRight = _zChangesCount >= 2 && Random.Next(2) == 1;

            if (moveLeftOrRight)
            {
                _zChangesCount = 0;
                return new List<Direction> {Direction.Left, Direction.Right};
            }

            _zChangesCount++;
            return new List<Direction> {Direction.Up};
        }

        private enum Direction
        {
            Up,
            Left,
            Right
        }
    }
}
