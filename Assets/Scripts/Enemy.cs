using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int Life;

    private int life;
    private ObjectPool mr;
    private GhostCopy ghostCopy;
    private Coroutine currentPushHandler;

    public void Initialize()
    {
        mr = ObjectPool.getObjectPool();

        life = Life;

        if (GetComponent<GhostCopy>() != null)
        {
            ghostCopy = GetComponent<GhostCopy>();
            ghostCopy.MovedByGhost = true;
        }
    }

    public void TakeDamage(Vector3 pushDirection, int force)
    {
        life--;
        if (life <= 0)
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
