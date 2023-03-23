using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ButtonColorChange : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] Color standardColor;
    [SerializeField] Color desiredColor;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        standardColor = text.color;
    }
    public void ChangeColor()
    {
        //text.color = Color.magenta;
        text.color = new Color(desiredColor.r, desiredColor.g, desiredColor.b);
    }

    public void NormalColor()
    {
        text.color = standardColor;
    }
}
