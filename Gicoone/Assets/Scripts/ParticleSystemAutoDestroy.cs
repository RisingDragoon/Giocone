using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(gameObject);
    }
}
