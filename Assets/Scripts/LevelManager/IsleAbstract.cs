using UnityEngine;
using System.Collections;

public class IsleAbstract
{


    public Isle IsleObj;
    public Vector2 Index;

    public enum IsleType { small, medium, big };

    public IsleType Type;

    public bool Connected = false;

    public ConnectionAbstract ConnectionUp;
    public ConnectionAbstract ConnectionUpRight;
    public ConnectionAbstract ConnectionDownRight;
    public ConnectionAbstract ConnectionDown;
    public ConnectionAbstract ConnectionDownLeft;
    public ConnectionAbstract ConnectionUpLeft;

    private UI_Isle ui_Isle;
    private bool finished;

    public IsleAbstract getIsleUp()
    {
        IsleAbstract isle = null;

        if (ConnectionUp != null)
        {
            if (ConnectionUp.Isle1 == this)
            {
                isle = ConnectionUp.Isle2;
            }
            else
            {
                isle = ConnectionUp.Isle1;
            }
        }

        return isle;

    }

    public IsleAbstract getIsleUpRight()
    {
        IsleAbstract isle = null;

        if (ConnectionUpRight != null)
        {
            if (ConnectionUpRight.Isle1 == this)
            {
                isle = ConnectionUpRight.Isle2;
            }
            else
            {
                isle = ConnectionUpRight.Isle1;
            }
        }

        return isle;

    }

    public IsleAbstract getIsleDownRight()
    {
        IsleAbstract isle = null;

        if (ConnectionDownRight != null)
        {
            if (ConnectionDownRight.Isle1 == this)
            {
                isle = ConnectionDownRight.Isle2;
            }
            else
            {
                isle = ConnectionDownRight.Isle1;
            }
        }

        return isle;

    }

    public IsleAbstract getIsleDown()
    {
        IsleAbstract isle = null;

        if (ConnectionDown != null)
        {
            if (ConnectionDown.Isle1 == this)
            {
                isle = ConnectionDown.Isle2;
            }
            else
            {
                isle = ConnectionDown.Isle1;
            }
        }

        return isle;

    }

    public IsleAbstract getIsleDownLeft()
    {
        IsleAbstract isle = null;

        if (ConnectionDownLeft != null)
        {
            if (ConnectionDownLeft.Isle1 == this)
            {
                isle = ConnectionDownLeft.Isle2;
            }
            else
            {
                isle = ConnectionDownLeft.Isle1;
            }
        }

        return isle;

    }

    public IsleAbstract getIsleUpLeft()
    {
        IsleAbstract isle = null;

        if (ConnectionUpLeft != null)
        {
            if (ConnectionUpLeft.Isle1 == this)
            {
                isle = ConnectionUpLeft.Isle2;
            }
            else
            {
                isle = ConnectionUpLeft.Isle1;
            }
        }

        return isle;

    }

    public int getConnectionCount()
    {
        int count = 0;

        if (ConnectionUp != null) count++;
        if (ConnectionUpRight != null) count++;
        if (ConnectionDownRight != null) count++;
        if (ConnectionDown != null) count++;
        if (ConnectionDownLeft != null) count++;
        if (ConnectionDownRight != null) count++;

        return count;
    }

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