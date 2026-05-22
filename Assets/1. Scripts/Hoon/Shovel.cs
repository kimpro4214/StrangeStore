using UnityEngine;
using Oculus.Interaction; // Meta SDK 필수
using System.Linq;        // FirstOrDefault 사용을 위해 필수

public class ShovelAction : MonoBehaviour
{
    [Header("Meta 그랩 컴포넌트")]
    [SerializeField] private GrabInteractable _grabInteractable;

    private bool _isGrabbed = false;
    private OVRInput.Controller _holdingController = OVRInput.Controller.None;

    private void OnEnable() => _grabInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // 1삽을 손으로 딱 잡았을 때 (Select)
        if (args.NewState == InteractableState.Select)
        {
            _isGrabbed = true;

            // 삽을 쥔 주체 정보
            var interactor = _grabInteractable.SelectingInteractors.FirstOrDefault();
            if (interactor != null)
            {
                // 잡은 손 매핑
                if (interactor.gameObject.name.Contains("Left"))
                {
                    _holdingController = OVRInput.Controller.LTouch;
                    Debug.Log("삽을 왼손으로 잡았습니다.");
                }
                else
                {
                    _holdingController = OVRInput.Controller.RTouch;
                    Debug.Log("삽을 오른손으로 잡았습니다.");
                }
            }
        }
        // 삽을 손에서 놓았을 때
        else if (args.NewState == InteractableState.Normal)
        {
            _isGrabbed = false;
            _holdingController = OVRInput.Controller.None;
            Debug.Log("삽을 놓았습니다.");
        }
    }

    private void Update()
    {
        // 최적화: 삽을 들고 있지 않다면 검지 트리거 체크를 매 프레임 하지 않고 즉시 패스합니다.
        if (!_isGrabbed) return;

        // 삽을 쥔 쪽 손의 검지 트리거를 누르는 순간
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, _holdingController))
        {
            ExecuteShovelFunction();
        }
    }

    // 트리거를 눌렀을 때 발동될 함수
    private void ExecuteShovelFunction()
    {
        Debug.Log("삽 기능 작동.");
    }
}