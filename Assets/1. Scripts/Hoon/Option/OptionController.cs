using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [Header("SFX 뮤트 버튼")]
    public Button SFXButton;
    [Header("BGM 뮤트 버튼")]
    public Button BGMButton;

    [Header("뮤트 스프라이트")]
    public Sprite muteOnSprite;
    [Header("언뮤트 스프라이트")]
    public Sprite muteOffSprite;

    [Header("Hint Pannel")]
    public GameObject hintPannel;

    // 힌트 패널의 원래 크기를 기억해둘 변수
    private Vector3 _hintPannelOriginScale;

    private void Start()
    {
        BGMButton.image.sprite = AudioManager.Instance.IsBGMMuted() ? muteOnSprite : muteOffSprite;
        SFXButton.image.sprite = AudioManager.Instance.IsSFXMuted() ? muteOnSprite : muteOffSprite;

        if (hintPannel != null)
        {
            _hintPannelOriginScale = hintPannel.transform.localScale;
            hintPannel.SetActive(false);
        }
    }

    public void SetBGMVolume(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void MuteBGMToggle()
    {
        AudioManager.Instance.ToggleBGMMute();
        BGMButton.image.sprite = AudioManager.Instance.IsBGMMuted() ? muteOnSprite : muteOffSprite;
    }

    public void MuteSFXToggle()
    {
        AudioManager.Instance.ToggleSFXMute();
        SFXButton.image.sprite = AudioManager.Instance.IsSFXMuted() ? muteOnSprite : muteOffSprite;
    }

    public void OnSFXSliderRelease()
    {
        AudioManager.Instance.Play2D(SoundName.snap_item);
    }

    public void RestartGame()
    {
        DOTween.KillAll();
        AudioManager.Instance.Play2D(SoundName.bgm1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenHintPannel()
    {
        if (hintPannel == null) return;

        hintPannel.transform.DOKill();

        hintPannel.transform.localScale = Vector3.zero;
        hintPannel.SetActive(true);

        hintPannel.transform.DOScale(_hintPannelOriginScale, 0.6f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void CloseHintPannel()
    {
        if (hintPannel == null) return;

        hintPannel.transform.DOKill();

        hintPannel.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => hintPannel.SetActive(false));
    }
}