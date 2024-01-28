using System.Linq;
using _Scripts.Player;
using _Scripts.Sounds;
using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace _Scripts.Objects
{
    public class DroppedObject : MonoBehaviour
    {
        [SerializeField] private DroppedObjectType _type;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GameObject _childCollider;
        [SerializeField] private float _weight;
        [SerializeField] private Vector2 _randomRotateLimits;
        [SerializeField] private GameObject _ropeHitVFX;

        public float Weight => _weight;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        private Tween _tween;

        private void OnEnable()
        {
            var value = _randomRotateLimits.RandomBetweenXAndY();
            var sign = UnityEngine.Random.Range(0, 1f) > 0.5f ? 1f : -1;
            _tween = // Use DOTween to animate the rotation
                transform.DORotate(new Vector3(0f, 0f, 360f * sign), value, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Incremental)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            
            SoundsPlayer.Instance.PlaySound(_type, SoundsConfig.SoundType.Appear);
        }

        public void SetTag(string newTag)
        {
            gameObject.tag = newTag;
            _childCollider.tag = newTag;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "DroppedObjectLeft":
                    FindAttachPoint(true);
                    break;
                case "DroppedObjectRight":
                    FindAttachPoint(false);
                    break;
                case "Death":
                    Destroy(gameObject);
                    break;
                case "PlayerLeft":
                case "PlayerRight":
                    TryToAttachToCatchPoint(other);
                    break;
                case "Rope":
                    GameObject.Instantiate(_ropeHitVFX, transform.position, transform.rotation);
                    SoundsPlayer.Instance.PlaySound(_type, SoundsConfig.SoundType.Crash);
                    GameEvents.GameEvents.ObstacleHitRope(this);
                    Destroy(gameObject);
                    break;
            }
        }

        private void TryToAttachToCatchPoint(Component other)
        {
            _tween.Kill();
            _tween = null;
            if (other.TryGetComponent<PlayerCatchPoint>(out var component))
            {
                SoundsPlayer.Instance.PlaySound(_type, SoundsConfig.SoundType.Catch);
                component.Attach(this);
            }
        }

        private void FindAttachPoint(bool left)
        {
            _tween.Kill();
            _tween = null;
            var attachPoints = FindObjectsOfType<PlayerCatchPoint>();
            var correct = attachPoints.FirstOrDefault(e => e.Left == left);

            if (correct == null)
            {
                Debug.LogError($"Couldn't find attach point. You may need to check there are 2 catch points in the scene");
                return;
            }

            SoundsPlayer.Instance.PlaySound(_type, SoundsConfig.SoundType.Catch);
            correct.Attach(this);
        }
    }
}