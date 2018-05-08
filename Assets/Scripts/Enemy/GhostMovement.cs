using UnityEngine;
using System.Collections;

public class GhostMovement : MonoBehaviour {

    public Vector3 NavMashPosition;
    [HideInInspector]
    public GhostCopy ghostCopy;
    [HideInInspector]
    public NavMeshTarget target;
    
    private UnityEngine.AI.NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {   
        
        if (ghostCopy.MovedByGhost == true)
        {
            agent.SetDestination(NavMashPosition + target.getPositionOnIsle());
        }
        else
        {
            transform.position = NavMashPosition + ghostCopy.getPositionOnIsle();
            transform.rotation = ghostCopy.gameObject.transform.rotation;
        }
    }

    public Vector3 getPositionInNav()
    {
        return transform.position - NavMashPosition;
    }

}
