using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    public GameObject CME;
    public GameObject particleSystem;

    private float timer;
    private float holdDuration = 3f;
    private bool filling = false;


    public bool Interact(Interactor interact)
    {
        filling = true;
        timer = Time.time;

        SFXManager.s.Audio.PlayOneShot(SFXManager.s.makingCoffee);

        return true;
    }



    // Start is called before the first frame update
    void Start()
    {
        CME.SetActive(false);
        particleSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (filling)
        {
            if (Input.GetKey(KeyCode.E))
            {
                particleSystem.SetActive(true);
                Invoke("turnOffParticles", holdDuration);
                if (Time.time - timer > holdDuration)
                {
                    timer = float.PositiveInfinity;
                    SFXManager.s.Audio.PlayOneShot(SFXManager.s.coffeeDone);
                    Debug.Log("Coffee is made!");
                    if (!PlayerMovement.player.holdingDrink)
                    {
                        PlayerMovement.player.holdingDrink = true;
                    }

                }
            }
            else
            {
                filling = false;
                timer = float.PositiveInfinity;
                SFXManager.s.Audio.Stop();
            }
        }

    
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

    public void turnOffParticles()
    {
        particleSystem.SetActive(false);
    }
}
