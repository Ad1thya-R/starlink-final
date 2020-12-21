using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100f;
    private SwitchCamera _switchCamera;
    private float _xRotation = 0f;

    void Start()
    {
        _switchCamera = GameObject.Find("CM vcam2").GetComponent<SwitchCamera>();
    }

    void Update()
    {
        if (_switchCamera.GameHasStarted() == true)
        {
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            // _xRotation = Mathf.Clamp(_xRotation, -17f, 90f);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);   
        }
    }
}