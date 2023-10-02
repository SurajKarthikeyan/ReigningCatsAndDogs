using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public SpriteRenderer sRend;
    public Sprite attackSprite;
    public Sprite baseSprite;
    public Sprite splatterSprite;

    //
    // Ghost Strength Variables
    //
    public float speed = 2.0f;
    public float maxHealth = 10.0f;
    public float damageDone = 2.0f;

    private GameObject target;
    private Vector2 targetPosition;

    public float currentHealth;
    private bool isDead = false;

    private List<GameObject> possibleTargets = new List<GameObject>();

    public static List<Ghost> allGhosts = new List<Ghost>();


    // Start is called before the first frame update
    void Start()
    {
        allGhosts.Add(this);

        currentHealth = GameManager.gameManager.ghostsMaxHealth;

        Cat[] cats = FindObjectsOfType<Cat>();

        foreach (Cat cat in cats)
        {
            possibleTargets.Add(cat.gameObject);
        }
        possibleTargets.Add(FindObjectsOfType<PlayerMovement>()[0].gameObject);

        target = possibleTargets[Random.Range(0, possibleTargets.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        float step = GameManager.gameManager.ghostsSpeed * Time.deltaTime;

        if (target != null)
        {
            targetPosition = target.transform.position;
        }
        else
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Count)];
        }

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(IsDead())
            Die();
    }

    public bool IsDead()
    {
        return !isDead && currentHealth <=0;
    }

    public void Die()
    {
        isDead = true;
        allGhosts.Remove(this);
        this.sRend.sprite = splatterSprite;
        Invoke("destroyThing", 0.2f);
    }

    public static Ghost GetClosestEnemy(Vector3 position, float maxRange)
    {
        Ghost closest = null;

        // Might be temporary but this is for level reloading to delete ghosts from the List that were 
        // deleted in the level restart
        List<Ghost> itemsToAdd = new List<Ghost>();
        foreach (Ghost g in allGhosts)
        {
            if (g != null)
            {
                itemsToAdd.Add(g);       
            }
        }

        foreach (Ghost ghost in itemsToAdd)
        {
            if(ghost.IsDead()) continue;

            if (ghost != null)
            {
                if (Vector3.Distance(position, ghost.gameObject.transform.position) <= maxRange)
                {
                    if (closest == null)
                        closest = ghost;
                    else
                    {
                        if (Vector3.Distance(position, ghost.gameObject.transform.position) <= Vector3.Distance(position, closest.gameObject.transform.position))
                            closest = ghost;
                    }
                }
            }
        }
        return closest;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Cat")
        {
            Health health = col.gameObject.GetComponent<Health>();
            health.TakeDamage(GameManager.gameManager.ghostsDamage);
            sRend.sprite = attackSprite;
            SFXManager.s.Audio.PlayOneShot(SFXManager.s.ghost);
            Invoke("revertAnimation", 1);
        }
    }

    void revertAnimation()
    {
        sRend.sprite = baseSprite;
    }

    void destroyThing()
    {
        Destroy(gameObject);

    }

}
