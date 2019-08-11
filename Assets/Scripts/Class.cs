using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "Classes/Class")]
public class Class : ScriptableObject
{
    public string className = "New Class";
    public string classDescription = "Class Description";

    public Equipment defaultWeapon;
    
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    
    private CharacterStats myStats;
    private readonly PlayerManager player = PlayerManager.Instance;

    void Awake()
    {
        myStats = player.GetComponent<CharacterStats>();
        EquipmentManager.Instance.Equip(defaultWeapon);
    }
}
