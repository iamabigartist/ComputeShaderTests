using System.Diagnostics;
using System.Linq;
using PrototypeUtils;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
namespace MyCShaderManipulateMeshTest
{
    public class MeshBufferViewer : MonoBehaviour
    {
        ComputeShader validate_cs;
        Mesh m_mesh;
        public int m_quad_count = 100;
        const MeshUpdateFlags FAST_SET_FLAG =
            MeshUpdateFlags.DontValidateIndices |
            MeshUpdateFlags.DontNotifyMeshUsers |
            MeshUpdateFlags.DontRecalculateBounds;

        int[] local_index_buffer = { 0, 1, 2, 2, 3, 0 };
        int[] GenTriIndices(int quad_count)
        {
            var result_index_buffer = new int[6 * quad_count];

            for (int quad_i = 0; quad_i < quad_count; quad_i++)
            {
                for (int local_v_i = 0; local_v_i < 6; local_v_i++)
                {
                    result_index_buffer[quad_i * 6 + local_v_i] = local_index_buffer[local_v_i] + quad_i * 4;
                }
            }

            return result_index_buffer;
        }

        Stopwatch stopwatch;

        [ContextMenu( "RunGenMesh" )]
        void RunGenMesh()
        {
            int index_count = m_quad_count * 6;
            int vertex_count = m_quad_count * 4;
            m_mesh = new();

            stopwatch.Restart();
            m_mesh.SetIndexBufferParams( index_count, IndexFormat.UInt32 );
            m_mesh.SetIndexBufferData( GenTriIndices( m_quad_count ), 0, 0, index_count, FAST_SET_FLAG );
            m_mesh.SetSubMesh( 0, new(0, index_count), FAST_SET_FLAG );
            stopwatch.Stop();
            Debug.Log( $"{stopwatch.Get_ms()} ms" );

            var vertices = new Vector3[vertex_count];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Random.rotation * Vector3.forward;
            }

            m_mesh.SetVertices( vertices, 0, vertex_count );
            m_mesh.RecalculateNormals();
            m_mesh.RecalculateBounds();
            m_mesh.indexBufferTarget |= GraphicsBuffer.Target.Structured;
            GetComponent<MeshFilter>().mesh = m_mesh;
        }

        public int cut_to_quad_count = 200;
        [ContextMenu( "CutMeshIndex" )]
        void CutMeshIndex()
        {
            // int index_count = m_quad_count * 6;
            int cut_to_index_count = cut_to_quad_count * 6;
            m_mesh.SetIndexBufferParams( cut_to_index_count, IndexFormat.UInt32 );
            m_mesh.SetSubMesh( 0, new(0, cut_to_index_count), FAST_SET_FLAG );
        }

        [ContextMenu( "ViewIndexBuffer" )]
        void ViewIndexBuffer()
        {
            m_mesh.indexBufferTarget |= GraphicsBuffer.Target.Structured;
            var index_buffer = m_mesh.GetIndexBuffer();
            Debug.Log( $"index count: {index_buffer.count}, buffer_target: {index_buffer.target}, mesh_index_buffer_target: {m_mesh.indexBufferTarget}" );
            index_buffer.Release();
        }


        [ContextMenu( "CutAndValidateIndex" )]
        void CutAndValidateIndex()
        {
            int cut_to_index_count = cut_to_quad_count * 6;
            m_mesh.SetIndexBufferParams( cut_to_index_count, IndexFormat.UInt32 );
            m_mesh.SetSubMesh( 0, new(0, cut_to_index_count), FAST_SET_FLAG );
            var index_buffer = m_mesh.GetIndexBuffer();
            validate_cs.SetBuffer( 0, "indices", index_buffer );
            validate_cs.SetInt( "position", cut_to_index_count - 1 );
            validate_cs.Dispatch( 0, 1, 1, 1 );
            var result = new int[cut_to_index_count];
            index_buffer.GetData( result );
            index_buffer.Release();
            Debug.Log( result.Last() );
            // var a = m_mesh.GetIndexBuffer().LockBufferForWrite<int>( 0, 1 );
            // a[0] = 1;
            // index_buffer.UnlockBufferAfterWrite<int>( 1 );
        }

        void Start()
        {
            stopwatch = new();
            validate_cs = Resources.Load<ComputeShader>( "ValidateBufferLength" );
        }


    }
}
