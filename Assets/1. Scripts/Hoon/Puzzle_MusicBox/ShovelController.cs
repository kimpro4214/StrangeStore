using UnityEngine;
using Oculus.Interaction;
using System.Collections;

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

    [Header("게이지 시간")]
    [SerializeField] private float _holdDuration = 2f;

    [Header("게이지 3D 오브젝트")]
    [SerializeField] private Transform _gaugeBar;  // 스케일로 표시할 바

    [SerializeField] private GameObject particle;

    private bool _isGrabbed = false;
    public bool canInteract = false;
    private bool isCleared = false;
    private Coroutine _digCoroutine;

    private bool _triggerArmed = false;  // 차징 입력 허용 여부 (잡을 때 눌린 트리거 무시용)

    private void OnEnable() => _grabInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= HandleStateChanged;

    private void Start()
    {
        key.SetActive(false);
        UpdateGauge(0f);
        _gaugeBar.gameObject.SetActive(false);
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        _isGrabbed = (args.NewState == InteractableState.Select);

        // 놓으면 차징 취소
        if (!_isGrabbed) CancelCharge();

        if (_isGrabbed)
        {
            DialogueShovel.Instance.OnGrab();
            particle.SetActive(false);
            _triggerArmed = false;   // 잡은 직후엔 차징 잠금 (트리거 한번 떼야 풀림)
        }
    }

    private void Update()
    {
        if (!_isGrabbed || isCleared) return;

        bool triggerHeld =
            OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
            OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

        // 잡을 때 눌려있던 트리거를 한 번 떼야 차징 가능 상태로 전환
        if (!_triggerArmed)
        {
            if (!triggerHeld) _triggerArmed = true;  // 손 뗐다 → 이제 허용
            return;
        }

        // 트리거 누르는 동안 차징 시작
        if (triggerHeld && _digCoroutine == null)
        {
            _digCoroutine = StartCoroutine(DigCharge());
            _gaugeBar.gameObject.SetActive(true);
            AudioManager.Instance.Play2D(SoundName.dig);
        }
        // 트리거 떼면 차징 취소
        else if (!triggerHeld && _digCoroutine != null)
        {
            CancelCharge();
            _gaugeBar.gameObject.SetActive(false);
            AudioManager.Instance.Stop2D(SoundName.dig);
        }
    }

    private IEnumerator DigCharge()
    {
        float elapsed = 0f;

        while (elapsed < _holdDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _holdDuration;
            UpdateGauge(progress);
            yield return null;
        }

        UpdateGauge(0f);  // 게이지 리셋
        _digCoroutine = null;
        Dig();
        _triggerArmed = false;
    }

    private void CancelCharge()
    {
        if (_digCoroutine != null)
        {
            StopCoroutine(_digCoroutine);
            _digCoroutine = null;
        }
        UpdateGauge(0f);
    }

    private void UpdateGauge(float progress)
    {
        if (_gaugeBar == null) return;

        // X축 스케일로 게이지 표현
        Vector3 scale = _gaugeBar.localScale;
        scale.x = progress * 0.25f;
        _gaugeBar.localScale = scale;
    }

    private void Dig()
    {
        _gaugeBar.gameObject.SetActive(false);
        if (isCleared) return;

        if (_digPoint.canDigging && canInteract)
        {
            Debug.Log("열쇠 발굴 성공!");
            AudioManager.Instance.Play2D(SoundName.dig_success);
            _whistleController.StopWhistle();
            key.SetActive(true);
            isCleared = true;
        }
        else
        {
            Debug.Log("발굴 위치가 아님 or 오르골 안건네줌");
            AudioManager.Instance.Play2D(SoundName.dig_fail);
        }
    }
}
