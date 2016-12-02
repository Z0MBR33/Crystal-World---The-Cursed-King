using UnityEngine;
using System.Collections;

public class IsleAbstract
{

    public Isle IsleObj;
    public Vector2 Index;

    public enum IsleObjectType { normal, start, boss, key };

    public bool Connected = false;

    public PortalAbstract PortalUp;
    public PortalAbstract PortalUpRight;
    public PortalAbstract PortalDownRight;
    public PortalAbstract PortalDown;
    public PortalAbstract PortalDownLeft;
    public PortalAbstract PortalUpLeft;

    public UI_Isle ui_Isle;
    public bool finished;
    public bool discovered;
    public IsleObjectType isleObjectType;

    public IsleAbstract getIsleUp()
    {
        return PortalUp.ConnectecPortal.isleAbstract;   
    }

    public IsleAbstract getIsleUpRight()
    {
        return PortalUpRight.ConnectecPortal.isleAbstract;
    }

    public IsleAbstract getIsleDownRight()
    {
        return PortalDownRight.ConnectecPortal.isleAbstract;
    }

    public IsleAbstract getIsleDown()
    {
        return PortalDown.ConnectecPortal.isleAbstract;
    }

    public IsleAbstract getIsleDownLeft()
    {
        return PortalDownLeft.ConnectecPortal.isleAbstract;
    }

    public IsleAbstract getIsleUpLeft()
    {
        return PortalUpLeft.ConnectecPortal.isleAbstract;

    }

    public int getConnectionCount()
    {
        int count = 0;

        if (PortalUp != null) count++;
        if (PortalUpRight != null) count++;
        if (PortalDownRight != null) count++;
        if (PortalDown != null) count++;
        if (PortalDownLeft != null) count++;
        if (PortalDownRight != null) count++;

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