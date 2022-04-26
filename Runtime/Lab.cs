using UnityEngine;

namespace PopupAsylum.GradientColorSpace
{
    public struct Lab
    {
        public float L;
        public float a;
        public float b;
        public float alpha;

        public Lab(float l, float a, float b, float alpha = 1)
        {
            L = l;
            this.a = a;
            this.b = b;
            this.alpha = alpha;
        }

        public static Lab Lerp(Lab a, Lab b, float t)
        {
            return new Lab()
            {
                L = Mathf.Lerp(a.L, b.L, t),
                a = Mathf.Lerp(a.a, b.a, t),
                b = Mathf.Lerp(a.b, b.b, t),
                alpha = Mathf.Lerp(a.alpha, b.alpha, t)
            };
        }
    }
}
