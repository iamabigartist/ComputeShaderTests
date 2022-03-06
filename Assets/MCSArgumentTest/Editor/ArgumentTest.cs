using UnityEditor;

using UnityEngine;

[ExecuteInEditMode]
public class ArgumentTest : MonoBehaviour
{
	public int array_l;

	public float[] result;
	private ComputeShader arg_shader;
	private ComputeBuffer gv_buffer;
	private ComputeBuffer result1_buffer;

	// Start is called before the first frame update
	private void OnEnable ()
	{
		this.arg_shader =
			AssetDatabase.LoadAssetAtPath<ComputeShader>(
				"Assets/MCSArgumentTest/ArgumentShader.compute" );
		this.gv_buffer = new ComputeBuffer( 1, sizeof(uint) );

		TestOnce();
	}

	private void TestOnce ()
	{
		this.result = new float[this.array_l];
		this.result1_buffer = new ComputeBuffer( this.array_l, sizeof(float) );
		this.result1_buffer.SetData( this.result );
		this.gv_buffer.SetData( new uint[] { 1 } );
		this.arg_shader.SetBuffer( 0, "gv", this.gv_buffer );
		this.arg_shader.SetBuffer( 0, "result1", this.result1_buffer );
		this.arg_shader.Dispatch( 0, this.array_l - 1, 1, 1 );
		this.result1_buffer.GetData( this.result );
		this.result1_buffer.Release();


	}
}