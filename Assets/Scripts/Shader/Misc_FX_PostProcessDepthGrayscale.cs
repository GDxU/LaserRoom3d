using UnityEngine;
using System.Collections;

//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
public class Misc_FX_PostProcessDepthGrayscale : MonoBehaviour
{
    public float sampleDist = 1.0f;

    public Material mat;
    void Start()
    {
        Camera.main.depthTextureMode |= DepthTextureMode.DepthNormals;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (CheckResources() == false)
        {
           // Graphics.Blit(source, destination);
            //return;
        }
        mat.SetFloat("_SampleDistance", sampleDist);
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to
    }
}