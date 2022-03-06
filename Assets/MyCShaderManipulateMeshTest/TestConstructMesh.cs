using System;
using UnityEngine;
namespace MyCShaderManipulateMeshTest
{
    public class TestConstructMesh : MonoBehaviour
    {
        ComputeShader cshader;
        Mesh mesh;
        void Start()
        {
            mesh = GetComponent<Mesh>();
            cshader = Resources.Load<ComputeShader>( "ConstructMesh" );
            mesh.OptimizeReorderVertexBuffer();
        }
    }
}
