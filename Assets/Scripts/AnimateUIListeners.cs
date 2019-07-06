using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyBox;
using System.Linq;

namespace EC
{
    [RequireComponent(typeof(Animate))]
    public class AnimateUIListeners : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        Animate animate;

        public bool pointerEnter;
        [ConditionalField("pointerEnter", true)] public AnimateEventListener pointerEnterAnims;
        int[] pointerEnterStates;

        public bool pointerExit;
        [ConditionalField("pointerEnter", true)] public AnimateEventListener pointerExitAnims;
        int[] pointerExitStates;

        public void Start()
        {
            animate = GetComponent<EC.Animate>();

            if (pointerEnter && !pointerEnterAnims.resetStates)
                pointerEnterStates = pointerEnterAnims.csvStateIndexes.Split(',').Select(s => { return int.Parse(s); }).ToArray();

            if (pointerExit && !pointerExitAnims.resetStates)
                pointerExitStates = pointerExitAnims.csvStateIndexes.Split(',').Select(s => { return int.Parse(s); }).ToArray();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerEnter)
                if (pointerEnterAnims.resetStates)
                    animate.Reset();
                else
                    animate.RunAnimations(pointerEnterStates, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (pointerExit)
                if (pointerExitAnims.resetStates)
                    animate.Reset();
                else
                    animate.RunAnimations(pointerExitStates, true);
        }

        [System.Serializable]
        public class AnimateEventListener
        {
            [Tooltip("CSV state indexes")]
            [ConditionalField("resetStates", false)] public string csvStateIndexes;
            public bool resetStates;
        }
    }
}