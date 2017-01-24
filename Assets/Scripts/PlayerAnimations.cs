using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {
    private CharacterController charController;
    private Animator playerAnimator;
	// Use this for initialization
	void Start () {
    charController = GetComponent<CharacterController>();
    playerAnimator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        if (charController.velocity.sqrMagnitude > 0.01)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
	}
}
