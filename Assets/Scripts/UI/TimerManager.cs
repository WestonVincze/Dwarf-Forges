using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    private float _startTime;
    private float _endTime;
    private bool _timerStarted;
    private Action _completedAction;

    [SerializeField] private Image _fillImage = null;

    void LateUpdate()
    {
        if(_fillImage.fillAmount > 1)
            _fillImage.fillAmount = 1;
        else if(_fillImage.fillAmount < 0)
            _fillImage.fillAmount = 0;
    }

    public void StartTimer(float timerLength, Action endAction)
    {
        _startTime = Time.time;
        _endTime = Time.time + timerLength;
        _completedAction = endAction;
        _timerStarted = true;
        _fillImage.fillAmount = 0;
    }

    void FixedUpdate()
    {
        if (_timerStarted)
        {
            if (_fillImage != null)
            {
                float elapsedTime = Time.time - _startTime;
                _fillImage.fillAmount = (elapsedTime/(_endTime - _startTime));
            }
        }

        if (Time.time >= _endTime && _timerStarted)
        {
            _completedAction();
            _fillImage.fillAmount = 0;
            _timerStarted = false;
        }
    }
}
