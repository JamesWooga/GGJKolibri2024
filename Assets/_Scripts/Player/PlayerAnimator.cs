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
        [SerializeField] private float _oopThreshold;
        [SerializeField] private float _frownThreshold;

        private TrackEntry _anim;
        private bool _forceJump;
        
        private void Start()
        {
            _anim = _skeletonAnimation.AnimationState.SetAnimation(0, "animation_smile", true);
            _anim.Complete += HandleAnimComplete;
        }
        
        private void OnDestroy()
        {
            _anim.Complete -= HandleAnimComplete;
        }

        public void Jump()
        {
            _forceJump = true;
        }

        private void HandleAnimComplete(TrackEntry trackentry)
        {
            _anim.Complete -= HandleAnimComplete;
            
            var abs = Mathf.Abs(_playerController.CurrentTilt);
            string state = string.Empty;

            if (_forceJump)
            {
                state = AnimOop;
            }
            else if (abs < _oopThreshold)
            {
                state = AnimSmile;
            }
            else if (abs >= _oopThreshold && abs < _frownThreshold)
            {
                state = AnimOop;
            }
            else if (abs >= _frownThreshold)
            {
                state = AnimFrown;
            }
            
            _anim = _skeletonAnimation.AnimationState.SetAnimation(0, state, true);
            _forceJump = false;

            _anim.Complete += HandleAnimComplete;
        }

        private void Update()
        {
            var mapped = _rigidbody.velocity.magnitude.Remap(_fromMovementRange.x, _fromMovementRange.y, _speedRange.x, _speedRange.y);
            _anim.TimeScale = mapped;
        }
    }
}