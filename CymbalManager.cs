using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CymbalManager : MonoBehaviour
{
    [SerializeField]private Sprite charged;
    [SerializeField]private Sprite exercised;
    [SerializeField]private Sprite burntout;
    [SerializeField]private float speedBoost = 1.2f;
    [SerializeField][Range(20f,70f)]private float angle = 45f;
    [SerializeField][Range(0.1f, 1.2f)]private float transTime = 0.6f;
    [SerializeField]private float cdDuration = 0.4f;
    [SerializeField]private int hp = 3;
    private bool isCharged = true;
    private PlayerMotion playerMotion;
    private Coroutine cdCoroutine = null;
    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = charged;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(playerMotion == null)
                playerMotion = collision.gameObject.GetComponent<PlayerMotion>();

            if(isCharged && cdCoroutine == null && playerMotion.IsDiving)
                cdCoroutine = StartCoroutine(Exercise());
        }
    }

    private IEnumerator Exercise()
    {
        sp.sprite = exercised;
        //make sounds
        //move the player
        playerMotion.IsDiving = false;
        playerMotion.BoostDiving(speedBoost, angle, transTime);
        playerMotion.IsDiving = true;
        yield return new WaitForSeconds(cdDuration);
        --hp;
        if(hp > 0) {
            sp.sprite = charged;
        } else {
            sp.sprite = burntout;
            isCharged = false;
            this.gameObject.tag = "Ground";
        }
        cdCoroutine = null;
    }

}
