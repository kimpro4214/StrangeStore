using DG.Tweening;
using Oculus.Interaction; // Meta SDK
using System.Collections;
using System.Collections.Generic;
using System.Linq;        // FirstOrDefault() 사용을 위해 필수!
using UnityEngine;

public class SnapTradableController : MonoBehaviour
{
    public static SnapTradableController Instance;

    [Header("Meta 스냅 컴포넌트")]
    [SerializeField] private SnapInteractable _snapInteractable;

    [Header("아이템별 고스트 프리뷰")]
    [SerializeField] private GhostPair[] _ghosts;

    [Header("템 이동 목표 트랜스폼")]
    [SerializeField] private Transform _targetTransform;

    [Header("템 스냅 유지 시간")]
    public float onBoardTime = 1f;
    [Header("템 상인에게 이동하는 시간")]
    public float onMoveTime = 1.5f;

    Dictionary<ItemType, GameObject> _ghostMap;

    [System.Serializable]
    public class GhostPair
    {
        public ItemType type;
        public GameObject ghost;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _ghostMap = _ghosts.ToDictionary(g => g.type, g => g.ghost);
        ActivateSnap();
    }

    private void OnEnable() => _snapInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _snapInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // 모든 고스트 끄기
        HideAllGhosts();

        if (args.NewState == InteractableState.Hover)
        {
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();
            if (currentInteractor != null)
            {
                TradableItem item = currentInteractor.gameObject.GetComponent<TradableItem>();
                ActiveGhost(item, true);
            }
        }
        else if (args.NewState == InteractableState.Select)
        {
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();

            // 잡기 사운드 재생, 잡기, 스냅 비활성화
            AudioManager.Instance.Play2D(SoundName.snap_item);
            currentInteractor.GetComponent<GrabInteractable>().enabled = false;

            // 트레이드 함수 실행
            TradableItem item = currentInteractor.GetComponent<TradableItem>();
            if (item != null) TradeManager.Instance.OnSnapTradableItem(item);

            ActiveGhost(item, false);
            StartCoroutine(HandleAndDestroy(currentInteractor.gameObject));
        }
    }

    void HideAllGhosts()
    {
        foreach (var pair in _ghosts)
            if (pair.ghost != null) pair.ghost.SetActive(false);
    }

    void ActiveGhost(TradableItem item, bool setting)
    {
        if (item == null) return;
        if (_ghostMap.TryGetValue(item.type, out var ghost) && ghost != null)
            ghost.SetActive(setting);
    }

    IEnumerator HandleAndDestroy(GameObject item)
    {
        // 1초 후 상인에게 이동.
        yield return new WaitForSeconds(onBoardTime);

        DeactivateSnap();

        var grab = item.GetComponent<Grabbable>();
        if (grab != null) grab.enabled = false;

        // 1.5초간 이동, 0.2배로 작아짐 후 제거
        item.transform.DOMove(_targetTransform.position, onMoveTime).SetEase(Ease.OutQuad);
        item.transform.DOScale(item.transform.localScale * 0.2f, onMoveTime);
        Destroy(item, onMoveTime);
    }

    public void ActivateSnap()
    {
        _snapInteractable.enabled = true;
    }
    public void DeactivateSnap()
    {
        _snapInteractable.enabled = false;
    }
}