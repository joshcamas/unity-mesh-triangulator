using UnityEngine;
using System.Text;
using System.Collections;

/// <summary>
/// Static class which emulates a mesh explosion into triangles
/// 
/// @autor Alex M.A.
/// </summary>
public class MeshTriangulator
{
    //number of the seconds which the triangles will be destroyed
    private static float destroySeconds = 5f;
    //force applyed to triangles emulating an explosion
    private static float explosionForce = 1500f;
   
    //mesh vars
    private static Transform _meshTransform;
    private static MeshFilter _meshFilter;
    private static MeshRenderer _meshRenderer;
    private static Mesh _mesh;
    private static Vector3[] _v3Verts;
    private static Vector3[] _v3Normals;
    private static Vector2[] _v3Uvs;

    //tris vars
    private static int _intNumberOfTriangles;
    private static int[] _intTriangles;
    private static Vector3[] _v3TriVertexs;
    private static Vector3[] _v3TriNormals;
    private static Vector2[] _v3TriUvs;
    private static Mesh _mTriMesh;
    private static GameObject _goNewTriangle;

    /// <summary>
    /// Splits the mesh of the given transform into triangles deactivating
    /// its gameobject and creating multiple triangles simulating and explosion
    /// </summary>
    /// <param name="_objTransform"></param>
    public static void Triangulate(Transform _objTransform)
    {
        //getting info about the mesh of the given transform
        _meshTransform = _objTransform;
        _meshFilter = _meshTransform.GetComponent<MeshFilter>();
        _meshRenderer = _meshTransform.GetComponent<MeshRenderer>();
        _mesh = _meshFilter.mesh;
        _v3Verts = _mesh.vertices;
        _v3Normals = _mesh.normals;
        _v3Uvs = _mesh.uv;
     
        //deactivating the gameobject which is going to be splitted
        _meshTransform.gameObject.SetActive(false);
       
        //looping the submeshes of the main mesh
        for (int intSubmeshIndex = 0, smCount = _mesh.subMeshCount; intSubmeshIndex < smCount; intSubmeshIndex++)
        {
            //getting the triangles of the submesh
            _intTriangles = _mesh.GetTriangles(intSubmeshIndex);
            //getting the number of triangles of the submesh
            _intNumberOfTriangles = _intTriangles.Length; 
            //looping the triangles and creating the corresponding triangles
            for (int i = 0; i < _intNumberOfTriangles; i += 3)
            {
                //init the triangles vars
                _v3TriVertexs = new Vector3[3];
                _v3TriNormals = new Vector3[3];
                _v3TriUvs = new Vector2[3];

                //getting the vertexs, uvs and normals from the main mesh
                for (int n = 0; n < 3; n++)
                {
                    int index = _intTriangles[i + n];
                    _v3TriVertexs[n] = _v3Verts[index];
                    _v3TriUvs[n] = _v3Uvs[index];
                    _v3TriNormals[n] = _v3Normals[index];
                }

                //Configuring the new triangle with the info of the main mesh
                _mTriMesh = new Mesh();
                _mTriMesh.vertices = _v3TriVertexs;
                _mTriMesh.normals = _v3TriNormals;
                _mTriMesh.uv = _v3TriUvs;
                _mTriMesh.triangles = new int[] { 0, 1, 2, 2, 1, 0};

                //Creating the new triangle game object
                _goNewTriangle = new GameObject( new StringBuilder().Append("Tiangle").Append((i / 3) + intSubmeshIndex).ToString() );
                //positioning the triangle at the same position of the original mesh
                _goNewTriangle.transform.position = _meshTransform.position;
                _goNewTriangle.transform.rotation = _meshTransform.rotation;
                _goNewTriangle.transform.localScale = _meshTransform.localScale;
                //adding meshrenderer and the material of the main mesh
                _goNewTriangle.AddComponent<MeshRenderer>().material = _meshRenderer.materials[intSubmeshIndex];
                //adding the triange mesh
                _goNewTriangle.AddComponent<MeshFilter>().mesh = _mTriMesh;
                //adding collider
                _goNewTriangle.AddComponent<BoxCollider>();
                //adding rigidbody and a explosion
                _goNewTriangle.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(50f,explosionForce), _meshTransform.position, 30);

                //to make clean the scene: destroy all the triangles when passed the destroySeconds
                Object.Destroy(_goNewTriangle, destroySeconds);
            }
        }
    }
   
}
