using UnityEngine;
using System.Collections;

public class Isle : MonoBehaviour
{
    public IsleAbstract isleAbstract;

    public Portal PortalUp;
    public Portal PortalUpRight;
    public Portal PortalDownRight;
    public Portal PortalDown;
    public Portal PortalDownLeft;
    public Portal PortalUpLeft;


    public void Initialize(IsleAbstract isle)
    {
        isleAbstract = isle;

        // disable Portals

        PortalUp.setDirection(0);
        PortalUpRight.setDirection(1);
        PortalDownRight.setDirection(2);
        PortalDown.setDirection(3);
        PortalDownLeft.setDirection(4);
        PortalUpLeft.setDirection(5);

        if (isle.ConnectionUp == null)
        {
            PortalUp.gameObject.SetActive(false);
        }
        if (isle.ConnectionUpRight == null)
        {
            PortalUpRight.gameObject.SetActive(false);
        }
        if (isle.ConnectionDownRight == null)
        {
            PortalDownRight.gameObject.SetActive(false);
        }
        if (isle.ConnectionDown == null)
        {
            PortalDown.gameObject.SetActive(false);
        }
        if (isle.ConnectionDownLeft == null)
        {
            PortalDownLeft.gameObject.SetActive(false);
        }
        if (isle.ConnectionUpLeft == null)
        {
            PortalUpLeft.gameObject.SetActive(false);
        }

    }

    public void OnMouseDown()
    {

        GetComponent<Renderer>().material.color = new Color(0, 255, 0);

        IsleAbstract isle = isleAbstract.getIsleUp();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        isle = isleAbstract.getIsleUpRight();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        isle = isleAbstract.getIsleDownRight();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        isle = isleAbstract.getIsleDown();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        isle = isleAbstract.getIsleDownLeft();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }

        isle = isleAbstract.getIsleUpLeft();
        if (isle != null)
        {
            isle.IsleObj.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
    }
}
