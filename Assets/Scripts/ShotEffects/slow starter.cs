using UnityEngine;
using System.Collections;
using System;

public class slowstarter : ShotEffect
{
    public override void triggerFixUpdate(GameObject shot)
    {
    }

    public override void triggerHitEnemy(GameObject shot, GameObject enemy)
    {
    }

    public override void triggerHitPlayer(GameObject shot)
    {
    }

    public override void triggerHitStructure(GameObject shot)
    {
    }

    public override void triggerStart(GameObject shot)
    {
        shot.transform.localScale /= 4;
        shot.GetComponent<Shot>()._shotStrength /= 4;
    }

    public override void triggerUpdate(GameObject shot)
    {
        if (shot.transform.localScale.x < 4) {
            shot.transform.localScale /= 0.94f;
            shot.GetComponent<Shot>()._shotStrength /= 0.94f;
        }
    }
}
