using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    [Header("Fractal Perlin")]
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public float noiseScale = 0.3f;
    public float heightScale = 2f;
    public Vector2 noiseOffset = Vector2.zero;
    public int seed = 0;

    void Start()
    {
        seed = Random.Range(0, 999);

        noiseOffset = new Vector2(Random.Range(0, 999), Random.Range(0, 999));

        mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32; // allow >65k vertices

        GetComponent<MeshFilter>().mesh = mesh;

        meshCollider = GetComponent<MeshCollider>();

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = PerlinGenerator(x, z) * heightScale;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; ++z)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    float PerlinGenerator(int x, int z)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0f;
        float totalAmplitude = 0f;

        float nx = (x + seed) * noiseScale + noiseOffset.x;
        float nz = (z + seed) * noiseScale + noiseOffset.y;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = nx * frequency;
            float sampleZ = nz * frequency;
            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
            noiseHeight += perlinValue * amplitude;

            totalAmplitude += amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        if (totalAmplitude > 0f) noiseHeight /= totalAmplitude;
        return noiseHeight;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }
}