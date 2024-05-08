using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingParticleArrayTextureController : MonoBehaviour {

    public Transform[] affectors;

    private Vector4[] positions;
    private ParticleSystemRenderer psr;
    private Texture2D tex;
    private int tempLength;
    private Color col;

	void Start () {
        psr = GetComponent<ParticleSystemRenderer>();
        CreateTexture();
        psr.material.SetTexture("_Affectors", tex);
    }

    // Sending an array of positions to particle shader
    void Update () {
        for (int i = 0; i < affectors.Length; i++)
        {
            col.r = affectors[i].position.x;
            col.g = affectors[i].position.y;
            col.b = affectors[i].position.z;
            col.a = 1f;
            tex.SetPixel(i,0,col);
        }
        tex.Apply();

        psr.material.SetInt("_AffectorCount", affectors.Length);
        psr.material.SetTexture("_Affectors", tex);

        if (affectors.Length != tempLength)
        {
            CreateTexture();
        }
        tempLength = affectors.Length;
    }

    private void CreateTexture()
    {
        tex = new Texture2D(affectors.Length, 1, TextureFormat.RGBAFloat,false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        //tex.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
    }
}
