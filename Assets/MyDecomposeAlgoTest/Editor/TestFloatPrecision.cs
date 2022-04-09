using UnityEditor;
using UnityEngine;
namespace MyDecomposeAlgoTest
{
    public class TestFloatPrecision : EditorWindow
    {
        [MenuItem( "Labs/MyDecomposeAlgoTest/TestFloatPrecision" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestFloatPrecision>();
            window.titleContent = new("TestFloatPrecision");
            window.Show();
        }


        float new_data;
        float cur_data;

        void OnEnable() { }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            new_data = EditorGUILayout.FloatField( "Choose a new data", new_data );

            if (GUILayout.Button( "Set" ))
            {
                cur_data = new_data;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField( $"cur_data: {cur_data}" );

        }

        void Update()
        {
            cur_data /= 99;
            cur_data *= 99;
        }


    }
}
