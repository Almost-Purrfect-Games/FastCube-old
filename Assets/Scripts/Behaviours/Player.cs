using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace games.almost_purrfect.fastcube.behaviours
{
    public class Player : MonoBehaviour
    {
        public List<Vector2> _inputBuffer;

        [SerializeField] private float moveDistance;

        [SerializeField] private float moveDuration;

        [SerializeField] private LayerMask groundCheckLayerMask;

        private Vector3 _originalPosition;

        private Vector3 _targetPosition;

        private bool _isMoving;

        private bool _wasMovingLastFrame;

        private bool _isGrounded;

        private float _currentMoveTime;

        private Touch _swipeStartTouch;

        private void Start()
        {
            GameStateManager.PlayerMoved = false;
            _inputBuffer = new List<Vector2>(5);
            _originalPosition = _targetPosition = transform.position;

            TouchSimulation.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void OnEnable()
        {
            GameStateManager.OnGamePaused += OnGamePaused;
            GameStateManager.OnGameUnpaused += OnGameUnpaused;
        }

        private void OnDisable()
        {
            GameStateManager.OnGamePaused -= OnGamePaused;
            GameStateManager.OnGameUnpaused -= OnGameUnpaused;
        }

        private void FixedUpdate()
        {
            var t = transform;
            _isGrounded = Physics.Raycast(t.position, Vector3.down, 1f, groundCheckLayerMask);
        }

        private void Update()
        {
            if (!GameStateManager.IsGamePlaying)
                return;

            var dt = Time.deltaTime;
            var t = transform;

            var touchInputVector = HandleTouchInput();

            if (touchInputVector != Vector2.zero)
            {
                _inputBuffer.Add(touchInputVector);
            }

            if (_inputBuffer.Count > 0)
            {
                GameStateManager.PlayerMoved = true;

                if (!_isMoving)
                {
                    var currentInput = _inputBuffer[0];

                    if (math.abs(currentInput.x) > .1f)
                    {
                        _currentMoveTime = 0f;
                        _originalPosition = t.position;
                        _targetPosition = _originalPosition +
                                          new Vector3(
                                              currentInput.x > 0f ? moveDistance : -moveDistance,
                                              0f, 0f);
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
                if (_currentMoveTime <= moveDuration && t.position != _targetPosition)
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

            if (!_isMoving && _wasMovingLastFrame)
            {
                GameStateManager.CurrentScore++;
            }

            _wasMovingLastFrame = _isMoving;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!context.performed || !enabled) return;

            var input = context.ReadValue<Vector2>();
            _inputBuffer.Add(input);
        }

        private Vector2 HandleTouchInput()
        {
            var verticalAxis = 0;
            var horizontalAxis = 0;

            Debug.Log($"Active fingers: {Touch.activeFingers.Count}");

            if (Touch.activeFingers.Count != 1) return Vector2.zero;

            if (EventSystem.current.IsPointerOverGameObject()) return Vector2.zero;

            var activeTouch = Touch.activeFingers[0].currentTouch;
            Debug.Log($"Phase: {activeTouch.phase}");

            if (activeTouch.phase == TouchPhase.Began)
            {
                _swipeStartTouch = activeTouch;
            }

            if (activeTouch.phase != TouchPhase.Ended) return Vector2.zero;

            var swipeEndTouch = activeTouch;

            var swipeVector = _swipeStartTouch.startScreenPosition -
                              swipeEndTouch.screenPosition;

            Debug.Log($"Swipe vector: {swipeVector}");

            if (swipeVector.x < 0) // Right
            {
                if (swipeVector.y < 0) // Up (move forward)
                {
                    verticalAxis = 1;
                }

                if (swipeVector.y > 0) // Down (move right)
                {
                    horizontalAxis = 1;
                }
            }

            if (swipeVector.x > 0) // Left
            {
                if (swipeVector.y < 0) // Up (move left)
                {
                    horizontalAxis = -1;
                }

                if (swipeVector.y > 0) // Down (move backwards)
                {
                    verticalAxis = -1;
                }
            }

            return new Vector2(horizontalAxis, verticalAxis);
        }

        private void OnGamePaused()
        {
            _inputBuffer.Clear();
            _swipeStartTouch = default;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void OnGameUnpaused()
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
