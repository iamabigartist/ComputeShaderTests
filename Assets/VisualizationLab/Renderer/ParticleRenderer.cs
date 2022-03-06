using UnityEngine;
/// <summary>
///     Use a particle shader to draw the given particle list <see cref="_particle_buffer" />
/// </summary>
public class ParticleRenderer
{
#region Shader

    readonly Material _cloud_renderer;
    /// <summary>
    ///     The material of the rendering shader
    /// </summary>
    public Material CloudRenderer => this._cloud_renderer;

    const string shader_name = "VertexToSquare_Cloud";
    const string main_color_name = "main_color";
    const string buffer_name = "buffer";
    const string screen_ratio_name = "screen_ratio";

    int BufferID;
    int MainColorID;
    int ScreenRatioID;

#endregion

#region Config

    ComputeBuffer _particle_buffer;
    Color _main_color;

#endregion Config

#region Interface

    public ParticleRenderer()
    {
        this._cloud_renderer = new Material(Shader.Find(shader_name));
        this.BufferID = Shader.PropertyToID(buffer_name);
        this.MainColorID = Shader.PropertyToID(main_color_name);
        this.ScreenRatioID = Shader.PropertyToID(screen_ratio_name);
        On(null, Color.white, new Vector2(Screen.width, Screen.height));
    }

    /// <summary>
    ///     Switch on the renderer using a particle buffer and a particle color
    /// </summary>
    /// <param name="particle_buffer">This should be a buffer of float3, which stores the positions of particles.</param>
    /// <param name="main_color">The color of particle</param>
    /// <param name="screen_size"></param>
    /// <remarks>Please init before using it!</remarks>
    public void On(ComputeBuffer particle_buffer, Color main_color, Vector2 screen_size)
    {
        this._particle_buffer = particle_buffer;

        Config(main_color, screen_size);
    }

    public void Config(Color main_color, Vector2 screen_size)
    {
        this._main_color = main_color;


    }

    public void Off()
    {
        this._particle_buffer = null;
    }

    /// <summary>
    ///     Draw the particle cloud once.
    /// </summary>
    public void Draw()
    {
        if (this._particle_buffer == null) return;

        this._cloud_renderer.SetBuffer(this.BufferID, this._particle_buffer);
        this._cloud_renderer.SetColor(this.MainColorID, this._main_color);
        //The shader variable need to be set every frame.
        float screen_ratio = (float)Screen.height / Screen.width;
        this._cloud_renderer.SetFloat(this.ScreenRatioID, screen_ratio);
        this._cloud_renderer.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, this._particle_buffer.count);
    }

#endregion Interface

}