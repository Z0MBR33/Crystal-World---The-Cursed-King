using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Classes.ShotMode ModeOfShots;

    public Vector3 Direction;
    public float Speed;

    private ObjectPool mr;

    // Use this for initialization
    void Start () {
        mr = ObjectPool.getObjectPool();    
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKeyDown(KeyCode.Space))
        {
          
            Shot shot = mr.getShot(0).GetComponent<Shot>();
            shot.gameObject.SetActive(true);
           
            if (ModeOfShots == Classes.ShotMode.Rocket)
            {
                shot.Initialize(ModeOfShots, transform.position + new Vector3(0, 1, 0), Direction, transform.rotation, 50, 1);
                
            }
            else if (ModeOfShots == Classes.ShotMode.Bomb)
            {
                shot.Initialize(ModeOfShots, transform.position + new Vector3(0,1,0), Direction, transform.rotation, 0, 1);
                Rigidbody rb = shot.gameObject.GetComponent<Rigidbody>();

                rb.AddForce(Direction.normalized * Speed, ForceMode.Impulse);

            }
            
        }
            
	}
}
