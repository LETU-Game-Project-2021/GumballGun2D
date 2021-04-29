using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool fly = false;
    public bool drill = false;
    public bool upgradeT = false;
    public bool upgradeP = false;
    public GameObject nest;
    public GameObject drillObject;
    public GameObject currentDrill;
    public Upgrader upgraderTemp;
    public Upgrader upgraderPerm;
    public Text coinCounter;
    private ParticleSystem jetParticler;
    private ParticleSystem stunParticler;
    public int availableDrills = 1;
    public int totalDrills = 1;
    public int totalJumps = 1;
    public int remainingJumps = 0;
    public bool jetpack = false;
    public bool drillPickup = false;
    public int coins;
    public bool stunned = false;
    private float stunTime = 2;
	public GameObject warpPortal;
	public bool canWarp = false;

    private void Start() {
        coinCounter = GameObject.Find("CoinCount").GetComponent<Text>();
        jetParticler = transform.GetComponentsInChildren<ParticleSystem>()[0];
        stunParticler = transform.GetComponentsInChildren<ParticleSystem>()[1];
    }

    // Update is called once per frame
    void Update()
    {

        if (!stunned) {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if(Input.GetButtonDown("Jump")) {
                jump = true;
            }
            if(Input.GetButton("Jump")) {
                fly = true;
            }
            if(Input.GetButtonDown("Use")) {
                playerUse();
            }
        }
        else {
            horizontalMove = 0;
        }
        if(Input.GetButtonUp("Jump")) {
            jetParticler.Stop();
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
        if(fly && jetpack && jetParticler.isStopped) {
            jetParticler.Play();
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, remainingJumps, fly, jetpack);
        jump = false;
        fly = false;
    }

    private void playerUse() {
        if(!PauseMenu.GameIsPaused) {
            if(drill && availableDrills > 0) {
                Drill drill = (Instantiate(drillObject, nest.transform.position, Quaternion.identity) as GameObject).GetComponent<Drill>();
                FindObjectOfType<SoundManager>().Play("Drill Start");
                availableDrills--;
                nest.gameObject.GetComponent<Nest>().startDrill();
                StartCoroutine(drill.displayTimer(Nest.drillTime));
            }
            else if(upgradeP) {
                StartCoroutine(upgraderPerm.openBuyMenu(!Upgrader.menuIsOpen));
            }
            else if(upgradeT && coins >= upgraderTemp.tempUpgradeCost) {
                if(!upgraderTemp.tempActive) {
                    upgraderTemp.getTemporaryEnhancement();
                    changeCoin(-upgraderTemp.tempUpgradeCost);
                    FindObjectOfType<SoundManager>().Play("Gumball Machine");
                }
            }
            else if(drillPickup) {
                availableDrills++;
                Destroy(currentDrill);
                drillPickup = false;
            }
			else if(canWarp)
			{
				warpPortal.GetComponent<LevelSelect>().select1();
				//Debug.Log("Hello1");
			}
        }
    }

    public void changeCoin(int diff) {
        coins += diff;
        coinCounter.text = coins.ToString();
    }

    public void stun() {
        stunned = true;
        StartCoroutine(isStunned());
    }

    IEnumerator isStunned() {
        stunParticler.Play();
        float startTime = Time.time;
        while(Time.time - startTime < stunTime) {
            yield return 0;
        }
        stunned = false;
        stunParticler.Stop();
    }
}