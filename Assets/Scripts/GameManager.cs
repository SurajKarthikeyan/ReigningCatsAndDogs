using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;

    public bool inWave = false;
    public int waveNumber = 1;
    public float totalCash = 0f;

    [SerializeField]
    private GameObject catPrefab;
    public int wavesBetweenCats = 5;
    public List<Seat> availableSeats = new List<Seat>();


    public int wavesBetweenBoost = 3;
    public float ghostsSpeed = 1.0f;
    public float ghostsMaxHealth = 1.0f;
    public float ghostsDamage = 1.0f;

    [SerializeField] private TextMeshProUGUI tipUI;


    void Awake()
    {
        gameManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForNewCustomer();

        tipUI.text = "$" + totalCash;
    }

    public void NextWave()
    {
        waveNumber++;
        catSpawned = false;

        if (waveNumber % wavesBetweenBoost == 0)
        {
            ghostsSpeed += 0.25f;
            ghostsMaxHealth += 0.25f;
            ghostsDamage += 0.25f;
        }
    }

    private bool catSpawned = false;
    private void CheckForNewCustomer()
    {
        CheckSeats();
        if (waveNumber % wavesBetweenCats == 0 && !catSpawned && availableSeats.Count > 0)
        {
            Instantiate(catPrefab, this.transform.position, this.transform.rotation);
            catSpawned = true;
        }
    }

    private void CheckSeats()
    {
        List<Seat> availables = new List<Seat>();
        Seat[] totalSeats = FindObjectsOfType<Seat>();

        foreach (Seat seat in totalSeats)
        {
            if (seat.isEmpty)
            {
                availables.Add(seat);
            }
        }

        availableSeats = availables.Distinct().ToList();
    }

}
