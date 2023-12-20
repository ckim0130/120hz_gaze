using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tobii.XR;

public class MyTool : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        CaptureCubemap();
        DEV_AppendToReport();
    }
   // private bool time = true;
   // public int k = 0;
   // public int f = 0;
   // public int Delaytime = 20;
   //// public int Delaytime = 0;

   // //wait for one second and call another getdata
   // private void Start()
   // {
   //     StartCoroutine(Timer());
   // }

   // private IEnumerator Timer()
   // {
   //     if (k == 40)
   //     {
   //         time = false;
   //     }

   //     if (time)
   //     {
   //         yield return new WaitForSeconds(Delaytime);
   //         DEV_AppendToReport();
   //         k++;
   //         f = f + Delaytime;
   //        // Delaytime = Delaytime + 5;
   //         StartCoroutine(Timer());
   //     }

        
        //yield return new WaitForSeconds(5f); //change to 0.5f for 0.5seconds
        //DEV_AppendToReport();
        //time = true;
    //}

    //gazedirection (local), the way you are looking at it. gaze is much more accurate than the eyes. move y
    public void DEV_AppendToReport()
    {
        for (int i = 0; i < this.GetComponent<EyeTrack>().isvalidlist.Count; i++)
        {
            SaveCsv.AppendToReport(new string[26] 
            {
                this.GetComponent<EyeTrack>().data[i].ToString(),
                this.GetComponent<EyeTrack>().duration[i].ToString(),
                this.GetComponent<EyeTrack>().time[i].ToString(), //GameObject.Find("time").getcomponent.ToString()
                this.GetComponent<EyeTrack>().converge[i].ToString(),
                this.GetComponent<EyeTrack>().convergeValid[i].ToString(),
                this.GetComponent<EyeTrack>().isvalidlist[i].ToString(),
                this.GetComponent<EyeTrack>().Local_origin[i].x.ToString(),
                this.GetComponent<EyeTrack>().Local_origin[i].y.ToString(),
                this.GetComponent<EyeTrack>().Local_origin[i].z.ToString(),
                this.GetComponent<EyeTrack>().World_origin[i].x.ToString(),
                this.GetComponent<EyeTrack>().World_origin[i].y.ToString(),
                this.GetComponent<EyeTrack>().World_origin[i].z.ToString(),
                this.GetComponent<EyeTrack>().Local_gaze[i].x.ToString(),
                this.GetComponent<EyeTrack>().Local_gaze[i].y.ToString(),
                this.GetComponent<EyeTrack>().Local_gaze[i].z.ToString(),
                this.GetComponent<EyeTrack>().World_gaze[i].x.ToString(),
                this.GetComponent<EyeTrack>().World_gaze[i].y.ToString(),
                this.GetComponent<EyeTrack>().World_gaze[i].z.ToString(),
                this.GetComponent<EyeTrack>().H_world_origin[i].x.ToString(),
                this.GetComponent<EyeTrack>().H_world_origin[i].y.ToString(),
                this.GetComponent<EyeTrack>().H_world_origin[i].z.ToString(),
                this.GetComponent<EyeTrack>().H_world_gaze[i].x.ToString(),
                this.GetComponent<EyeTrack>().H_world_gaze[i].y.ToString(),
                this.GetComponent<EyeTrack>().H_world_gaze[i].z.ToString(),
                this.GetComponent<EyeTrack>().LeftEye[i].ToString(),
                this.GetComponent<EyeTrack>().RightEye[i].ToString(),
            }
            );
        }
        //SaveCsv.BackupNote();
    }

    public RenderTexture cubemap;
    public RenderTexture equirect;
    public Camera CubeMapCamera;

    public void CaptureCubemap()
        //make sure to index cubemap + equirect inside the asset folder. 
    {
        var transform = CameraHelper.GetCameraTransform();
        string path = "wraparound_0.72.png";
        // CubeMapCamera.transform.SetPositionAndRotation(new Vector3(-0.06f, 0f, 4.2f), Quaternion.Euler(new Vector3(0, 220, 0)));
        CubeMapCamera.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, 220, 0)));
        CubeMapCamera.RenderToCubemap(cubemap, 63, Camera.MonoOrStereoscopicEye.Left); //cubemap is sphere
        cubemap.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono); // equirect projection
        RenderTexture.active = equirect;
        Texture2D tex = new Texture2D(equirect.width, equirect.height, TextureFormat.RGB24, false); 
        tex.ReadPixels(new Rect(0, 0, equirect.width, equirect.height), 0, 0);
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
    }
}


// public GameObject Output;

//private void start()
//{
//    SaveCsv.checker();
//}

//private void OnApplicationQuit()
//{
//    // this.GetComponent<TrackAOI>().check= false; //terminate the forloop 
//    DEV_AppendToReport();
//}
//bool Invoked = false;
//////public float delayTime = 15f; //5 mins

//void Update()
//{
//    if (!Invoked)
//    {
//        Invoked = true;
//        Invoke("DEV_AppendToReport", 10f);
//        //Invoked = false;
//    }
//}

//public float Delay = 15f;
//float timer;
//private void Update()
//{
//    timer += Time.deltaTime;
//    if (timer > Delay)
//    {
//        // WaitForSeconds(0.1f);
//        DEV_AppendToReport();
//        timer -= Delay;
//    }
//}