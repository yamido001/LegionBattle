using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGenerater {

    public static GameObject AttachMeshRender(Material ma, Mesh mesh)
    {
        GameObject obj = new GameObject();
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer render = obj.AddComponent<MeshRenderer>();
        render.material = ma;
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        render.receiveShadows = false;
        return obj;
    }

    public static Mesh CreateCircle(float radius)
    {
        Mesh ret = new Mesh();

        int edgeVerticeCount = (int)(40 * radius);

        Vector3[] vertices = new Vector3[edgeVerticeCount + 1];
        int[] triangles = new int[edgeVerticeCount * 3];
        Vector2[] uvs = new Vector2[edgeVerticeCount + 1];

        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 0.5f);
        for(int i = 0; i < edgeVerticeCount; ++i)
        {
            vertices[i + 1] = GetPosByAngleAndRadius(radius, i * 360f / edgeVerticeCount);
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i == edgeVerticeCount - 1 ? 1 : i + 2;
            uvs[i + 1] = new Vector2(vertices[i + 1].x / (2 * radius), vertices[i + 1].z / (2 * radius)) + uvs[0];
        }
        ret.vertices = vertices;
        ret.uv = uvs;
        ret.triangles = triangles;
        
        return ret;
    }

    public static Mesh CreateRing(float radiusMax, float radiusMin)
    {
        Mesh ret = new Mesh();

        int edgeVerticeCount = (int)(40 * radiusMax);

        Vector3[] vertices = new Vector3[edgeVerticeCount * 2];
        int[] triangles = new int[edgeVerticeCount * 2 * 3];
        Vector2[] uvs = new Vector2[edgeVerticeCount * 2];
        
        for (int i = 0; i < edgeVerticeCount; ++i)
        {
            float rangle = i * 360f / edgeVerticeCount;
            int verIndex = i * 2;

            vertices[verIndex] = GetPosByAngleAndRadius(radiusMin, rangle);
            vertices[verIndex + 1] = GetPosByAngleAndRadius(radiusMax, rangle);

            uvs[verIndex] = GetUVByPos(vertices[verIndex], radiusMax);
            uvs[verIndex + 1] = GetUVByPos(vertices[verIndex + 1], radiusMax);

            int triangleIndex = i * 6;
            if(i == edgeVerticeCount - 1)
            {
                triangles[triangleIndex] = verIndex;
                triangles[triangleIndex + 1] = verIndex + 1;
                triangles[triangleIndex + 2] = 1;

                triangles[triangleIndex + 3] = verIndex;
                triangles[triangleIndex + 4] = 1;
                triangles[triangleIndex + 5] = 0;
            }
            else
            {
                triangles[triangleIndex] = verIndex;
                triangles[triangleIndex + 1] = verIndex + 1;
                triangles[triangleIndex + 2] = verIndex + 3;

                triangles[triangleIndex + 3] = verIndex;
                triangles[triangleIndex + 4] = verIndex + 3;
                triangles[triangleIndex + 5] = verIndex + 2;
            }
        }
        ret.vertices = vertices;
        ret.uv = uvs;
        ret.triangles = triangles;
        return ret;
    }

    static Vector2 GetUVByPos(Vector3 pos, float radius)
    {
        return new Vector2(pos.x / (2 * radius) + 0.5f, pos.z / (2 * radius) + 0.5f);
    }

    static Vector3 GetPosByAngleAndRadius(float radius, float angle)
    {
        Vector3 zeroAnglePos = new Vector3(radius, 0f, 0f);
        return Quaternion.AngleAxis(angle, Vector3.up) * zeroAnglePos;
    }
}
