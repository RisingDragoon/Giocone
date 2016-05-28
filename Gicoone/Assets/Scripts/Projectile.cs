using UnityEngine;
using System.Collections;

public class Projectile : Mobile
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Blocking"))
        {
            if (other.tag == "Player")
            {
                Debug.Log("Il player deve fre cose");
            }
            Destroy(gameObject);
        }
    }
}
