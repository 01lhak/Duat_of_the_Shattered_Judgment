using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Friend_DialogueManager : MonoBehaviour
{
    public static Friend_DialogueManager Instance;
    public List<GameObject> panels;

    // 현재 실행 중인 대사창의 타이핑 효과를 참조
    public Friend_TypewriterEffect CurrentTypewriter { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void Show(int index, string text, Sprite sprite)
    {
        for (int i = 0; i < panels.Count; i++)
            panels[i].SetActive(false);

        GameObject panel = panels[index];
        panel.SetActive(true);

        Image img = panel.GetComponentInChildren<Image>();
        if (img != null) img.sprite = sprite;

        // TypewriterEffect 찾기 및 저장
        CurrentTypewriter = panel.GetComponentInChildren<Friend_TypewriterEffect>();
        if (CurrentTypewriter != null)
            CurrentTypewriter.ShowText(text, null);
    }
}