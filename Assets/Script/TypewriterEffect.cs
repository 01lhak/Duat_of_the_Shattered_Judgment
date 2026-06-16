using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float typingSpeed = 0.05f;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(Type(sentence));
    }

    IEnumerator Type(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void SkipTyping(string sentence)
    {
        StopAllCoroutines();
        dialogueText.text = sentence;
        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}