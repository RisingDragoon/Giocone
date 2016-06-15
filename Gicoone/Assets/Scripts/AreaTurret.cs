using UnityEngine;

public class AreaTurret : MonoBehaviour
{
    private Player player;
    private Turret father;
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        father = GetComponentInParent<Turret>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (player.inStealth)
        {
            father.SeePlayer = false;
        }
        else
        {
            if (other.tag == "Player")
            {
                //La torretta si ferma e inizia a sparare
                father.SeePlayer = true;
            }
        }     
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            father.SeePlayer = false;
        }
    }
}