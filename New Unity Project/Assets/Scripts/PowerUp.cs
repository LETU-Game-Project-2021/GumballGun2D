using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public Sprite menuImg;
    public Sprite activeImg;
    public int cost;
    public string description;
    public string keyword;
    public Upgrader upgrader;
    public static bool waiting = false;

    public PowerUp(string upgradeWord, int coins, Sprite menu, Sprite active, string text, Upgrader parent) {
        menuImg = menu;
        activeImg = active;
        cost = coins;
        description = text;
        keyword = upgradeWord;
        upgrader = parent;
    }

    public void buy() {
        upgrader.enhanceBreak = false;
        StartCoroutine(waitForEnhancementClear());
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
