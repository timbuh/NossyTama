using UnityEngine;

public class CurtainScript : MonoBehaviour
{
    public GameObject Curtains;
    public bool CurtainIsOn;
    public Animator CurtainAnimator;

    public void OpenOrClose()
    {
        if(CurtainIsOn == false)
        {
            CurtainClose();
            CurtainIsOn = true;

        }

        else
        {
            CurtainOpen();
            CurtainIsOn = false;
        }
    }

    void CurtainOpen()
    {
        CurtainIsOn = true;
        CurtainAnimator.SetTrigger("CurtainOpen");
    }

    void CurtainClose()
    {
        CurtainIsOn = false;
        CurtainAnimator.SetTrigger("CurtainClose");
    }
}
