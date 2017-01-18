using UnityEngine;
using System.Collections;
using System;

public class shootedFromPlayer : ShotEffect
{
    public override void triggerFixUpate(GameObject shot)
    {
    }

    public override void triggerHitEnemy(GameObject shot, GameObject enemy)
    {
    }

    public override void triggerHitStructure(GameObject shot)
    {
    }

    public override void triggerStart(GameObject shot)
    {
        Shot shotData = shot.GetComponent<Shot>();
        MonoBehaviour.print("Prüfe auf Spielerschuss");
        try
        {

        }
        catch (Exception)
        {
            MonoBehaviour.print("Nicht Spieler Schuss");
        }
        
    }

    public override void triggerUpdate(GameObject shot)
    {
    }
}
