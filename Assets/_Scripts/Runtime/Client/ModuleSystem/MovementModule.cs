using DebugingUtility;
using Runtime.Clients.ModuleSystem;
using Runtime.Shared.Packet;
using UnityEngine;

namespace Runtime.Clients.Agents
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementModule : NetworkObject, IModule
    {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;

        private CharacterController _controller;

        private Vector3 _velocity;
        private Vector3 _moveDirection;
        
        private bool _isGrounded => _controller.isGrounded;
        
        private float _moveSpeed = 7f;
        private float _jumpHeight = 2.5f;
        private float _gravity = -25f; 
        private float _groundDistance = 0.2f;
        private float _horizontalInput;

        public void Initialize(ModuleOwner owner)
        {
            _controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            HandleGravity();
            HandleRotation();
            CalculateVelocity();

            _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
            _controller.Move(_velocity * Time.deltaTime);
        }

        protected override void Sync(IPacket packet)
        {
            CustomLog.Log("나중에 동기화 시켜야 할 부분");
        }

        public void SetHorizontalInput(float direction)
        {
            _horizontalInput = direction;
        }

        public void Jump()
        {
            if(_isGrounded)
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
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
