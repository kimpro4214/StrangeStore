using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTyper : MonoBehaviour
{
    public TMP_Text textUI;
    public float charDelay = 0.05f;
    public bool isTyping = false;

    public void ShowText(string fullText, float autoEraseDelay = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText, autoEraseDelay));
    }

    IEnumerator TypeText(string fullText, float autoEraseDelay)
    {
        isTyping = true;
        textUI.text = fullText;
        textUI.maxVisibleCharacters = 0;
        for (int i = 0; i <= fullText.Length; i++)
        {
            textUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(charDelay);
        }
        isTyping = false;

        // 자동 삭제
        if (autoEraseDelay > 0f)
        {
            yield return new WaitForSeconds(autoEraseDelay);
            EraseText();
        }
    }

    public void EraseText()
    {
        StopAllCoroutines();
        isTyping = false;
        textUI.text = "";
    }
}