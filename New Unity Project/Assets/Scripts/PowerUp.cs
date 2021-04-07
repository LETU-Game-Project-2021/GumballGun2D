using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp
{
    public Image menuImg;
    public Image activeImg;
    public int cost;
    public string description;
    public string keyword;
    public Upgrader upgrader;

    public PowerUp(string upgradeWord, int coins, Image menu, Image active, string text, Upgrader parent) {
        menuImg = menu;
        activeImg = active;
        cost = coins;
        description = text;
        keyword = upgradeWord;
        upgrader = parent;
    }

    public void buy() {
        upgrader.getPermanentEnhancement(keyword, cost);
    }
}
