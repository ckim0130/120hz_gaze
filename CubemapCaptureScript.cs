using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubemapCaptureScript : MonoBehaviour
{
    public RenderTexture cubemap;
    public RenderTexture equirect;
    public Camera CubeMapCamera;
    public void CaptureCubemap(string path)
    {
        CubeMapCamera.transform.SetPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(new Vector3(0,0,0)));
        CubeMapCamera.RenderToCubemap(cubemap, 63, Camera.MonoOrStereoscopicEye.Left);
        cubemap.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);
        RenderTexture.active = equirect;
        Texture2D tex = new Texture2D(equirect.width, equirect.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, equirect.width, equirect.height), 0, 0);
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
    }

}


