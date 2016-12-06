using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private ObjectPool mr;
    private GhostCopy ghostCopy;
    private Coroutine currentPushHandler;

    public void Initialize()
    {
        mr = ObjectPool.getObjectPool();

        if (GetComponent<Stats>() != null)
        {
            GetComponent<Stats>().resetStats();
        }

        if (GetComponent<GhostCopy>() != null)
        {
            ghostCopy = GetComponent<GhostCopy>();
            ghostCopy.MovedByGhost = true;
        }
    }

    public void die()
    {
        StopAllCoroutines();

        GameMaster gm = GameMaster.getGameMaster();

        gm.ListEnemies.Remove(gameObject);

        mr.returnEnemy(gameObject, 0);

        // destroy ghost
        if (ghostCopy != null)
        {
            Destroy(ghostCopy.ghost.gameObject);
        }

        ExplosionScript expl = mr.getExplosion(2).GetComponent<ExplosionScript>();
        expl.gameObject.SetActive(true);

        expl.Initialize(ExplosionScript.ExplosionType.EnemyDeath, transform.position, new Quaternion());
    }

    public void getPushed(Vector3 pushDirection, float force)
    {

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
