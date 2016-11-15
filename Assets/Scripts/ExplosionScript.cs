using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(timerHandler());
    }
	
	private IEnumerator timerHandler()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
