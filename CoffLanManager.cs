using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffLanManager : MonoBehaviour
{
    [SerializeField]private Sprite idle;
    [SerializeField]private Sprite crushed;
    [SerializeField] private float launchPower = 100f;
    [SerializeField] private float cooldown = 0.7f;
    private Coroutine CoffLanCoroutine = null;
    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.CompareTag("Player"))
        {
            if(CoffLanCoroutine == null) {
                Vector2 launchVec = new Vector2(0f, ((Vector2)obj.transform.position - obj.contacts[0].point).y).normalized * launchPower;
                obj.gameObject.GetComponent<Rigidbody2D>().AddForce(launchVec, ForceMode2D.Impulse);
                CoffLanCoroutine = StartCoroutine(CoffLanCooldown());
            }
        }
    }
    private IEnumerator CoffLanCooldown()
    {
        sp.sprite = crushed;
        yield return new WaitForSeconds(cooldown);
        sp.sprite = idle;
        CoffLanCoroutine = null;
    }
}
