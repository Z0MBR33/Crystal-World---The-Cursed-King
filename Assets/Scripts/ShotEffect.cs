using UnityEngine;
using System.Collections;

public abstract class ShotEffect
{
    public abstract void triggerStart(GameObject shot);
    public abstract void triggerUpdate(GameObject shot);
    public abstract void triggerFixUpdate(GameObject shot);
    public abstract void triggerHitEnemy(GameObject shot, GameObject enemy);
    public abstract void triggerHitPlayer(GameObject shot);
    public abstract void triggerHitStructure(GameObject shot);
}
