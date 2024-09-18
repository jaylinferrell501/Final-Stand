using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{ 
   [SerializeField] private int m_ammoAmount = 200;

   [SerializeField] private AmmoType m_ammoType;

   public enum AmmoType
   {
       kAk47,
       kRevolver,
       kShotGun
   }
    public int GetAmmoAmount()
    {
        return m_ammoAmount;
    }

    public AmmoType GetAmmoType()
    {
        return m_ammoType;
    }
}
