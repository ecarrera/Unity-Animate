using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using MyBox;
using System.Linq;

namespace EC {

    public enum AnimationStateParameterType { Vector2, Vector3, Color, Integer, Float }

    [System.Serializable]
    public class AnimationState
    {
        public string stateName;
        public string componentType;
        public string propertyName;
        public float duration = 1f;
        public LERP_TYPE lerpType = LERP_TYPE.LINEAR;

        public AnimationStateParameterType parameterType;
        [ConditionalField("parameterType", AnimationStateParameterType.Vector2)] public Vector2 vect2;
        [ConditionalField("parameterType", AnimationStateParameterType.Vector3)] public Vector3 vect3;
        [ConditionalField("parameterType", AnimationStateParameterType.Color)] public Color color;
        [ConditionalField("parameterType", AnimationStateParameterType.Integer)] public int integerNum;
        [ConditionalField("parameterType", AnimationStateParameterType.Float)] public float floatNum;

        public UnityEngine.Events.UnityEvent actionOnEnd;

        public bool linkedAnimation;
        [ConditionalField("linkedAnimation", true)] public GameObject linkedGameObject;
    }

    public class Animate : MonoBehaviour
	{
		public AnimationState[] states;
        AnimationState[] initialStates;
        List<IEnumerator> runningAnimations = new List<IEnumerator>();

        public bool advanced;
        [ConditionalField("advanced", true)] public LERP_TYPE resetLerpType = LERP_TYPE.LINEAR;
        [ConditionalField("advanced", true)] public float resetDuration;

        private void Start()
        {
            GetInitialComponentStates();
        }

        /// <summary>
        /// Get the initial values of the component properties states
        /// </summary>
        void GetInitialComponentStates()
        {
            List<string> addedInitialStates = new List<string>() { };

            initialStates = new AnimationState[states.Length];
            for (int i = 0; i < initialStates.Length; i++)
            {
                int instanceId = states[i].linkedAnimation ? states[i].linkedGameObject.GetInstanceID() : GetInstanceID();

                // Add unique states: instanceId+Component+Property

                if (!addedInitialStates.Contains(instanceId + states[i].componentType + states[i].propertyName))
                {
                    initialStates[i] = new AnimationState();
                    initialStates[i].componentType = states[i].componentType;
                    initialStates[i].propertyName = states[i].propertyName;
                    initialStates[i].parameterType = states[i].parameterType;
                    initialStates[i].linkedAnimation = states[i].linkedAnimation;
                    initialStates[i].linkedGameObject = states[i].linkedGameObject;
                    Component component = initialStates[i].linkedAnimation ? initialStates[i].linkedGameObject.GetComponent(initialStates[i].componentType) : gameObject.GetComponent(initialStates[i].componentType);

                    switch (initialStates[i].parameterType)
                    {
                        case AnimationStateParameterType.Vector2:
                            initialStates[i].vect2 = GetPropertyValue<Vector2>(component, initialStates[i].propertyName);
                            break;
                        case AnimationStateParameterType.Vector3:
                            initialStates[i].vect3 = GetPropertyValue<Vector3>(component, initialStates[i].propertyName);
                            break;
                        case AnimationStateParameterType.Color:
                            initialStates[i].color = GetPropertyValue<Color>(component, initialStates[i].propertyName);
                            break;
                        case AnimationStateParameterType.Integer:
                            initialStates[i].integerNum = GetPropertyValue<int>(component, initialStates[i].propertyName);
                            break;
                        case AnimationStateParameterType.Float:
                            initialStates[i].floatNum = GetPropertyValue<float>(component, initialStates[i].propertyName);
                            break;
                    }

                    addedInitialStates.Add(instanceId + states[i].componentType + states[i].propertyName);
                }
            }
        }

        /// <summary>
        /// Run all state animations
        /// </summary>
        public void RunAnimations(bool stopRunning = false)
        {
            if (stopRunning)
                StopRunningAnimations();

            for (int i = 0; i < states.Length; i++)
                AnimateState(i);
        }

        /// <summary>
        /// Runs the array of state animations
        /// </summary>
        public void RunAnimations(int[] statesToRun, bool stopRunning = false)
        {
            if (stopRunning)
                StopRunningAnimations();

            for (int i = 0; i < statesToRun.Length; i++)
                AnimateState(statesToRun[i]);
        }

        /// <summary>
        /// Stops the running animations
        /// </summary>
        void StopRunningAnimations()
        {
            for (int i = 0; i < runningAnimations.Count; i++)
                if (runningAnimations[i] != null)
                    StopCoroutine(runningAnimations[i]);

            runningAnimations.Clear();
        }

        /// <summary>
        /// Runs the array of state animations
        /// </summary>
        public void Reset()
        {
            StopRunningAnimations();

            for (int i=0; i<initialStates.Length; i++)
            {
                AnimationState state = initialStates[i];

                if (state != null)
                {
                    Component component = state.linkedAnimation ? state.linkedGameObject.GetComponent(state.componentType) : gameObject.GetComponent(state.componentType);

                    if (component != null)
                    {
                        IEnumerator anim = null;
                        switch (initialStates[i].parameterType)
                        {
                            case AnimationStateParameterType.Vector2:
                                anim = StartAnimation(component, state.propertyName, state.vect2, resetDuration, resetLerpType, state.actionOnEnd);
                                break;
                            case AnimationStateParameterType.Vector3:
                                anim = StartAnimation(component, state.propertyName, state.vect3, resetDuration, resetLerpType, state.actionOnEnd);
                                break;
                            case AnimationStateParameterType.Color:
                                anim = StartAnimation(component, state.propertyName, state.color, resetDuration, resetLerpType, state.actionOnEnd);
                                break;
                            case AnimationStateParameterType.Integer:
                                anim = StartAnimation(component, state.propertyName, state.integerNum, resetDuration, resetLerpType, state.actionOnEnd);
                                break;
                            case AnimationStateParameterType.Float:
                                anim = StartAnimation(component, state.propertyName, state.floatNum, resetDuration, resetLerpType, state.actionOnEnd);
                                break;
                        }

                        if (anim != null)
                        {
                            runningAnimations.Add(anim);
                            StartCoroutine(anim);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Animates an state animation by its index
        /// </summary>
        /// <param name="stateIndex"></param>
        public void AnimateState(int stateIndex)
        {
            AnimationState state = states[stateIndex];
            Component component = state.linkedAnimation ? state.linkedGameObject.GetComponent(state.componentType) : gameObject.GetComponent(state.componentType);

            if (component != null)
            {
                IEnumerator anim = null;
                switch (state.parameterType)
                {
                    case AnimationStateParameterType.Vector2:
                        anim = StartAnimation(component, state.propertyName, state.vect2, state.duration, state.lerpType, state.actionOnEnd);
                        break;
                    case AnimationStateParameterType.Vector3:
                        anim = StartAnimation(component, state.propertyName, state.vect3, state.duration, state.lerpType, state.actionOnEnd);
                        break;
                    case AnimationStateParameterType.Color:
                        anim = StartAnimation(component, state.propertyName, state.color, state.duration, state.lerpType, state.actionOnEnd);
                        break;
                    case AnimationStateParameterType.Integer:
                        anim = StartAnimation(component, state.propertyName, state.integerNum, state.duration, state.lerpType, state.actionOnEnd);
                        break;
                    case AnimationStateParameterType.Float:
                        anim = StartAnimation(component, state.propertyName, state.floatNum, state.duration, state.lerpType, state.actionOnEnd);
                        break;
                }

                if (anim != null)
                {
                    runningAnimations.Add(anim);
                    StartCoroutine(anim);
                }
            }
            else
                Debug.LogError("Component: " + state.componentType + " is null");
        }

        /// <summary>
        /// Starts an animation state: Vector2
        /// </summary>
        /// <returns></returns>
        IEnumerator StartAnimation(Component component, string propertyName, Vector2 endValue, float duration, LERP_TYPE lerpType, UnityEngine.Events.UnityEvent actionOnEnd)
        {
            float interpolator = 0;
            Vector2 initialValue = GetPropertyValue<Vector2>(component, propertyName);
            float curve;

            while (interpolator < 1)
            {
                interpolator += Time.deltaTime/duration;
                curve = LerpUtils.Lerp(interpolator, lerpType);
                SetProperty(component, propertyName, Vector2.Lerp(initialValue, endValue, curve));
                yield return null;
            }

            SetProperty(component, propertyName, endValue);

            if (actionOnEnd != null)
                actionOnEnd.Invoke();
        }

        /// <summary>
        /// Starts an animation state: Vector3
        /// </summary>
        /// <returns></returns>
        IEnumerator StartAnimation(Component component, string propertyName, Vector3 endValue, float duration, LERP_TYPE lerpType, UnityEngine.Events.UnityEvent actionOnEnd)
        {
            float interpolator = 0;
            Vector3 initialValue = GetPropertyValue<Vector3>(component, propertyName);
            float curve;

            while (interpolator < 1)
            {
                interpolator += Time.deltaTime / duration;
                curve = LerpUtils.Lerp(interpolator, lerpType);
                SetProperty(component, propertyName, Vector3.Lerp(initialValue, endValue, curve));
                yield return null;
            }

            SetProperty(component, propertyName, endValue);

            if (actionOnEnd != null)
                actionOnEnd.Invoke();
        }

        /// <summary>
        /// Starts an animation state: Vector2
        /// </summary>
        /// <returns></returns>
        IEnumerator StartAnimation(Component component, string propertyName, Color endValue, float duration, LERP_TYPE lerpType, UnityEngine.Events.UnityEvent actionOnEnd)
        {
            float interpolator = 0;
            Color initialValue = GetPropertyValue<Color>(component, propertyName);
            float curve;

            while (interpolator < 1)
            {
                interpolator += Time.deltaTime / duration;
                curve = LerpUtils.Lerp(interpolator, lerpType);
                SetProperty(component, propertyName, Color.Lerp(initialValue, endValue, curve));
                yield return null;
            }

            SetProperty(component, propertyName, endValue);

            if (actionOnEnd != null)
                actionOnEnd.Invoke();
        }

        /// <summary>
        /// Starts an animation state: Vector2
        /// </summary>
        /// <returns></returns>
        IEnumerator StartAnimation(Component component, string propertyName, int endValue, float duration, LERP_TYPE lerpType, UnityEngine.Events.UnityEvent actionOnEnd)
        {
            float interpolator = 0;
            int initialValue = GetPropertyValue<int>(component, propertyName);
            float curve;

            while (interpolator < 1)
            {
                interpolator += Time.deltaTime / duration;
                curve = LerpUtils.Lerp(interpolator, lerpType);
                SetProperty(component, propertyName, (int)Mathf.Lerp(initialValue, endValue, curve));
                yield return null;
            }

            SetProperty(component, propertyName, endValue);

            if (actionOnEnd != null)
                actionOnEnd.Invoke();
        }

        /// <summary>
        /// Starts an animation state: Float
        /// </summary>
        /// <returns></returns>
        IEnumerator StartAnimation(Component component, string propertyName, float endValue, float duration, LERP_TYPE lerpType, UnityEngine.Events.UnityEvent actionOnEnd)
        {
            float interpolator = 0;
            float initialValue = GetPropertyValue<float>(component, propertyName);
            float curve;

            while (interpolator < 1)
            {
                interpolator += Time.deltaTime / duration;
                curve = LerpUtils.Lerp(interpolator, lerpType);
                SetProperty(component, propertyName, Mathf.Lerp(initialValue, endValue, curve));
                yield return null;
            }

            SetProperty(component, propertyName, endValue);

            if (actionOnEnd != null)
                actionOnEnd.Invoke();
        }

        /// <summary>
        /// Returns the property T of an object by its name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(object inObj, string propertyName) where T : struct
        {
            return (T)GetProperty(inObj, propertyName);
        }

        /// <summary>
        /// Sets the property of an object by its name
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public static void SetProperty(object inObj, string propertyName, object newValue)
        {
            PropertyInfo info = inObj.GetType().GetProperty(propertyName);
            if (info != null)
                info.SetValue(inObj, newValue);
        }

        /// <summary>
        /// Gets the property of an object by its name
        /// </summary>
        /// <param name="inObj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static object GetProperty(object inObj, string propertyName)
        {
            object ret = null;
            PropertyInfo info = inObj.GetType().GetProperty(propertyName);
            if (info != null)
                ret = info.GetValue(inObj);

            return ret;
        }
    }

}