using System;
using games.almost_purrfect.fastcube.level_generation_utils;
using UnityEngine;
using Random = System.Random;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class LevelGeneratorBehaviour : MonoBehaviour
    {

        public static int NumberOfTilesInLevel;

        [Header("Generator settings")] [SerializeField]
        private int seed = 42;

        [SerializeField] private int initialTilesCount = 20;

        [SerializeField] private int minimumTilesCount = 20;

        [SerializeField] private float stepSize = 1.2f;

        [Header("Game Objects")] [SerializeField]
        private GameObject groundPrefab;

        private Vector3 _lastPosition;

        private void Start()
        {
            Generator.Random = new Random(seed);
            GenerateInitialGrounds();
        }

        private void Update()
        {
            for (var i = NumberOfTilesInLevel; i <= minimumTilesCount; i++)
            {
                _lastPosition = Generator.PickNextPosition(stepSize, _lastPosition);
                Instantiate(groundPrefab, _lastPosition, Quaternion.identity);
                NumberOfTilesInLevel++;
            }
        }

        private void GenerateInitialGrounds()
        {
            _lastPosition = new Vector3(0f, 0f, 0f);
            for (var i = 0; i < initialTilesCount; i++)
            {
                var nextPosition = Generator.PickNextPosition(stepSize, _lastPosition);
                Instantiate(groundPrefab, nextPosition, Quaternion.identity);
                _lastPosition = nextPosition;
            }

            NumberOfTilesInLevel = initialTilesCount;

            GameManager.LevelReady = true;
        }
    }
}
