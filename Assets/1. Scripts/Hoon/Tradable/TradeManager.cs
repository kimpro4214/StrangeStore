using Oculus.Interaction;
using System.Collections;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager instance;
    [SerializeField] private WhistleController _whistleController;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnSnapTradableItem(TradableItem item)
    {
         ProcessTrade(item.type);
    }

    // 아이템을 올려놨을 때 타입별로 실행할 분기
    public void ProcessTrade(ItemType type)
    {
        switch (type)
        {
            case ItemType.Apple:
                Debug.Log("사과 스냅.");
                // 여기에 애니메이션 실행 등 로직 추가
                break;
            case ItemType.Money:
                Debug.Log("돈 스냅.");
                break;
            case ItemType.MusicBox:
                Debug.Log("뮤직 박스 스냅. 휘파람 재생.");
                if (_whistleController != null)
                {
                    _whistleController.StartWhistle();
                }
                break;
        }
    }

    IEnumerator DestroyItem(GameObject item)
    {
        yield return new WaitForSeconds(3f);
        item.SetActive(false);
    }
}
