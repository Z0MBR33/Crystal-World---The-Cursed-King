using UnityEngine;
using System.Collections;

public abstract class ShotEffect : MonoBehaviour
{
    public abstract void triggerStart(GameObject shot);
    public abstract void triggerUpdate(GameObject shot);
    public abstract void triggerFixUpate(GameObject shot);
    public abstract void triggerHitEnemy(GameObject shot, GameObject enemy);
    public abstract void triggerHitStructure(GameObject shot);
}
