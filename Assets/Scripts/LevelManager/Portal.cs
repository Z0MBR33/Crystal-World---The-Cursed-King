using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

    public bool PortalActivated = false;

    public PortalSpiral portalSpiral;

    private int Direction;

    public void setDirection(int direction)
    {
        this.Direction = direction;
    }

    public int getDirection()
    {
        return this.Direction;
    }



}