using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class SumReductionCompare : MonoBehaviour
{
    public int array_length;
    [SerializeField] private float[] array;

    private ComputeShader half_reduce;
    private ComputeBuffer half_reduce_result;
    private ComputeBuffer half_reduce_gv;

    [SerializeField] private float[] result;

    private struct HalfReduceGV
    {
        public uint length;
        public uint length_half;
        public bool stop_flag;

        public int Sizeof ()
        {
            return 2 * sizeof( uint ) + sizeof( bool );
        }

        public HalfReduceGV ( uint length , uint length_half , bool stop_flag )
        {
            this.length = length;
            this.length_half = length_half;
            this.stop_flag = stop_flag;
        }
    }

    private void Start ()
    {
        array = new float[array_length]; InitArray1(); result = new float[array_length];
        half_reduce = Resources.Load<ComputeShader>( "SumHalfReduction" );
        half_reduce_result = new ComputeBuffer( array_length , sizeof( float ) ); half_reduce_result.SetData( array );
        half_reduce_gv = new ComputeBuffer( 3 , sizeof( uint ) ); half_reduce_gv.SetData( new uint[3] { (uint)array_length , 0 , 0 } );

        half_reduce.SetBuffer( 0 , "result" , half_reduce_result );
        half_reduce.SetBuffer( 0 , "gv" , half_reduce_gv );
        half_reduce.SetInt( "control_num_thread" , array_length + 1 );

        print( $"SumCPUSerial: {SumCPUSerial()}" );
        print( $"SumGPUHalfReduction: {SumGPUHalfReduction()}" );
        SumCPUHalfReduction();
        half_reduce_result.Release();
        half_reduce_gv.Release();
    }

    private void InitArray1 ()
    {
        for (int i = 0; i < array_length; i++)
        {
            array[i] = Random.Range( -10.0f , 10 );
        }
    }

    private void Compare1 ()
    {
    }

    private float SumCPUSerial ()
    {
        float sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }

    private float SumGPUHalfReduction ()
    {
        half_reduce.Dispatch( 0 , Mathf.CeilToInt( array_length / 256.0f ) , 1 , 1 );
        half_reduce_result.GetData( result );
        return result[0];
    }

    private void SumCPUHalfReduction ()
    {
        int length = array_length;
        int length_half = Mathf.CeilToInt( length / 2.0f );
        while (length != 1)
        {
            for (int i = length_half; i < length; i++)
            {
                array[i - length_half] += array[i];
            }
            length = length_half;
            length_half = Mathf.CeilToInt( length / 2.0f );
        }
    }
}