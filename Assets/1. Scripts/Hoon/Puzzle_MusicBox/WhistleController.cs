using System;
using System.Collections;
using UnityEngine;

public class WhistleController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _whistleClips;
    [SerializeField] private AudioSource _source;
    private Coroutine whistleCoroutine;

    public void StartWhistle()
    {
        if (whistleCoroutine == null) whistleCoroutine = StartCoroutine(WhistleLoop());
    }

    public void StopWhistle()
    {
        if (whistleCoroutine != null) { StopCoroutine(whistleCoroutine); whistleCoroutine = null; }
    }

    private IEnumerator WhistleLoop()
    {
        while (true)
        {
            // 랜덤 휘파람 재생 후 2-4초 랜덤으로 기다린 후 다시 랜덤 휘파람 재생 반복.
            var clip = _whistleClips[UnityEngine.Random.Range(0, _whistleClips.Length)];
            _source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length + UnityEngine.Random.Range(2f, 4f));
        }
    }
}
