using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public GameObject targetLight;
    public GameObject targetMainCamera;
    public Material[] skys;
    public float dayTimer;
    public bool isCycle;
    private void Awake()
    {
        targetLight = GameObject.FindGameObjectWithTag("Light");
        targetMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Start()
    {
        dayTimer = targetLight.GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCycle)
        {

            targetLight.GetComponent<Light>().intensity = dayTimer -= Time.deltaTime * 0.03f;

            if (dayTimer <= 0)
            {
                isCycle = true;
            }
        }

        else if (isCycle)
        {

            targetLight.GetComponent<Light>().intensity = dayTimer += Time.deltaTime * 0.03f;

            if (dayTimer >= 1)
            {
                isCycle = false;
            }
        }
        changeCycle();
    }

    void changeCycle()
    {
        if (dayTimer >= 0.1f)
        {
            targetMainCamera.GetComponent<Skybox>().material = skys[0];
        }
        else if (dayTimer >= 0.6f)
        {
            targetMainCamera.GetComponent<Skybox>().material = skys[1];
        }
    }
}