using UnityEngine;

namespace Runtime.ModuleSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementModule : MonoBehaviour, IModule
    {
        //FSM 만들기 전 임시 코드
        [SerializeField] RenderModule render;
        private bool _isPlayingIdle = false;
        private bool _isPlayingRun = false;
        private bool _isPlayingJump = false;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;

        private CharacterController _controller;

        private Vector3 _velocity;
        private Vector3 _moveDirection;
        
        private bool _isGrounded => _controller.isGrounded;
        
        private float _moveSpeed = 7f;
        private float _jumpHeight = 2.5f;
        private float _gravity = -25f; 
        private float _horizontalInput;

        public void Initialize(ModuleOwner owner)
        {
            _controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            //FSM 만들기 전 임시 코드
            if (_horizontalInput == 0f && _isGrounded && !_isPlayingIdle)
            {
                _isPlayingIdle = true;

                int hash = Animator.StringToHash("IDLE");
                render.PlayClip(hash, 0, 0);

                _isPlayingRun = false;
                _isPlayingJump = false;
            }
            else if (_horizontalInput != 0f && _isGrounded && !_isPlayingRun)
            {
                _isPlayingRun = true;

                int hash = Animator.StringToHash("RUN");
                render.PlayClip(hash, 0, 0);

                _isPlayingIdle = false;
                _isPlayingJump = false;
            }
            else if (!_isGrounded && !_isPlayingJump)
            {
                _isPlayingJump = true;

                int hash = Animator.StringToHash("JUMP");
                render.PlayClip(hash, 0, 0);

                _isPlayingIdle = false;
                _isPlayingRun = false;
            }

            HandleGravity();
            HandleRotation();
            CalculateVelocity();

            _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
            _controller.Move(_velocity * Time.deltaTime);
        }

        public void SetHorizontalInput(float input)
        {
            _horizontalInput = input;
        }

        public void Jump()
        {
            if (_isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }

        private void CalculateVelocity()
        {
            _moveDirection = new Vector3(_horizontalInput, 0f, 0f).normalized;
        }

        private void HandleGravity()
        {
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            _velocity.y += _gravity * Time.deltaTime;
        }

        private void HandleRotation()
        {
            if (_horizontalInput > 0)
                transform.rotation = Quaternion.Euler(0f, 90f, 0f); 
            else if (_horizontalInput < 0)
                transform.rotation = Quaternion.Euler(0f, -90f, 0f); 
        }
    }
}
