using System.Diagnostics;
using PrototypeUtils;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
namespace MyDecomposeAlgoTest
{
    public class TestDecomposeEfficiency : EditorWindow
    {
        [MenuItem( "Labs/MyDecomposeAlgoTest/TestDecomposeEfficiency" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestDecomposeEfficiency>();
            window.titleContent = new("TestDecomposeEfficiency");
            window.Show();
        }


        bool stop;
        const int GROUP_SIZE_X = 1024;
        int group_number;
        ComputeShader c_shader;

        ComputeBuffer getter_buffer;
        int[] getter;
        Stopwatch stopwatch;
        int run_times;

        ComputeBuffer result_buffer;
        int4[] result;
        string result_string;


        void ReStart()
        {
            int work_count = group_number * GROUP_SIZE_X;
            result_buffer = new(work_count, sizeof(int) * 4);
            c_shader.SetBuffer( 0, "result", result_buffer );
            result = new int4[work_count];
            run_times = 0;
            stopwatch.Reset();
            stop = false;
        }

        void Stop()
        {
            result_buffer.GetData( result );
            result_string = $"Result: {string.Join( ",", result[..3000] )}";
            result_buffer.Release();
            stop = true;
        }

        void RunOnce()
        {
            stopwatch.Start();
            c_shader.Dispatch( 0, group_number, 1, 1 );
            getter_buffer.GetData( getter );
            stopwatch.Stop();
            run_times++;
        }

        void OnEnable()
        {
            c_shader = Resources.Load<ComputeShader>( "DecomposeAlgo" );
            stopwatch = new();
            stop = true;
            getter_buffer = new(1, sizeof(int));
            c_shader.SetBuffer( 0, "getter", getter_buffer );
            getter = new int[1];
            getter_buffer.SetData( getter );
        }

        Vector2 view_position;
        void OnGUI()
        {
            view_position = GUILayout.BeginScrollView( view_position );

            if (stop)
            {
                EditorGUILayout.BeginHorizontal();

                group_number = EditorGUILayout.IntField( nameof(group_number), group_number );

                if (GUILayout.Button( "Start" ))
                {
                    ReStart();
                }

                EditorGUILayout.EndHorizontal();

                if (result != null)
                {
                    GUILayout.Box( result_string );
                }

            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField( $"{nameof(group_number)}: {group_number}" );

                if (GUILayout.Button( "Stop" ))
                {
                    Stop();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField( $"Average Time: {stopwatch.Get_ms() / run_times} ms" );

            }

            GUILayout.EndScrollView();

        }

        void Update()
        {
            if (!stop)
            {
                RunOnce();
            }
        }

        void OnDisable()
        {
            getter_buffer.Release();
        }

        void OnInspectorUpdate()
        {
            if (!stop)
            {
                Repaint();
            }
        }
    }
}
