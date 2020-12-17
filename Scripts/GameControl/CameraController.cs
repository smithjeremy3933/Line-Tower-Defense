using UnityEngine;
using UnityEngine.InputSystem;

namespace LTD.Controller
{
    public class CameraController : MonoBehaviour
    {
        public InputAction horizontalMoveAction;
        public InputAction verticalMoveAction;

        bool _isMovingVert = false;
        bool _isMovingHoriz = false;
        float _xSpeed = 3f;
        float _ySpeed = 3f;
        float _xRange = 6f;
        float _currHorizDir = 0;
        float _currVertDir = 0;

        private void Awake()
        {
            verticalMoveAction.performed += VerticalMoveAction_performed;
            verticalMoveAction.canceled += VerticalMoveAction_canceled;
            horizontalMoveAction.started += HorizontalMoveAction_started;
            horizontalMoveAction.canceled += HorizontalMoveAction_canceled;
        }

        private void Update()
        {
            if (_isMovingHoriz)
            {
                MoveHoriz();
            }

            if (_isMovingVert)
            {
                MoveVert();
            }
        }

        private void HorizontalMoveAction_started(InputAction.CallbackContext obj)
        {         
            if (!_isMovingHoriz)
            {
                _isMovingHoriz = true;
                _currHorizDir = obj.action.ReadValue<float>();
            }
        }

        private void HorizontalMoveAction_canceled(InputAction.CallbackContext obj)
        {
            _isMovingHoriz = false;
            _currHorizDir = 0;
        }


        private void MoveHoriz()
        {
            float yOffset = _currHorizDir * _ySpeed * Time.deltaTime;
            float rawNewYPos = transform.position.z + yOffset;
            transform.position = new Vector3(transform.position.x, transform.position.y, rawNewYPos);
        }

        private void VerticalMoveAction_performed(InputAction.CallbackContext obj)
        {
            if (!_isMovingVert)
            {
                _isMovingVert = true;
                _currVertDir = obj.action.ReadValue<float>();
            }
        }

        private void VerticalMoveAction_canceled(InputAction.CallbackContext obj)
        {
            _isMovingVert = false;
            _currVertDir = 0;
        }


        private void MoveVert()
        {
            float xOffset = _currVertDir * _xSpeed * Time.deltaTime;
            float rawNewXPos = transform.position.x + xOffset;
            transform.position = new Vector3(rawNewXPos, transform.position.y, transform.position.z);
        }

        private void OnEnable()
        {
            horizontalMoveAction.Enable();
            verticalMoveAction.Enable();
        }
    }
}

