using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingOutline : MonoBehaviour
{
    public Color _currentColor;
    public Color _targetColor;
    public float LerpFactor;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentColor = Color.Lerp(
            _currentColor,
            _targetColor,
            Time.deltaTime * LerpFactor);
        mat.SetColor("_GlowColor", _currentColor);
        
    }
}
