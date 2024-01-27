using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tightrope
{
    public class TightropeCreator : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _leftAttach;
        [SerializeField] private Rigidbody2D _rightAttach;
        [SerializeField] private Rigidbody2D _attachPointPrefab;
        [SerializeField] private int _amount;
        [SerializeField] private float _springFrequency = 20f;

        private readonly List<Rigidbody2D> _rigidbodies = new();

        private void Awake()
        {
            Create();
        }

        private void Create()
        {
            var leftPoint = _leftAttach.transform.position;
            var rightPoint = _rightAttach.transform.position;
            var distance = Mathf.Abs(leftPoint.x) + Mathf.Abs(rightPoint.x);
            var step = distance / (_amount + 1);

            for (int i = 0; i < _amount; i++)
            {
                var xPos = leftPoint.x + step * (i + 1);
                var pos = new Vector3(xPos, leftPoint.y);
                var instance = Instantiate(_attachPointPrefab, pos, Quaternion.identity);
                var spring = instance.GetComponent<SpringJoint2D>();

                _rigidbodies.Add(instance);
                AttachSpring(spring, i);

                spring.distance = step;
                spring.frequency = _springFrequency;
            }

            var rightSpring = _rightAttach.GetComponent<SpringJoint2D>();
            rightSpring.connectedBody = _rigidbodies.Last();
            rightSpring.distance = step;
            rightSpring.frequency = _springFrequency;
        }

        private void AttachSpring(SpringJoint2D joint, int index)
        {
            joint.connectedBody = index == 0 ? _leftAttach : _rigidbodies[index - 1];
        }
    }
}