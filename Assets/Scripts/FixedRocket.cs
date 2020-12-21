using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedRocket : MonoBehaviour
{
    [SerializeField] private GameObject fixedRocket;
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject progressBar;
    private UIManager _uiManager;
    
    private bool _rebuilt = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.RocketBroken();
        progressBarImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))
        {
        
        }
    }
}
