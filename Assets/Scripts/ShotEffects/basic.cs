using UnityEngine;
using System.Collections;
using System;

public class basic : ShotEffect {
    public override void triggerFixUpate(GameObject shot)
    {
    }

    public override void triggerHitEnemy(GameObject shot, GameObject enemy)
    {
        GameObject expl = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.explosion, (int)ObjectPool.explosion.playerShot);
        expl.GetComponent<ExplosionScript>().Initialize(shot.transform.position);
        
        enemy.GetComponent<Enemy>().getPushed(shot.GetComponent<Rigidbody>().velocity, 5);
        Stats targetStats = enemy.GetComponent<Stats>();
        


        try
        {
            targetStats.gotHit(shot.GetComponent<Shot>()._shootedFrom.GetComponent<Stats>().shotStrength);
        }
        catch (Exception ex)
        {
            MonoBehaviour.print("---------------------------------------------------------------------");
        }
    }

    public override void triggerHitPlayer(GameObject shot)
    {
        Stats playerData = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.essential,(int)ObjectPool.essential.player).GetComponent<Stats>();
        playerData.gotHit(shot.GetComponent<Shot>()._shootedFrom.GetComponent<Stats>().shotStrength);

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
