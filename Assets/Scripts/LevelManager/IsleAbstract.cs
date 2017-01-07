using UnityEngine;
using System.Collections;

public class IsleAbstract
{

    public Isle IsleObj;
    public Vector2 Index;

    public enum IsleObjectType { normal, start, boss, key };

    public bool Connected = false;

    public PortalAbstract PortalUpRight;
    public PortalAbstract PortalDownRight;
    public PortalAbstract PortalDown;
    public PortalAbstract PortalDownLeft;
    public PortalAbstract PortalUpLeft;

    public PortalAbstract[] Portals= new PortalAbstract[6];

    [HideInInspector]
    public UI_Isle ui_Isle;
    [HideInInspector]
    public bool finished;
    [HideInInspector]
    public bool discovered;
    public IsleObjectType isleObjectType;
    public int keyNumber;
      
    public IsleAbstract getIsleFromForection(int direction)
    {
        return Portals[direction].ConnectedPortal.isleAbstract;
    }

    public IsleAbstract getIsleUp()
    {
        return Portals[0].ConnectedPortal.isleAbstract;   
    }

    public IsleAbstract getIsleUpRight()
    {
        return Portals[1].ConnectedPortal.isleAbstract;
    }

    public IsleAbstract getIsleDownRight()
    {
        return Portals[2].ConnectedPortal.isleAbstract;
    }

    public IsleAbstract getIsleDown()
    {
        return Portals[3].ConnectedPortal.isleAbstract;
    }

    public IsleAbstract getIsleDownLeft()
    {
        return Portals[4].ConnectedPortal.isleAbstract;
    }

    public IsleAbstract getIsleUpLeft()
    {
        return Portals[5].ConnectedPortal.isleAbstract;

    }

    public int getConnectionCount()
    {
        int count = 0;

        for (int i = 0; i < 6; i++)
        {
            if (Portals[i] != null)
            {
                count++;
            }
        }

        return count;
    }
}