using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SecurityCinemachine : MonoBehaviour
{
    public static SecurityCinemachine Instance;

    [Header("이동 대상")]
    [SerializeField] private Transform rig;        // 이동시킬 OVRCameraRig 루트(또는 플레이어 루트)
    [SerializeField] private Transform target;     // 도착 지점(빈 GameObject)

    [Header("연출 설정")]
    [SerializeField] private float duration = 3f;  // 이동 시간(초)
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("이동 중 비활성화할 로코모션")]
    [SerializeField] private OVRPlayerController ovrPlayerController; // 이동/스냅턴 등 입력 컴포넌트

    [Header("이벤트")]
    [SerializeField] private UnityEvent onStarted;  // 비네팅 ON 등 (선택)
    [SerializeField] private UnityEvent onArrived;  // NPC 대화 시작 + 비네팅 OFF

    private bool playing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // 트리거에서 이걸 호출하세요
    public void StartSequence()
    {
        if (playing) return;
        playing = true;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        SetLocomotion(false);
        onStarted?.Invoke();

        Vector3 from = rig.position;
        Vector3 to = target.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = ease.Evaluate(Mathf.Clamp01(t / duration));
            rig.position = Vector3.Lerp(from, to, k); // 위치만, 회전 X
            yield return null;
        }
        rig.position = to;

        SetLocomotion(true);
        onArrived?.Invoke();
        playing = false;
    }

    private void SetLocomotion(bool on)
    {
        ovrPlayerController.enabled = on;
    }
}