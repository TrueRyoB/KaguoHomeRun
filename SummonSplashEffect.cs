using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSplashEffect : MonoBehaviour
{
    [SerializeField] private GameObject splash;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite rareSprite;
    [SerializeField] private int numKaguoAtOnce = 10;
    // private Queue<GameObject> pool = new Queue<GameObject>();

    private int frameCount = 0;
    private int chanceThreshold;
    private int rareActivated = 0;

    private void Start()
    {
        chanceThreshold = Random.Range(2000, 4000000);//base threshold (1 in 10mins)
    }

    private void FixedUpdate()
    {
        frameCount++;

        if (frameCount >= chanceThreshold) {
            frameCount = 0;
            rareActivated = numKaguoAtOnce;
            chanceThreshold = Random.Range(20000, 4000000);
        }
    }

    public void Splash()
    {
        GameObject newParticle = Instantiate(splash, transform.position, splash.transform.rotation);
        ParticleSystem particleSystem = newParticle.GetComponent<ParticleSystem>();
        if(rareActivated != 0) {
            particleSystem.textureSheetAnimation.SetSprite(0, rareSprite);
            -- rareActivated;
        }
        particleSystem.Play();
        Destroy(newParticle, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        // GameObject newParticle = GetPooledObject(splash);
        // ParticleSystem particleSystem = newParticle.GetComponent<ParticleSystem>();
        // if(rareActivated != 0) {
        //     particleSystem.textureSheetAnimation.SetSprite(0, rareSprite);
        //     -- rareActivated;
        // } else {
        //     particleSystem.textureSheetAnimation.SetSprite(0, normal);
        // }
        // particleSystem.Play();
        // StartCoroutine(ReturnEffToPool(newParticle, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax));
    }

    // private GameObject GetPooledObject(GameObject eff)
    // {
    //     if (pool.Count > 0) {
    //         GameObject obj = pool.Dequeue();
    //         obj.SetActive(true);
    //         return obj;
    //     }
    //     else{
    //         return Instantiate(eff, transform.position, eff.transform.rotation);
    //     }
    // }

    // private void EmptyPool()
    // {
    //     while(pool.Count > 0){
    //         GameObject del = pool.Dequeue();
    //         Destroy(del);
    //     }
    // }

    // private IEnumerator ReturnEffToPool(GameObject obso, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     obso.SetActive(false);
    //     pool.Enqueue(obso);
    // }
}
