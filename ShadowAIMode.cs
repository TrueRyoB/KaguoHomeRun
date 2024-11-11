using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAIMode : MonoBehaviour
{
    ShadowMotion Brain;
    public bool isPerforming;

    void Start()
    {
        Brain = GetComponent<ShadowMotion>();
    } 

    public void ClimbingFirstWall()
    {
        isPerforming = true;
        Brain.ResetAnimation();
        StartCoroutine(PressRight(0.46f));
        StartCoroutine(Jump(0.78f));
        StartCoroutine(PressLeft(1.49f));
        StartCoroutine(LeaveRight(1.5f));
        StartCoroutine(Jump(1.54f));
        StartCoroutine(PressRight(1.7f));
        StartCoroutine(LeaveLeft(1.72f));
        StartCoroutine(LeaveRight(2.31f));
        StartCoroutine(PressLeft(2.32f));
        StartCoroutine(Jump(2.34f));
        StartCoroutine(PressRight(2.53f));
        StartCoroutine(LeaveLeft(2.53f));
        StartCoroutine(LeaveRight(3.06f));
        StartCoroutine(PressLeft(3.07f));
        StartCoroutine(Jump(3.1f));
        StartCoroutine(LeaveLeft(3.28f));
        StartCoroutine(PressRight(3.29f));
        StartCoroutine(LeaveRight(3.85f));
        StartCoroutine(PressLeft(3.87f));
        StartCoroutine(Jump(3.87f));
        StartCoroutine(PressRight(4.07f));
        StartCoroutine(LeaveLeft(4.07f));
        StartCoroutine(LeaveRight(4.75f));
        StartCoroutine(PressLeft(4.75f));
        StartCoroutine(Jump(4.76f));
        StartCoroutine(LeaveLeft(4.93f));
        StartCoroutine(PressRight(4.94f));
        StartCoroutine(LeaveRight(5.65f));
        StartCoroutine(PressLeft(7.73f));
        StartCoroutine(LeaveLeft(8.11f));
        StartCoroutine(ResetPerforming(8.12f));
        Brain.WannaTalk = true;
    }

    IEnumerator ResetPerforming(float time)
    {
        yield return new WaitForSeconds(time);
        isPerforming = false;
    }

    IEnumerator PressLeft(float time)
    {
        yield return new WaitForSeconds(time);
        Brain.HeadLeft();
    }

    IEnumerator PressRight(float time)
    {
        yield return new WaitForSeconds(time);
        Brain.HeadRight();
    }

    IEnumerator LeaveLeft(float time)
    {
        yield return new WaitForSeconds(time);
        Brain.DontHeadLeft();
    }

    IEnumerator LeaveRight(float time)
    {
        yield return new WaitForSeconds(time);
        Brain.DontHeadRight();
    }
    
    IEnumerator Jump(float time)
    {
        yield return new WaitForSeconds(time);
        Brain.Jump();
    }

}
