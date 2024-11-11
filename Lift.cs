// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum LiftPattern
// {
//     ConstantlyMoving,
//     ActiveUponTouch,
//     StopUponTouch,
//     ConstantUponTouch,
// }

// public enum MP //move pattern
// {
//     Constant,
//     Accelerate,
// }

// public enum TP //trigger pattern
// {
//     AlwaysActive,
//     UponTouch,
//     BeforeTouch,
// }

// //regulates lift movement by making it follow the rail path (between two points specifically)
// public class Lift : MonoBehaviour
// {
//     [SerializeField][Header("MovePattern")] private MP movePattern = MP.Constant;
//     [SerializeField][Header("TriggerPattern")] private TP triggerPattern = TP.UponTouch;
//     [SerializeField][Header("Reposit Itself? (always true for Constant)")] private bool volReposit = true;

//     [SerializeField][Header("AccelRate")]   private float acceleration = 50f; //テスト
//     [SerializeField][Header("Pattern")]     private LiftPattern _pattern = LiftPattern.ConstantlyMoving;
//     [SerializeField][Header("MoveSpeed")]   private float _movespeed = 20f;
//     [SerializeField][Header("Final Speed for Accel")] private float _maxspeed = 2000f;
//     [SerializeField][Header("Edges")]       private Transform[] _edges;

//     private bool _stopOnTouch = false;
//     private bool _moveVoluntarily = false;
//     private bool _accelOnTouch = false;
//     private bool _constOnTouch = false;

//     private bool _isAccelerating = false;
//     //private float debugtimer = 0f;
    
//     private Rigidbody2D rb;

//     private int index = 0;
//     private int n = 0;  //size of _edges

//     private Vector2 oldPos; 
//     private Vector2 myVelocity;//exists as a kinetic obj cannot implicitly generate vel from rb

//     public Vector2 Velocity
//     {
//         get => myVelocity;
//     }
    
//     private void Start() 
//     {
//         rb = GetComponent<Rigidbody2D>();
//         if (_edges != null && _edges.Length > 0&& rb != null) {
//             n = _edges.Length;
//             rb.position = _edges[0].position;
//         }
//         else
//             Debug.LogError("This gameobject is not properly assigned.");
        
//         switch(_pattern)
//         {
//             case LiftPattern.ActiveUponTouch:
//             _accelOnTouch = true;
//             break;
//             case LiftPattern.StopUponTouch:
//             _stopOnTouch = true;
//             _moveVoluntarily = true;
//             break;
//             case LiftPattern.ConstantlyMoving:
//             _moveVoluntarily = true;
//             break;
//             case LiftPattern.ConstantUponTouch:
//             _constOnTouch = true;
//             break;
//             default:
//             Debug.LogError("This Enumerator is not registered");
//             break;
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (triggerPattern == TP.UponTouch  && !_isAccelerating && collision.gameObject.CompareTag("Player"))
//         {
//             _isAccelerating = true;
//             StartCoroutine(SuddenAccel(_edges[0].position, _edges[1].position, true)); //assuming the lift is already at _edges[0].position
//         }
//         // if(_constOnTouch && collision.gameObject.CompareTag("Player"))
//         // {
//         //     _moveVoluntarily = true;
//         // }
//         if(triggerPattern == TP.UponTouch && collision.gameObject.CompareTag("Player")) {
//             _moveVoluntarily = true;
//         }
//     }

//     private IEnumerator SuddenAccel(Vector3 start, Vector3 target, bool isOutbound)
//     {
//         float initialSpeed = 0f;
//         float finalSpeed = _maxspeed;
//         Vector3 direction = (target - start).normalized;

//         float currentSpeed = initialSpeed;

//         while (true)
//         {
//             if (currentSpeed < finalSpeed){
//                 currentSpeed += acceleration * Time.deltaTime;
//                 currentSpeed = Mathf.Min(currentSpeed, finalSpeed);
//             }

//             // Calculate the next position based on current speed
//             Vector3 nextPosition = transform.position + direction * currentSpeed * Time.deltaTime;

//             // Check if we’re past the target by comparing directions
//             Vector3 toTarget = target - transform.position;
//             if (Vector3.Dot(toTarget, direction) <= 0) // Dot product is 0 or negative when past target
//             {
//                 rb.MovePosition(target); // Snap to target position to prevent overshoot
//                 break; // Exit the loop once target is reached
//             }

//             // Otherwise, move towards the target
//             rb.MovePosition(nextPosition);

//             myVelocity = ((Vector2)transform.position - (Vector2)oldPos) * Time.deltaTime;
//             oldPos = (Vector2)transform.position;

//             yield return null; // Wait for the next frame
//         }

//         rb.MovePosition(target);
//         _isAccelerating = false;

//         if(volReposit && isOutbound)
//         {
//             _isAccelerating = true;
//             StartCoroutine(SuddenAccel(target, start, false));
//         }
//     }

//     private void FixedUpdate()
//     {
//         if(_moveVoluntarily) 
//         {
//             //keep moving until the distance between this object and the target is smaller than epsilon
//             if(Vector2.Distance(rb.position, _edges[index].position) > 0.1f){
//                 Vector2 toVector = Vector2.MoveTowards((Vector2)rb.position, (Vector2)_edges[index].position, _movespeed * Time.deltaTime);
//                 rb.MovePosition(toVector);
//             }
//             else {
//                 rb.MovePosition(_edges[index].position);
//                 index = Increment(index);
//             }
//         }

//         if(_stopOnTouch)
//         {
//             Debug.LogError("Implement something here!");
//         }

//         myVelocity = (rb.position - (Vector2)oldPos) / Time.deltaTime;
//         oldPos = (Vector2)rb.position;
//     }

//     private int Increment(int x)
//     {
//         return (x < n-1)? x+1 : 0;
//     }
// }