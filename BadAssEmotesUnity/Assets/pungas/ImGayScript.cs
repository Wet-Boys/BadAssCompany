using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImGayScript : MonoBehaviour
{
    public Texture2D tex;
    public string path;
    void Start()
    {
        SaveTextureAsPNG(tex, path);
    }
    // Start is called before the first frame update
    public void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }
}
