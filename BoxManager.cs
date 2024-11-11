using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [SerializeField]private Sprite cracked;
    [SerializeField]private int reward = 1;
    [SerializeField]private int hp = 2;
    private float immunePeriod = 0.4f;
    private PlayerMotion playerMotion = null;
    private bool isImmune = false;
    private Coroutine immuCoroutine = null;
    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(playerMotion == null)
                playerMotion = collision.gameObject.GetComponent<PlayerMotion>();

            if(!isImmune) {
                if(playerMotion.IsDiving) 
                    hp -= 10;
                else
                    hp -= 1;
            }

            if(hp > 0) {
                if(immuCoroutine == null) {
                    immuCoroutine = StartCoroutine(EnterImmunity());
                    sp.sprite = cracked;
                }
            } else {
                DestroySelf();
            }
        }
    }

    private IEnumerator EnterImmunity() {
        isImmune = true;
        yield return new WaitForSeconds(immunePeriod);
        isImmune = false;
        immuCoroutine = null;
    }

    private void DestroySelf()
    {
        playerMotion.GetPoint(reward);//give points to the player
        Destroy(this.gameObject);
    }
}
