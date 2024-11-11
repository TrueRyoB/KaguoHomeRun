using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMotion : MonoBehaviour
{
    public GameObject Foot;
    public float jumpForce; //3000f
    public float boundsPower; //100f
    public float speed; //40f
    public GameObject honest;

    //忍者ジャンプ
    private SpriteRenderer myRenderer;
    private Animator anim;
    private FootCollider footcollider;
    //ジャンプ
    private Rigidbody2D rbody2D;
    private int jumpCount = 0;
    //横移動
    private float xSpeed;
    private bool headingRight = false;
    private bool headingLeft = false;
    [SerializeField]private bool wannaTalk = true;
    private ShadowAIMode shadowAI;
    private SignManager signManager;

    private void Start()
    {
        signManager = GetComponent<SignManager>();
        shadowAI = GetComponent<ShadowAIMode>();
        myRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rbody2D = this.GetComponent<Rigidbody2D>();
        footcollider = Foot.GetComponent<FootCollider>();
    }
    public void Recognized()
    {
        signManager.ModifyCaption2("same", "");
    }
    public bool WannaTalk
    {
        get => wannaTalk;
        set => wannaTalk = value;
    }
    public void Performance_StageT()
    {
        wannaTalk = false;
        shadowAI.ClimbingFirstWall();
    }

    void Update()
    {
        Walking();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag=="Ground"&&!footcollider.IsGround)
        {
            myRenderer.flipX = false;
            anim.SetBool("onwall", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag=="Ground")
        {
            myRenderer.flipX = true;
            anim.SetBool("onwall", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }

        if(collision.gameObject.name=="magma_collider"||collision.gameObject.tag=="Enemy")
        {
            
            Vector3 hitPos = collision.contacts[0].point;
            
            Vector3 boundVec = this.transform.position - hitPos;
            
            Vector3 forceDir = boundsPower * boundVec.normalized;
            rbody2D.AddForce(forceDir, ForceMode2D.Impulse);
        }
    }
    
    private void Walking()
    {
        if(headingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            honest.transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = speed;
        }
        else if(headingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            honest.transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = -speed;
        }
        else
        {
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }
        rbody2D.velocity = new Vector2(xSpeed, rbody2D.velocity.y);
    }

    //これを呼び出してシャドーかぐおを操作する
    public void HeadLeft()
    {
        headingLeft = true;
    }

    public void HeadRight()
    {
        headingRight = true;
    }

    public void DontHeadLeft()
    {
        headingLeft = false;
    }

    public void DontHeadRight()
    {
        headingRight = false;
    }

    public void Jump()
    {
        if(jumpCount==0)
        {   
            this.rbody2D.AddForce(transform.up * jumpForce);
            jumpCount++;
        }
    }

    public void HappyDance()
    {
        anim.SetBool("happy", true);
    }

    public void ResetAnimation()
    {
        anim.SetBool("happy", false);
    }

    public void NinjaIsGone()
    {
        this.transform.position = new Vector3(640,52,0);
    }
}
