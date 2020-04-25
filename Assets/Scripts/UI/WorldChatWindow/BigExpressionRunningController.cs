using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigExpressionRunningController : MonoBehaviour
{

    private int _int_currentTime = 0;
    private int _int_currentIdex = 0;
    private const int _int_durationOfFrame = 5;

    private KUIImage _img_current;

    private void Awake()
    {
        _img_current = this.GetComponent<KUIImage>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_int_currentTime >= _int_durationOfFrame)
        {
            if (_int_currentIdex < _img_current.sprites.Length - 1)
            {
                _img_current.overrideSprite = _img_current.sprites[_int_currentIdex];
                _int_currentIdex++;
            }
            else
            {
                _int_currentIdex = 0;
            }
            _int_currentTime = 0;
        }
        else
        {
            _int_currentTime++;
        }
    }
}
