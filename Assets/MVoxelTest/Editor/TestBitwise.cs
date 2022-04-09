using System;
using UnityEditor;
using UnityEngine;
namespace MVoxelTest.Editor
{
    public class TestBitwise : EditorWindow
    {
        [MenuItem( "Labs/MVoxelTest.Editor/TestBitwise" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestBitwise>();
            window.titleContent = new GUIContent( "TestBitwise" );
            window.Show();
        }


        ComputeShader m_shader;
        ComputeBuffer m_buffer;
        uint[] result;
        void OnEnable()
        {
            m_shader = Resources.Load<ComputeShader>( "BitwiseCS" );
            m_buffer = new ComputeBuffer( 2, sizeof(uint) );
            result = new uint[2];
            m_shader.SetBuffer( 0, "Result", m_buffer );
            m_shader.Dispatch( 0, 1, 1, 1 );
            m_buffer.GetData( result );
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField( $"source: {Convert.ToString( result[0], 2 )}" );
            EditorGUILayout.LabelField( $"result: {Convert.ToString( result[1], 2 )}" );
        }

        void OnDisable()
        {
            m_buffer.Release();
        }
    }
}
