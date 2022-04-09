using MUtility;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
namespace MyDecomposeAlgoTest
{
    public class TestDecomposePrecision : EditorWindow
    {
        [MenuItem( "Labs/MyDecomposeAlgoTest/TestDecomposePrecision" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestDecomposePrecision>();
            window.titleContent = new("TestDecomposePrecision");
            window.Show();
        }

        const int GROUP_SIZE_X = 1024;
        int group_number;
        ComputeShader shader;
        ComputeBuffer result_buffer;
        ComputeBuffer getter_buffer;
        float3[] result_array;
        float3[] getter_array;
        string result_string;
        string getter_string;

        void OnEnable()
        {
            group_number = 8000;
            int position_count = GROUP_SIZE_X * group_number;
            shader = Resources.Load<ComputeShader>( "FloatDecomposeAlgo" );
            result_buffer = new(position_count, 3 * sizeof(float));
            getter_buffer = new(position_count, 3 * sizeof(float));
            result_array = new float3[position_count];
            getter_array = new float3[position_count];
            shader.SetBuffer( 0, "result", result_buffer );
            shader.SetBuffer( 0, "getter", getter_buffer );
            shader.Dispatch( 0, group_number, 1, 1 );
            result_buffer.GetData( result_array );
            getter_buffer.GetData( getter_array );
            result_string = result_array[^3000..].ToMString( ",\t" );
            getter_string = getter_array[^3000..].ToMString( ",\t" );
        }

        Vector2 view_position;
        void OnGUI()
        {
            view_position = GUILayout.BeginScrollView( view_position );
            GUILayout.Box( "result:\n" + result_string );
            GUILayout.Box( "getter:\n" + getter_string );
            GUILayout.EndScrollView();
        }

        void OnDestroy()
        {
            result_buffer.Release();
            getter_buffer.Release();
        }
    }
}
