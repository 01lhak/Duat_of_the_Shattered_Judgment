using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Friend_TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float speed = 0.05f;

    // 타이핑 중인지 확인하는 변수
    public bool IsTyping { get; private set; }

    Coroutine routine;
    string currentText;

    public void ShowText(string msg, Action onComplete)
    {
        currentText = msg;
        IsTyping = true; // 타이핑 시작

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Type(msg, onComplete));
    }

    public void Skip()
    {
        if (routine != null) StopCoroutine(routine);
        textUI.text = currentText;
        IsTyping = false; // 타이핑 즉시 완료
    }

    IEnumerator Type(string msg, Action onComplete)
    {
        textUI.text = "";
        foreach (char c in msg)
        {
            textUI.text += c;
            yield return new WaitForSeconds(speed);
        }
        IsTyping = false; // 타이핑 끝
        onComplete?.Invoke();
    }
}