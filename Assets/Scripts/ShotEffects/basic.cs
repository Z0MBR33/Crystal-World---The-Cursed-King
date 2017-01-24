using UnityEngine;
using System.Collections;
using System;

public class basic : ShotEffect
{
    public override void triggerFixUpdate(GameObject shot)
    {
    }

    public override void triggerHitEnemy(GameObject shot, GameObject enemy)
    {
        GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.playerShot);
        expl.GetComponent<ExplosionScript>().Initialize(shot.transform.position);

        try
        {
            enemy.GetComponent<Enemy>().getPushed(shot.GetComponent<Rigidbody>().velocity, 5);
            Stats targetStats = enemy.GetComponent<Stats>();
            targetStats.gotHit(shot.GetComponent<Shot>()._shotStrength);
        }
        catch (Exception ex)
        {
            Debug.Log("Enemy not found");
        }
    }

    public override void triggerHitPlayer(GameObject shot)
    {
        Player playerData = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.player).GetComponent<Player>();
        playerData.TakeDamage(shot.GetComponent<Shot>()._shotStrength);

        GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.enemyShot);
        expl.GetComponent<ExplosionScript>().Initialize(shot.transform.position);
    }

    public override void triggerHitStructure(GameObject shot)
    {

        if (shot.tag == "Player")
        {
            GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.playerShot);
            expl.GetComponent<ExplosionScript>().Initialize(shot.transform.position);
        }
        else
        {
            GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.enemyShot);
            expl.GetComponent<ExplosionScript>().Initialize(shot.transform.position);
        }
    }

    public override void triggerStart(GameObject shot)
    {
        shot.GetComponent<Rigidbody>().AddForce(shot.GetComponent<Shot>()._startDirection, ForceMode.VelocityChange);
    }

    public override void triggerUpdate(GameObject shot)
    {
    }
}
