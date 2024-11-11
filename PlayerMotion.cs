using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotion : MonoBehaviour
{
    public float speed; //40f
    public float boundsPower;//100f
    public float jumpForce; //3000f
    public GameObject tonsuke;

    private LiftDetector liftDetector;
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalKey;
    private float xSpeed;
    private float startTime;
    public static float totalTime;
    private bool movable;
    private bool jumpable;
    private bool autoright = false;
    private bool autoleft = false;

    //for running implemention
    private float runCoefficient = 1.3f;
    private float doubleTapThreshold = 0.2f; 
    private float lastEnterTime; 
    private float lastReleaseTime;
    private float lastDirection;
    private bool hasJustBeenPressed = false; 
    private bool isFollowedByTap = false;
    private bool isRunning = false;
    private Coroutine runCoroutine = null;
    [SerializeField]private SummonSplashEffect summonSpasheffect;
    [SerializeField]private FootCollider footCollider;

    //for diving / hip-drop
    private bool divable = true;
    private bool isDiving = false;
    [SerializeField][Tooltip("diveSpeed = diveScaler * speed")]private float diveScaler = 2f;
    [SerializeField][Tooltip("time took in 45 to -45 transition in seconds")]private float transTime = 0.2f;
    [SerializeField][Range(20f, 70f)]private float inciAngle = 70f;
    private Coroutine diveCoroutine = null;
    private Vector2 diveVec = Vector2.zero;
    private float speedBoost;
    private float maxSpeedY;

    //for reading signs
    private bool onSign = false;
    private string signID = "";
    private TonsukeLog tonsukeLog;

    private void Start()
    {
        maxSpeedY = -speed * 3f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        liftDetector = GetComponent<LiftDetector>();

        xSpeed = 0.0f;
        movable = true;
        jumpable = true;
        CheckIfTutorialIsDone();
    }

    //????
    private void CheckIfTutorialIsDone()
    {
        // if(SceneChanger.TutorialDone)
        // {
        //     Unmovable();
        //     tonsuke.SetActive(true);
        //     tonsuke.GetComponent<special_tonsuke>().ResetCount();
        //     tonsuke.GetComponent<special_tonsuke>().AgainstMainSecond();
        // }
        // else
        // {
        //     tonsuke.SetActive(false);
        //     StartRecording();
        // }
    }

    private TonsukeLog TonsukeLog
    {
        get {
            if(tonsukeLog == null)
                tonsukeLog = tonsuke.GetComponent<TonsukeLog>();
            return tonsukeLog;
        }
    }

    public void BoostDiving(float effectiveness, float angle, float duration)
    {
        diveScaler = effectiveness;
        inciAngle = angle;
        transTime = duration;
    }

    public void GetPoint(int x)
    {
        Debug.Log("Thanks!");
    }

    public void StunHim()
    {
        movable = false;
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        //アニメーション animation
    }
    public void CleanseHim()
    {
        movable = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && movable && onSign)
        {
            tonsuke.SetActive(true);
            TonsukeLog.Narrate(signID);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpable)
        {
            if(movable && !IsDiving)
            {
                rb.AddForce(transform.up * jumpForce);
                jumpable = false;
            }
        }

        if(movable && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) {
            if(divable) { //when no S is pressed beforehand
                if(!footCollider.IsGround) {
                    IsDiving = true;
                    divable = false;
                } else {
                    rb.velocity = Vector2.zero; //crouching
                }
            } else {
                if(IsDiving) {
                    IsDiving = false;//cancellation (アニメーションもつける)
                }
            }
        }
    }

    public bool IsDiving
    {
        get => isDiving;
        set {
            if(value != isDiving) {
                isDiving = value;
                if(value) {
                    if(diveCoroutine == null)
                        diveCoroutine = StartCoroutine(DiveDropping());
                }
                else {
                    if(diveCoroutine != null) {
                        StopCoroutine(diveCoroutine);
                        diveCoroutine = null;
                    }
                    diveVec = Vector2.zero;
                    diveScaler = 1f;
                    inciAngle = 70f;
                    transTime = 0.2f;
                }
            }
        }
    }

    private Vector2 VecOnTime(float t, float ori)
    {
        float angle = (-(inciAngle*2) * t / transTime + inciAngle) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle) * ori, Mathf.Sin(angle)) * diveScaler * speed;
    }

    //(make it cancellable with another S input)
    private IEnumerator DiveDropping()
    {
        float ori = Mathf.Sign(transform.localScale.x);
        diveVec = VecOnTime(0f, ori);
        float timer = Time.time;
        while(Time.time - timer <= transTime) {
            diveVec = VecOnTime(Time.time - timer, ori);
            yield return null;
        }
        diveVec = VecOnTime(transTime, ori);

        while(!footCollider.IsGround) {
            yield return null;
        }
        
        diveVec = Vector2.zero;
        IsDiving = false;
    }

    private bool IsRunning
    {
        get => isRunning;
        set
        {
            isRunning = value;
            if(value && runCoroutine == null)
                runCoroutine = StartCoroutine(EffectWhileRunning());
            else if(!value && runCoroutine != null) {
                StopCoroutine(runCoroutine);
                runCoroutine = null;
            }
        }
    }

    private IEnumerator EffectWhileRunning()
    {
        while(isRunning) {
            if(footCollider.IsGround)
                summonSpasheffect.Splash();
            yield return null;
        }
        runCoroutine = null;
    }

    private void FixedUpdate()
    {
        if(movable) 
        {
            if(!isDiving) {
                horizontalKey = Input.GetAxis("Horizontal");

                if (horizontalKey != 0) {
                    if (!hasJustBeenPressed) {
                        hasJustBeenPressed = true;
                        lastEnterTime = Time.time;
                    }
                    anim.SetBool("run", true);
                    xSpeed = isRunning ? runCoefficient * horizontalKey * speed : horizontalKey * speed;
                    transform.localScale = new Vector3(Mathf.Sign(horizontalKey), 1, 1);

                    if(isFollowedByTap && (Time.time - lastReleaseTime <= doubleTapThreshold)) {
                        IsRunning = true;
                    }
                    if(Mathf.Sign(horizontalKey) != lastDirection) {
                        IsRunning = false;
                    }
                    lastDirection = Mathf.Sign(horizontalKey);

                } else {
                    if (hasJustBeenPressed) {
                        hasJustBeenPressed = false;
                        isFollowedByTap = false;
                        lastReleaseTime = Time.time;
                        if (Time.time - lastEnterTime <= doubleTapThreshold) {
                            isFollowedByTap = true;
                        }
                    }
                    if (Time.time - lastReleaseTime >= doubleTapThreshold || !isRunning) {
                        anim.SetBool("run", false);
                        IsRunning = false;
                        xSpeed = 0f;
                    }
                }
            }
            //preventing an excessive acceleration due to gravity
            if(liftDetector.Lift != null)
                rb.velocity = new Vector2(xSpeed, Mathf.Max(rb.velocity.y, maxSpeedY)) + new Vector2(liftDetector.Lift.Velocity.x, 0f) + diveVec;
            else {
                if(diveVec != Vector2.zero)
                    rb.velocity = new Vector2(xSpeed, 0f) + diveVec;
                else
                    rb.velocity = new Vector2(xSpeed, Mathf.Max(rb.velocity.y, maxSpeedY));
            }
        }
        if(autoright)
            rb.velocity = new Vector2(40f, Mathf.Min(rb.velocity.y, 0f));
        if(autoleft)
            rb.velocity = new Vector2(-40f, Mathf.Min(rb.velocity.y, 0f));
    }

    public void DigTeleport()
    {
        if(rb.velocity.y >= 0f && rb.velocity.y <= 10f) {
            rb.position += Vector2.up * 1f;//1 unit of ASCENTION every frame
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Sign")) {
            onSign = true;
            signID = obj.gameObject.GetComponent<SignManager>().ID;
        } else if(obj.CompareTag("Shadow"))
        {
            GameObject Shadow = obj.transform.parent.gameObject;
            if(Shadow.GetComponent<ShadowMotion>().WannaTalk) {
                onSign = true;
                signID =Shadow.GetComponent<SignManager>().ID;
            }
        } else if(obj.gameObject.CompareTag("StrongSign"))
        {
            onSign = true;
            signID = obj.gameObject.GetComponent<SignManager>().ID;
            tonsuke.SetActive(true);
            TonsukeLog.Narrate(signID);
        }
    }
    private void OnTriggerExit2D(Collider2D obj)
    {
        if(obj.CompareTag("Sign") || obj.CompareTag("Shadow")) {
            onSign = false;
            signID = "";
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name=="magma_collider"||other.gameObject.tag=="Enemy")
        {
            // 衝突位置を取得する
            Vector3 hitPos = other.contacts[0].point;
            // 衝突位置から自機へ向かうベクトルを求める
            Vector3 boundVec = this.transform.position - hitPos;
            
            // 逆方向にはねる
            Vector3 forceDir = boundsPower * boundVec.normalized;
            this.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
        }
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Lift") || other.gameObject.CompareTag("Slippable"))
        {
            IsDiving = false;
            jumpable = true;
            divable = true;
        }
        if(other.gameObject.CompareTag("Cymbal"))
        {
            jumpable = true;
        }
    }

    public void StartRecording()
    {
        startTime = Time.time;
    }

    public void Goaled()//改装されるかも
    {
        StunHim();
        HappyDance();
        totalTime = Time.time - startTime;
        // SceneManager.loadScene("end");
    }

    public void HappyDance()
    {
        ResetAllAnimation();
        anim.SetBool("goal", true);
        StartCoroutine(MoveSideBySide());
    }

    void ResetAllAnimation()
    {
        anim.SetBool("run", false);
        anim.SetBool("onwall", false);
        anim.SetBool("goal", false);
    }

    IEnumerator MoveSideBySide(){
        while(true){
            autoleft=true;autoright=false;
            yield return new WaitForSeconds(0.6f);
            autoleft=false;autoright=true;
            yield return new WaitForSeconds(1.2f);
            autoleft=true;autoright=false;
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void Unmovable()
    {
        movable = false;
    }

    public void Movable()
    {
        movable = true;
    }

}
