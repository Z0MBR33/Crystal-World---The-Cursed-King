using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    private ObjectPool mr;
    private GhostCopy ghostCopy;
    private Coroutine currentPushHandler;
    private Isle currentIsle;

    public void Initialize()
    {
        mr = ObjectPool.getObjectPool();
        currentIsle = LevelManager.getLevelManager().getCurrentIsle().IsleObj;

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
        currentIsle.EnemyCount--;
        if (currentIsle.ListEnemies.Remove(gameObject) == false)
        {
            print("Komischer Fehler!");
        }

        StopAllCoroutines();

        mr.returnObject(gameObject);

        // destroy ghost
        if (ghostCopy != null)
        {
            mr.returnObject(ghostCopy.ghost.gameObject);
        }

        ExplosionScript expl = mr.getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.enemy).GetComponent<ExplosionScript>();
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

        if (gameObject.activeSelf == true)
        {
            currentPushHandler = StartCoroutine(pushHandler());
        }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "DeathPlane")
        {
            Stats stats = GetComponent<Stats>();
            stats.gotHit(collision.collider.GetComponent<Stats>().strength);
        }
    }
}
