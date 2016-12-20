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

    public UI_Isle ui_Isle;
    public bool finished;
    public bool discovered;
    public IsleObjectType isleObjectType;
      
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


    // Delete useless methods!
    public UI_Isle getUIIsle()
    {
        return ui_Isle;
    }

    public void setUIIsle(UI_Isle isle)
    {
        ui_Isle = isle;
    }

    public bool getFinishState()
    {
        return finished;
    }

    public void setFinishState(bool state)
    {
        finished = state;
    }

}