using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FormTrapezoid : MonoBehaviour
{
    [SerializeField]private Material material;
    [SerializeField]private int top;
    [SerializeField]private int bot;
    [SerializeField]private int height;
 
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
 
        Vector3[] verts = new Vector3[4];
        int[] trinangles = { 0, 1, 3, 1, 2, 3 };
        //2 3
        //0 1
 
        verts[0] = new Vector3(-top/2, height/2, 1);
        verts[1] = new Vector3(top/2, height/2, 1);
        verts[2] = new Vector3(bot/2, -height/2, 1);
        verts[3] = new Vector3(-bot/2, -height/2, 1);
 
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = trinangles;
        mesh.RecalculateNormals();
 
        mf.sharedMesh = mesh;
        mr.sharedMaterial = material;
    }
}
