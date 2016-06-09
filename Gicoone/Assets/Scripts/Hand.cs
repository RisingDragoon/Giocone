using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
        {
            anim.SetBool("Punching", true);
        }
    }
}
