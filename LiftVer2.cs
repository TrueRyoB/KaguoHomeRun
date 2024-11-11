using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveP //move pattern
{
    Constant,
    Accelerate,
}

public enum TriggerP //trigger pattern
{
    AlwaysActive,
    UponTouch,
    BeforeTouch,
}

public class LiftVer2 : MonoBehaviour
{
    [SerializeField][Header("MovePattern")] private MoveP mP = MoveP.Constant;
    [SerializeField][Header("TriggerPattern")] private TriggerP tP = TriggerP.UponTouch;
    [SerializeField][Header("Reposit Itself? (always true for Constant)")] private bool volReposit = true;

    [SerializeField][Header("AccelRate")]   private float acceleration = 50f;
    [SerializeField][Header("MoveSpeed")]   private float _movespeed = 20f;
    [SerializeField][Header("Final Speed for Accel")] private float _maxspeed = 2000f;
    [SerializeField][Header("Edges")]       private Transform[] _ends;
    
        private Rigidbody2D rb;

    private int index = 0;
    private int n = 0;  //size of _ends

    private Vector2 oldPos; 
    private Vector2 myVelocity;//exists as a kinetic obj cannot implicitly generate vel from rb

    private Coroutine accelCoroutine = null;
    private bool isPaused = false;
    private bool _tempVoluntarily = false;

    public Vector2 Velocity
    {
        get => myVelocity;
    }
    
    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        if (_ends != null && _ends.Length > 0&& rb != null) {
            n = _ends.Length;
            rb.position = _ends[0].position;
        }
        else
            Debug.LogError("One of the lifts is not properly assigned its ends.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(tP == TriggerP.UponTouch && collision.gameObject.CompareTag("Player"))
        {
            if (mP == MoveP.Accelerate && accelCoroutine == null) { //assuming the lift is already at _ends[0].position
                accelCoroutine = StartCoroutine(SuddenAccel(_ends[0].position, _ends[1].position, true));
            } else if (mP == MoveP.Constant) {
                _tempVoluntarily = true;
            }
        }
        if(tP == TriggerP.BeforeTouch && collision.gameObject.CompareTag("Player"))
        {
            isPaused = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if(tP == TriggerP.BeforeTouch && collision.gameObject.CompareTag("Player"))
            isPaused = false;
    }

    private IEnumerator SuddenAccel(Vector3 start, Vector3 target, bool isOutbound)
    {
        float initialSpeed = 0f;
        float finalSpeed = _maxspeed;
        Vector3 direction = (target - start).normalized;

        float currentSpeed = initialSpeed;

        while (true)
        {
            if (currentSpeed < finalSpeed){
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, finalSpeed);
            }

            Vector3 nextPosition = transform.position + direction * currentSpeed * Time.deltaTime;
            Vector3 toTarget = target - transform.position;
            if (Vector3.Dot(toTarget, direction) <= 0) {
                rb.MovePosition(target); 
                break; 
            }

            rb.MovePosition(nextPosition);

            myVelocity = ((Vector2)transform.position - (Vector2)oldPos) * Time.deltaTime;
            oldPos = (Vector2)transform.position;

            yield return null; 
        }

        rb.MovePosition(target);

        if(volReposit && isOutbound) {
            StartCoroutine(SuddenAccel(target, start, false));
        } else {
            accelCoroutine = null;
        }
    }

    int count = 0;

    private void FixedUpdate()
    {
        if((mP == MoveP.Constant) && (tP == TriggerP.AlwaysActive || _tempVoluntarily) && !isPaused)
        {
            //keep moving until the distance between this object and the target is smaller than epsilon
            if(Vector2.Distance(rb.position, _ends[index].position) > 0.1f){
                Vector2 toVector = Vector2.MoveTowards((Vector2)rb.position, (Vector2)_ends[index].position, _movespeed * Time.deltaTime);
                rb.MovePosition(toVector);
            }
            else {
                rb.MovePosition(_ends[index].position);
                index = Increment(index);
                if (index == 1) 
                {
                    count++;
                    if (count >= 2) {
                        _tempVoluntarily = false;
                        count = 1; 
                    }
                }
            }
        }

        myVelocity = (rb.position - (Vector2)oldPos) / Time.deltaTime;
        oldPos = (Vector2)rb.position;
    }

    private int Increment(int x)
    {
        return (x < n-1)? x+1 : 0;
    }
}