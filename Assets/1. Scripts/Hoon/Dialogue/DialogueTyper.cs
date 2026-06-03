using System;
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
        StartCoroutine(TypeText(fullText, null, autoEraseDelay)); // 목소리 인자에 null 전달
    }

    public void ShowText(string fullText, SoundName voice, float autoEraseDelay = 0f, float minFitch = 0.9f, float maxFitch = 1.3f)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText, voice, autoEraseDelay, minFitch, maxFitch)); // 받은 목소리 그대로 전달
    }

    IEnumerator TypeText(string fullText, SoundName? voice, float autoEraseDelay, float minFitch = 0.9f, float maxFitch = 1.3f)
    {
        isTyping = true;
        textUI.text = fullText;
        textUI.maxVisibleCharacters = 0;

        int validCharCount = 0;

        for (int i = 0; i <= fullText.Length; i++)
        {
            textUI.maxVisibleCharacters = i;

            if (voice.HasValue && i > 0 && i - 1 < fullText.Length)
            {
                char targetChar = fullText[i - 1];

                if (targetChar != ' ' && targetChar != '.' && targetChar != ',' &&
                    targetChar != '!' && targetChar != '?' && targetChar != '\n')
                {
                    validCharCount++;

                    if (validCharCount % 2 == 0)
                    {
                        AudioManager.Instance.PlayNPCVoice(voice.Value, minFitch, maxFitch);
                    }
                }
            }

            yield return new WaitForSeconds(charDelay);
        }
        isTyping = false;

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