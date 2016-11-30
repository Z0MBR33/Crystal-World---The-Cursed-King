using UnityEngine;
using System.Collections;

public class NavMeshTarget : MonoBehaviour {

    public Vector3 IslePosition;

    public Vector3 getPositionOnIsle()
    {
        return transform.position - IslePosition;
    }
}
