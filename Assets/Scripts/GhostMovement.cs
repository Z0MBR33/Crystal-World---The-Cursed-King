using UnityEngine;
using System.Collections;

public class GhostMovement : MonoBehaviour {

    public Vector3 NavMashPosition;
    private GhostCopy ghostCopy;
    private NavMeshTarget target;
    
    private NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   // TODO
        /*
        if (ghostCopy.MovedByGhost == true)
        {
            agent.SetDestination(NavMashPosition + target.getPositionOnIsle());
        }
        else
        {
            transform.position = NavMashPosition + ghostCopy.getPositionOnIsle();
        }*/
    }

    public void setTarget(NavMeshTarget target) {
        this.target = target;
    }

    public void setghostCopy(GhostCopy copy)
    {
        ghostCopy = copy;
    }

    public Vector3 getPositionInNav()
    {
        return transform.position - NavMashPosition;
    }

}
