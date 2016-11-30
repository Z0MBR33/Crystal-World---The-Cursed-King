using UnityEngine;
using System.Collections;

public class GhostCopy : MonoBehaviour {

    public bool MovedByGhost;

    public GhostMovement ghost;

    public Vector3 IslePosition;

    void Start()
    {
        MovedByGhost = true;
    }

    void FixedUpdate()
    {

        if (MovedByGhost)
        {
            transform.position = IslePosition + ghost.getPositionInNav();
        }

    }

    public Vector3 getPositionOnIsle()
    {
        return transform.position - IslePosition;
    }
}
