using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

    private ObjectPool mr;

    public void Initialize(Vector3 position)
    {
        mr = ObjectPool.getObjectPool();

        transform.position = position;

        StartCoroutine(timerHandler());
    }
	
	private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(5);

        StopCoroutine(timerHandler());

        mr.returnObject(gameObject);
        
    }
}
