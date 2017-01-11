using UnityEngine;
using System.Collections;

public class justLook : MonoBehaviour {

    public GameObject objectToLookAt;

	void OnPreCull()
    {
        if(objectToLookAt == null)
        {
            objectToLookAt = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player);
        }

        transform.LookAt(objectToLookAt.transform);
	}
}
