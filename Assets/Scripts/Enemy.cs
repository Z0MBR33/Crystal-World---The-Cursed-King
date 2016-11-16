using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Life;

    private ObjectPool mr;

    public void TakeDamage(int damage, Vector3 pushDirection, int force)
    {
        Life--;
        if (Life <= 0)
        {
            StopAllCoroutines();
            mr.returnEnemie(gameObject, 0);

            ExplosionScript expl = mr.getExplosion(1).GetComponent<ExplosionScript>();
            expl.gameObject.SetActive(true);

            expl.Initialize(transform.position, new Quaternion());

            return;
        }

        pushDirection.Normalize();
        pushDirection = pushDirection * force;

        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<enemyMovement>().enabled = false;

        gameObject.GetComponent<Rigidbody>().AddForce(pushDirection, ForceMode.Impulse);

        StopCoroutine(pushHandler());
        StartCoroutine(pushHandler());
    }

    public IEnumerator pushHandler()
    {
        yield return new WaitForSeconds(1);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<enemyMovement>().enabled = true;
    }

	// Use this for initialization
	void Start () {
        mr = ObjectPool.getObjectPool();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
