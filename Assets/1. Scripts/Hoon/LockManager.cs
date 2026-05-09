using DG.Tweening;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    [System.Serializable]
    public class DrumLine
    {
        [Header("드럼 오브젝트 (외형 참조용)")]
        public Transform drumTransform;
        [Header("현재 인덱스 (0~9)"), Range(0, 9)]
        public int curIndex = 0;
        [Header("정답 인덱스 (인스펙터에서 입력)")]
        public int answerIndex = 0;
        [HideInInspector] public int prevIndex = 0;
        [HideInInspector] public Tweener currentTween;
    }

    [Header("드럼 라인 설정")]
    [SerializeField] private DrumLine[] drumLines;

    [Header("회전 설정")]
    [SerializeField] private float rotateDuration = 0.4f;
    [SerializeField] private Ease rotateEase = Ease.OutBack;

    private const float DEG = 36f;
    private bool _cleared;

    private void Start()
    {
        foreach (var L in drumLines)
        {
            if (L.drumTransform == null) continue;
            L.drumTransform.localRotation = Quaternion.Euler(0f, 0f, -L.curIndex * DEG);
            L.prevIndex = L.curIndex;
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < drumLines.Length; i++)
            if (drumLines[i].curIndex != drumLines[i].prevIndex)
                SetDrumIndex(i, drumLines[i].curIndex);
    }

    public void SetDrumIndex(int idx, int val)
    {
        if (idx < 0 || idx >= drumLines.Length) return;
        val = Mathf.Clamp(val, 0, 9);
        drumLines[idx].curIndex = val;

        DrumLine L = drumLines[idx];
        if (L.drumTransform == null) { L.prevIndex = val; return; }
        L.currentTween?.Kill();

        float tZ = val * DEG;
        float cZ = L.drumTransform.localEulerAngles.z;
        if (cZ > 180f) cZ -= 360f;
        float d = tZ - cZ;
        if (d > 180f) d -= 360f;
        if (d < -180f) d += 360f;

        L.currentTween = L.drumTransform
            .DOLocalRotate(new Vector3(0f, 0f, cZ + d), rotateDuration, RotateMode.Fast)
            .SetEase(rotateEase)
            .OnComplete(() =>
            {
                Vector3 e = L.drumTransform.localEulerAngles;
                e.z = ((tZ % 360f) + 360f) % 360f;
                L.drumTransform.localEulerAngles = e;
            });

        L.prevIndex = val;

        CheckAnswer();
    }

    private void CheckAnswer()
    {
        foreach (var line in drumLines)
        {
            if (line.curIndex != line.answerIndex)
            {
                _cleared = false;
                return;
            }
        }

        if (_cleared) return;
        _cleared = true;
        OnClear();
    }

    private void OnClear()
    {
        Debug.Log("[LockManager] CLEAR!");
    }

    public void IncrementDrum(int idx)
    {
        Debug.Log("인크리먼트!");
        if (idx < 0 || idx >= drumLines.Length) return;
        SetDrumIndex(idx, (drumLines[idx].curIndex + 1) % 10);
    }

    public void DecrementDrum(int idx)
    {
        if (idx < 0 || idx >= drumLines.Length) return;
        SetDrumIndex(idx, (drumLines[idx].curIndex + 9) % 10);
    }

    public void ResetAll()
    {
        _cleared = false;
        for (int i = 0; i < drumLines.Length; i++)
        {
            drumLines[i].curIndex = 0;
            drumLines[i].currentTween?.Kill();
            if (drumLines[i].drumTransform != null)
                drumLines[i].drumTransform.localRotation = Quaternion.identity;
            drumLines[i].prevIndex = 0;
        }
    }

    public int[] GetCurrentIndices()
    {
        int[] r = new int[drumLines.Length];
        for (int i = 0; i < drumLines.Length; i++) r[i] = drumLines[i].curIndex;
        return r;
    }

    // ===== VR용: 드럼별 회전 함수 (Inspector에서 이벤트로 연결) =====

    public void IncrementDrum0() { IncrementDrum(0); }
    public void IncrementDrum1() { IncrementDrum(1); }
    public void IncrementDrum2() { IncrementDrum(2); }
}