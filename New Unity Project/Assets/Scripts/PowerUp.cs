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
    private Player player;
    private Upgrader upgrader;
    public static bool waiting = false;
    private Text descriptionBox;
    public Button buyBtn;

    private void Start() {
        player = GameObject.FindObjectOfType<Player>();
        this.GetComponentInParent<Image>().sprite = menuImg;
        descriptionBox = GameObject.Find("PowerUpDescription").GetComponent<Text>();
        buyBtn = GameObject.Find("PowerUpBuyBtn").GetComponent<Button>();
        textbox.text = "Cost: " + cost;
    }

    public void buy() {
        Upgrader.enhanceBreak = false;
        StartCoroutine(waitForEnhancementClear());
        cost = 0;
        textbox.text = "Bought";
    }

    public void select() {
        descriptionBox.text = description;
        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(delegate{
            buy();
        });
        buyBtn.GetComponentInChildren<Text>().text = (textbox.text=="Bought"?"Apply":"Buy");
    }

    IEnumerator waitForEnhancementClear() {
        while(waiting) {
            yield return 0;
        }
        upgrader = player.upgraderPerm;
        upgrader.getPermanentEnhancement(keyword, cost);
        Upgrader.enhanceBreak = true;
        StartCoroutine(upgrader.openBuyMenu(false));
    }
}