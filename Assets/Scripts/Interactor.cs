using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region PrivateFields
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius;
    private readonly Collider2D[] colliders = new Collider2D[3];
    [SerializeField] private float damage;
    #endregion
    [SerializeField] LayerMask interactionLayer;

    [SerializeField] private int numFound;

    // Update is called once per frame
    void Update()
    {
        FindInteractables();
    }


    public void FindInteractables()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionRadius, colliders, interactionLayer);
        if (numFound >= 1)
        {
            var interactable = colliders[0].GetComponent<IInteractable>();

            if (interactable != null && Input.GetKeyDown(KeyCode.E))
                interactable.Interact(this);
        }
    }

    // Temproary function to see interaction range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }



}
