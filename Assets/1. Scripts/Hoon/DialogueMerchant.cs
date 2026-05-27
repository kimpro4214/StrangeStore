using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMerchant : MonoBehaviour
{
    public static DialogueMerchant Instance;

    public DialogueTyper typer;

    [Header("다음 대사까지의 딜레이")]
    public float autoNextDelay = 3f;

    Dictionary<ItemType, string[]> reactions = new Dictionary<ItemType, string[]>
    {
        { ItemType.Apple, new[] {
            "사과군. 신선해 보이는데?",
            "이런 건 시장에 널렸지만...",
            "그래도 받아두지."
        }},
        { ItemType.Money, new[] {
            "오, 현금이라니. 좋지.",
            "역시 거래는 이래야지."
        }},
        { ItemType.MusicBox, new[] {
            "이런 골동품을... 어디서 구했나?",
            "흠.. 내가 마법으로 열쇠 위치를 알려주겠네.",
            "지금 들리는 휘파람 소리를 따라가서 땅을 파보게."
        }},
        { ItemType.Dumbbell, new[] { 
            "이건... 운동하라는 건가?" ,
            "내 이두근을 보게. 운동이 더 필요하겠는가?",
            "됐으면 이제 가보게."} },
        { ItemType.Fish,     new[] {
            "아니 이것은.. 내가 가장 좋아하는 광어잖는가!!",
            "어디에 쓰이는 지는 모르겠지만.. 이 열쇠로 교환해주겠네."} },
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    Queue<string> lineQueue = new Queue<string>();
    Coroutine autoNextRoutine;

    // 아이템이 책상에 스냅되면 호출됨.
    public void OnItemSnapped(TradableItem item) => StartCoroutine(OnItemGiven(item));
    IEnumerator OnItemGiven(TradableItem item)
    {
        yield return new WaitForSeconds(SnapTradableController.Instance.onBoardTime);
        StopAutoNext();
        lineQueue.Clear();

        if (reactions.TryGetValue(item.type, out string[] lines))
            foreach (var l in lines) lineQueue.Enqueue(l);
        else
            lineQueue.Enqueue("Tradable 아이템 대사가 지정되지 않음.");

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
        // 타이핑 다 될 때까지 기다리기
        yield return new WaitUntil(() => !typer.isTyping);
        // 다음 대사까지 딜레이만큼 기다리기
        yield return new WaitForSeconds(autoNextDelay);
        autoNextRoutine = null;
        // 다음 대사 진행시키기
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
    }
}