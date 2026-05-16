using UnityEngine;
using Oculus.Interaction; // Meta SDK
using System.Linq;        // FirstOrDefault() 사용을 위해 필수!

public class SnapZoneGhostKey : MonoBehaviour
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
        _circleGhost.SetActive(false);
        _polygonGhost.SetActive(false);
        _starGhost.SetActive(false);

        // 2. Hover 분기 (물건을 들고 테이블 구역에 진입했을 때)
        if (args.NewState == InteractableState.Hover)
        {
            // 호버한 오브젝트 가져오기.
            SnapInteractor currentInteractor = _snapInteractable.Interactors.FirstOrDefault();

            if (currentInteractor != null)
            {
                // 그 녀석의 GameObject에서 아이템 종류(TradableItem)를 읽어옵니다.
                KeyItem item = currentInteractor.gameObject.GetComponent<KeyItem>();

                if (item != null)
                {
                    // ★ 여기가 바로 고스트 띄우기 분기점!
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

        // Grab 풀어서 테이블에 아이템 Snap됐을 때.
        else if (args.NewState == InteractableState.Select)
        {
            // 테이블에 올려놓은 아이템 가져오기
            SnapInteractor currentInteractor = _snapInteractable.SelectingInteractors.FirstOrDefault();

            if (currentInteractor != null)
            {
                KeyItem item = currentInteractor.gameObject.GetComponent<KeyItem>();
                if (item != null)
                {
                    ProcessUnlock(item.type);
                }
            }
        }
    }

    // 아이템을 올려놨을 때 타입별로 실행할 분기
    private void ProcessUnlock(KeyType type)
    {
        switch (type)
        {
            case KeyType.Circle:
                Debug.Log("Circle Key 언락");
                // 여기에 애니메이션 실행이나 작동 로직 추가
                break;

            case KeyType.Polygon:
                Debug.Log("Polygon Key 언락");
                break;
            case KeyType.Star:
                Debug.Log("Star Key 언락");
                break;
        }
    }
}