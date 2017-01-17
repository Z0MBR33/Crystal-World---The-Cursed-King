using UnityEngine;
using System.Collections;

public class Octopus : Enemy {

    private bool walking;
    private Coroutine octopusRythm;

    public override void Initialize(EnemyPoint enemyPoint, Vector3 islePosition, Vector3 navMeshPosition, NavMeshTarget target)
    {
        base.Initialize(enemyPoint, islePosition, navMeshPosition, target);

        octopusRythm = StartCoroutine(octopusRythmHandler());
    }

    protected override GameObject getGhost()
    {
        return mr.getObject(ObjectPool.categorie.enemy, (int)ObjectPool.enemy.octopusGhost);
    }

    public override void die()
    {
        StopAllCoroutines();

        base.die();
    }

    public override void getPushed(Vector3 pushDirection, float force)
    {

    }

    public IEnumerator octopusRythmHandler()
    {
        walking = true;

        while (true)
        {
            if (walking)
            {
                ghostCopy.MovedByGhost = true;

                yield return new WaitForSeconds(5);
                walking = false;
            }
            else
            {
                ghostCopy.MovedByGhost = false;

                yield return new WaitForSeconds(3);
                walking = true;
            }
        }
    }
}
