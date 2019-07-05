namespace EC
{
    public class LerpUtils
    {
        public static float Lerp(float t, LERP_TYPE lerpType)
        {

            switch (lerpType)
            {
                case LERP_TYPE.LINEAR:
                    // Do nothing
                    break;
                case LERP_TYPE.EASE_IN:
                    t = EaseIn(t);
                    break;
                case LERP_TYPE.EASE_OUT:
                    t = EaseOut(t);
                    break;
                case LERP_TYPE.EXPONENTIAL:
                    t = Exponential(t);
                    break;
                case LERP_TYPE.SMOOTHSTEP:
                    t = Smoothstep(t);
                    break;
                case LERP_TYPE.SMOOTHERSTEP:
                    t = Smootherstep(t);
                    break;
            }

            return t;
        }

        public static float EaseIn(float t)
        {
            return 1f - UnityEngine.Mathf.Cos(t * UnityEngine.Mathf.PI * 0.5f);
        }

        public static float EaseOut(float t)
        {
            return UnityEngine.Mathf.Sin(t * UnityEngine.Mathf.PI * 0.5f);
        }

        public static float Exponential(float t)
        {
            return t * t;
        }

        public static float Smoothstep(float t)
        {
            return t * t * (3f - 2f * t);
        }

        public static float Smootherstep(float t)
        {
            return t * t * t * (t * (6f * t - 15f) + 10f);
        }

    }

    public enum LERP_TYPE
    {
        LINEAR,
        EASE_IN,
        EASE_OUT,
        EXPONENTIAL,
        SMOOTHSTEP,
        SMOOTHERSTEP
    }
}