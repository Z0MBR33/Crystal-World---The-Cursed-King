using UnityEngine;
using System.Collections;

public class EnemyPoint : MonoBehaviour {

    public Vector3 IslePosition;

    public Vector3 getPositionOnIsle()
    {
        return transform.position - IslePosition;
    }
}
