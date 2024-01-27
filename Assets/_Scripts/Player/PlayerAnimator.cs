using Spine;
using Spine.Unity;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private const string AnimSmile = "animation_smile";
        private const string AnimDead = "animation_dead";
        private const string AnimFrown = "animation_frown";
        private const string AnimOop = "animation_oop";
        
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Vector2 _fromMovementRange;
        [SerializeField] private Vector2 _speedRange;
        [SerializeField] private PlayerController _playerController;

        private TrackEntry _anim;
        
        private void Start()
        {
            _anim = _skeletonAnimation.AnimationState.SetAnimation(0, "animation_smile", true);
        }

        private void Update()
        {
            var mapped = _rigidbody.velocity.magnitude.Remap(_fromMovementRange.x, _fromMovementRange.y, _speedRange.x, _speedRange.y);
            _anim.TimeScale = mapped;
        }
    }
}