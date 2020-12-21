using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameObject title;

    [SerializeField] private GameObject healthHUD;
    [SerializeField] private GameObject oxygenHUD;
    [SerializeField] private GameObject minimapHUD;
    [SerializeField] private GameObject carbonHUD;
    [SerializeField] private GameObject aluminiumHUD;
    
    private bool _gameHasStarted = false; 
    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = transform.GetComponent<CinemachineVirtualCamera>();
        healthHUD.SetActive(false);
        oxygenHUD.SetActive(false);
        minimapHUD.SetActive(false);
        carbonHUD.SetActive(false);
        aluminiumHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameHasStarted && Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<AudioManager>().Play("Button");
            _virtualCamera.Priority = 1;
            _gameHasStarted = true;
            title.SetActive(false);
            healthHUD.SetActive(true);
            oxygenHUD.SetActive(true);
            minimapHUD.SetActive(true);
            carbonHUD.SetActive(true);
            aluminiumHUD.SetActive(true);
        }
    }

    public bool GameHasStarted()
    {
        return _gameHasStarted;
    }
}
