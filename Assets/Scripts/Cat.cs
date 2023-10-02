using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour, IInteractable
{
    // https://github.com/h8man/NavMeshPlus for 2D NavMesh
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Seat targetSeat;
    private bool reachedSeat = false;

    [SerializeField] private float damage = 0.5f;
    private bool attacking = false;
    private Ghost targetGhost;
    [SerializeField] private float range = 10f;
    [SerializeField] private float fireRate = 2f;
    private float fireRateTimer;

    private float baseTip = 5.0f;
    private bool wantsDrink = false;


    [SerializeField]
    private Sprite leftS, rightS, upS, downS;

    [SerializeField] private SpriteRenderer catRenderer;

    private float oldX;
    private float oldY;


    // Start is called before the first frame update
    void Start()
    {
        FixRotationFor2D();
        FindSeat();

        oldX = this.gameObject.transform.position.x;
        oldY = this.gameObject.transform.position.y;
        
    }
    //up right : -8, -1
    //down left: -15, -11

    // Update is called once per frame
    void Update()
    {

        if (!reachedSeat)
        {
            MoveTo(targetSeat.gameObject.transform);
            SetSprite();
            reachedSeat = ReachedSeat();
        }
        else
        {
            catRenderer.sprite = downS;
            if (!attacking)
            {
                if (FindObjectsOfType<Ghost>().Length > 0)
                {
                    targetGhost = GetClosestEnemy();
                    if (targetGhost != null)
                        attacking = true;
                }
            }
            else
            {
                Attack();
            }

            if (wantsDrink)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Drink();
            }
        }

    }

    #region Cat Movement

    void FixRotationFor2D()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void FindSeat()
    {
        targetSeat = GameManager.gameManager.availableSeats[Random.Range(0, GameManager.gameManager.availableSeats.Count)];
        targetSeat.isEmpty = false;
    }


    void MoveTo(Transform targetPos)
    {
        if (Vector2.Distance(transform.position, targetPos.position) > navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPos.position);
        }
    }


    bool ReachedSeat()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            wantsDrink = true;
            SFXManager.s.Audio.PlayOneShot(SFXManager.s.meow);
            return true;
        }
        return false;
    }

    void SetSprite()
    {
        float directionX = this.gameObject.transform.position.x - oldX;
        float directionY = this.gameObject.transform.position.y - oldY;

        if (Mathf.Abs(directionX) >= Mathf.Abs(directionY))
        {
            if (directionX <= 0)
            {
                catRenderer.sprite = leftS;
            }
            else
            {
                catRenderer.sprite = rightS;
            }
        }
        else
        {
            if (directionY <= 0)
            {
                catRenderer.sprite = downS;
            }
            else
            {
                catRenderer.sprite = upS;
            }
        }

        oldX = this.gameObject.transform.position.x;
        oldY = this.gameObject.transform.position.y;

    }

    public void EmptySeat() { targetSeat.isEmpty = true; }

    #endregion

    #region Cat Attacking

    private Ghost GetClosestEnemy()
    {
        return Ghost.GetClosestEnemy(transform.position, range);
    }

    private void Attack()
    {
        if (Time.time > fireRateTimer)
        {
            // fireRateTimer = fireRate;
            Debug.Log("Enemy in range!");
            targetGhost.TakeDamage(damage);
            SFXManager.s.Audio.PlayOneShot(SFXManager.s.catAttack);
            fireRateTimer = Time.time + fireRate;
            // Did the ghost die?
            if (targetGhost == null)
                attacking = false;
        }
    }

    #endregion

    #region Cat Ordering
    public bool Interact(Interactor interact)
    {
        // If we happen to add drinks we can move this logic to a whole Ordering thing 
        // that checks which drink player is holding, if its correct, base drink price?, etc.
        if (wantsDrink && interact.gameObject.GetComponent<PlayerMovement>().holdingDrink)
        {
            interact.gameObject.GetComponent<PlayerMovement>().GiveDrink();
            ReceiveDrink();
            return true;
        }

        return false;
    }

    private void ReceiveDrink()
    {            
        //Get rid of UI Drink bubble attached to Cat
        transform.GetChild(0).gameObject.SetActive(false);
        wantsDrink = false;
        SFXManager.s.Audio.PlayOneShot(SFXManager.s.meow);
        CalculateTip();
    }

    private float timeDrinkingLeft = 30f;
    private void Drink()
    {
        //Add cat drinking visual
        timeDrinkingLeft -= Time.deltaTime;
        if (timeDrinkingLeft <= 0)
        {
            wantsDrink = true;
            SFXManager.s.Audio.PlayOneShot(SFXManager.s.meow);
            timeDrinkingLeft = 30f;
        }
    }

    void CalculateTip()
    {
        //display tip effect
        float tip = baseTip + this.gameObject.GetComponent<Health>().currentHealth;
        GameManager.gameManager.totalCash += tip;
        Debug.Log("Tipped: " + tip);
    }
    #endregion

}
