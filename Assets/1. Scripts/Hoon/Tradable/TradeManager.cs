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

    // 아이템을 올려놓은 순간 타입별로 실행할 분기
    public void OnSnapTrade(ItemType type)
    {
        switch (type)
        {
            case ItemType.Apple:
                Debug.Log("사과 거래.");
                if (merchantHintAnimator != null)
                {
                    merchantHintAnimator.PlayHintSequence();
                }
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
