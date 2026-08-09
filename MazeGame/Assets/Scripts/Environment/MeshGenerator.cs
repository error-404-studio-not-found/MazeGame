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

    [Header("Edge Divots (circular bays at center-left and center-right)")]
    [Tooltip("Carves an organic, circular divot into the terrain at the vertical center of the left edge and the right edge.")]
    public bool useEdgeDivots = true;
    [Tooltip("Radius, in grid units, of the fully-carved (height 0) core of each divot.")]
    public float divotRadius = 6f;
    [Tooltip("Extra grid units beyond divotRadius over which the divot blends back up to full height.")]
    public float divotBlend = 4f;
    [Tooltip("How far (in grid units) the divot's rim wanders from a perfect circle, for an organic edge.")]
    public float divotNoiseStrength = 2f;
    [Tooltip("Scale of the noise driving that wander. Smaller = broader, slower ripples around the rim.")]
    public float divotNoiseScale = 0.15f;

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

                if (useEdgeDivots)
                {
                    y *= EdgeDivotMask(x, z);
                }

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

    // 0 inside 'divotRadius' of either the left-edge or right-edge center
    // point, blending up to 1 over the next 'divotBlend' units. A noise
    // sample perturbs the distance itself so each rim reads as an organic,
    // hand-carved bay rather than a perfect circle.
    float EdgeDivotMask(int x, int z)
    {
        float centerZ = zSize / 2f;

        float wobble = (Mathf.PerlinNoise(x * divotNoiseScale + seed, z * divotNoiseScale + seed) - 0.5f) * 2f * divotNoiseStrength;

        float distLeft = Vector2.Distance(new Vector2(x, z), new Vector2(0f, centerZ)) + wobble;
        float distRight = Vector2.Distance(new Vector2(x, z), new Vector2(xSize, centerZ)) + wobble;

        float dist = Mathf.Min(distLeft, distRight);

        float t = Mathf.Clamp01((dist - divotRadius) / Mathf.Max(divotBlend, 0.0001f));
        return t * t * (3f - 2f * t); // smoothstep for an eased, organic blend
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
