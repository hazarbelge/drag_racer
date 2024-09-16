using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform target;
        
        private Vector3 _previousMousePos;
        private bool _isDragging;

        private CinemachineTransposer _transposer;
        private CinemachineComposer _composer;

        public void Init()
        {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }

        public void OnCountdownStart()
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }

        public void OnRaceEnd()
        {
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _previousMousePos = eventData.position;
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                RotateAroundTarget(eventData.position - (Vector2)_previousMousePos);
                _previousMousePos = eventData.position;
            }
        }

        private void RotateAroundTarget(Vector3 deltaMouse)
        {
            _transposer.m_FollowOffset = Quaternion.AngleAxis(deltaMouse.x * 0.2f, Vector3.up) * _transposer.m_FollowOffset;
            _composer.m_TrackedObjectOffset = Vector3.zero;
        }
    }
}
