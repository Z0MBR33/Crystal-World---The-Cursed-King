using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    protected ObjectPool mr;
    private GhostCopy ghostCopy;
    private Coroutine currentPushHandler;
    private Isle currentIsle;

    public void Initialize(EnemyPoint enemyPoint, Vector3 islePosition, Vector3 navMeshPosition, NavMeshTarget target)
    {
        mr = ObjectPool.getObjectPool();
        currentIsle = LevelManager.getLevelManager().currentIsle.IsleObj;

        GetComponent<Stats>().resetStats();
        
        ghostCopy = GetComponent<GhostCopy>();
        ghostCopy.MovedByGhost = true;

        transform.position = enemyPoint.transform.position;
        gameObject.GetComponent<GhostCopy>().IslePosition = islePosition;

        GameObject slimeGhost = getGhost();
        slimeGhost.GetComponent<NavMeshAgent>().enabled = false;
        slimeGhost.transform.position = navMeshPosition + enemyPoint.getPositionOnIsle();
        slimeGhost.GetComponent<GhostMovement>().NavMashPosition = navMeshPosition;
        slimeGhost.GetComponent<GhostMovement>().target = target;
        slimeGhost.GetComponent<GhostMovement>().ghostCopy = ghostCopy;
        slimeGhost.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<GhostCopy>().ghost = slimeGhost.GetComponent<GhostMovement>();

    }

    protected virtual GameObject getGhost()
    {
        return mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.slimeGhost);
    }

    public void die()
    {
        if (currentIsle.ListEnemies.Remove(gameObject) == false)
        {
            print("Komischer Fehler!");  // TODO schauen, ob Fehler noch auftritt
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
