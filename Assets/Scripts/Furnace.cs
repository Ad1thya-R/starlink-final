using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Furnace : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask baseMask;
    [SerializeField] private LayerMask toolMask;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image progressBarImage;
    private float _progress = 0;
    
    private bool _isGrounded;
    private bool _isOnBase;
    
    private Vector3 _mOffset;
    private float _mZCoord;
    private Vector3 _velocity;

    private bool _mouseDown;
    
    private static Animator _anim;
    
    private Transform _player;
    private Player _playerScript;
    private UIManager _uiManager;

    private float dist;

    [SerializeField] private Transform furnaceOutputPosition;
    [SerializeField] private GameObject thruster;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        progressBarImage.fillAmount = 0;
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CalculateMovement();
        if (_player)
        {
            dist = Vector3.Distance(_player.position, transform.position);
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity, toolMask))
            {

                if (hitInfo.transform.CompareTag("Furnace"))
                {
                    // if (dist < 5f)
                    // {
                        // _uiManager.OnButtonMove();
                        // _uiManager.OnInteractWithFurnace();
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            if (_playerScript.GetAluminiumAmount() >= 3 && _playerScript.GetCarbonAmount() >= 2)
                            {
                                FindObjectOfType<AudioManager>().Play("Button");
                                _playerScript.UseAluminium(3);
                                _playerScript.UseCarbon(2);
                                StartCoroutine(ShowProgress());
                                StartCoroutine(PrintThruster());

                            }
                            else
                            {
                                FindObjectOfType<AudioManager>().Play("Button");
                                _uiManager.OnNotEnoughResources();
                            }
                        // }   
                    }
                }
                else
                {
                    // _uiManager.OnStopInteractWithFurnace();
                    _uiManager.OnRemoveNotEnoughResourcesWarning();
                }
            }
            else
            { 
                // _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnRemoveNotEnoughResourcesWarning();
            }
        }
    }

    IEnumerator PrintThruster()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(thruster, furnaceOutputPosition.position, Quaternion.identity);  
        _anim.SetTrigger("open");
    }

    IEnumerator ShowProgress()
    {
        progressBar.SetActive(true);
        while (_progress < 100f)
        {
            yield return new WaitForSeconds(0.5f);
            _progress += 10;
            progressBarImage.fillAmount = _progress / 100; 
        }
        progressBar.SetActive(false);
    }
    // private void OnMouseDown()
    // {
    //     if (_uiManager.GetSelectedButton() == "move" && dist < 5f)
    //     {
    //         _mouseDown = true;
    //         _mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //         _mOffset = gameObject.transform.position - GetMouseWorldPos();
    //
    //         Debug.DrawLine(_player.position, transform.position, Color.blue);
    //     }
    // }

    // private void OnMouseUp()
    // {
    //     _mouseDown = false;
    // }
    //
    // private void OnMouseDrag()
    // {
    //     if (_uiManager.GetSelectedButton() == "move" && dist < 5f)
    //     {
    //         transform.position = GetMouseWorldPos() + _mOffset;
    //         transform.rotation = _player.rotation;
    //
    //         Debug.DrawLine(_player.position, transform.position, Color.blue);
    //     }
    // }

    // private Vector3 GetMouseWorldPos()
    // {
    //     Vector3 mousePoint = Input.mousePosition;
    //     mousePoint.z = _mZCoord;
    //     return Camera.main.ScreenToWorldPoint(mousePoint);
    //
    // }
    
    void CalculateMovement()
    {
        _isOnBase = Physics.CheckSphere(groundCheck.position, groundDistance, baseMask);
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if ((_isGrounded || _isOnBase) && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        
        // if (!_mouseDown)
        // {
        //     _velocity.y += gravity * Time.deltaTime;
        //     _controller.Move(_velocity * Time.deltaTime);
        // }
        
    }
}
