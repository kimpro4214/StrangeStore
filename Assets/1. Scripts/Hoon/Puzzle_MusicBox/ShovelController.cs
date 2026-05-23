using UnityEngine;
using Oculus.Interaction;
using System.Linq;

public class ShovelController : MonoBehaviour
{
    [Header("Meta 그랩 컴포넌트")]
    [SerializeField] private GrabInteractable _grabInteractable;

    [Header("Dig_Point")]
    public DigPoint _digPoint;

    [Header("Whistle_Controller")]
    public WhistleController _whistleController;

    [Header("Reward Key")]
    public GameObject key;

    private bool _isGrabbed = false;
    private bool isCleared = false;

    private void OnEnable() => _grabInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= HandleStateChanged;

    private void Start()
    {
        key.SetActive(false);
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        _isGrabbed = false;

        // 삽을 손으로 딱 잡았을 때
        if (args.NewState == InteractableState.Select)
        {
            Debug.Log("삽을 잡았습니다.");
            _isGrabbed = true;
        }
    }

    private void Update()
    {
        // 삽 안들고 있으면 패스
        if (!_isGrabbed) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            Dig();
        }
    }

    // 트리거를 눌렀을 때 발동될 함수
    private void Dig()
    {
        if (isCleared) return;
        if (_digPoint.canDigging)
        {
            Debug.Log("열쇠 발굴 성공!");
            AudioManager.Instance.Play2D(SoundName.dig_success);
            _whistleController.StopWhistle();
            key.SetActive(true);
            isCleared = true;
        }
        else
        {
            Debug.Log("발굴 위치가 아님.");
            AudioManager.Instance.Play2D(SoundName.dig_fail);
        }
    }
}