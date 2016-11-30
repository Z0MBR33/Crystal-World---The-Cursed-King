using UnityEngine;
using System.Collections;
/// <summary>
/// In this class are all the different player Attributs collected.
/// This class is not suitable to be on a Gameobject!
/// </summary>
public class PlayerStats{

    //Char stats
    public float inked = 0.0f;
    public float speed = 10.0f;
    public float luck = 0.0f;

    //Shoot stats
    public Vector3 playerShootOffset = new Vector3(0, 0, 0);
    public float shotStartVerticalDegree = 45;
    public float shotSpeed = 1.0f;
    public float fireRate = 1.0f;

}
