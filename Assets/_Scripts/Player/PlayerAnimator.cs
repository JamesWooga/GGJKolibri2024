using Spine.Unity;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private float _speed;

        private void Start()
        {
            _skeletonAnimation.AnimationState.SetAnimation(0, "animation", true);
        }
    }
}