using UnityEngine;
using System.Collections;

public class AreaTurret : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag=="Player")
        {
            //La torretta si 'anima' e inizia a sparare
            Turret father = GetComponentInParent<Turret>();
            Debug.Log(father.active.ToString());
            father.active = true;
        }
    }
}
