using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class MLGEffect : MonoBehaviour
{
    public float intensity;
    public Material material;
    // Creates a private material used to the effect
    void Awake()
    {
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
        return;
        if (intensity == 0)
        {
        }
        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}