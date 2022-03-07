using UnityEditor;
namespace MyDecomposeAlgoTest
{
    public class TestValueRange : EditorWindow
    {
        [MenuItem( "Labs/MyDecomposeAlgoTest/TestValueRange" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestValueRange>();
            window.titleContent = new("TestValueRange");
            window.Show();
        }

        void OnEnable() { }

        void OnGUI()
        {
            EditorGUILayout.FloatField( "FloatMaxValue", float.MaxValue );
            EditorGUILayout.FloatField( "FloatMinValue", float.MinValue );
            EditorGUILayout.IntField( "IntMaxValue", int.MaxValue );
            EditorGUILayout.IntField( "IntMinValue", int.MinValue );

            EditorGUILayout.FloatField( "IntMaxToFloat", int.MaxValue );
            EditorGUILayout.DoubleField( "IntMaxMaxToDouble", int.MaxValue );

            EditorGUILayout.IntField( "DoubleToInt", (int)1.9234 );


            EditorGUILayout.IntField( "Int^3", 999 * 999 * 999 );
            EditorGUILayout.FloatField( "Int^3ToFloat", 999 * 999 * 999 );
            EditorGUILayout.DoubleField( "Int^3ToDouble", 999 * 999 * 999 );
        }
    }
}
