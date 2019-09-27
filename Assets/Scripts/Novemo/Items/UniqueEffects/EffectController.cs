using Novemo.Controllers;
using Novemo.Player;
using Novemo.Stats;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    public class EffectController : MonoBehaviour
    {
        private PlayerStats _playerStats;
        private CharacterCombat _playerCombat;

        private void Start()
        {
            _playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            _playerCombat = PlayerManager.Instance.player.GetComponent<CharacterCombat>();
        }

        private void Update()
        {
            InitializeEffect();
        }

        private void InitializeEffect()
        {
            foreach (var eq in EquipmentManager.Instance.currentEquipment)
            {
                if (eq != null && eq.effects.Count > 0)
                {
                    switch (eq.effects[0].effectType)
                    {
                        case EffectType.Passive:
                        {
                            var passiveEffect = (PassiveEffect) eq.effects[0];
                            switch (passiveEffect.passiveType)
                            {
                                case PassiveTypes.Regenerate:
                                {
                                    var regenerate = (RegenerateEffect) passiveEffect;
                                    if (regenerate.CheckForEffect(_playerStats))
                                        StartCoroutine(regenerate.Passive(_playerStats));
                                    break;
                                }
                                case PassiveTypes.MitigateDamage:
                                {
                                    var mitigate = (MitigateEffect) passiveEffect;
                                    if (mitigate.CheckForEffect())
                                        StartCoroutine(mitigate.Passive(_playerStats));
                                    break;
                                }
                                case PassiveTypes.Thorns:
                                {
                                    var thorn = (ThornsEffect) passiveEffect;
                                    StartCoroutine(thorn.GetEffect(_playerCombat));
                                    StartCoroutine(thorn.Passive(_playerStats));
                                    break;
                                }
                                case PassiveTypes.StatBoost:
                                {
                                    var statBoost = (StatBoostEffect) passiveEffect;
                                    StartCoroutine(statBoost.Passive(_playerStats));
                                    break;
                                }
                            }
                            break;
                        }
                        case EffectType.Active:
                        {
                            var activeEffect = (ActiveEffect) eq.effects[0];
                            break;
                        }
                    }
                }
            }
        }
    }
}