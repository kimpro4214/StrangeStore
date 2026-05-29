using Oculus.Interaction;
using System.Collections;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;
    [SerializeField] public WhistleController _whistleController;
    public GameObject key;
    [SerializeField] private MerchantHintAnimator merchantHintAnimator;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OnSnapTradableItem(TradableItem item)
    {
        // 상인 대사
        DialogueMerchant.Instance.OnItemSnapped(item);
        OnSnapTrade(item.type);
    }

    // 아이템을 올려놓은 순간 실행.
    public void OnSnapTrade(ItemType type)
    {
        // 상인 힌트 애니메이션 type에 따라 실행.
        merchantHintAnimator.PlayHintSequence(type);

        // 올려놓은 아이템 타입별 실행 함수 분기
        switch (type)
        {
            case ItemType.Apple:
                Debug.Log("사과 거래.");
                break;
        }
    }

    // 상인의 대사가 모두 끝난 직후 실행할 분기
    public void OnDialogueEnded(ItemType type)
    {
        switch (type)
        {
            case ItemType.MusicBox:
                _whistleController.StartWhistle();
                break;
            case ItemType.Fish:
                AudioManager.Instance.Play2D(SoundName.key_spawn);
                key.SetActive(true);
                break;
        }
    }
}
