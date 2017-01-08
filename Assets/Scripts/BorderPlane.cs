using UnityEngine;
using System.Collections;

public class BorderPlane : MonoBehaviour {


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().DieOnCollision = true;
        }
    }
}
