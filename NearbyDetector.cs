using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyDetector : MonoBehaviour
{
    [SerializeField]private SignManager signManager;

    private void Start()
    {
        if(signManager == null)
            Debug.LogError("No signmanager assigned >:(");
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.CompareTag("Player"))
            signManager.IsNearby = true;
    }
    private void OnTriggerExit2D(Collider2D obj)
    {
        if(obj.gameObject.CompareTag("Player"))
            signManager.IsNearby = false;
    }
}
