using UnityEngine;
using DG.Tweening;

public class StartPanel : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private OVRPlayerController _player;

    [Header("페이드")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private bool _started = false;
    private float _originalSpeed;
    private float _originalRotSpeed;

    void Start()
    {
        // 원래 속도 저장 후 0으로
        _originalSpeed = _player.Acceleration;
        _originalRotSpeed = _player.RotationAmount;
        _player.Acceleration = 0f;
        _player.RotationAmount = 0f;
    }

    void Update()
    {
        if (_started) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        _started = true;
        _player.Acceleration = _originalSpeed;

        _canvasGroup.DOFade(0f, _fadeDuration)
            .OnComplete(() => {
                // 속도 복원
                _player.RotationAmount = _originalRotSpeed;
                gameObject.SetActive(false);
            });
    }
}