using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private RectTransform rect;

    private Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        //AudioManager.instance.EffectBgm(true);
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].Onclick();
    }

    private void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
        int[] random = new int[3];
        while (true)
        {
            random[0] = Random.Range(0, items.Length);
            random[1] = Random.Range(0, items.Length);
            random[2] = Random.Range(0, items.Length);

            if (random[0] != random[1] && random[1] != random[2] && random[0] != random[2])
                break;            
        }

        for (int i = 0; i < random.Length; i++)
        {
            Item randomItem = items[random[i]];

            // 3. 만렙 아이템의 경우는 소비 아이템을 대체
            if (randomItem.level == randomItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                randomItem.gameObject.SetActive(true);
            }
        }

        
    }
}
