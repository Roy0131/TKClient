using UnityEngine;
using System.Collections.Generic;
using Framework.UI;

public class SoundMgr : Singleton<SoundMgr>
{
    private Queue<SoundPlayer> _playerPool;
    private List<SoundPlayer> _lstBGPlayers;
    private Dictionary<string, List<SoundPlayer>> _dictEffectPlayers;

    private Transform _soundPoolRoot;
    private Transform _soundRunRoot;

    private float _bgSoundVol = 0.6f;//music
    private float _effectSoundVol = 0.8f;//sound

    public void Init()
    {
        _playerPool = new Queue<SoundPlayer>();
        _lstBGPlayers = new List<SoundPlayer>();
        _dictEffectPlayers = new Dictionary<string, List<SoundPlayer>>();

        GameObject soundRoot = GameObject.Find("SoundRoot");
        _soundPoolRoot = soundRoot.transform.Find("SoundPoolRoot");
        _soundRunRoot = soundRoot.transform.Find("SoundRunRoot");

        OpenOrCloseSound(LocalDataMgr.IsMusic, false);
        OpenOrCloseSound(LocalDataMgr.IsSound, true);
    }

    private SoundPlayer GetSoundPlayer()
    {
        SoundPlayer player;
        if (_playerPool.Count > 0)
        {
            player = _playerPool.Dequeue();
        }
        else
        {
            AudioSource source = LogicMonoHelper.CreateSoundAudio();
            player = new SoundPlayer(source);
        }
        ObjectHelper.AddChildToParent(player.mRoot, _soundRunRoot);
        return player;
    }

    public void PlayBackGroundSound(string name, bool blLoop = true, int delay = 0)
    {
        SoundPlayer player = GetSoundPlayer();
        player.PlaySound(name, blLoop, delay, _bgSoundVol);
        _lstBGPlayers.Add(player);
    }

    public void PlayEffectSound(string name, int delay = 0, bool blLoop = false)
    {
        if (_dictEffectPlayers == null)
            return;
        SoundPlayer player = GetSoundPlayer();
        player.PlaySound(name, blLoop, delay, _effectSoundVol);
        List<SoundPlayer> lst;
        if (_dictEffectPlayers.ContainsKey(name))
            lst = _dictEffectPlayers[name];
        else
        {
            lst = new List<SoundPlayer>();
            _dictEffectPlayers.Add(name, lst);
        }
        lst.Add(player);
    }

    public void StopAllEffectSound()
    {
        if (_dictEffectPlayers == null)
            return;
        foreach (var kv in _dictEffectPlayers)
        {
            for (int i = 0; i < kv.Value.Count; i++)
                kv.Value[i].StopSound();
        }
    }

    public void StopAllBGSound()
    {
        if (_dictEffectPlayers == null)
            return;
        for (int i = _lstBGPlayers.Count - 1; i >= 0; i--)
            _lstBGPlayers[i].StopSound();
    }

    public void StopAllSound()
    {
        StopAllBGSound();
        StopAllEffectSound();
    }

    public void PlayEnd(SoundPlayer player)
    {
        _playerPool.Enqueue(player);
        if(_dictEffectPlayers.ContainsKey(player.SoundName))
        {
            if (_dictEffectPlayers[player.SoundName].Contains(player))
                _dictEffectPlayers[player.SoundName].Remove(player);
        }
        else if (_lstBGPlayers.Contains(player))
            _lstBGPlayers.Remove(player);
        ObjectHelper.AddChildToParent(player.mRoot, _soundPoolRoot);
    }

    public void StopEffectSound(string name)
    {
        if (_dictEffectPlayers == null)
            return;
        if (_dictEffectPlayers.ContainsKey(name))
        {
            List<SoundPlayer> lst = _dictEffectPlayers[name];
            for (int i = 0; i < lst.Count; i++)
                lst[i].StopSound();
        }
    }

    public void OpenOrCloseSound(bool blOpen, bool blBGSound)
    {
        if (blOpen)
        {
            if (blBGSound)
                _effectSoundVol = 0.8f;
            else
                _bgSoundVol = 0.6f;
        }
        else
        {
            if (blBGSound)
                _effectSoundVol = 0;
            else
                _bgSoundVol = 0;
        }

        if (_lstBGPlayers != null)
        {
            for (int i = 0; i < _lstBGPlayers.Count; i++)
                _lstBGPlayers[i].SetVolume(_bgSoundVol);
        }
        if (_dictEffectPlayers != null)
        {
            foreach(var kv in _dictEffectPlayers)
            {
                for (int i = 0; i < kv.Value.Count; i++)
                    kv.Value[i].SetVolume(_effectSoundVol);
            }
        }
    }
}