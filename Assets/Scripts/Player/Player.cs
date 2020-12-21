using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private static Animator _anim;
    [SerializeField] private GameObject astronaut;
    
    private CharacterController _controller;
    private UIManager _uiManager;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask resourcesMask;
    [SerializeField] private LayerMask baseMask;
    [SerializeField] private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isOnBase;

    [SerializeField] private int _carbon = 0;
    [SerializeField] private int _aluminium = 0;
    [SerializeField] private int _fins = 0;

    [SerializeField] private Transform _base;
    
    [SerializeField] private float _maxFallForce;
    [SerializeField] private float _baseFallDamage;
    [SerializeField] private float _fallForce;
    
    
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _oxygen = 50f;

    private SwitchCamera _switchCamera;    
    
    public Image oxygenBar;
    public GameObject oxygenBarGameObject;
    public Image HUDOxygenBar;
    public Image HUDHealthBar;

    private bool _isJumping = false;
    private bool _isDead = false;
    private bool _rebuiltRocket = false;
    private bool _hasDanced = false;

    private AudioSource _audioSource;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _base = GameObject.Find("Base").GetComponent<Transform>();
        _anim = astronaut.GetComponent<Animator>();
        _switchCamera = GameObject.Find("CM vcam2").GetComponent<SwitchCamera>();
        _audioSource = GetComponent<AudioSource>();
        
        FindObjectOfType<AudioManager>().Play("Background");
    }

    void Update()
    {
        if (_health <= 0 || _oxygen <= 0)
        {
            _isDead = true;
        }

        if (_isDead)
        {
            oxygenBarGameObject.SetActive(false);
            _anim.SetTrigger("isDead");

            StartCoroutine(RestartLevel());
        }

        if (_rebuiltRocket && !_hasDanced)
        {
            oxygenBarGameObject.SetActive(false);
            StartCoroutine(SetOxygenActive());
            _anim.SetTrigger("isDancing");
            _hasDanced = true;
        }
        
        Ray envRayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit envRayHitInfo;
        

        if (Physics.Raycast(envRayOrigin, out envRayHitInfo, Mathf.Infinity) && _switchCamera.GameHasStarted() == true)
        {
            if (envRayHitInfo.transform.gameObject.layer == 8 && envRayHitInfo.transform.CompareTag("Terrain")) 
            {
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithTerrain();
                _uiManager.OnButtonTerrain();
            }
            else if (envRayHitInfo.transform.CompareTag("Carbon")) 
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithCarbon();
                _uiManager.OnButtonSelect();
            }
            else if (envRayHitInfo.transform.CompareTag("Aluminium"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithAluminium();
                _uiManager.OnButtonSelect();
            }
            else if (envRayHitInfo.transform.CompareTag("Enemy"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithEnemy();
                _uiManager.OnButtonKill();
            } else if (envRayHitInfo.transform.CompareTag("Rocket"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithRocket();
                _uiManager.OnButtonMove();
            } else if (envRayHitInfo.transform.CompareTag("Thruster"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithThruster();
                _uiManager.OnButtonSelect();
            }
            else if (envRayHitInfo.transform.CompareTag("Furnace"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithFurnace();
                _uiManager.OnButtonMove();
            } else if (envRayHitInfo.transform.CompareTag("RocketFixed"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithFixedRocket();
                _uiManager.OnButtonMove();
            } else if (envRayHitInfo.transform.CompareTag("Dome"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithTree();
                
                _uiManager.OnInteractWithDome();
                _uiManager.OnButtonMove();
            } else if (envRayHitInfo.transform.CompareTag("Tree"))
            {
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                
                _uiManager.OnInteractWithTree();
                _uiManager.OnButtonMove();
            }
            else
            {
                _uiManager.OnStopInteractWithAluminium();
                _uiManager.OnStopInteractWithCarbon();
                _uiManager.OnStopInteractWithTerrain();
                _uiManager.OnStopInteractWithEnemy();
                _uiManager.OnStopInteractWithRocket();
                _uiManager.OnStopInteractWithThruster();
                _uiManager.OnStopInteractWithFurnace();
                _uiManager.OnStopInteractWithFixedRocket();
                _uiManager.OnStopInteractWithDome();
                _uiManager.OnStopInteractWithTree();
            }
        }
        else
        {
            _uiManager.OnStopInteractWithAluminium();
            _uiManager.OnStopInteractWithCarbon();
            _uiManager.OnStopInteractWithTerrain();
            _uiManager.OnStopInteractWithEnemy();
            _uiManager.OnStopInteractWithRocket();
            _uiManager.OnStopInteractWithThruster();
            _uiManager.OnStopInteractWithFurnace();
            _uiManager.OnStopInteractWithFixedRocket();
            _uiManager.OnStopInteractWithDome();
            _uiManager.OnStopInteractWithTree();
        }

        if (Input.GetMouseButtonDown(0) && _uiManager.GetSelectedButton() == "collect")
        {
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;
        
            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity, resourcesMask))
            {
                if (hitInfo.transform.CompareTag("Carbon"))
                {
                    FindObjectOfType<AudioManager>().Play("Button");
                    _carbon += 1;
                    _uiManager.OnCollectCarbon(_carbon);
                }
                else if (hitInfo.transform.CompareTag("Aluminium"))
                {
                    FindObjectOfType<AudioManager>().Play("Button");
                    _aluminium += 1;
                    _uiManager.OnCollectAluminium(_aluminium);
                }
                Destroy(hitInfo.transform.gameObject);
            }
        }

        FallDamage();

        ControlOxygenLevel();
        
        CalculateMovement();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Projectile"))
        {
            _health -= 20f;
            HUDHealthBar.fillAmount = _health / 100f;
        }
    }

    void ControlOxygenLevel()
    {
        _isOnBase = Physics.CheckSphere(groundCheck.position, groundDistance, baseMask);
        if (_isOnBase || _switchCamera.GameHasStarted() == false)
        {
            _oxygen = 100f;
            oxygenBar.fillAmount = _oxygen / 100f;
            HUDOxygenBar.fillAmount = _oxygen / 100f;
        }
        else
        {
            _oxygen -= 0.75f * Time.deltaTime;
            oxygenBar.fillAmount = _oxygen / 100f;
            HUDOxygenBar.fillAmount = _oxygen / 100f;
        }
    }

    IEnumerator SetOxygenActive()
    {
        yield return new WaitForSeconds(5f);
        oxygenBarGameObject.SetActive(true);
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    void CalculateMovement()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if ((_isGrounded || _isOnBase) && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        
            if ((!_isDead && _oxygen > 0) && _switchCamera.GameHasStarted() == true)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
                Vector3 velocity = direction * speed;

                velocity = transform.transform.TransformDirection(velocity);

                if ((verticalInput != 0 || horizontalInput != 0) && !_isJumping)
                {
                    _anim.SetBool("isRunning", true);
                    // transform.Rotate(new Vector3(0, 60 * horizontalInput * 2 *  Time.deltaTime, 0));
                } else {
                    _anim.SetBool("isRunning", false);
                }
        
                _controller.Move(velocity * Time.deltaTime);

                if (Input.GetButtonDown("Jump") && (_isGrounded || _isOnBase))
                {
                    _isJumping = true;
                    _anim.SetTrigger("isJumping");
                    StartCoroutine(Jump());
                }
            }
        else
        {
            _anim.SetBool("isRunning", false);
        }
        
       

        _velocity.y += gravity * Time.deltaTime;
        
        _controller.Move(_velocity * Time.deltaTime);
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.50f);
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _isJumping = false;
        FindObjectOfType<AudioManager>().Play("Foot");
    }
    
    public void OnDamage(float injury)
    {
        _health -= injury * Time.deltaTime;
        HUDHealthBar.fillAmount = _health / 100f;
        
        if (_health <= 0)
        {
            _isDead = true;
        }
        // _uiManager.OnDamagePlayer(_health);
    }

    void FallDamage()
    {
        if (!_isGrounded && !_isOnBase)
        {
            _fallForce = Mathf.Abs(_velocity.y);
        }

        if ((_isGrounded || _isOnBase))
        {
            if (_fallForce > _maxFallForce)
            {
                float damage = _fallForce * _baseFallDamage;
                _fallForce = 0;
                _health -= damage;
                HUDHealthBar.fillAmount = _health / 100f;
                // _uiManager.OnDamagePlayer(_health);
                if (_health <= 0)
                {
                    _isDead = true;
                }
            }
        }
    }

    public void CollectAluminium()
    {
        FindObjectOfType<AudioManager>().Play("Collect");
        _aluminium += 1;
        _uiManager.OnCollectAluminium(_aluminium);
    }

    public void CollectCarbon()
    {
        FindObjectOfType<AudioManager>().Play("Collect");
        _carbon += 1;
        _uiManager.OnCollectCarbon(_carbon);
    }

    public void CollectFins()
    {
        _fins += 1;
    }

    public int GetAluminiumAmount()
    {
        return _aluminium;
    }

    public int GetCarbonAmount()
    {
        return _carbon;
    }

    public int GetFinsAmount()
    {
        return _fins;
    }

    public void UseAluminium(int amount)
    {
        _aluminium -= amount;
        _uiManager.OnCollectAluminium(_aluminium);
    }

    public void UseCarbon(int amount)
    {
        _carbon -= amount;
        _uiManager.OnCollectCarbon(_carbon);
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void RebuildRocket()
    {
        _rebuiltRocket = true;
    }

    private void Step()
    {
        _audioSource.PlayOneShot(_audioSource.clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dome"))
        {
            _oxygen = 100;
        }
    }
}
