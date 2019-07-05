using System.Collections;
using System.Reflection;
using UnityEngine;
using MyBox;

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
    }

    public class Animate : MonoBehaviour
	{
		public AnimationState[] states;

        /// <summary>
        /// Run all state animations
        /// </summary>
        public void RunAnimations()
        {
            for (int i = 0; i < states.Length; i++)
                AnimateState(i);
        }

        /// <summary>
        /// Animates an state animation by its index
        /// </summary>
        /// <param name="stateIndex"></param>
        public void AnimateState(int stateIndex)
        {
            AnimationState state = states[stateIndex];
            Component component = gameObject.GetComponent(state.componentType);

            if (component != null)
            {
                switch (state.parameterType)
                {
                    case AnimationStateParameterType.Vector2:
                        StartCoroutine(StartAnimation(component, state.propertyName, state.vect2, state.duration, state.lerpType, state.actionOnEnd));
                        break;
                    case AnimationStateParameterType.Vector3:
                        StartCoroutine(StartAnimation(component, state.propertyName, state.vect3, state.duration, state.lerpType, state.actionOnEnd));
                        break;
                    case AnimationStateParameterType.Color:
                        StartCoroutine(StartAnimation(component, state.propertyName, state.color, state.duration, state.lerpType, state.actionOnEnd));
                        break;
                    case AnimationStateParameterType.Integer:
                        StartCoroutine(StartAnimation(component, state.propertyName, state.integerNum, state.duration, state.lerpType, state.actionOnEnd));
                        break;
                    case AnimationStateParameterType.Float:
                        StartCoroutine(StartAnimation(component, state.propertyName, state.floatNum, state.duration, state.lerpType, state.actionOnEnd));
                        break;
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