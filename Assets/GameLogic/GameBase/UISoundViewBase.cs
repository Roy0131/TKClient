
using Framework.UI;

public class UISoundViewBase : UIBaseView
{

    #region Sound
    public void StopEffectSoundName(string name)
    {
        SoundMgr.Instance.StopEffectSound(name);
    }

    protected void PlayEffectSound(string name, bool isEndLast = false, bool isEndAll = false)
    {
        if (isEndAll)
        {
            SoundMgr.Instance.StopAllEffectSound();
        }
        else
        {
            if (isEndLast)
                SoundMgr.Instance.StopEffectSound(name);
        }
        SoundMgr.Instance.PlayEffectSound(name);
    }

    protected void StopEffectSound(string name)
    {
        SoundMgr.Instance.StopEffectSound(name);
    }

    protected void StopAllEffectSound()
    {
        SoundMgr.Instance.StopAllEffectSound();
    }
    #endregion
}
