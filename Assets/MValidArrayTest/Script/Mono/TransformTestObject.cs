using UnityEngine;

namespace MValidArrayTest.Script.Mono
{
	public class TransformTestObject : MonoBehaviour
	{
		public int ArrayLen;

		[SerializeField] private ComputeShader transform_test_shader;

		private ComputeBuffer index_list;
		private ComputeBuffer mesh;
		private ComputeBuffer ori_array;
		private ComputeBuffer transit_array;

		private void Start ()
		{
			this.transform_test_shader = Resources.Load<ComputeShader>( "TransformTest" );

			this.ori_array     = new ComputeBuffer( this.ArrayLen, sizeof(float) );
			this.transit_array = new ComputeBuffer( this.ArrayLen, 3 * sizeof(float) + sizeof(int) );
			this.index_list    = new ComputeBuffer( this.ArrayLen, sizeof(int) );
			this.mesh          = new ComputeBuffer( this.ArrayLen, 3 * sizeof(float) );
			TestOnce();

		}

		private void TestOnce ()
		{
			this.transform_test_shader.SetInt( "array_len", 1 );
			// this.transform_test_shader.Dispatch();
		}
	}
}