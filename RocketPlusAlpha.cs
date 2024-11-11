using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPlusAlpha : MonoBehaviour
{
    [SerializeField][Tooltip("Assign a prefab")] private GameObject zzzEff;
    [SerializeField] private GameObject smokeEff;
    [SerializeField] Sprite awake;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private GameObject instantiatedSys = null;
    private Coroutine roCoroutine = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        
        instantiatedSys = Instantiate(zzzEff, new Vector2(transform.position.x - 13f, transform.position.y + 12f), zzzEff.transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player")) {
            sp.sprite = awake;
            Destroy(instantiatedSys);
            instantiatedSys = Instantiate(smokeEff, new Vector2(transform.position.x -3f, transform.position.y - 25f), smokeEff.transform.rotation);
            roCoroutine = StartCoroutine(UpdateSmokePos());
        }
    }

    private IEnumerator UpdateSmokePos()
    {
        instantiatedSys.transform.position = new Vector2(transform.position.x -3f, transform.position.y - 25f);
        yield return null;
    }
}
