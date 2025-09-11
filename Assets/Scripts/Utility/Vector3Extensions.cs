using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static List<Vector3> GetPositionsInRadius(Vector3 center, float radius, float height, int count)
    {
        List<Vector3> positions = new List<Vector3>(count);

        for (int i = 0; i < count; i++)
        {
            float angle = Mathf.PI * 2f * (i / (float)count);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 position = new Vector3(center.x + x, height, center.z + z);
            positions.Add(position);
        }

        return positions;
    }
}
