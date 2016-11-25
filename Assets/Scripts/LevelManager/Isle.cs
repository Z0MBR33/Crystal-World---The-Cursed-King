using UnityEngine;
using System.Collections;

public class Isle : MonoBehaviour
{
    public IsleAbstract isleAbstract;

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
