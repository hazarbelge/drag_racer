using Cinemachine;
using UnityEngine;
using static Enums;

namespace Controller
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform target;

        private GameController _gameController;
        
        private Vector3 _previousMousePos;
        private bool _isDragging;

        private CinemachineTransposer _transposer;
        private CinemachineComposer _composer;

        public void Init(GameController gameController)
        {
            _gameController = gameController;
            
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }

        public void OnRaceStart()
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }

        public void OnRaceEnd()
        {
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
        }

        private void Update()
        {
            if (_gameController.GameState == GameState.Menu) return;
            
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousMousePos = Input.mousePosition;
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                RotateAroundTarget(Input.mousePosition - _previousMousePos);
                _previousMousePos = Input.mousePosition;
            }
        }

        private void RotateAroundTarget(Vector3 deltaMouse)
        {
            _transposer.m_FollowOffset = Quaternion.AngleAxis(deltaMouse.x * 0.2f, Vector3.up) * _transposer.m_FollowOffset;
            _composer.m_TrackedObjectOffset = Vector3.zero;
        }
    }
}