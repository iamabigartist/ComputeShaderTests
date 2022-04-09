using UnityEditor;
using UnityEngine;
namespace MIncrementBufferTest
{

    public class IncrementBufferWindow : EditorWindow
    {
        [MenuItem( "Labs/IncrementBufferTest/IncrementBufferWindow" )]
        static void ShowWindow()
        {
            var window = GetWindow<IncrementBufferWindow>();
            window.titleContent = new("IncrementBufferWindow");
            window.Show();
        }

        ComputeShader cs;

        (
            int array_length,
            int result,
            int dict) ids;

        (
            int array_length,
            ComputeBuffer result,
            ComputeBuffer dict) args;


        void Awake()
        {
            cs = Resources.Load<ComputeShader>( "IncrementArrayCS" );
            var monster = Resources.Load<GameObject>( "MyCreatures/Monster" );
            ids = (
                Shader.PropertyToID( "array_length" ),
                Shader.PropertyToID( "result" ),
                Shader.PropertyToID( "dict" ));
            args = (
                10,
                new(100, sizeof(int), ComputeBufferType.Counter),
                new(100, sizeof(int), ComputeBufferType.Counter));



            result = new int[100];
            dict = new int[100];

            cs.SetInt( ids.array_length, args.array_length );
            cs.SetBuffer( 0, ids.result, args.result );
            cs.SetBuffer( 0, ids.dict, args.dict );

        }

        int[] result;
        int[] dict;


        void OnInspectorUpdate()
        {
            args.dict.SetCounterValue( 0 );
            args.result.SetCounterValue( 0 );
            cs.Dispatch( 0, 1, 1, 1 );
            args.result.GetData( result );
            args.dict.GetData( dict );

            Repaint();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField( $"result: {string.Join( ",", result )}" );
            EditorGUILayout.LabelField( $"dict: {string.Join( ",", dict )}" );
        }

        void OnDestroy()
        {
            args.dict.Release();
            args.result.Release();
        }
    }
}
