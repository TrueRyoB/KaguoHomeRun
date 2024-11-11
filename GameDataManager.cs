using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.IO;
using System;

[System.Serializable]
public class InputData
{
    public int Frame; 
    public string keyPressed;
    public string inputPhase;
    //add more later on
}

[System.Serializable]
public class InputLogWrapper
{
    public List<InputData> inputLog;
}

[System.Serializable]
public class PlayData
{
    public string impression; //for player to store their feeling right after the game
    public float record;       //time took to pass the game
    public DateTime date;       //indicating the time when a player passe the game
    public string inputData;    //is jsoned
}

//only class that may modify the PlayerPrefs data. Other class may only see the data
public class GameDataManager : MonoBehaviour
{
    public List<InputData> inputLog = new List<InputData>();
    private bool shouldRecord = false;

    // private InputEventTrace Trace = null;
    private InputAction moveAction;

    private void Start()
    {
        StartRecording(); //test
    }

    public void StartRecording()
    {
        StartCoroutine(RecordInput((json) =>
        {
            Debug.Log("recorded data: " + json);
        }));
    }

    public void StopRecording()
    {
        if(shouldRecord == false)
        {
            Debug.LogError("Nothing was recorded :(");
        } else 
        {
            shouldRecord = false;
        }
    }

    private IEnumerator RecordInput(Action<string> onComplete)
    {
        shouldRecord = true;
        inputLog.Clear();
        moveAction = new InputAction("Move", binding: "<Keyboard>/anyKey");
        moveAction.Enable();
        moveAction.performed += OnInputPerformed;
        moveAction.canceled += OnInputCanceled;

        while(shouldRecord) {
            yield return null;
        }

        string json = JsonUtility.ToJson(new InputLogWrapper { inputLog = inputLog });
        onComplete?.Invoke(json);
        moveAction.performed -= OnInputPerformed;
        moveAction.canceled -= OnInputPerformed;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InputData data = new InputData
            {
                Frame = Time.frameCount,
                keyPressed = context.control.displayName,
                inputPhase = "Pressed",
            };
            inputLog.Add(data);
            Debug.Log("押されたボタン: " + context.control.displayName);
        }
    }

    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            InputData data = new InputData
            {
                Frame = Time.frameCount,
                keyPressed = context.control.displayName,
                inputPhase = "Released",
            };
            inputLog.Add(data);
        }
    }


    // private IEnumerator RecordInput(Action<string> onComplete)
    // {
    //     moveAction = new InputAction("Move", binding: "<Keyboard>/anyKey");
    //     moveAction.Enable();

    //     shouldRecord = true;
    //     Trace = new InputEventTrace(Keyboard.current);
    //     Trace.onEvent += ev =>
    //     {
    //         if (ev is KeyEvent keyEvent) {
    //             if (keyEvent.phase == InputPhase.Performed) {
    //                 InputData data = new InputData
    //                 {
    //                     Frame = Time.frameCount,
    //                     keyPressed = $"{keyEvent.control.displayName} (Pressed)",
    //                 };
    //                 inputLog.Add(data);
    //             }
    //             else if (keyEvent.phase == InputPhase.Canceled) {
    //                 InputData data = new InputData
    //                 {
    //                     Frame = Time.frameCount,
    //                     keyPressed = $"{keyEvent.control.displayName} (Released)",
    //                 };
    //                 inputLog.Add(data);
    //             }
    //         }
    //     };
    //     while(shouldRecord) {
    //         yield return null;
    //     }
    //     Trace.Dispose();
    //     string json = JsonUtility.ToJson(new InputLogWrapper { inputLog = inputLog });
    //     onComplete?.Invoke(json);
    // }

    // private IEnumerator RecordInput(Action<string> onComplete)
    // {
    //     Trace = new InputEventTrace(Keyboard.current);
    //     Trace.onEvent += ev => Debug.Log(ev.ToString()); //debug
    //     shouldRecord = true;
    //     while(shouldRecord) {
    //         if(Input.anyKeyDown) {
    //             InputData data = new InputData
    //             {
    //                 Frame = Time.frameCount,
    //                 keyPressed = Input.inputString,
    //             };
    //             inputLog.Add(data);
    //         }
    //         yield return null;
    //     }
    //     string json = JsonUtility.ToJson(new InputLogWrapper { inputLog = inputLog });
    //     onComplete?.Invoke(json);
    // }

    private IEnumerator ReplayInput(string filename)
    {
        List<InputData> replayData = new List<InputData>();
        if(PlayerPrefs.HasKey(filename))
            replayData = JsonUtility.FromJson<List<InputData>>(PlayerPrefs.GetString(filename));
        else {
            Debug.LogError("No such a file named " + filename + " was found!");
            yield break;
        }

        int i = 0;
        while(i < replayData.Count) {
            InputData currentData = replayData[i];
            if (Time.frameCount == currentData.Frame) {
                ProcessInput(currentData);
                ++ i;
            }
            yield return null;
        }
    }
    private void ProcessInput(InputData inputData)
    {
        Debug.Log("Please implement something here! Thank you.");
    }

    ///<summary>
    /// this function assumes that both the filename duplication check and consent are done beforehand
    ///</summary>
    private void SavePlayData(string filename, string stagename, PlayData playData)
    {
        string json = JsonUtility.ToJson(playData); //maybe compress the data here?
        string isStoredAs = stagename + "_" + filename;
        PlayerPrefs.SetString(isStoredAs, json);
        PlayerPrefs.Save();
    }
}

/*
DeleteAll();
DeleteKey(); //delets both the key and the corresponding value
GetFloat();
GetInt();
GetString();
HasKey(); //returns a boolean value
Save(); ..writes all modified preferences to disk
SetFloat();
SetInt();
SetString();

*/
