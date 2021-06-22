using System;
using Unity.Mathematics;
using UnityEngine;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class Ground : MonoBehaviour
    {

        [SerializeField]
        private float lifetime;

        private bool _visitedByPlayer;


        private float _lifetimeDecreaseFactor;

        private void Start()
        {
            _lifetimeDecreaseFactor = transform.localScale.y / lifetime;
        }

        private void Update()
        {
            if (!_visitedByPlayer) return;

            var dt = Time.deltaTime;

            lifetime = math.max(lifetime - dt, 0f);

            if (lifetime == 0)
            {
                Destroy(gameObject);
                return;
            }

            var t = transform;
            var localScale = t.localScale;
            var localPosition = t.localPosition;

            localScale.y -= dt * _lifetimeDecreaseFactor;
            localPosition.y += dt * _lifetimeDecreaseFactor / 2;

            t.localScale = localScale;
            t.localPosition = localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _visitedByPlayer = true;
            }
        }
    }
}
