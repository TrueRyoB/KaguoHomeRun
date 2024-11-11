using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyManager : MonoBehaviour
{
    [SerializeField]private Transform miniarrow;
    private BalloonguyManager balloonguy;
    private float magCir = 0.18f; //the guy's size of En

    private void Start()
    {
        balloonguy = this.gameObject.transform.parent.gameObject.GetComponent<BalloonguyManager>();
        // magCir = GetComponent<Collider2D>().bounds.size.x;
        // Debug.Log("magCir: " + magCir);
        if (miniarrow == null)
            Debug.LogError("Miniarrow is not set!");
    }

    public void UpdateMiniarrow(Vector2 goal)
    {
        Vector2 dir = (goal - (Vector2)transform.position).normalized;
        float rad = Mathf.Atan2(dir.y, dir.x);

        //????
        miniarrow.localPosition = magCir * new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        miniarrow.rotation = Quaternion.Euler(0, 0, rad*Mathf.Rad2Deg);
        //miniarrow.rotation = Quaternion.Euler(dir.x, dir.y, 0f);
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.CompareTag("Ground") || obj.gameObject.CompareTag("Lift") || obj.gameObject.CompareTag("Player"))
        {
            balloonguy.BodyKnockbacked(obj.GetContact(0).point);
        }
    }
}
