using UnityEngine;
using System.Text;
using System.Collections;

/// <summary>
/// Static class which emulates a mesh explosion into triangles
/// 
/// @author Alex M.A. and joshcamas
/// </summary>
public class MeshTriangulator
{

    /// <summary>
    /// Splits the mesh of the given transform into triangles deactivating
    /// its gameobject and creating multiple triangles simulating and explosion
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="explosionForce">The force applied to the generated triangles</param>
    /// <param name="destroySeconds">How long the generated triangles will live</param>
    public static void Triangulate(Transform transform, float explosionForce = 1500f, float destroySeconds = 5f)
    {
        //getting info about the mesh of the given transform
        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();

        Triangulate(transform, meshFilter, meshRenderer, explosionForce, destroySeconds);
    }

    /// <summary>
    /// Splits the mesh of the given transform into triangles deactivating
    /// its gameobject and creating multiple triangles simulating and explosion
    /// </summary>
    /// <param name="meshTransform"></param>
    /// <param name="meshFilter"></param>
    /// <param name="meshRenderer"></param>
    /// <param name="explosionForce"></param>
    /// <param name="destroySeconds"></param>
    public static void Triangulate(Transform meshTransform, MeshFilter meshFilter, MeshRenderer meshRenderer,float explosionForce = 1500f, float destroySeconds = 5f)
    {
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        //deactivating the gameobject which is going to be splitted
        meshTransform.gameObject.SetActive(false);

        //looping the submeshes of the main mesh
        for (int intSubmeshIndex = 0, smCount = mesh.subMeshCount; intSubmeshIndex < smCount; intSubmeshIndex++)
        {
            //getting the triangles of the submesh
            int[] triangles = mesh.GetTriangles(intSubmeshIndex);
            //getting the number of triangles of the submesh
            int numberOfTriangles = triangles.Length;
            //looping the triangles and creating the corresponding triangles
            for (int i = 0; i < numberOfTriangles; i += 3)
            {
                //init the triangles vars
                Vector3[] triVertexs = new Vector3[3];
                Vector3[] triNormals = new Vector3[3];
                Vector2[] triUvs = new Vector2[3];

                //getting the vertexs, uvs and normals from the main mesh
                for (int n = 0; n < 3; n++)
                {
                    int index = triangles[i + n];
                    triVertexs[n] = verts[index];

                    //Sometimes UV's don't exit
                    if(uvs.Length > n)
                    triUvs[n] = uvs[index];

                    triNormals[n] = normals[index];
                }

                //Configuring the new triangle with the info of the main mesh
                Mesh triMesh = new Mesh();
                triMesh.vertices = triVertexs;
                triMesh.normals = triNormals;
                triMesh.uv = triUvs;
                triMesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                //Creating the new triangle game object
                GameObject triangleObj = new GameObject(new StringBuilder().Append("Tiangle").Append((i / 3) + intSubmeshIndex).ToString());
                //positioning the triangle at the same position of the original mesh
                triangleObj.transform.position = meshTransform.position;
                triangleObj.transform.rotation = meshTransform.rotation;
                triangleObj.transform.localScale = meshTransform.localScale;
                //adding meshrenderer and the material of the main mesh
                triangleObj.AddComponent<MeshRenderer>().material = meshRenderer.sharedMaterials[intSubmeshIndex];
                //adding the triange mesh
                triangleObj.AddComponent<MeshFilter>().sharedMesh = triMesh;
                //adding collider
                triangleObj.AddComponent<BoxCollider>();
                //adding rigidbody and a explosion
                triangleObj.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(50f, explosionForce), meshTransform.position, 30);

                //to make clean the scene: destroy all the triangles when passed the destroySeconds
                Object.Destroy(triangleObj, destroySeconds);
            }
        }

    }

   
}
