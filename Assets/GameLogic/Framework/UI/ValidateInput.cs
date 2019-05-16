using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValidateInput
{
    private int _max;
    public ValidateInput(int max)
    {
        _max = max;
    }

    public char OnValidateInput(string text, int charInput, char addedChar)
    {
        if (System.Text.Encoding.UTF8.GetBytes(text + addedChar).Length > _max)
        {
            return '\0'; //返回空
        }
        return addedChar;
    }
}
