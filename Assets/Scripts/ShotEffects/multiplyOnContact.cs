using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class multiplyOnContact : ShotEffect
{
    public override void triggerFixUpate(GameObject shot)
    {
    }

    public override void triggerHitEnemy(GameObject shot, GameObject enemy)
    {
        multiply(shot);
    }

    public override void triggerHitStructure(GameObject shot)
    {
        multiply(shot);
    }

    public override void triggerStart(GameObject shot)
    {
    }

    public override void triggerUpdate(GameObject shot)
    {
    }

    private void multiply(GameObject shot)
    {
        float velocityOld = shot.GetComponent<Rigidbody>().velocity.magnitude;

        Vector3[] directions = new Vector3[4];
        directions[0] = shot.transform.up - shot.transform.forward;
        directions[1] = shot.transform.right - shot.transform.forward;
        directions[2] = -shot.transform.up - shot.transform.forward;
        directions[3] = -shot.transform.right - shot.transform.forward;

        float veloTweaker = shot.GetComponent<Shot>()._shootedFrom.GetComponent<Stats>().shotSpeed * 0.5f;

        for (int i = 0; i < directions.Length; i++)
        {
            shotNewShot(shot, directions[i] * veloTweaker);
        }
    }

    private void shotNewShot(GameObject shot, Vector3 direction)
    {
        Shot oldShotData = shot.GetComponent<Shot>();
        
        List<ShotEffect> desiredShotEffects = oldShotData.getActivShotEffects();
        desiredShotEffects.Remove(this);

        GameObject newShot = ObjectPool.getObjectPool().getObject(ObjectPool.categorie.shot, (int)ObjectPool.shot.round);
        newShot.GetComponent<Shot>().reset(oldShotData._shootedFrom, shot.transform.position, direction, new Quaternion(), desiredShotEffects);
    }
}
