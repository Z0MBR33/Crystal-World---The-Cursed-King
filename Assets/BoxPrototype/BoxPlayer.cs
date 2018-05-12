using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlayer : MonoBehaviour {

    public GameObject BoxShotPrefab;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private bool canShoot = true;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // shoot

        if (Input.GetKey(KeyCode.Space))
        {
            if (canShoot)
            {
                GameObject shot = Instantiate(BoxShotPrefab, this.transform.position + new Vector3(0, 0, 1), new Quaternion());

                shot.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 10), ForceMode.VelocityChange);

                canShoot = false;

                StartCoroutine(shootCoolDown());
            }
        }

        // move

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection *= 6;
        }

        moveDirection.y -= 10 * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

    IEnumerator shootCoolDown()
    {
        yield return new WaitForSeconds(1);

        canShoot = true;
    }

}
