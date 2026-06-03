using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueMerchant : MonoBehaviour
{
    public static DialogueMerchant Instance;
    public DialogueTyper typer;
    private ItemType currentItem;

    [Header("다음 대사까지의 딜레이")]
    public float autoNextDelay = 2.2f;

    [Header("아이템별 대사")]
    [SerializeField] private ItemReaction[] reactions;

    [Header("대사 배경")]
    [SerializeField] private GameObject background;

    [System.Serializable]
    public class ItemReaction
    {
        public ItemType type;
        [TextArea(2, 4)] public string[] lines;
    }

    Dictionary<ItemType, string[]> _reactionMap;
    Queue<string> lineQueue = new Queue<string>();
    Coroutine autoNextRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _reactionMap = reactions.ToDictionary(r => r.type, r => r.lines);
        background.SetActive(false);
    }

    public void OnItemSnapped(TradableItem item) => StartCoroutine(OnItemGiven(item));

    IEnumerator OnItemGiven(TradableItem item)
    {
        currentItem = item.type;
        yield return new WaitForSeconds(SnapTradableController.Instance.onBoardTime);
        StopAutoNext();
        lineQueue.Clear();

        if (_reactionMap.TryGetValue(item.type, out string[] lines))
            foreach (var l in lines) lineQueue.Enqueue(l);
        else
            lineQueue.Enqueue("Tradable 아이템 대사가 지정되지 않음.");
        background.SetActive(true);
        ShowNext();
    }

    void ShowNext()
    {
        if (lineQueue.Count <= 0)
        {
            EndDialogue();
            return;
        }
        typer.ShowText(lineQueue.Dequeue(), SoundName.speak_merchant);
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
        background.SetActive(false);
        typer.EraseText();
        TradeManager.Instance.OnDialogueEnded(currentItem);
        SnapTradableController.Instance.ActivateSnap();
    }
}