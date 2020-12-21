using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject fixedRocket;
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject progressBar;
    private Player _player;
    [SerializeField] private GameObject error;
    private UIManager _uiManager;
    private float _progress = 0;
    
    private bool _rebuilt = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.RocketBroken();
        _player = GameObject.Find("Player").GetComponent<Player>();
        progressBarImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.CompareTag("Rocket") && _rebuilt == false)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (_player.GetFinsAmount() > 0)
                    {
                        FindObjectOfType<AudioManager>().Play("Button");
                        StartCoroutine(FixRocket());
                        StartCoroutine(ShowProgress());
                    }
                    else
                    {
                        error.SetActive(true);
                        FindObjectOfType<AudioManager>().Play("Button");
                    }
                    
                }
            }
            else
            {
                error.SetActive(false);
            }
        }
    }

    IEnumerator FixRocket()
    {
        yield return new WaitForSeconds(5f);

        fixedRocket.SetActive(true);
        _uiManager.RocketRebuilt();
        _rebuilt = true;
        _player.RebuildRocket();
        gameObject.SetActive(false);
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
}
