using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    Mesh mesh;
    
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 200;
    public int zSize = 200;
    
    public float layerA_detail = 0.01f;
    public float layerA_attenuation = 50f;
    public float layerB_detail = 0.3f;
    public float layerB_attenuation = 2f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateShape();
        UpdateMesh();
    }
    
    float perlinLayer(int x, int z, float detail, float attenuation)
    {
        return Mathf.PerlinNoise(x * detail, z * detail) * attenuation;
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++) 
        {
            for (int x = 0; x <= xSize; x++) 
            {
                float y = perlinLayer(x, z, layerA_detail, layerA_attenuation)
                    + perlinLayer(x, z, layerB_detail, layerB_attenuation);

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        
        triangles = new int[xSize * zSize *  6];
        
        int vert = 0;
        int tris = 0;
        
        for (int z = 0; z < zSize; ++z)
        {
            for (int x = 0; x < xSize; ++x)
            {
                triangles[tris + 0] = vert;
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
    
    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }
}
