using UnityEngine;
using System.Collections;

public class justLook : MonoBehaviour {
	void OnPreCull()
    {
        transform.LookAt(ObjectPool.getObjectPool().getPlayer().transform);
	}
}
