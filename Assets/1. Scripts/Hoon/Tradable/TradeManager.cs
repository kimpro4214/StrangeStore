using Oculus.Interaction;
using System.Collections;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager instance;
    [SerializeField] public WhistleController _whistleController;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnSnapTradableItem(TradableItem item)
    {
        // 상인 대사
        DialogueMerchant.Instance.OnItemSnapped(item);
        ProcessTrade(item.type);
    }

    // 아이템을 올려놓은 순간 타입별로 실행할 분기
    public void ProcessTrade(ItemType type)
    {
        switch (type)
        {
            case ItemType.Apple:
                Debug.Log("사과 스냅.");
                break;
            case ItemType.Money:
                Debug.Log("돈 스냅.");
                break;
            case ItemType.MusicBox:
                Debug.Log("뮤직 박스 스냅.");
                break;
            case ItemType.Fish:
                Debug.Log("생선 스냅. 열쇠 소환");
                break;
        }
    }

}
