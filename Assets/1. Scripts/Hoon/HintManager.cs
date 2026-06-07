using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hintImages;
    public GameObject closeButton;

    private GameObject _currentOpenHint;

    private Dictionary<GameObject, Vector3> _originScales = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        foreach (var hint in hintImages)
        {
            if (hint != null)
            {
                _originScales[hint] = hint.transform.localScale;
                hint.SetActive(false);
            }
        }

        if (closeButton != null)
        {
            _originScales[closeButton] = closeButton.transform.localScale;
            closeButton.SetActive(false);
        }
    }

    public void GetHint(int hintNum)
    {
        if (hintNum < 0 || hintNum >= hintImages.Length) return;

        if (_currentOpenHint != null && _currentOpenHint != hintImages[hintNum])
        {
            AnimateClose(_currentOpenHint);
        }

        if (_currentOpenHint == null) AnimateOpen(closeButton);

        _currentOpenHint = hintImages[hintNum];

        AnimateOpen(_currentOpenHint);
    }

    public void ClickCloseButton()
    {
        if (_currentOpenHint != null)
        {
            AnimateClose(_currentOpenHint);
            _currentOpenHint = null;
        }

        AnimateClose(closeButton);
    }

    private void AnimateOpen(GameObject obj)
    {
        if (obj == null) return;

        obj.transform.DOKill();
        obj.transform.localScale = Vector3.zero;
        obj.SetActive(true);

        Vector3 targetScale = _originScales.ContainsKey(obj) ? _originScales[obj] : Vector3.one;

        obj.transform.DOScale(targetScale, 0.4f)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
    }

    private void AnimateClose(GameObject obj)
    {
        if (obj == null) return;

        obj.transform.DOKill();

        obj.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                obj.SetActive(false);
            });
    }
}