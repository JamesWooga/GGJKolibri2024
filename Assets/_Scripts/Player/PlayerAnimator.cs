using Spine.Unity;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private void Start()
        {
            var anim = _skeletonAnimation.AnimationState.SetAnimation(0, "animation", true);
            anim.TimeScale = _speed;
        }
    }
}