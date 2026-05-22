using UnityEngine;
using Oculus.Interaction; // Meta SDK
using System.Linq;        // FirstOrDefault() 사용을 위해 필수!

public class SnapTradableController : MonoBehaviour
{
    [Header("Meta 스냅 컴포넌트")]
    [SerializeField] private SnapInteractable _snapInteractable;

    [Header("표시할 고스트 프리뷰들")]
    [SerializeField] private GameObject _appleGhost;
    [SerializeField] private GameObject _moneyGhost;
    [SerializeField] private GameObject _musigBoxGhost;

    private void OnEnable() => _snapInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _snapInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        _appleGhost.SetActive(false);
        _moneyGhost.SetActive(false);
        _musigBoxGhost.SetActive(false);

        // Hover 분기 (물건을 들고 테이블 구역에 진입했을 때)
        if (args.NewState == InteractableState.Hover)
        {
            // 호버한 오브젝트 가져오기.
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();

            if (currentInteractor != null)
            {
                // 그 녀석의 GameObject에서 아이템 종류(TradableItem)를 읽어옵니다.
                TradableItem item = currentInteractor.gameObject.GetComponent<TradableItem>();
                ActiveGhost(item);
            }
        }

        // Grab 풀어서 테이블에 아이템 Snap됐을 때.
        else if (args.NewState == InteractableState.Select)
        {
            // 스냅된 아이템 매니저에 건네주기
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();
            TradableItem item = currentInteractor.GetComponent<TradableItem>();
            if (item != null) TradeManager.instance.OnSnapTradableItem(item);
        }
    }

    void ActiveGhost(TradableItem item)
    {
        if (item != null)
        {
            // 고스트 띄우기 분기점
            switch (item.type)
            {
                case ItemType.Apple:
                    _appleGhost.SetActive(true);
                    break;
                case ItemType.Money:
                    _moneyGhost.SetActive(true);
                    break;
                case ItemType.MusicBox:
                    _musigBoxGhost.SetActive(true);
                    break;
            }
        }
    }
}