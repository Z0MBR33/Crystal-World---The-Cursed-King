using UnityEngine;
using System.Collections;

public class justLook : MonoBehaviour {
	void OnPreCull()
    {
        transform.LookAt(ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player).transform);
	}
}
