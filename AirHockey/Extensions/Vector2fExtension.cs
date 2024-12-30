using SFML.System;

namespace AirHockey.Extensions;

static class Vector2fExtension
{
    public static Vector2f Normalize(this Vector2f vector)
    {
        float vectorLength = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        float inversedLength = (1 / vectorLength);
        
        vector *= inversedLength;

        return vector;
    }
}