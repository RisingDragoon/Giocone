using UnityEngine;

public class Hand : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag =="Player")
        {
            Animator anim = GetComponentInParent<Animator>();
            if (anim != null)
            {
                anim.SetBool("Punching", true);
            }
        }
        
    }
}
