using UnityEngine;

namespace PopupAsylum.GradientColorSpace
{
    /// <summary>
    /// https://www.magnetkern.de/srlab2.html
    /// </summary>
    public static class SRLAB2
    {
        public static Lab FromColor(Color color)
        {
            float red = color.r;
            float green = color.g;
            float blue = color.b;
            float x, y, z;
            if (red <= 0.03928) red /= 12.92f;
            else red = pow((red + 0.055f) / 1.055f, 2.4f);
            if (green <= 0.03928) green /= 12.92f;
            else green = pow((green + 0.055f) / 1.055f, 2.4f);
            if (blue <= 0.03928) blue /= 12.92f;
            else blue = pow((blue + 0.055f) / 1.055f, 2.4f);
            x = 0.320530f * red + 0.636920f * green + 0.042560f * blue;
            y = 0.161987f * red + 0.756636f * green + 0.081376f * blue;
            z = 0.017228f * red + 0.108660f * green + 0.874112f * blue;
            if (x <= 216.0 / 24389.0) x *= 24389.0f / 2700.0f;
            else x = 1.16f * pow(x, 1.0f / 3.0f) - 0.16f;
            if (y <= 216.0 / 24389.0) y *= 24389.0f / 2700.0f;
            else y = 1.16f * pow(y, 1.0f / 3.0f) - 0.16f;
            if (z <= 216.0 / 24389.0) z *= 24389.0f / 2700.0f;
            else z = 1.16f * pow(z, 1.0f / 3.0f) - 0.16f;

            return new Lab(37.0950f * x + 62.9054f * y - 0.0008f * z,
                663.4684f * x - 750.5078f * y + 87.0328f * z,
                63.9569f * x + 108.4576f * y - 172.4152f * z,
                color.a);
        }

        public static Color ToColor(Lab lab)
        {
            float lightness = lab.L;
            float a = lab.a;
            float b = lab.b;
            float red, green, blue;
            float x, y, z, rd, gn, bl;
            x = 0.01f * lightness + 0.000904127f * a + 0.000456344f * b;
            y = 0.01f * lightness - 0.000533159f * a - 0.000269178f * b;
            z = 0.01f * lightness - 0.005800000f * b;
            if (x <= 0.08f) x *= 2700.0f / 24389.0f;
            else x = pow((x + 0.16f) / 1.16f, 3.0f);
            if (y <= 0.08f) y *= 2700.0f / 24389.0f;
            else y = pow((y + 0.16f) / 1.16f, 3.0f);
            if (z <= 0.08f) z *= 2700.0f / 24389.0f;
            else z = pow((z + 0.16f) / 1.16f, 3.0f);
            rd = 5.435679f * x - 4.599131f * y + 0.163593f * z;
            gn = -1.168090f * x + 2.327977f * y - 0.159798f * z;
            bl = 0.037840f * x - 0.198564f * y + 1.160644f * z;
            if (rd <= 0.00304f) red = rd * 12.92f;
            else red = 1.055f * pow(rd, 1.0f / 2.4f) - 0.055f;
            if (gn <= 0.00304f) green = gn * 12.92f;
            else green = 1.055f * pow(gn, 1.0f / 2.4f) - 0.055f;
            if (bl <= 0.00304f) blue = bl * 12.92f;
            else blue = 1.055f * pow(bl, 1.0f / 2.4f) - 0.055f;

            return new Color(red, green, blue, lab.alpha);
        }

        private static float pow(float x, float y) => Mathf.Pow(x, y);
    }
}
