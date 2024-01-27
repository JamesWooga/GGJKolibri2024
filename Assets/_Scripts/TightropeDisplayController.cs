using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightropeDisplayController : MonoBehaviour
{
    public LineRenderer tightrope;

    public float tightropeAdjacentDistance = 0.4f;

    public Vector3 ropeInitialPosition = new Vector3(-0.5f, 2.48f, 1);

    void OnCollisionStay2D(Collision2D other)
    {
       Vector3 collisionPoint = other.GetContact(0).point;
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       tightrope.SetPosition(1, collisionPoint - collisionOffset);
       tightrope.SetPosition(2, collisionPoint);
       tightrope.SetPosition(3, collisionPoint + collisionOffset);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
       Vector3 collisionPoint = other.GetContact(0).point;
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       tightrope.SetPosition(1, collisionPoint - collisionOffset);
       tightrope.SetPosition(2, collisionPoint);
       tightrope.SetPosition(3, collisionPoint + collisionOffset);
    }

    void OnCollisionExit2D(Collision2D other)
    {
       Vector3 collisionOffset = new Vector3(tightropeAdjacentDistance, 0, 0);
       tightrope.SetPosition(1, ropeInitialPosition - collisionOffset);
       tightrope.SetPosition(2, ropeInitialPosition);
       tightrope.SetPosition(3, ropeInitialPosition + collisionOffset);
    }
}
