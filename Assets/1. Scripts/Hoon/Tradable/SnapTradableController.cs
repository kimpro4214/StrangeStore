using UnityEngine;
using Oculus.Interaction; // Meta SDK
using System.Linq;        // FirstOrDefault() 사용을 위해 필수!
using System.Collections;
using DG.Tweening;

public class SnapTradableController : MonoBehaviour
{
    [Header("Meta 스냅 컴포넌트")]
    [SerializeField] private SnapInteractable _snapInteractable;

    [Header("표시할 고스트 프리뷰들")]
    [SerializeField] private GameObject _appleGhost;
    [SerializeField] private GameObject _moneyGhost;
    [SerializeField] private GameObject _musigBoxGhost;

    [Header("템 이동 목표 트랜스폼")]
    [SerializeField] private Transform _targetTransform;

    [Header("템 스냅 유지 시간")]
    public float onBoardTime = 1f;
    [Header("템 상인에게 이동하는 시간")]
    public float onMoveTime = 1.5f;

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
                ActiveGhost(item, true);
            }
        }

        // Grab 풀어서 테이블에 아이템 Snap됐을 때.
        else if (args.NewState == InteractableState.Select)
        {
            // 스냅된 아이템 매니저에 건네주기
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();

            // 스냅된 아이템 잡기 비활성화
            currentInteractor.GetComponent<GrabInteractable>().enabled = false;

            // 스냅된 아이템 상호작용 매니저에서 진행시킴.
            TradableItem item = currentInteractor.GetComponent<TradableItem>();
            if (item != null) TradeManager.instance.OnSnapTradableItem(item);

            // 고스트 제거
            ActiveGhost(item, false);

            // 아이템 상인에게 이동 후 제거
            StartCoroutine(HandleAndDestroy(currentInteractor.gameObject));

        }
    }

    void ActiveGhost(TradableItem item, bool setting)
    {
        if (item != null)
        {
            // 고스트 띄우기 분기점
            switch (item.type)
            {
                case ItemType.Apple:
                    _appleGhost.SetActive(setting);
                    break;
                case ItemType.Money:
                    _moneyGhost.SetActive(setting);
                    break;
                case ItemType.MusicBox:
                    _musigBoxGhost.SetActive(setting);
                    break;
            }
        }
    }

    IEnumerator HandleAndDestroy(GameObject item)
    {
        // 1초 후 상인에게 이동.
        yield return new WaitForSeconds(onBoardTime);
        var grab = item.GetComponent<Grabbable>();
        if (grab != null) grab.enabled = false;

        var grabInter = item.GetComponent<GrabInteractable>();
        if (grabInter != null) grabInter.enabled = false;

        var snapInteractor = item.GetComponent<SnapInteractor>();
        if (snapInteractor != null) snapInteractor.enabled = false;

        // 1.5초간 이동, 0.2배로 작아짐 후 제거
        item.transform.DOMove(_targetTransform.position, onMoveTime).SetEase(Ease.OutQuad);
        item.transform.DOScale(item.transform.localScale * 0.2f, onMoveTime);
        Destroy(item, onMoveTime);
    }
}