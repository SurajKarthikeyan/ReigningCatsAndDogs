using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : MonoBehaviour, IInteractable
{
    public int amountToHeal = 2;

    public bool Interact(Interactor interact)
    {
        interact.gameObject.GetComponent<Health>().Heal(amountToHeal);
        return true;
    }

}
