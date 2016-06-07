using UnityEngine;

public class AreaTurret : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag=="Player")
        {
            //La torretta si ferma e inizia a sparare
            Turret father = GetComponentInParent<Turret>();
            father.seePlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            Turret father = GetComponentInParent<Turret>();
            father.seePlayer = false;
        }
    }
}
