using Oculus.Interaction; // Meta SDK
using Oculus.Interaction.HandGrab;
using System.Linq;
using UnityEngine;

public class SnapKeyController : MonoBehaviour
{
    [Header("Meta 스냅 컴포넌트")]
    [SerializeField] private SnapInteractable _snapInteractable;

    [Header("표시할 고스트 프리뷰들")]
    [SerializeField] private GameObject _circleGhost;
    [SerializeField] private GameObject _polygonGhost;
    [SerializeField] private GameObject _starGhost;

    private void OnEnable() => _snapInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _snapInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // 고스트 싹 다 끄기
        _circleGhost.SetActive(false);
        _polygonGhost.SetActive(false);
        _starGhost.SetActive(false);

        // Hover 분기 (물건을 들고 테이블 구역에 진입했을 때)
        if (args.NewState == InteractableState.Hover)
        {
            // 호버한 오브젝트 가져와서 고스트 띄우기
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();
            ActiveGhost(currentInteractor);
        }

        // Grab 풀어서 테이블에 아이템 Snap됐을 때.
        else if (args.NewState == InteractableState.Select)
        {
            // 테이블에 올려놓은 아이템 가져오기
            SnapInteractor currentInteractor = _snapInteractable.SelectingInteractors.FirstOrDefault();

            // 올려 놓은 아이템 Grab 비활성화
            var handGrab = currentInteractor.GetComponent<HandGrabInteractable>();
            if (handGrab != null) handGrab.enabled = false;

            // 자물쇠 열기 시작
            LockItem lockItem = _snapInteractable.GetComponentInParent<LockItem>();
            KeyItem keyItem = currentInteractor.GetComponent<KeyItem>();
            if (lockItem != null) Debug.Log("Lock 인스턴스 존재");
            if (keyItem != null) Debug.Log("Key 인스턴스 존재");
            FinalLockManager.instance.Unlock(lockItem, keyItem);
        }
    }
    private void ActiveGhost(SnapInteractor currentInteractor)
    {
        if (currentInteractor != null)
        {
            // 열쇠 종류 불러오기
            KeyItem item = currentInteractor.gameObject.GetComponent<KeyItem>();

            if (item != null)
            {
                // 고스트 띄우기 분기점
                switch (item.type)
                {
                    case KeyType.Circle:
                        _circleGhost.SetActive(true);
                        break;
                    case KeyType.Polygon:
                        _polygonGhost.SetActive(true);
                        break;
                    case KeyType.Star:
                        _starGhost.SetActive(true);
                        break;
                }
            }
        }
    }
}