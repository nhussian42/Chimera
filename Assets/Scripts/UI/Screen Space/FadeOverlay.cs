using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeOverlay : MonoBehaviour
{
    private Image _image;
    private float _opacity
    {
        set
        {
            _image.color = new Color(0,0,0, value);
        }
    }

    private void OnEnable()
    {
        ChimeraSceneManager.FadeValueChanged += UpdateOpacity;
        
        _image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        ChimeraSceneManager.FadeValueChanged -= UpdateOpacity;
    }

    private void UpdateOpacity(float fadeValue)
    {
        _opacity = fadeValue;
    }
}
