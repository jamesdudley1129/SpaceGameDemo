using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapBoundaryBeh : MonoBehaviour
{
    MeshFilter the_meshFilter;
    Mesh the_mesh;
    public Material the_material;
    float y_offset = 0;
    float next_UpdatedTime = 0f;
    public float OffsetSpeedDevisor;
    Collider the_colider;
    public WorldTracker worldTracker;
    private void Start()
    {
        the_meshFilter = GetComponent<MeshFilter>();
        the_mesh = the_meshFilter.mesh;
        //the_material = GetComponent<Material>();
        the_colider = GetComponent<Collider>();
        worldTracker = transform.parent.GetComponent<WorldTracker>();
        /*
        Vector3[] vertice = the_mesh.vertices;
        Vector3[] normals = the_mesh.vertices;
        List<Vector3> newVertice = new List<Vector3>();
        List<Vector3> newNormals = new List<Vector3>();
        Color[] colors = the_mesh.colors;
        List<Color> newColors = new List<Color>();
        foreach (Vector3 vert in vertice)
        {
            Vector3 opposite = -vert;
            newVertice.Add(opposite);
        }
        foreach (Vector3 norm in normals)
        {
            Vector3 opposite = -norm;
            newNormals.Add(opposite);
        }
        foreach (Color Oldcolor in colors)
        {
            Color color = new Color(0,0,1f,0.5f);
            newColors.Add(color);
        }         

        the_mesh.SetVertices(newVertice);
        the_mesh.RecalculateBounds();
        */
    }
    
    private void Update()
    {
        if (Time.time >= next_UpdatedTime)
        {
            if (y_offset <= 1f)
            {
                next_UpdatedTime = Time.time + OffsetSpeedDevisor;
                the_material.mainTextureOffset = new Vector2(0.5f, y_offset);
                y_offset += 0.001f;
            }
            else
            {
                y_offset = 0f;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (worldTracker.player == other.gameObject || worldTracker.OtherPlayer == other.gameObject || worldTracker.world.Contains(other.gameObject))
            {
                other.gameObject.GetComponent<GameManagerRefrence>().DestroyObject();
            }
        }
    }
}
