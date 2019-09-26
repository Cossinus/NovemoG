using System.Collections.Generic;
using System.Linq;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] public List<UniqueEffect> uniqueEffects;

        private UniqueEffect UniqueEffect => uniqueEffects.FirstOrDefault();

        private Equipment _equipmentWithEffect;

        private CharacterStats _playerStats;

        private void Start()
        {
            _playerStats = PlayerManager.Instance.player.GetComponent<CharacterStats>();
        }

        private void Update()
        {
            _equipmentWithEffect = EquipmentManager.Instance.GetEquipmentWithEffect;
            if (_equipmentWithEffect != null)
            {
                InitializeEffect();
            }
        }

        private void InitializeEffect()
        {
            if (_equipmentWithEffect.effects[0].effectName == UniqueEffect.effectName)
            {
                switch (UniqueEffect.effectType)
                {
                    case EffectType.Passive:
                    {
                        var passiveEffect = (PassiveEffect) _equipmentWithEffect.effects[0];
                        switch (passiveEffect.passiveType)
                        {
                            case PassiveTypes.Regenerate:
                            {
                                var regenerate = (RegenerateEffect) passiveEffect;
                                if (regenerate.CheckForEffect(_playerStats))
                                    StartCoroutine(regenerate.Passive(_playerStats));
                                break;
                            }
                        }
                        break;
                    }
                    case EffectType.Active:
                    {
                        var activeEffect = (ActiveEffect) UniqueEffect;
                        break;
                    }
                }
            }
        }
    }
}