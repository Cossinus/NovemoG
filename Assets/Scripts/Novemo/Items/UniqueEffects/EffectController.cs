using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Items.UniqueEffects
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] public List<UniqueEffect> uniqueEffects;

        private UniqueEffect UniqueEffect
        {
            get
            {
                foreach (var effect in uniqueEffects)
                {
                    return effect;
                }

                return null;
            }
        }

        void Update()
        {
            var eqWithEffect = EquipmentManager.Instance.GetEquipmentWithEffect;
            if (eqWithEffect != null)
            {
                if (eqWithEffect.effects[0].name == UniqueEffect.eName)
                {
                    if (UniqueEffect.eType == EffectType.Passive)
                    {
                        var effect = eqWithEffect.effects[0] as PassiveEffect;
                        if (effect.pType == PassiveTypes.Regenerate)
                        {
                            if (effect.IsRegenerating == false)
                                StartCoroutine(effect.Passive());
                        }
                        else
                        {
                            StartCoroutine(effect.Passive());
                        }
                    }
                    else
                    {
                        var effect = UniqueEffect as ActiveEffect;
                    }
                }
            }
        }

        private void InitializeEffect()
        {
            
        }
    }
}