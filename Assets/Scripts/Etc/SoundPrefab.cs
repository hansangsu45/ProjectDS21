using System.Collections.Generic;
using UnityEngine;

public class SoundPrefab : MonoBehaviour
{
    GameManager gameMng;
    [SerializeField] private AudioSource _audio;
    private bool isSoundPlaying;

    private void OnEnable()  //1
    {
        isSoundPlaying = false;
    }

    public void PlaySound(SoundEffectType set)  //2
    {
        if (gameMng == null) gameMng = GameManager.Instance;

        if (gameMng.savedData.option.soundEffectSize <= 0) gameObject.SetActive(false);

        _audio.clip = SoundManager.Instance.effectClips[(int)set];
        _audio.volume = gameMng.savedData.option.bgmSize;
        _audio.Play();

        isSoundPlaying = true;
    }

    private void Update()  //3
    {
        if (isSoundPlaying && !_audio.isPlaying) gameObject.SetActive(false);
    }
}
