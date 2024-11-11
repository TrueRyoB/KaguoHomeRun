using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDetector : MonoBehaviour
{
    [SerializeField] private LayerMask inclusiveMask;
    private float raycastDistance = 0f;
    private LiftVer2 _lift;

    public LiftVer2 Lift {
        get => _lift;
    }

    void Start()
    {
        raycastDistance = GetComponent<PlayerBio>().PlayerHeight / 2f + 0.1f;
    }

    void Update()
    {
        DetectRide();
    }

    // Cast a ray downward from the player's position
    private void DetectRide()
    {
        Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.red, 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, inclusiveMask);

        if (hit.collider != null)
            _lift = hit.collider.gameObject.GetComponent<LiftVer2>();
        else
            _lift = null;
    }
}
