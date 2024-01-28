using System.Collections;
using _Scripts.GameState;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Scenes
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _poleLeft;
        [SerializeField] private Transform _poleRight;
        [SerializeField] private float _polePosition;
        [SerializeField] private float _poleMoveDuration;
        [SerializeField] private float _boxColliderWidth;
        [SerializeField] private float _lineRenderPoint;

        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;

        private void Start()
        {
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                _poleLeft.DOLocalMove(new Vector3(_polePosition, 0f, 0f), _poleMoveDuration);
                _poleRight.DOLocalMove(new Vector3(-_polePosition, 0f, 0f), _poleMoveDuration);
                DOTween.To(() => _boxCollider.size, newSize => _boxCollider.size = newSize, new Vector2(_boxColliderWidth, _boxCollider.size.y), _poleMoveDuration);

                StartCoroutine(TweenLineRenderer(0, _poleMoveDuration, -_lineRenderPoint));
                StartCoroutine(TweenLineRenderer(_lineRenderer.positionCount - 1, _poleMoveDuration, _lineRenderPoint));
            }
        }
        
        private IEnumerator TweenLineRenderer(int index, float duration, float finalPos)
        {
            var t = 0f;
            var startPos = _lineRenderer.GetPosition(index);
            while(t <= duration)
            {
                var pos = Mathf.Lerp(startPos.x, finalPos, t);
                _lineRenderer.SetPosition(index, new Vector3(pos, startPos.y, startPos.z));
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}