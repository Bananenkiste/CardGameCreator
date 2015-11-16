using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour 
{

    public void Enable()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.GetComponent<Animator>().Play("Intro");
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    
    }

    
    
    public void Disable()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.GetComponent<Animator>().Play("Outro");
            StartCoroutine(AfterAnim(false));
        }
    }

    IEnumerator AfterAnim(bool state)
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(state);
    }
}
