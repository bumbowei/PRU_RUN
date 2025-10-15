﻿using TMPro;
using UnityEngine;


[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private Skin[] skinList;

    [Header("UI details")]
    [SerializeField] private int skinIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;

    [SerializeField] private TextMeshProUGUI buySelectText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI bankText;

    private void Start()
    {
        LoadSkinUnlocked();
        UpdateSkinDisplay();
    }

    private void LoadSkinUnlocked()
    {
        for (int i = 0; i < skinList.Length; i++)
        {
            string skinName = skinList[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if(skinUnlocked || i == 0)
                skinList[i].unlocked = true;
        }
    }

    public void SelectSkin()
    {
        if (skinList[skinIndex].unlocked == false)
            BuySkin(skinIndex);
        else
            SkinManager.instance.setSkinId(skinIndex);

        UpdateSkinDisplay();
    }

    public void NextSkin()
    {
        skinIndex++;
        if (skinIndex > maxIndex)
            skinIndex = 0;

        UpdateSkinDisplay();
    }
    public void PreviousSkin()
    {
        skinIndex--;
        if (skinIndex < 0)
        {
            skinIndex = maxIndex;
        }
        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        bankText.text = "Bank: " + FruitInBank();

        for (int i = 0; i <= maxIndex; i++) //maxIndex = skinDisplay.layerCount
        {
            skinDisplay.SetLayerWeight(i, 0);
        }
        skinDisplay.SetLayerWeight(skinIndex, 1);

        if (skinList[skinIndex].unlocked)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            buySelectText.text = "Select";
        }
        else
        {
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text = "Price: " + skinList[skinIndex].skinPrice;
            buySelectText.text = "Buy";
        }
    }

    private void BuySkin(int index)
    {
        if (HaveEnoughFruits(skinList[index].skinPrice) == false)
        {
            Debug.Log("Không đủ trái cây bạn ơi! Liu Liu!!!");
            return;
        }
        string skinName = skinList[skinIndex].skinName;
        skinList[skinIndex].unlocked = true;

        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }

    private int FruitInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");
    private bool HaveEnoughFruits(int price)
    {
        if(FruitInBank() > price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitInBank() - price);
            return true;
        }
        else
            return false;
    }
}