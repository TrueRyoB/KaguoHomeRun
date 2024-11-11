using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TonsukeExecutor : MonoBehaviour
{
    private Dictionary<string, Action> commandMap;
    [SerializeField]private ShadowMotion Shadow;
    [SerializeField]private GameFlagManager gameFlagManager;
    [SerializeField]private GameDataManager gameDataManager;
    [SerializeField]private GameFlowManager gameFlowManager;
    public static bool isAfterTutorial = false;

    private void InitializeMap()
    {
        commandMap = new Dictionary<string, Action>
        {
            {"countdown_3", () => gameDataManager.StartRecording()},
            {"goal_0", () => gameFlowManager.PlayerGoaled()},
            {"t_3_11", () => SummonShadow(new Vector2(580f, 97.8801f))},
            {"t_5_0", () => Shadow.Recognized()},
            {"t_5_5", () => Shadow.Performance_StageT()},
            {"t_6_0", () => SummonShadow(new Vector2(640f, 60f))},
            {"t_7_4", () => Tut2Stage1()},
            //{"",},
            //{"",},
        };
    }

    public void ExecuteInNeed(string logID)
    {
        if(commandMap == null)
            InitializeMap();
        if(commandMap.TryGetValue(logID, out Action action)) {
            action.Invoke();
        }
    }

    private void SummonShadow(Vector2 loc)
    {
        if(Shadow != null) {
            Shadow.gameObject.transform.position = loc;
            Shadow.HappyDance();
        } else {
            Debug.LogError("A gameobject with a tag Shadow was not found!");
        }
    }
    private void Tut2Stage1()
    {
        GameFlagManager.isAfterTutorial = true;
        LoadSceneOf("Stage1");
    }

    private void LoadSceneOf(string SceneName)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void MarkPresent()
    {
        isAfterTutorial = true;
    }
}
