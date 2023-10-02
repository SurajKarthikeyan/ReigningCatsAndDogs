using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
   public static PlayerMovement player;
    public Animator anim;
    public Image coffeeSprite;

    public bool isForward;
    public bool isBackward;
    public bool isLeft;
    public bool isRight;
    public bool isTopLeft;
    public bool isBottomLeft;
    public bool isTopRight;
    public bool isBottomRight;
    public bool isMoving;

    #region PrivateFields
    [SerializeField] 
   private Sprite rSprite, trSprite, tSprite, tlSprite, lSprite, blSprite, bSprite, brSprite;
   [SerializeField] private SpriteRenderer playerRenderer;

   Rigidbody2D body;
   float horizontal;
   float vertical;
   float moveLimiter = 0.7f;

   #endregion

   public float runSpeed = 10.0f;

   public bool holdingDrink = false;

   private void Awake()
   {
      if (player != null && player != this)
      {
         Destroy(this);
         return;
      }
      player = this;
      DontDestroyOnLoad(this);
   }

   void Start ()
   {
      body = GetComponent<Rigidbody2D>();
        isForward = false;
        isBackward = false;
        isLeft = false;
        isRight = false;
        isTopLeft = false;
        isBottomLeft = false;
        isTopRight = false;
        isBottomRight = false;

        coffeeSprite.gameObject.SetActive(false);
   }

   void Update()
   {
        Debug.Log(holdingDrink);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("IsForward", isForward);
        anim.SetBool("IsBackward", isBackward);
        anim.SetBool("IsLeft", isLeft);
        anim.SetBool("IsRight", isRight);
        anim.SetBool("IsTopLeft", isTopLeft);
        anim.SetBool("IsBottomLeft", isBottomLeft);
        anim.SetBool("IsTopRight", isTopRight);
        anim.SetBool("IsBottomRight", isBottomRight);


        if (!UpgradeMenu.menuOpened)
      {
         // Gives a value between -1 and 1
         horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
         vertical = Input.GetAxisRaw("Vertical"); // -1 is down

         //
         // Calculate angle to draw correct sprite
         //

         float currentRotation = Gun.GunInstance.rotationZ;
         GameObject aim = transform.Find("Aim").gameObject;
         GameObject testGun = aim.transform.Find("TestGun").gameObject;
         SpriteRenderer gunRenderer = testGun.GetComponent<SpriteRenderer>();
         if (-22.5f <= currentRotation && currentRotation < 22.5f)   // Right
         {
            SetDirectionsFalse();
            isRight = true;
            playerRenderer.sortingOrder = 1;
            gunRenderer.sprite = rSprite;
         }
         
         else if (22.5f <= currentRotation && currentRotation < 67.5f)   //Top Right
         {
            SetDirectionsFalse();
            isTopRight = true;
            playerRenderer.sortingOrder = 3;
            gunRenderer.sprite = trSprite;
         }
         
         else if (67.5f <= currentRotation && currentRotation < 112.5f)   //Top
         {
            SetDirectionsFalse();
            isBackward = true;
            playerRenderer.sortingOrder = 3;
            gunRenderer.sprite = tSprite;
         }
         
         else if (112.5f <= currentRotation && currentRotation < 157.5f)   //Top Left
         {
            SetDirectionsFalse();
            isTopLeft = true;
            playerRenderer.sortingOrder = 3;
            gunRenderer.sprite = tlSprite;

         }
         
         else if (currentRotation < -157.5f || currentRotation >= 157.5f)   //Left
         {
            SetDirectionsFalse();
            isLeft = true;
            playerRenderer.sortingOrder = 1;
            gunRenderer.sprite = lSprite;
         }
         
         else if (currentRotation >= -157.5f && currentRotation < -112.5f)   //Bottom Left
         {
            SetDirectionsFalse();
            isBottomLeft = true;
            playerRenderer.sortingOrder = 1;
            gunRenderer.sprite = blSprite;
         }
         
         else if (currentRotation >= -112.5f && currentRotation < -67.5f)   //Bottom
         {
            SetDirectionsFalse();
            isForward = true;
            playerRenderer.sortingOrder = 1;
            gunRenderer.sprite = bSprite;
         }
         else if (currentRotation >= -67.5f && currentRotation < -22.5f)   //Bottom Right
         {
            SetDirectionsFalse();
            isBottomRight = true;
            playerRenderer.sortingOrder = 1;
            gunRenderer.sprite = brSprite;
         }
      }


        if (holdingDrink)
        {
            coffeeSprite.gameObject.SetActive(true);

        } else
        {
            coffeeSprite.gameObject.SetActive(false);

        }
    }

   void FixedUpdate()
   {
      if (!UpgradeMenu.menuOpened)
      {
         // Are we moving diagonally?
         if (horizontal != 0 && vertical != 0)
         {
            // limit movement speed diagonally by moveLimiter float
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
         } 

         body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
      }

        Vector2 result = body.velocity - Vector2.zero;
        if (Mathf.Abs(result.x) < 0.00001f && Mathf.Abs(result.y) < 0.0001f)
        {
            isMoving = false;
        } 
        else
        {
            isMoving = true;
        }
    }

   

   public void GiveDrink()
   {
        //Get rid of drink visual
        holdingDrink = false;
   }

    public void SetDirectionsFalse()
    {
        isForward = false;
        isBackward = false;
        isLeft = false;
        isRight = false;
        isTopLeft = false;
        isBottomLeft = false;
        isTopRight = false;
        isBottomRight = false;
    }
}
