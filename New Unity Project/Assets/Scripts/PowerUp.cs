﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public Sprite menuImg;
    public Sprite activeImg;
    public Text textbox;
    public int cost;
    public string description;
    public string keyword;
    public Upgrader upgrader;
    public static bool waiting = false;
    private Text descriptionBox;
    public Button buyBtn;

    /*public PowerUp(string upgradeWord, int coins, Sprite menu, Sprite active, string text, Upgrader parent) {
        menuImg = menu;
        activeImg = active;
        cost = coins;
        description = text;
        keyword = upgradeWord;
        upgrader = parent;
    }*/
    private void Start() {
        this.GetComponentInParent<Image>().sprite = menuImg;
        descriptionBox = GameObject.Find("PowerUpDescription").GetComponent<Text>();
        buyBtn = GameObject.Find("PowerUpBuyBtn").GetComponent<Button>();
        textbox.text = "Cost: " + cost;
    }

    public void buy() {
        upgrader.enhanceBreak = false;
        StartCoroutine(waitForEnhancementClear());
    }

    public void select() {
        descriptionBox.text = description;
        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(delegate{
            buy();
        });
    }

    IEnumerator waitForEnhancementClear() {
        while(waiting) {
            yield return 0;
        }
        upgrader.getPermanentEnhancement(keyword, cost);
        upgrader.enhanceBreak = true;
        StartCoroutine(upgrader.openBuyMenu(false));
    }
}
