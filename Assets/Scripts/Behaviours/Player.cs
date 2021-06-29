using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class Player : MonoBehaviour
    {

        public List<Vector2> _inputBuffer;

        [SerializeField]
        private float moveDistance;

        [SerializeField]
        private float moveDuration;

        [SerializeField]
        private LayerMask groundCheckLayerMask;

        private Vector3 _originalPosition;

        private Vector3 _targetPosition;

        private bool _isMoving;

        private bool _isGrounded;

        private float _currentMoveTime;

        private void Start()
        {
            _inputBuffer = new List<Vector2>(5);
            _originalPosition = _targetPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var t = transform;
            _isGrounded = Physics.Raycast(t.position, Vector3.down, 1f, groundCheckLayerMask);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            var t = transform;

            if (_inputBuffer.Count > 0)
            {
                if (!_isMoving)
                {
                    var currentInput = _inputBuffer[0];

                    if (math.abs(currentInput.x) > .1f)
                    {
                        _currentMoveTime = 0f;
                        _originalPosition = t.position;
                        _targetPosition = _originalPosition +
                                          new Vector3(
                                              currentInput.x > 0f ? moveDistance : -moveDistance, 0f, 0f);
                    }

                    if (math.abs(currentInput.y) > .1f)
                    {
                        _currentMoveTime = 0f;
                        _originalPosition = t.position;
                        _targetPosition = _originalPosition +
                                          new Vector3(0f, 0f,
                                              currentInput.y > 0f ? moveDistance : -moveDistance);
                    }

                    _inputBuffer.RemoveAt(0);
                }
            }

            if (_isGrounded)
            {
                if (_currentMoveTime <= moveDuration)
                {
                    _currentMoveTime += dt;
                    t.position = math.lerp(_originalPosition, _targetPosition,
                        _currentMoveTime / moveDuration);
                    _isMoving = true;
                }
                else
                {
                    t.position = _targetPosition;
                    _isMoving = false;
                }
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            var input = context.ReadValue<Vector2>();
            _inputBuffer.Add(input);
        }
    }
}
