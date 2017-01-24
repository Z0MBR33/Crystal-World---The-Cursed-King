using UnityEngine;
using System.Collections;
using System;

public class bluff : ShotEffect
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
        shot.GetComponent<Shot>()._shotStrength *= 4;
    }

    public override void triggerUpdate(GameObject shot)
    {
        shot.GetComponent<Shot>()._shotStrength *= 0.96f;
    }
}
