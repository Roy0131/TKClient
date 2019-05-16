using UnityEngine;
using Framework.Core;
public class SoundPlayer : UpdateBase
{
    private int _delayFrame;
    private AudioSource _source;
    private bool _blLoop;
    private bool _blStart;
    public string SoundName { get; private set; }
    public Transform mRoot { get; private set; }

    public SoundPlayer(AudioSource source)
    {
        _source = source;
        mRoot = _source.transform;
    }

    public void PlaySound(string name, bool blLoop, int delayFrame, float vol)
    {
        SoundName = name;
        if (_source.isPlaying)
            _source.Stop();
        _blLoop = blLoop;

        _source.gameObject.name = "s_" + name;
        AudioClip clip = GameResMgr.Instance.LoadSound(name);
        _source.clip = clip;
        _source.loop = blLoop;
        _source.volume = vol;
        _delayFrame = delayFrame;
        _blStart = _delayFrame == 0;
        _source.pitch = Time.timeScale;
        if (_blStart)
            _source.Play();
        Initialize();
    }

    public override void Update()
    {
        if (_blStart)
        {
            if (_blLoop)
                return;
            if (!_source.isPlaying)
                SoundEnd();
            return;
        }
        if (_delayFrame <= 0)
        {
            _blStart = true;
            _source.Play();
            return;
        }
        _delayFrame -= (int)Time.timeScale;
    }

    public void StopSound()
    {
        _source.Stop();
        _source.clip = null;
        _source.loop = false;
        _blStart = false;
        _blLoop = false;
        SoundMgr.Instance.PlayEnd(this);
        Remove();
    }

    public void SetVolume(float value)
    {
        if (_source != null)
            _source.volume = value;
    }

    private void SoundEnd()
    {
        StopSound();
    }
}