using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    private ObjectPool mr;

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        mr = ObjectPool.getObjectPool();

        transform.position = position;
        transform.rotation = rotation;

        StartCoroutine(timerHandler());
    }
	
	private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(5);

        StopCoroutine(timerHandler());

        mr.returnObject(gameObject);
        
    }
}
