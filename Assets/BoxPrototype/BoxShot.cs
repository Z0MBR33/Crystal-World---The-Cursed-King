using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxShot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);
            return;
        }

        if(collision.collider.tag == "Enemy")
        {
            BoxEnemy enemy = collision.gameObject.GetComponent<BoxEnemy>();

            HitEnemy(enemy);

            Destroy(this.gameObject);
            return;
        }
    }


    private void HitEnemy(BoxEnemy enemy)
    {
        
    }
}
