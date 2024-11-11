using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BalloonguyManager : MonoBehaviour
{
    [SerializeField] private Tilemap obstacleMap;
    [SerializeField] private GuyManager guyManager;
    [SerializeField] private Transform[] path;
    [SerializeField] private int reward = 5;
    [SerializeField] private float speed = 10;
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite sad;
    [SerializeField] private Sprite mocking;
    [SerializeField] private Sprite exploded;
    [SerializeField] private float knockbackForce = 50f;
    private SpriteRenderer sp;
    private Rigidbody2D rb;
    private int index = 0;
    private int n = 0;
    private float epsilon = 0.1f;
    private bool isIntact = true;
    private Vector2 appliedVec = Vector2.zero;
    private float adjustScaler = 1f; //to cover a loss caused by appliedVec
    private Coroutine kbCoroutine = null; //knockback
    private bool [,] matrix;
    //private Vector2 closestTarget; //unlock this later on after implementing the SPBM
    private Vector2 finalGoal = Vector2.zero;

    private void Start()
    {
        if(obstacleMap==null) {
            Debug.LogError("This ballonguy isn't assigned its map!");
            return;
        } else {
            //InitializeMatrix();
            //do something
        }
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sp.sprite = idle;
        rb.gravityScale = 0f;
        if(path == null || path.Length == 0) {
            Debug.LogError("This ballonguy isn't assigned its path!");
            return;
        } else {
            n = path.Length;
            finalGoal = (Vector2)path[n-1].position;
        }
    }

    private bool isPassable(Vector3Int pos)
    {
        int x = pos.x - obstacleMap.cellBounds.xMin;
        int y = pos.y - obstacleMap.cellBounds.yMin;

        if(x<0 || x>=matrix.GetLength(0) || y<0 || y >= matrix.GetLength(1))
            return false;
        return matrix[x,y];
    }

    private void InitializeMatrix()
    {
        BoundsInt bounds = obstacleMap.cellBounds;
        matrix = new bool[bounds.size.x, bounds.size.y];

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = obstacleMap.GetTile(pos);
            matrix[pos.x-bounds.xMin, pos.y-bounds.yMin] = (tile == null);//true means passable
        }
    }

    private void Update()
    {
        guyManager.UpdateMiniarrow(finalGoal);
        if(isIntact && n > 1) {
            if ((Vector2.Distance((Vector2)rb.position, (Vector2)path[index].position) >= epsilon) ){
                Vector2 toPath = speed * ((Vector2)path[index].position - (Vector2)rb.position).normalized;
                rb.velocity = toPath * adjustScaler + appliedVec;
            }
            else if (index < n-1) {
                ++ index;
                if(index == n-1)
                    sp.sprite = mocking;
            }
        }
    }

    public void BodyKnockbacked(Vector2 collLoc)
    {
        Vector2 knockbackVec = knockbackForce * ((Vector2)rb.position - collLoc).normalized;
        if(kbCoroutine != null) {
            StopCoroutine(kbCoroutine);
            kbCoroutine = null;
        }
        StartCoroutine(UpdateAppliedForce(knockbackVec, 1f));
    }

    private IEnumerator UpdateAppliedForce(Vector2 initVec, float duration)
    {
        adjustScaler = 1f;
        appliedVec = Vector2.zero;

        float initiated = Time.time;
        while (Time.time-initiated <= duration) {
            float ratio = (Time.time - initiated) / duration;
            appliedVec = Vector2.Lerp(initVec, Vector2.zero, ratio);
            yield return null;
        }
        appliedVec = Vector2.zero;
        
        //to come back quicker to the path after a certain deviation
        initiated = Time.time;
        float maxScale = Mathf.Max(initVec.magnitude / speed, 1f);
        while (Time.time-initiated <= duration) {
            adjustScaler = getScale(Time.time-initiated, maxScale, duration);
            yield return null;
        }
        adjustScaler = 1f;
        kbCoroutine = null;
    }

    //assuming s being the maxScale and d being the entire duration, and x being the current time
    //to flow from 1f, maxScale, to 1f in d seconds
    private float getScale(float x, float s, float d)
    {
        return (-(s+2f)/4f) * Mathf.Cos(2f*Mathf.PI*x/d) + (s+6f)/4f;
    }

    public void BallonExplodes(PlayerMotion playerMotion)
    {
        isIntact = false;
        sp.sprite = exploded;
        rb.gravityScale = 2f;
        playerMotion.GetPoint(reward);
        Destroy(this.gameObject, 5f);
    }
}
