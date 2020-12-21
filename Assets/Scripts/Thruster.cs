using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private Transform _player;
    private Player _playerScript;
    private UIManager _uiManager;
    private static Animator _anim;
    
    private CharacterController _controller;
    [SerializeField] private float gravity = -9.81f;
    // [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask toolsMask;
    
    private bool _isGrounded;
    private bool _isOnFurnace;
    
    private Vector3 _mOffset;
    private float _mZCoord;
    private Vector3 _velocity;

    private bool _mouseDown;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _controller = GetComponent<CharacterController>();
        // groundCheck = transform.GetChild(0);
        _anim = GameObject.Find("Furnace").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray envRayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit envRayHitInfo;
        
        if (Physics.Raycast(envRayOrigin, out envRayHitInfo, Mathf.Infinity))
        {
            if (envRayHitInfo.transform.CompareTag("Thruster"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _anim.SetTrigger("close");
                    _playerScript.CollectFins();
                    FindObjectOfType<AudioManager>().Play("Button");
                    Destroy(this.gameObject);
                }
            }
        }
    }
    
    // private void OnMouseDown()
    // {
    //     if (_uiManager.GetSelectedButton() == "move")
    //     {
    //         _mouseDown = true;
    //         _mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //         _mOffset = gameObject.transform.position - GetMouseWorldPos();
    //
    //     }
    // }
    //
    // private void OnMouseUp()
    // {
    //     _mouseDown = false;
    // }
    //
    // private void OnMouseDrag()
    // {
    //     if (_uiManager.GetSelectedButton() == "move")
    //     {
    //         transform.position = GetMouseWorldPos() + _mOffset;
    //         transform.rotation = _player.rotation;
    //
    //         Debug.DrawLine(_player.position, transform.position, Color.blue);
    //     }
    // }
    //
    // private Vector3 GetMouseWorldPos()
    // {
    //     Vector3 mousePoint = Input.mousePosition;
    //     mousePoint.z = _mZCoord;
    //     return Camera.main.ScreenToWorldPoint(mousePoint);
    //
    // }
    //
    // void CalculateMovement()
    // {
    //     _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    //     _isOnFurnace = Physics.CheckSphere(groundCheck.position, groundDistance, toolsMask);
    //
    //     if (_isOnFurnace)
    //     {
    //         _isGrounded = true;
    //     }
    //     
    //     if (_isGrounded && _velocity.y < 0)
    //     {
    //         _velocity.y = -2f;
    //     }
    //
    //     // if (!_mouseDown)
    //     {
    //         _velocity.y += gravity * Time.deltaTime;
    //         _controller.Move(_velocity * Time.deltaTime);
    //     // }
    //     
    // }
}
