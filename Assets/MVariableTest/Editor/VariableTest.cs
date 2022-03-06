using UnityEditor;
using UnityEngine;
public class VariableTest : EditorWindow
{
    [MenuItem("Tests/VariableTest")]
    private static void Init()
    {
        var w = GetWindow<VariableTest>();
        w.Show();
    }

    private readonly Vector2 pos1 = Vector2.zero;

    private void OnGUI()
    {
        if (GUILayout.Button("Init")) OnInit();
        this.ArrayLen = EditorGUILayout.IntField("ArrayLen", this.ArrayLen);

        if (this.inited)
        {
            if (GUILayout.Button("test")) TestOnce();
            using (var s = new EditorGUILayout.ScrollViewScope(this.pos1, true, true))
            {
                EditorGUILayout.SelectableLabel(ArrayString(this.array1));
            }
        }


    }



    private bool inited = false;
    private void OnInit()
    {

        this.variable_test_shader = Resources.Load<ComputeShader>("VariableTest");

        this.array1_buffer = new ComputeBuffer(this.ArrayLen, sizeof(int));
        this.array1 = new int[this.ArrayLen];
        this.inited = true;

    }

    public int ArrayLen;

    [SerializeField] private ComputeShader variable_test_shader;

    private ComputeBuffer array1_buffer;
    private int[] array1;

    private void DispatchShader(
        in ComputeShader shader,
        int kernel,
        Vector3 data_size,
        Vector3 group_size)
    {
        shader.Dispatch(kernel,
            Mathf.CeilToInt(data_size.x / group_size.x),
            Mathf.CeilToInt(data_size.y / group_size.y),
            Mathf.CeilToInt(data_size.z / group_size.z));
    }

    private string ArrayString(int[] array)
    {
        return $"{array.Length}: ({string.Join(",", array)})";
    }

    private void TestOnce()
    {
        this.variable_test_shader.SetInt("count", this.ArrayLen);
        this.variable_test_shader.SetBuffer(0, "array", this.array1_buffer);
        DispatchShader(this.variable_test_shader, 0,
            new Vector3(this.ArrayLen, 1, 1),
            new Vector3(256, 1, 1));
        this.array1_buffer.GetData(this.array1);
    }
}