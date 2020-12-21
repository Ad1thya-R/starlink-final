using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    private Player _player;
    private SwitchCamera _switchCamera;
    [SerializeField] private float sensitivity = 1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _switchCamera = GameObject.Find("CM vcam2").GetComponent<SwitchCamera>();

    }

    void Update()
    {
        if (!_player.IsDead() && _switchCamera.GameHasStarted() == true)
        {
            float mouseX = Input.GetAxis("Mouse X");

            Vector3 newRotation = transform.localEulerAngles;
            newRotation.y += mouseX * sensitivity;
            transform.localEulerAngles = newRotation;
        }
        
    }
}
