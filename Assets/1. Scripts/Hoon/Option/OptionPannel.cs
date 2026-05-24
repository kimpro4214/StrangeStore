using UnityEngine;
using DG.Tweening;

public class OptionPanelToggle : MonoBehaviour
{
    [Header("옵션 패널")]
    [SerializeField] private GameObject _panel;
    [Header("플레이어 머리")]
    [Tooltip("OVRCameraRig - TrackingSpace - CenterEyeAnchor")]
    [SerializeField] private Transform _headTransform;
    [Header("머리 앞 거리")]
    [SerializeField] private float _distance = 1.5f;
    [Header("기울임 정도")]
    [SerializeField] private float _heightOffset = -0.2f;
    [Header("애니메이션 시간")]
    [SerializeField] private float _animDuration = 1f;

    private bool _isOpen = false;
    private bool _isAnimating = false;
    private Vector3 originScale;

    void Start()
    {
        _panel.SetActive(false);
        originScale = _panel.transform.localScale;
    }

    void Update()
    {
        if (_isAnimating) return;

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.LTouch))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        _isOpen = !_isOpen;

        if (_isOpen) OpenPanel();
        else ClosePanel();
    }

    private void OpenPanel()
    {
        _isAnimating = true;

        // 목표 위치 계산
        Vector3 forward = _headTransform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 targetPos = _headTransform.position + forward * _distance;
        targetPos.y += _heightOffset;

        // 머리 바로 앞에서 시작
        Vector3 startPos = _headTransform.position + forward * 0.3f;
        startPos.y += _heightOffset;

        _panel.transform.position = startPos;
        _panel.transform.localScale = Vector3.zero;

        // 패널이 플레이어를 바라보게
        _panel.transform.LookAt(_headTransform);
        _panel.transform.Rotate(0, 180, 0);

        _panel.SetActive(true);

        // 눈 앞에서 목표까지 이동 + 스케일 커지기
        _panel.transform.DOMove(targetPos, _animDuration).SetEase(Ease.OutQuad);
        _panel.transform.DOScale(originScale, _animDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => _isAnimating = false);
    }

    private void ClosePanel()
    {
        _isAnimating = true;

        Vector3 closePos = _headTransform.position + _headTransform.forward * 0.3f;

        // 작아짐
        _panel.transform.DOScale(Vector3.zero, _animDuration * 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                _panel.SetActive(false);
                _isAnimating = false;
            });
    }

    public void ButtonTest()
    {
        Debug.Log("와우 버튼이 눌렸어요");
        AudioManager.Instance.Play2D(SoundName.snap_item);
    }
}