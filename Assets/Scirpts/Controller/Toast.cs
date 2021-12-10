using UnityEngine;
using System.Collections;

public class Toast : MonoBehaviour {
    public Animator anim;

    void OnEnable()
    {
        gameObject.transform.SetAsLastSibling();
        anim.SetTrigger("toast");
        Invoke("Destroy", 1.4f);
    }


    void Destroy()
    {
        gameObject.SetActive(false);
    }


    void OnDisable()
    {
        CancelInvoke();
    }
}
