using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBio : MonoBehaviour
{
    private SpriteRenderer _sp;

    public float PlayerHeight 
    {
        get => GetPlayerHeight();
    }

    private float GetPlayerHeight()
    {
        _sp = GetComponentInChildren<SpriteRenderer>(); 
        if(_sp == null) {
                Debug.LogError("SpriteRenderer is not set");
                return 0f;
        }

        Vector3 sizeInWorld = Vector3.Scale(_sp.bounds.size, transform.lossyScale);
        return Mathf.Abs(sizeInWorld.y);
    }
}
