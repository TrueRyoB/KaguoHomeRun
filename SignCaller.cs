using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignCaller : MonoBehaviour
{
    [SerializeField] private SignManager signManager;
    [SerializeField][Tooltip("Alt Caption")] private string altCaption;
    [SerializeField][Tooltip("Alt ID")] private string altID;

    private void Start()
    {
        if(signManager == null)
            Debug.LogError("Please register this collider to an apposite sign");
        if(string.IsNullOrEmpty(altCaption) || string.IsNullOrEmpty(altID))
            Debug.LogError("The alt text is empty");
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.CompareTag("Player"))
            signManager.ModifyCaption2(altID, altCaption);
    }
}
