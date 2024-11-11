using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinzyaAnimation : MonoBehaviour
{
    public GameObject Player;
    public GameObject Foot;

    private SpriteRenderer myRenderer;
    private Animator anim;
    private FootCollider footcollider;

    void Start()
    {
        myRenderer = Player.GetComponent<SpriteRenderer>();
        anim = Player.GetComponent<Animator>();
        footcollider = Foot.GetComponent<FootCollider>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if((collider.gameObject.tag=="Ground" || collider.gameObject.tag=="Lift" || collider.gameObject.tag == "Smooth") &&footcollider.IsGround==false)
        {
            myRenderer.flipX = false;
            anim.SetBool("onwall", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag=="Ground" || collider.gameObject.tag=="Lift" || collider.gameObject.tag == "Smooth")
        {
            myRenderer.flipX = true;
            anim.SetBool("onwall", false);
        }
    }

    
}