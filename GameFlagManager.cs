using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlagManager : MonoBehaviour
{
    public static bool isAfterTutorial = false;

    private void Awake() => DontDestroyOnLoad(gameObject);
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(isAfterTutorial)
        {
            FindObjectOfType<TonsukeLog>().Narrate("t_8");
            isAfterTutorial = false;
        }
    }
}
