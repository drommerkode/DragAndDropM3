using UnityEngine;
using System.Collections;

public class SoundsForPlay : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private SoundType soundType;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField,Range(0,1)] private float volumeMultiplier = 1f;
    [SerializeField] private bool autoPlay;
    [SerializeField] private bool waitStop;
    [SerializeField] private float delayTime = 0.1f;
    [SerializeField] private bool loop;
    [SerializeField] private bool randomPlay;
    [SerializeField] private bool needPauseOnPause;
    private int curSoundsNum;
    private int soundsLengthMinusOne = 0;
    private int soundsCount;

    private enum SoundType { 
        sfx,
        music
    }

    private void Awake() {
        ManagerGame.OnSetSoundSFX.AddListener(SetSoundVolume);
        ManagerGame.OnSetSoundMusic.AddListener(SetSoundVolume);
        ManagerGame.OnPauseSet.AddListener(PauseSound);
        audioSource = GetComponent<AudioSource>();
        soundsCount = sounds.Length;
    }

    private void Start() {
        SetSoundVolume();
        audioSource.loop = loop;
        audioSource.playOnAwake = autoPlay;

        if (soundsCount == 0) { return; }

        soundsLengthMinusOne = soundsCount - 1;
        if (soundsLengthMinusOne > 0) {
            curSoundsNum = Random.Range(0, soundsLengthMinusOne);
        }
        if (autoPlay) {
            StartCoroutine(DelayPlayCoroutine());
        }
    }

    private IEnumerator DelayPlayCoroutine() {
        yield return new WaitForSeconds(delayTime);
        PlaySound();
    }

    private void SetSoundVolume() {
        switch (soundType) { 
            case SoundType.sfx:
                audioSource.volume = SaveLoad.saveData.soundSFX * volumeMultiplier;
                break;
            case SoundType.music:
                audioSource.volume = SaveLoad.saveData.soundMusic * volumeMultiplier;
                break;
        }
    }

    public void SetPitch(float _pitch) { 
        audioSource.pitch = _pitch;
    }

    public void StopSound() {
        audioSource.Stop();
    }

    public void PlaySound() {
        if (soundsCount == 0) { return; }
        if (soundsLengthMinusOne > 0) {
            if (randomPlay) {
                curSoundsNum = Random.Range(0, soundsLengthMinusOne);
            }
            else {
                curSoundsNum += 1;
                if (curSoundsNum > soundsLengthMinusOne) {
                    curSoundsNum = 0;
                }
            }
        }
        audioSource.clip = sounds[curSoundsNum];

        if (audioSource.isPlaying && waitStop) { return; }
        audioSource.Play();
    }

    private void PauseSound(bool _isPause) {
        if (!needPauseOnPause) { return; }
        if (_isPause) {
            audioSource.Pause();
        }
        else {
            audioSource.UnPause();
        }
    }
}
