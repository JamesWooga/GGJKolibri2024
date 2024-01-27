using Spine;
using Spine.Unity;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Vector2 _fromMovementRange;
        [SerializeField] private Vector2 _speedRange;

        private TrackEntry _anim;
        
        private void Start()
        {
            _anim = _skeletonAnimation.AnimationState.SetAnimation(0, "animation", true);
        }

        private void Update()
        {
            var mapped = _rigidbody.velocity.magnitude.Remap(_fromMovementRange.x, _fromMovementRange.y, _speedRange.x, _speedRange.y);
            _anim.TimeScale = mapped;
        }
    }
}