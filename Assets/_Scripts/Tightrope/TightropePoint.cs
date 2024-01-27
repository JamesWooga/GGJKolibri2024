using UnityEngine;

namespace Tightrope
{
    public class TightropePoint : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private SpringJoint2D _springJoint2D;
        [SerializeField] private Rigidbody2D _rigidbody;

        private Rigidbody2D _attachedPoint;
        private bool _isAttached;
        
        public void SetAttachPoint(Rigidbody2D point)
        {
            _springJoint2D.connectedBody = point;
            _lineRenderer.positionCount = 2;
            _attachedPoint = point;
            _isAttached = true;
        }

        private void FixedUpdate()
        {
            if (_isAttached)
            {
                UpdatePositions();
            }
        }

        private void UpdatePositions()
        {
            _lineRenderer.SetPosition(0, _rigidbody.position);
            _lineRenderer.SetPosition(1, _attachedPoint.position);
        }
    }
}