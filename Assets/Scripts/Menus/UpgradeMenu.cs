using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour, IInteractable
{
    public static bool menuOpened = false;

    [Header("World Objects")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject CME;

    [Header("Menu Objects")]
    [SerializeField] private Button weaponButton;
    [SerializeField] private GameObject ExpressUpgrade0;
    [SerializeField] private GameObject ExpressUpgrade1;

    private bool upgraded = false;

    public bool Interact(Interactor interactor)
    {
        if (!menuOpened)
        {
            menuUI.SetActive(true);
            // fireRateButton.onClick.AddListener(OnFireRateClick);
            weaponButton.onClick.AddListener(OnWeaponSelect);
        }
        else
        {
            // if opened, close it!
            Debug.Log("Upgrade Menu closed");
            if (!upgraded)
                ExpressUpgrade0.SetActive(false);
            else
                ExpressUpgrade1.SetActive(false);
            menuUI.SetActive(false);
            weaponButton.onClick.RemoveListener(OnWeaponSelect);
        }
        menuOpened = !menuOpened;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        CME.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            CME.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            CME.SetActive(false);
        }
    }

    private void OnWeaponSelect()
    {
        if (!upgraded)
            ExpressUpgrade0.SetActive(true);
        else
            ExpressUpgrade1.SetActive(true);
    }

    private int upgrade1Cost = 10;
    public void IncreaseFireRate()
    {
        if (!upgraded && GameManager.gameManager.totalCash >= upgrade1Cost)
        {
            Debug.Log("Purchased");
            GameManager.gameManager.totalCash -= upgrade1Cost;
            Gun.GunInstance.UpgradeFireRate(1);
            SFXManager.s.Audio.PlayOneShot(SFXManager.s.upgrade);
            upgraded = true;
            ExpressUpgrade1.SetActive(true);
            ExpressUpgrade0.SetActive(false);
        }
        else
        {
            Debug.Log("Too Broke");
        }

    }
}
