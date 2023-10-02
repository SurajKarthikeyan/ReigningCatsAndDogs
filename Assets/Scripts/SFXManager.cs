using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager s;

    public AudioSource Audio;

    public AudioClip attack;
    public AudioClip upgrade;
    public AudioClip makingCoffee;
    public AudioClip coffeeDone;
    public AudioClip catAttack;
    public AudioClip ghost;
    public AudioClip meow;

    private void Awake()
    {
        if (s != null && s != this)
        {
            Destroy(this);
            return;
        }
        s = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
