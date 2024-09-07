using System;

namespace UnityEngine.Extension
{
    public enum EasingMode
    {
        Ease,
        EaseIn,
        EaseOut,
        EaseInOut,
        Linear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce
    }

    public static class Easing
    {
        private const float HalfPi = MathF.PI / 2F;

        public static EasingMode GetInverseEasingMode(this EasingMode mode)
        {
            switch (mode)
            {
                default:
                    return mode;
                case EasingMode.EaseIn:
                    return EasingMode.EaseOut;
                case EasingMode.EaseOut:
                    return EasingMode.EaseIn;
                case EasingMode.EaseInSine:
                    return EasingMode.EaseOutSine;
                case EasingMode.EaseOutSine:
                    return EasingMode.EaseInSine;
                case EasingMode.EaseInCubic:
                    return EasingMode.EaseOutCubic;
                case EasingMode.EaseOutCubic:
                    return EasingMode.EaseInCubic;
                case EasingMode.EaseInCirc:
                    return EasingMode.EaseOutCirc;
                case EasingMode.EaseOutCirc:
                    return EasingMode.EaseInCirc;
                case EasingMode.EaseInElastic:
                    return EasingMode.EaseOutElastic;
                case EasingMode.EaseOutElastic:
                    return EasingMode.EaseInElastic;
                case EasingMode.EaseInBack:
                    return EasingMode.EaseOutBack;
                case EasingMode.EaseOutBack:
                    return EasingMode.EaseInBack;
                case EasingMode.EaseInBounce:
                    return EasingMode.EaseOutBounce;
                case EasingMode.EaseOutBounce:
                    return EasingMode.EaseInBounce;
            }
        }

        public static float PerformEase(float t, EasingMode mode)
        {
            switch (mode)
            {
                default:
                case EasingMode.Ease:
                    // "Best-fit" cubic curve trying to match start/end points and derivatives (stays within 0.079 of the exact curve).
                    // y = a t^3 + b t^2 + c t + d, where a = -0.2, b = -0.6, c = 1.8, d = 0
                    // Should be equivalent to cubic-bezier(0.25,0.1,0.25,1)
                    return t * (1.8f + t * (-0.6f + t * -0.2f));
                case EasingMode.EaseIn:
                    // Should be equivalent to cubic-bezier(0.42,0,1,1)
                    return InQuad(t);
                case EasingMode.EaseOut:
                    // Should be equivalent to cubic-bezier(0,0,0.58,1)
                    return OutQuad(t);
                case EasingMode.EaseInOut:
                    // Should be equivalent to cubic-bezier(0.42,0,0.58,1)
                    return InOutQuad(t);
                case EasingMode.Linear:
                    // Should be equivalent to cubic-bezier(0,0,1,1)
                    return Linear(t);
                case EasingMode.EaseInSine:
                    return InSine(t);
                case EasingMode.EaseOutSine:
                    return OutSine(t);
                case EasingMode.EaseInOutSine:
                    return InOutSine(t);
                case EasingMode.EaseInCubic:
                    return InCubic(t);
                case EasingMode.EaseOutCubic:
                    return OutCubic(t);
                case EasingMode.EaseInOutCubic:
                    return InOutCubic(t);
                case EasingMode.EaseInCirc:
                    return InCirc(t);
                case EasingMode.EaseOutCirc:
                    return OutCirc(t);
                case EasingMode.EaseInOutCirc:
                    return InOutCirc(t);
                case EasingMode.EaseInElastic:
                    return InElastic(t);
                case EasingMode.EaseOutElastic:
                    return OutElastic(t);
                case EasingMode.EaseInOutElastic:
                    return InOutElastic(t);
                case EasingMode.EaseInBack:
                    return InBack(t);
                case EasingMode.EaseOutBack:
                    return OutBack(t);
                case EasingMode.EaseInOutBack:
                    return InOutBack(t);
                case EasingMode.EaseInBounce:
                    return InBounce(t);
                case EasingMode.EaseOutBounce:
                    return OutBounce(t);
                case EasingMode.EaseInOutBounce:
                    return InOutBounce(t);
            }
        }        

        public static float Linear(float t)
        {
            return t;
        }

        public static float InSine(float t)
        {
            return Mathf.Sin(MathF.PI / 2f * (t - 1f)) + 1f;
        }

        public static float OutSine(float t)
        {
            return Mathf.Sin(t * (MathF.PI / 2f));
        }

        public static float InOutSine(float t)
        {
            return (Mathf.Sin(MathF.PI * (t - 0.5f)) + 1f) * 0.5f;
        }

        public static float InQuad(float t)
        {
            return t * t;
        }

        public static float OutQuad(float t)
        {
            return t * (2f - t);
        }

        public static float InOutQuad(float t)
        {
            t *= 2f;
            if (t < 1f)
            {
                return t * t * 0.5f;
            }

            return -0.5f * ((t - 1f) * (t - 3f) - 1f);
        }

        public static float InCubic(float t)
        {
            return InPower(t, 3);
        }

        public static float OutCubic(float t)
        {
            return OutPower(t, 3);
        }

        public static float InOutCubic(float t)
        {
            return InOutPower(t, 3);
        }

        public static float InPower(float t, int power)
        {
            return Mathf.Pow(t, power);
        }

        public static float OutPower(float t, int power)
        {
            int num = ((power % 2 != 0) ? 1 : (-1));
            return (float)num * (Mathf.Pow(t - 1f, power) + (float)num);
        }

        public static float InOutPower(float t, int power)
        {
            t *= 2f;
            if (t < 1f)
            {
                return InPower(t, power) * 0.5f;
            }

            int num = ((power % 2 != 0) ? 1 : (-1));
            return (float)num * 0.5f * (Mathf.Pow(t - 2f, power) + (float)(num * 2));
        }

        public static float InBounce(float t)
        {
            return 1f - OutBounce(1f - t);
        }

        public static float OutBounce(float t)
        {
            if (t < 0.363636374f)
            {
                return 7.5625f * t * t;
            }

            if (t < 0.727272749f)
            {
                float num = (t -= 0.545454562f);
                return 7.5625f * num * t + 0.75f;
            }

            if (t < 0.909090936f)
            {
                float num2 = (t -= 0.8181818f);
                return 7.5625f * num2 * t + 0.9375f;
            }

            float num3 = (t -= 21f / 22f);
            return 7.5625f * num3 * t + 63f / 64f;
        }

        public static float InOutBounce(float t)
        {
            if (t < 0.5f)
            {
                return InBounce(t * 2f) * 0.5f;
            }

            return OutBounce((t - 0.5f) * 2f) * 0.5f + 0.5f;
        }

        public static float InElastic(float t)
        {
            if (t == 0f)
            {
                return 0f;
            }

            if (t == 1f)
            {
                return 1f;
            }

            float num = 0.3f;
            float num2 = num / 4f;
            float num3 = Mathf.Pow(2f, 10f * (t -= 1f));
            return 0f - num3 * Mathf.Sin((t - num2) * (MathF.PI * 2f) / num);
        }

        public static float OutElastic(float t)
        {
            if (t == 0f)
            {
                return 0f;
            }

            if (t == 1f)
            {
                return 1f;
            }

            float num = 0.3f;
            float num2 = num / 4f;
            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num2) * (MathF.PI * 2f) / num) + 1f;
        }

        public static float InOutElastic(float t)
        {
            if (t < 0.5f)
            {
                return InElastic(t * 2f) * 0.5f;
            }

            return OutElastic((t - 0.5f) * 2f) * 0.5f + 0.5f;
        }

        public static float InBack(float t)
        {
            float num = 1.70158f;
            return t * t * ((num + 1f) * t - num);
        }

        public static float OutBack(float t)
        {
            return 1f - InBack(1f - t);
        }

        public static float InOutBack(float t)
        {
            if (t < 0.5f)
            {
                return InBack(t * 2f) * 0.5f;
            }

            return OutBack((t - 0.5f) * 2f) * 0.5f + 0.5f;
        }

        public static float InBack(float t, float s)
        {
            return t * t * ((s + 1f) * t - s);
        }

        public static float OutBack(float t, float s)
        {
            return 1f - InBack(1f - t, s);
        }

        public static float InOutBack(float t, float s)
        {
            if (t < 0.5f)
            {
                return InBack(t * 2f, s) * 0.5f;
            }

            return OutBack((t - 0.5f) * 2f, s) * 0.5f + 0.5f;
        }

        public static float InCirc(float t)
        {
            return 0f - (Mathf.Sqrt(1f - t * t) - 1f);
        }

        public static float OutCirc(float t)
        {
            t -= 1f;
            return Mathf.Sqrt(1f - t * t);
        }

        public static float InOutCirc(float t)
        {
            t *= 2f;
            if (t < 1f)
            {
                return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
            }

            t -= 2f;
            return 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
        }
    }
}
