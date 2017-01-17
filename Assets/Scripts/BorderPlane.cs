using UnityEngine;
using System.Collections;

public class BorderPlane : MonoBehaviour {


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Player>() == null)
            {
                return;
            }

            col.gameObject.GetComponent<Player>().DieOnCollision = true;
        }
    }
}
