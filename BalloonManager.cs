using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    private BalloonguyManager balloonguy;

    private void Start()
    {
        balloonguy = this.gameObject.transform.parent.gameObject.GetComponent<BalloonguyManager>();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.CompareTag("Player"))
        {
            balloonguy.BallonExplodes(obj.gameObject.GetComponent<PlayerMotion>());
            Destroy(this.gameObject);
        }
    }
}
