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
    private void Start()
    {
        BGMButton.image.sprite = AudioManager.Instance.IsBGMMuted() ? muteOnSprite : muteOffSprite;
        SFXButton.image.sprite = AudioManager.Instance.IsSFXMuted() ? muteOnSprite : muteOffSprite;
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

    }
}
