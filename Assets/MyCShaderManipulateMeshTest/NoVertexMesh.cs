using MUtility;
using UnityEngine;
using UnityEngine.Rendering;
namespace MyCShaderManipulateMeshTest
{
    [ExecuteInEditMode]
    public class NoVertexMesh : MonoBehaviour
    {

    #region Process

        Vector3[] GenUV()
        {
            var vertices = new Vector3[3 * 1];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Random.rotation * Vector3.forward;
            }

            return vertices;
        }

    #endregion

    #region Entry

    #region Behaviour

        /// <summary>
        ///     Conclusion: UV 是可以用来单独渲染模型的！
        /// </summary>
        [ContextMenu( "GenMesh" )]
        void GenMesh()
        {
            var mesh = new Mesh();
            mesh.SetVertexBufferParams( 3 * 1,
                new VertexAttributeDescriptor( VertexAttribute.TexCoord0 ) );
            mesh.SetVertexBufferData( GenUV(), 0, 0, 3 * 1, flags: MeshUpdateFlags.Default );
            mesh.SetIndices( (..(3 * 1)).ToArray(), MeshTopology.Triangles, 0, false );
            mesh.bounds = new(Vector3.zero, Vector3.one * 3);
            GetComponent<MeshFilter>().mesh = mesh;

        }

    #endregion

    #region Unity

        void Start() { }

    #endregion

    #endregion
    }
}
