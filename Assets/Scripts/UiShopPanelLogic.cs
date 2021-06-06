using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiShopPanelLogic : MonoBehaviour
{
    public Button IncreaseSpeedBTN;
    public Button DecreaseRadiusBTN;
    public Button CloseShopBTN;
    public TMP_Text ContributionRemaining;
    public GameObject FadingTMP;

    public static Action<float> OnDecreaseRadius;

    private FadingTMP ftp;
    // Start is called before the first frame update
    void Start()
    {
        IncreaseSpeedBTN.onClick.AddListener(IncreaseSpeed);
        DecreaseRadiusBTN.onClick.AddListener(DecreaseRadius);
        CloseShopBTN.onClick.AddListener(CloseShop);
    }

    private void DecreaseRadius()
    {
        //Decrease Player infection Radius and reduce CP by 1
        if(GameManager.Instance.ContributionPoints < 1)
        {
            //Spawn a fader text stating not enough CP
            GameObject go = Instantiate(FadingTMP, GameManager.Instance.WorldCanvas.transform);
            ftp = go.GetComponent<FadingTMP>();
            ftp.Details.text = "Not Enough Contribution Points to Purchase this Stat";
            AudioManager.Instance.PlaySound(SoundEffectsType.Wrong);
        }
        else
        {
            //Decrease SPread Radius
            //Also decreease Particle size
            OnDecreaseRadius?.Invoke(-1.65f);
            MyData.SpreadRadius--;
            GameManager.Instance.DecreaseContribution(1);
        }
    }

    private void IncreaseSpeed()
    {
        //Increase Speed and reduce CP by 1
        if (GameManager.Instance.ContributionPoints < 1)
        {
            //Spawn a fader text saying not enough CP
            GameObject go = Instantiate(FadingTMP, GameManager.Instance.WorldCanvas.transform);
            ftp = go.GetComponent<FadingTMP>();
            ftp.Details.text = "Not Enough Contribution Points to Purchase this Stat";
            AudioManager.Instance.PlaySound(SoundEffectsType.Wrong);
        }
        else
        {
            PlayerActions.IncreaseMoveSpeed();
            GameManager.Instance.DecreaseContribution(1);
        }
    }

    private void OnDestroy()
    {
        CloseShopBTN.onClick.RemoveAllListeners();
    }

    private void CloseShop()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        ContributionRemaining.text = String.Format("{0}", GameManager.Instance.ContributionPoints);
    }
}
