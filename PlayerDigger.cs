using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    [SerializeField] private PlayerMotion playerMotion;
    private Coroutine digCoroutine = null;

    private void Start()
    {
        if(playerMotion == null) {
            Debug.LogError("PlayerMotion is not set to PlayerDigger!");
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D obj)
    {
        //if(digCoroutine == null && obj.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
        //intentionally excludes Lift for a reason
        if(digCoroutine == null && (obj.gameObject.CompareTag("Ground") || obj.gameObject.CompareTag("Cymbal"))) {
            digCoroutine = StartCoroutine(DigPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (digCoroutine != null) {
            StopCoroutine(digCoroutine);
            digCoroutine = null;
        }
    }

    private IEnumerator DigPlayer()
    {
        while(true){
            playerMotion.DigTeleport();
            yield return null;
        }
    }
}
