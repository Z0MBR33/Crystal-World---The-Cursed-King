using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Life;

    private ObjectPool mr;
    private GhostCopy ghostCopy;
    private Coroutine currentPushHandler;

    void Start()
    {
        mr = ObjectPool.getObjectPool();

        if (GetComponent<GhostCopy>() != null)
        {
            ghostCopy = GetComponent<GhostCopy>();
        }
    }

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

        if (ghostCopy != null)
        {
            ghostCopy.MovedByGhost = false;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        gameObject.GetComponent<Rigidbody>().AddForce(pushDirection, ForceMode.Impulse);

        if (currentPushHandler != null)
        {
            StopCoroutine(currentPushHandler);
        }
        currentPushHandler = StartCoroutine(pushHandler());
    }

    public IEnumerator pushHandler()
    {
        yield return new WaitForSeconds(1);

        if (ghostCopy != null)
        {
            ghostCopy.MovedByGhost = true;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

    }
	
	
}
