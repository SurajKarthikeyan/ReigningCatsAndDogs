using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.XR;

// Class that handles the gun (aiming and shooting)
public class Gun : MonoBehaviour
{
    public static Gun playerGun = null;

    #region PrivateFields
    private Camera mainCamera;
    private Vector3 mousePos;
    private Transform attackSpawn;
    private bool shooting = false;
    [SerializeField]
    public float attackDuration = 0.5f;
    [SerializeField]
    private GameObject attackPrefab;
    [SerializeField] public float bulletForce;
    #endregion

    #region PublicFields

    public static Gun GunInstance;
    public float rotationZ;
    #endregion

    /*
     * Define GunSingleton Instance for usage in rotation
     */
    private void Awake()
    {
        playerGun = this;
        if (GunInstance != null && GunInstance != this)
        {
            Destroy(this);
        }
        else
        {
            GunInstance = this;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        attackSpawn = GetComponentsInChildren<Transform>(true)[2];
    }

    private void Update()
    {
        if (!UpgradeMenu.menuOpened && !PauseMenu.isPaused)
        {
            var testMousePos = Input.mousePosition;
            testMousePos.z = 10;
            mousePos = mainCamera.ScreenToWorldPoint(testMousePos);

            Vector3 rotation = mousePos - transform.position;
            rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,rotationZ);

            if(Input.GetMouseButtonDown(0))
            {
                if (!shooting)
                {
                    StartCoroutine(FireGun());
                }
            }
        }

    }

    IEnumerator FireGun()
    {
        shooting = true;
        GameObject attack = Instantiate(attackPrefab, attackSpawn.position, attackSpawn.rotation);
        attack.transform.localScale = new Vector3(0.1f, 0.1f, 0f);
        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        rb.AddForce(attackSpawn.right * bulletForce, ForceMode2D.Impulse);
        SFXManager.s.Audio.PlayOneShot(SFXManager.s.attack);
        yield return new WaitForSeconds(attackDuration); 

        shooting = false;
        Destroy(attack);
    }

    public void UpgradeFireRate(float multiplier)
    {
        multiplier += 1;
            attackDuration = attackDuration / multiplier;

        bulletForce *= 2;
    }
}
