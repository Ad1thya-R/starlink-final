using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI carbonTextField;
    [SerializeField] private TextMeshProUGUI aluminiumTextField;
    
    [SerializeField] private Button collectButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button terrainButton;

    [SerializeField] private GameObject furnaceContextMenu;
    [SerializeField] private GameObject terrainContextMenu;
    [SerializeField] private GameObject carbonContextMenu;
    [SerializeField] private GameObject aluminiumContextMenu;
    [SerializeField] private GameObject enemyContextMenu;
    [SerializeField] private GameObject rocketContextMenu;
    [SerializeField] private GameObject fixedRocketContextMenu;
    [SerializeField] private GameObject thrusterContextMenu;
    [SerializeField] private GameObject domeContextMenu;

    [SerializeField] private GameObject treeContextMenu;
    
    [SerializeField] private GameObject rebuildText;
    [SerializeField] private GameObject launchText;

    [SerializeField] private GameObject warning;

    private string _selectedButton = "collect";
    void Start()
    {
        carbonTextField.text = "Carbon: 0";
        aluminiumTextField.text = "Aluminium: 0";
        
        // collectButton.onClick.AddListener(OnButtonSelect);
        // moveButton.onClick.AddListener(OnButtonMove);
        // terrainButton.onClick.AddListener(OnButtonTerrain);

        collectButton.interactable = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedButton = "collect";
            
            collectButton.interactable = false;
            moveButton.interactable = true;
            terrainButton.interactable = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            moveButton.interactable = false;
            collectButton.interactable = true;
            terrainButton.interactable = true;

            _selectedButton = "move";
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            terrainButton.interactable = false;
            moveButton.interactable = true;
            collectButton.interactable = true;

            _selectedButton = "terrain";
        }
    }

    public void OnCollectCarbon(int amount)
    {
        carbonTextField.text = "Carbon: " + amount;
    }

    public void OnCollectAluminium(int amount)
    {
        aluminiumTextField.text = "Aluminium: " + amount;
    }

    public void OnInteractWithFurnace()
    {
        furnaceContextMenu.SetActive(true);
    }

    public void OnStopInteractWithFurnace()
    {
        furnaceContextMenu.SetActive(false);
    }

    public void OnInteractWithTerrain()
    {
        terrainContextMenu.SetActive(true);
    }

    public void OnStopInteractWithTerrain()
    {
        terrainContextMenu.SetActive(false);
    }

    public void OnInteractWithEnemy()
    {
        enemyContextMenu.SetActive(true);
    }

    public void OnStopInteractWithEnemy()
    {
        enemyContextMenu.SetActive(false);
    }

    public void OnInteractWithCarbon()
    {
        carbonContextMenu.SetActive(true);
    }

    public void OnInteractWithRocket()
    {
        rocketContextMenu.SetActive(true);
    }

    public void OnStopInteractWithRocket()
    {
        rocketContextMenu.SetActive(false);
    }
    
    public void OnStopInteractWithCarbon()
    {
        carbonContextMenu.SetActive(false);
    }
    
    public void OnInteractWithAluminium()
    {
        aluminiumContextMenu.SetActive(true);
    }

    public void OnStopInteractWithAluminium()
    {
        aluminiumContextMenu.SetActive(false);
    }

    public void OnInteractWithThruster()
    {
        thrusterContextMenu.SetActive(true);
    }

    public void OnStopInteractWithThruster()
    {
        thrusterContextMenu.SetActive(false);
    }

    public void OnInteractWithFixedRocket()
    {
        fixedRocketContextMenu.SetActive(true);
    }

    public void OnStopInteractWithFixedRocket()
    {
        fixedRocketContextMenu.SetActive(false);
    }

    public void RocketRebuilt()
    {
        rebuildText.SetActive(false);
        launchText.SetActive(true);
    }
    
    public void RocketBroken()
    {
        rebuildText.SetActive(true);
        launchText.SetActive(false);
    }

    public void OnInteractWithDome()
    {
        domeContextMenu.SetActive(true);
    }

    public void OnStopInteractWithDome()
    {
        domeContextMenu.SetActive(false);
    }

    public void OnInteractWithTree()
    {
        treeContextMenu.SetActive(true);
    }

    public void OnStopInteractWithTree()
    {
        treeContextMenu.SetActive(false);
    }
    
    public bool GetFurnaceContextMenuStatus()
    {
        return furnaceContextMenu.activeSelf;
    }

    public void OnNotEnoughResources()
    {
        warning.SetActive(true);
    }

    public void OnRemoveNotEnoughResourcesWarning()
    {
        warning.SetActive(false);
    }

    public void OnButtonSelect()
    {
        collectButton.interactable = false;
        moveButton.interactable = true;
        terrainButton.interactable = true;
        
        _selectedButton = "collect";
    }
    
    public void OnButtonMove()
    {
        moveButton.interactable = false;
        collectButton.interactable = true;
        terrainButton.interactable = true;
    
        _selectedButton = "move";
    }
    
    public void OnButtonTerrain()
    {
        terrainButton.interactable = false;
        moveButton.interactable = true;
        collectButton.interactable = true;
    
        _selectedButton = "terrain";
    }

    public void OnButtonKill()
    {
        _selectedButton = "kill";
    }

    public string GetSelectedButton()
    {
        return _selectedButton;
    }
}
