using UnityEngine;
public class CosSinWave : MonoBehaviour
{

#region Shader

    const string compute_shader_name = "CosSinWave(x=(x,y,z))";
    const string len_name = "array_len";
    const string time_name = "time";
    const string input_buffer_name = "matrix_x";
    const string particle_buffer_name = "matrix_f_xyz";
    const string origin_name = "origin";
    ComputeShader compute_shader;
    ComputeBuffer input_buffer;
    ComputeBuffer particle_buffer;

    const float numthreads_x = 1024;
    int threadgroup_x => Mathf.CeilToInt(this.particle_count / numthreads_x);


    ParticleRenderer particle_renderer;

#endregion

#region Config

    [SerializeField]
    int particle_count = 10000;
    public Color main_color = (Color.magenta + 2 * Color.black) / 3;
    public float step = 0.01f;

#endregion

#region Operation

    void InitShaders()
    {
    #region Compute

        this.compute_shader =
            Resources.Load<ComputeShader>(compute_shader_name);

        this.input_buffer =
            new ComputeBuffer(
                this.particle_count,
                sizeof(float));
        this.particle_buffer =
            new ComputeBuffer(
                this.particle_count,
                3 * sizeof(float));

        //Init the input buffer
        var array = new float[this.particle_count];
        for (int i = 0; i < this.particle_count; i++) array[i] = this.step * i;
        this.input_buffer.SetData(array);

    #endregion

    #region Render

        this.particle_renderer = new ParticleRenderer();

    #endregion

    }

    void BindShaders()
    {

    #region Compute

        this.compute_shader.SetInt(len_name, this.particle_count);
        this.compute_shader.SetBuffer(0, input_buffer_name, this.input_buffer);
        this.compute_shader.SetBuffer(0, particle_buffer_name, this.particle_buffer);

    #endregion

    #region Render

        this.particle_renderer.On(this.particle_buffer, this.main_color, new Vector2(Screen.width, Screen.height));

    #endregion

    }

#endregion

#region UnityEvent

    void Start()
    {
        InitShaders();
        BindShaders();
    }

    // Do computation in the Update
    void Update()
    {
        var _ = transform.position;
        this.compute_shader.SetFloats(origin_name, _.x, _.y, _.z);
        this.compute_shader.SetFloat(time_name, Time.time);
        this.compute_shader.Dispatch(0, threadgroup_x, 1, 1);
    }

    //Do Rendering in the OnRenderObject
    void OnRenderObject()
    {
        this.particle_renderer.Draw();
    }

    void OnDestroy()
    {
        this.input_buffer.Release();
        this.particle_buffer.Release();
    }

#endregion

}