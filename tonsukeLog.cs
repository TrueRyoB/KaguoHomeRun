using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class TonsukeLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private PlayerMotion Player;
    [SerializeField] private GameObject Textbox;
    [SerializeField] private Image Face;
    [SerializeField] private GameObject EventSystem;

    [SerializeField] private Sprite tonsuke;
    [SerializeField] private Sprite shaddie;
    [SerializeField] private Sprite kaguo_calm;
    [SerializeField] private Sprite kaguo_sad;

    [SerializeField]private List<LogSet> logSetList = new List<LogSet>();

    public bool IsTalking { get; private set;}
    public bool IsResult { get; private set;} = false;
    private TonsukeExecutor tonsukeExecutor;

    void Start()
    {
        if(Player == null)
            Debug.LogError("Player is not set to the tonsuke asset!");
        if(logSetList.Count() < 1)
            Debug.LogWarning("No element is set to the list logSetList!");
    }

    public void Narrate(string signID)
    {
        Player.StunHim();
        IsTalking = true;
        LogSet logSet = logSetList.FirstOrDefault(logset => logset.ID == signID);
        if(logSet==null) {
            Debug.LogError("No LogSet matching to the given ID " + signID + " was found!");
            return;
        }

        bool isFirstTime = logSet.IsReadFirstTime;
        logSet.MarkAsRead();

        StartCoroutine(PlayLogSequentially(logSet.logInfos, isFirstTime, signID));
    }

    private IEnumerator PlayLogSequentially(List<LogInfo> logInfos, bool isFirstTime, string signID)
    {
        int i = 0;
        foreach (LogInfo logInfo in logInfos)
        {
            bool shouldDisplay = logInfo.DisplayConditionLog switch
            {
                LogInfo.DisplayCondition.Always => true,
                LogInfo.DisplayCondition.OnlyFirstTime => isFirstTime,
                LogInfo.DisplayCondition.FromSecondTime => !isFirstTime,
                _ => false
            };

            if(shouldDisplay) 
            {
                SetIcon(logInfo.IconTypeLog);
                if(tonsukeExecutor == null)
                    tonsukeExecutor = GetComponent<TonsukeExecutor>();
                tonsukeExecutor.ExecuteInNeed(signID + "_" + i.ToString());
                yield return StartCoroutine(PlayText(logInfo.Text));
            }
            ++i;
        }
        IsTalking = false;
        Player.CleanseHim();
        this.gameObject.SetActive(false);
    }

    private IEnumerator PlayText(string s)
    {
        textLabel.text = "";
        int r = 0, c = 0;
        int n = s.Length;
        adjust4SS();
        bool isWaiting4Enter = true;

        while(r < n)
        {
            if(s[r] == '$' && r < n-1 && s[r+1] == 'e') { //wait for enter key
                r += 2;
                while(!Input.GetKeyDown(KeyCode.Return))
                    yield return null;
            } else if(s[r] == '$' && r < n-2 && s[r+1] == 't' && isRestrictivelyDigit(s[r+2])) { //wait for extra seconds
                int cd = s[r+2]-'0';
                r += 3;
                while(r < n && isRestrictivelyDigit(s[r])) {
                    cd = cd * 10 + (s[r]-'0');
                    ++r;
                }
                yield return new WaitForSeconds(0.1f * cd);
            } else if(s[r] == '$' && r < n-1 && s[r+1] == 's') { //skip waiting for enter key
                r += 2;
                isWaiting4Enter = false;
            } else if(s[r] == '$' && r < n-1 && s[r+1] == 'c') { //cleanse him
                r += 2;
                Player.CleanseHim();
            } else if(s[r] == '$' && r < n-1 && s[r+1] == 'g') { //goal (dont setactive false himself)
                r += 2;
                IsResult = true;
                while(IsResult)
                    yield return new WaitForSeconds(10f);
            } else { //display text
                textLabel.text += s[r];
                ++c;
                ++r;
                if(c > 18)  adjust4LS();
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
        while(isWaiting4Enter && !Input.GetKeyDown(KeyCode.Return))
            yield return null;
    }


    private bool isRestrictivelyDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void adjust4LS()
    {
        Textbox.GetComponent<RectTransform>().localPosition = new Vector3(-60, 260, 0);
    }
    private void adjust4SS()
    {
        Textbox.GetComponent<RectTransform>().localPosition = new Vector3(-60, 220, 0);
    }

    private void SetIcon(IconType iconType)
    {
        //I'm not implementing a matrix system as these two don't change their face this entire time

        switch(iconType.IconLog)
        {
            case IconType.Icon.Tonsuke:
                Face.sprite = tonsuke;
                break;
            case IconType.Icon.Shadie:
                Face.sprite = shaddie;
                break;

            case IconType.Icon.Kaguo:
                if(iconType.EmoLog == IconType.Emo.Calm)
                    Face.sprite = kaguo_calm;
                else
                    Face.sprite = kaguo_sad;
                break;
            default:
                Debug.LogError("No apposite icon is registered!");
                break;
        }
        
    }
}
