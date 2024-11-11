using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private TonsukeLog tonsukeLog;
    private GameDataManager gdManager;
    void Start()
    {
        gdManager = GetComponent<GameDataManager>();
        if(tonsukeLog == null)
            Debug.LogError("No tonsukelog is assigned to class GameFlowManager (eventsystem)!");
        else
            tonsukeLog.gameObject.SetActive(false);
        
        //debug
        tonsukeLog.gameObject.SetActive(true);
        tonsukeLog.Narrate("countdown");
    }

    public void PlayerGoaled()
    {
        gdManager.StopRecording();
        TransitToResult();
    }
    private void TransitToResult()
    {
        Debug.Log("Hi!");
    }
}
