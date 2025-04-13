using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    [Header("Notice")]
    public GameObject uiNotice;

    private WaitForSecondsRealtime wait;

    public enum Achieve
    {
        UnlockPotato,
        UnlockBean
    }
    private Achieve[] achieves;

    private void Awake()
    {
        achieves = (Achieve[])System.Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5f);

        if (false == PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }        
    }

    private void Start()
    {
        UnlockCharacter();
    }
    

    private void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }    
    }

    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    private void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    private void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
            default:
                break;
        }
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
