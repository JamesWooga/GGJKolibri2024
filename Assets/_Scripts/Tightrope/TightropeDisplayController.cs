using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TightropeDisplayController : MonoBehaviour
{
    private LineRenderer _tightrope;

    public float tightropeAdjacentDistance = 0.4f;

    private Vector3[] _initialPositions;

    private void Awake()
    {
      _tightrope = GetComponent<LineRenderer>();
      _initialPositions = new Vector3[_tightrope.positionCount];
      _tightrope.GetPositions(_initialPositions);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
       DistortRope(other);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       DistortRope(other);
    }

    private void DistortRope(Collision2D other)
    {
       Vector3 collisionPoint = other.GetContact(0).point;
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       _tightrope.SetPosition(1, collisionPoint - collisionOffset- (collisionOffset/2));
       _tightrope.SetPosition(2, collisionPoint- collisionOffset);
       _tightrope.SetPosition(3, collisionPoint);
       _tightrope.SetPosition(4, collisionPoint + collisionOffset);
       _tightrope.SetPosition(5, collisionPoint + collisionOffset + (collisionOffset/2));
    }

    private void OnCollisionExit2D(Collision2D other)
    {
       _tightrope.SetPositions(_initialPositions);
    }
   
}
