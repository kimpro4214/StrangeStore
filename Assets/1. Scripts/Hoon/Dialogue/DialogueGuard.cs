using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class DialogueGuard : MonoBehaviour
{
    public static DialogueGuard Instance;
    public DialogueTyper typer;

    [Header("다음 대사까지의 딜레이")]
    public float autoNextDelay = 2.2f;

    [Header("가드 대사 리스트")]
    [SerializeField] private string[] reactions;

    [Header("가드 대사 끝나고 나올 엔딩 크레딧")]
    [SerializeField] EndingCreditController endingCreditController;

    Queue<string> lineQueue = new Queue<string>();

    Coroutine autoNextRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // UnityEvent에서 호출
    public void ShowText()
    {
        StopAutoNext();
        lineQueue.Clear();

        foreach (string text in reactions)
        {
            lineQueue.Enqueue(text);
        }

        ShowNext();
    }

    void ShowNext()
    {
        if (lineQueue.Count <= 0)
        {
            EndDialogue();
            return;
        }
        typer.ShowText(lineQueue.Dequeue());
        autoNextRoutine = StartCoroutine(AutoNext());
    }

    IEnumerator AutoNext()
    {
        yield return new WaitUntil(() => !typer.isTyping);
        yield return new WaitForSeconds(autoNextDelay);
        autoNextRoutine = null;
        ShowNext();
    }

    void StopAutoNext()
    {
        if (autoNextRoutine != null)
        {
            StopCoroutine(autoNextRoutine);
            autoNextRoutine = null;
        }
    }

    void EndDialogue()
    {
        typer.EraseText();
        StartCoroutine(endingCreditController.FadeIn());
    }
}
