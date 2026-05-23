// A 버튼 토글로 OVRPlayerController의 카메라 높이를 앉기/일어서기 상태로 부드럽게 전환
using UnityEngine;

[RequireComponent(typeof(OVRPlayerController))]
public class CrouchToggle : MonoBehaviour
{
    [Header("토글 입력")]
    [SerializeField] private OVRInput.Button _toggleButton = OVRInput.Button.One; // 우측 A 버튼

    [Header("CameraHeight 오프셋")]
    [SerializeField] private float _standHeight = 0f;
    [SerializeField] private float _crouchHeight = -0.6f;

    [Header("보간 속도")]
    [SerializeField] private float _lerpSpeed = 8f;

    private OVRPlayerController _player;
    private bool _isCrouching;
    private float _targetHeight;

    private void Awake()
    {
        _player = GetComponent<OVRPlayerController>();
        _targetHeight = _standHeight;
        _player.CameraHeight = _standHeight;
    }

    private void Update()
    {
        if (OVRInput.GetDown(_toggleButton))
        {
            _isCrouching = !_isCrouching;
            _targetHeight = _isCrouching ? _crouchHeight : _standHeight;
        }

        _player.CameraHeight = Mathf.Lerp(_player.CameraHeight, _targetHeight, Time.deltaTime * _lerpSpeed);
    }
}
