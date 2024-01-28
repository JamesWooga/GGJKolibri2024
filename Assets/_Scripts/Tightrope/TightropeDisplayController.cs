using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TightropeDisplayController : MonoBehaviour
{
    private LineRenderer tightrope;
    private EdgeCollider2D edgeCollider;

    public float tightropeAdjacentDistance = 0.4f;

    public Vector3 ropeInitialPosition = new Vector3(-0.5f, 2.48f, 1);

    private Vector3[] _initialPositions;
    
    void Awake()
    {
      tightrope = GetComponent<LineRenderer>();
      edgeCollider = GetComponent<EdgeCollider2D>();
      _initialPositions = new Vector3[tightrope.positionCount];
      tightrope.GetPositions(_initialPositions);
    }

    void OnCollisionStay2D(Collision2D other)
    {
       Vector3 collisionPoint = other.GetContact(0).point;
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       tightrope.SetPosition(1, collisionPoint - collisionOffset - (collisionOffset/2));
       tightrope.SetPosition(2, collisionPoint - collisionOffset);
       tightrope.SetPosition(3, collisionPoint);
       tightrope.SetPosition(4, collisionPoint + collisionOffset);
       tightrope.SetPosition(5, collisionPoint + collisionOffset + (collisionOffset/2));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
       Vector3 collisionPoint = other.GetContact(0).point;
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       tightrope.SetPosition(1, collisionPoint - collisionOffset- (collisionOffset/2));
       tightrope.SetPosition(2, collisionPoint- collisionOffset);
       tightrope.SetPosition(3, collisionPoint);
       tightrope.SetPosition(4, collisionPoint + collisionOffset);
       tightrope.SetPosition(5, collisionPoint + collisionOffset + (collisionOffset/2));
    }

    void OnCollisionExit2D(Collision2D other)
    {
       tightrope.SetPositions(_initialPositions);
    }
   
}
