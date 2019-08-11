using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName = "Ability Name";
    [TextArea(2, 5)]
    public string abilityDescription = "Ability Description";
    public float abilityDamage;
    
    private PlayerManager player = PlayerManager.Instance;

    public virtual void Use()
    {
        
    }
}
