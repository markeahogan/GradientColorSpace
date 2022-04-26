using UnityEngine;

namespace PopupAsylum.GradientColorSpace
{
    static class Oklab
    {
        public static Lab FromColor(Color color)
        {
            return FromLinearColor(color.linear);
        }

        public static Lab FromLinearColor(Color linearColor)
        {
            float l = 0.4122214708f * linearColor.r + 0.5363325363f * linearColor.g + 0.0514459929f * linearColor.b;
            float m = 0.2119034982f * linearColor.r + 0.6806995451f * linearColor.g + 0.1073969566f * linearColor.b;
            float s = 0.0883024619f * linearColor.r + 0.2817188376f * linearColor.g + 0.6299787005f * linearColor.b;

            float l_ = cbrtf(l);
            float m_ = cbrtf(m);
            float s_ = cbrtf(s);

            return new Lab(
                0.2104542553f * l_ + 0.7936177850f * m_ - 0.0040720468f * s_,
            1.9779984951f * l_ - 2.4285922050f * m_ + 0.4505937099f * s_,
            0.0259040371f * l_ + 0.7827717662f * m_ - 0.8086757660f * s_,
            linearColor.a);

            static float cbrtf(float l) => Mathf.Pow(l, 1f / 3f);
        }

        public static Color ToColor(Lab lab)
        {
            return ToLinearColor(lab).gamma;
        }

        public static Color ToLinearColor(Lab lab)
        {
            float l_ = lab.L + 0.3963377774f * lab.a + 0.2158037573f * lab.b;
            float m_ = lab.L - 0.1055613458f * lab.a - 0.0638541728f * lab.b;
            float s_ = lab.L - 0.0894841775f * lab.a - 1.2914855480f * lab.b;

            float l = l_ * l_ * l_;
            float m = m_ * m_ * m_;
            float s = s_ * s_ * s_;

            return new Color(
                +4.0767416621f * l - 3.3077115913f * m + 0.2309699292f * s,
                -1.2684380046f * l + 2.6097574011f * m - 0.3413193965f * s,
                -0.0041960863f * l - 0.7034186147f * m + 1.7076147010f * s,
                lab.alpha);
        }
    }
}
