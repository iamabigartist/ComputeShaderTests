using UnityEditor;
using UnityEngine;
namespace MMultiDispatchTest
{
    public class MultiDispatchTestWindow : EditorWindow
    {
        [MenuItem( "Labs/MMultiDispatchTest.Editor/MultiDispatchTestWindow" )]
        static void ShowWindow()
        {
            var window = GetWindow<MultiDispatchTestWindow>();
            window.titleContent = new GUIContent( "MultiDispatchTestWindow" );
            window.Show();
        }

//第一个发送 读取线程坐标，乘1.1 100次写入一个Buffer
//第二个发送使用那个Buffer,除 1.1 100次写入最终的Buffer
//显示的时候显示中间Buffer和最终Buffer
        ComputeShader cs;
        ComputeBuffer temporary_b;
        ComputeBuffer final_b;
        float[] temporary_a;
        float[] final_a;

        void Awake()
        {
            cs = Resources.Load<ComputeShader>( "MultiKernelDependencyCS" );
            temporary_b = new ComputeBuffer( 100, sizeof(float) );
            final_b = new ComputeBuffer( 100, sizeof(float) );
            temporary_a = new float[100];
            final_a = new float[100];
            cs.SetBuffer( cs.FindKernel( "Pass1" ), nameof(temporary_b), temporary_b );
            cs.SetBuffer( cs.FindKernel( "Pass2" ), nameof(temporary_b), temporary_b );
            cs.SetBuffer( cs.FindKernel( "Pass2" ), nameof(final_b), final_b );

            cs.Dispatch( cs.FindKernel( "Pass1" ), Mathf.CeilToInt( 100 / 16f ), 1, 1 );
            cs.Dispatch( cs.FindKernel( "Pass2" ), Mathf.CeilToInt( 100 / 16f ), 1, 1 );


            final_b.GetData( final_a );

            temporary_b.GetData( temporary_a );

        }

        void OnGUI()
        {
            EditorStyles.label.wordWrap = true;
            EditorGUILayout.LabelField( $"temp: {string.Join( ",", temporary_a )}" );
            EditorGUILayout.LabelField( $"final: {string.Join( ",", final_a )}" );
        }

    }
}
