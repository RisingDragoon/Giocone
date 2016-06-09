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
            father.seePlayer = false;
        }
        else
        {
            if (other.tag == "Player")
            {
                //La torretta si ferma e inizia a sparare
                father.seePlayer = true;
            }
        }     
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            father.seePlayer = false;
        }
    }
}