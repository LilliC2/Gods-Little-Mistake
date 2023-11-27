using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class FireDirectionManager : Singleton<FireDirectionManager>
{
    [Header("Head Fire")]
    public bool leftHasFired;
    public bool leftHasHeat;
    public bool leftFireFilling;
    public GameObject leftFillObject;
    public Image leftFirerate;
    public Image leftOverheat;
    public GameObject leftMouseHeat;
    public GameObject leftMouseDis;
    public float leftFireCurrent;
    public float leftFireTotal;
    public float leftHeatCurrent;
    public float leftHeatTotal;

    [Header("Torso Fire")]
    public bool rightHasFired;
    public bool rightHasHeat;
    public bool rightFireFilling;
    public GameObject rightFillObject;
    public Image rightFirerate;
    public Image rightOverheat;
    public GameObject rightMouseHeat;
    public GameObject rightMouseDis;
    public float rightFireCurrent;
    public float rightFireTotal;
    public float rightHeatCurrent;
    public float rightHeatTotal;


    // Start is called before the first frame update
    void Start()
    {
        leftHasFired = false;
        rightHasFired = false;
        leftHasHeat = false;
        rightHasHeat = false;
        leftFireFilling = false;
        rightFireFilling = false;

        //Set firerate fill to full
        leftFirerate.fillAmount = 1;
        rightFirerate.fillAmount = 1;

        //overheat def to 0
        leftOverheat.fillAmount = 0;
        rightOverheat.fillAmount = 0;

        //disbale overheat mouse indicator
        leftMouseHeat.SetActive(false);
        rightMouseHeat.SetActive(false);
        leftMouseDis.SetActive(false);
        rightMouseDis.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Left firerate fill
        if (leftHasFired && !leftFireFilling)
        {
            StartCoroutine(ProcessLeftFire());

        }

        //Right firerate fill
        if (rightHasFired && !rightFireFilling)
        {
            StartCoroutine(ProcessRightFire());
        }

        //Left overheat fill
        if (leftHasHeat)
        {

            if (leftHeatCurrent > 0)
            {
                leftHeatCurrent -= Time.deltaTime;
                leftOverheat.fillAmount = leftHeatCurrent / leftHeatTotal;
                Debug.Log(rightFirerate.fillAmount);
            }
            if (leftHeatCurrent <= 0)
            {
                leftHasHeat = false;
            }
        }

        //Right overheat fill
        if (rightHasHeat)
        {

            if (rightHeatCurrent > 0)
            {
                rightHeatCurrent -= Time.deltaTime;
                rightOverheat.fillAmount = rightHeatCurrent / rightHeatTotal;
                Debug.Log(rightFirerate.fillAmount);
            }
            if (rightHeatCurrent <= 0)
            {
                rightHasFired = false;
            }

        }
    }

    public void SetLeftAttack(float firerate)
    {
        leftHasFired = true;
        leftFireCurrent = 0;
        leftFireTotal = firerate;
    }

    public void SetRightAttack(float firerate)
    {
        rightHasFired = true;
        rightFireCurrent = 0;
        rightFireTotal = firerate;
    }

    public void SetRightHeat(float firerate)
    {
        rightHasHeat = true;
        rightFirerate.fillAmount = 1;
        rightHeatCurrent = firerate;
    }

    public IEnumerator HeatRight()
    {
        leftFireFilling = true;

        while (leftHeatCurrent > 0)
        {
            leftMouseDis.SetActive(true);
            leftFireCurrent += Time.deltaTime; // Increment by Time.deltaTime for a linear 
            leftFirerate.fillAmount = leftFireCurrent / leftFireTotal;
            yield return null;
        }

        leftMouseDis.SetActive(false);
        leftHasFired = false; // Set to false only when it reaches the total

        leftFireFilling = false;
    }

    public IEnumerator ProcessLeftFire()
    {
        leftFireFilling = true;

        while (leftFireCurrent < leftFireTotal)
        {
            rightHeatCurrent -= Time.deltaTime;
            rightOverheat.fillAmount = rightHeatCurrent / rightHeatTotal;
            Debug.Log(rightFirerate.fillAmount);
            yield return null;
        }

        leftMouseDis.SetActive(false);
        leftHasFired = false; // Set to false only when it reaches the total

        leftFireFilling = false;

    }

    public IEnumerator ProcessRightFire()
    {
        rightFireFilling = true;

        while (rightFireCurrent < rightFireTotal)
        {
            rightMouseDis.SetActive(true);
            rightFireCurrent += Time.deltaTime; // Increment by Time.deltaTime for a linear 
            rightFirerate.fillAmount = rightFireCurrent / rightFireTotal;
            yield return null;
        }

        rightMouseDis.SetActive(false);
        rightHasFired = false; // Set to false only when it reaches the total

        rightFireFilling = false;

    }


}
