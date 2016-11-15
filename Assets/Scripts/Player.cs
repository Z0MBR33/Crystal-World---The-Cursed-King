using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Classes.ShotMode ModeOfShots;

    public GameObject Shots;
    public Vector3 Direction;
    public float Speed;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ModeOfShots == Classes.ShotMode.Rocket)
            {
                Shot.Create(Shots, ModeOfShots, transform.position, Direction, transform.rotation, 50, 1);
            }
            else if (ModeOfShots == Classes.ShotMode.Bomb)
            {
                GameObject obj = Shot.Create(Shots, ModeOfShots, transform.position, Direction, transform.rotation, 0, 1);
                Rigidbody rb = obj.GetComponent<Rigidbody>();

  
                rb.AddForce(Direction.normalized * Speed, ForceMode.Impulse);

            }
            
        }
            
	}
}
