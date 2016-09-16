using UnityEngine;
using System.Collections;

public class HexInfo : MonoBehaviour {
    public Vector3[] Vertices;
    public Vector2[] uv;
    public int[] Triangles;
    public Texture texture;
    public Renderer rend;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        MeshSetup();
    }
	
	void Update ()
    {
	
	}

    void MeshSetup()
    {
        

        float floorLevel = 0;
        Vertices = new Vector3[]
        {
            new Vector3(-1f, floorLevel, -.5f),
            new Vector3(-1f, floorLevel, .5f),
            new Vector3(0f, floorLevel, 1f),
            new Vector3(1f, floorLevel, .5f),
            new Vector3(1f, floorLevel, -.5f),
            new Vector3(0f, floorLevel, -1f)
        };

        Triangles = new int[]
        {
            1,5,0,
            1,4,5,
            1,2,4,
            2,3,4
        };
        uv = new Vector2[]
        {
            new Vector2(0,0.25f),
            new Vector2(0,0.75f),
            new Vector2(0.5f,1),
            new Vector2(1,0.75f),
            new Vector2(1,0.25f),
            new Vector2(0.5f,0)
        };

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        rend.material.mainTexture = texture;
    }
}
