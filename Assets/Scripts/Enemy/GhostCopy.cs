using UnityEngine;
using System.Collections;

public class GhostCopy : MonoBehaviour {

    [HideInInspector]
    public bool MovedByGhost;

    [HideInInspector]
    public GhostMovement ghost;

    [HideInInspector]
    public Vector3 IslePosition;

    void Update()
    {     
        if (MovedByGhost)
        {
            transform.position = IslePosition + ghost.getPositionInNav();
            transform.rotation = ghost.transform.rotation;
        }
    }

    public Vector3 getPositionOnIsle()
    {
        return transform.position - IslePosition;
    }
}
