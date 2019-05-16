using System;
using UnityEngine.UI;

public static class UIEventExtend
{
    public static void Add(this InputField.SubmitEvent submitEvent, Action<string> action)
    {
        submitEvent.AddListener(
            (s) =>
            {
                action(s);
            }
            );
    }

    public static void Add(this Button.ButtonClickedEvent buttonClickedEvent, Action action)
    {
        buttonClickedEvent.AddListener(
            () =>
            {
                action();
                PlayButtonSound();
            }
        );
    }

    private static void PlayButtonSound()
    {
        GameEntry.Instance.PlayButtonSound();
    }

    public static void Add(this Toggle.ToggleEvent toggleEvent, Action<bool> action)
    {
        toggleEvent.AddListener(
                        (bl) =>
                        {
                            action(bl);
                            if (bl)
                                PlayButtonSound();
                        }
            );
    }

    public static void Add(this InputField.OnChangeEvent onChangeEvent, Action<string> action)
    {
        onChangeEvent.AddListener(
            (value) =>
            {
                action(value);
            }
        );
    }

}
