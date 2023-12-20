using System.Collections; //use array
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System.Diagnostics;
using System.Linq;
using System;
using UnityEditor;
// using ViveSR.anipal.Eye;

public class EyeTrack : MonoBehaviour
{
    public List<string> time = new List<string>();
    public List<float> duration = new List<float>();
    public List<float> converge = new List<float>();
    public List<bool> convergeValid = new List<bool>();
    public List<bool> isvalidlist = new List<bool>();
    public List<Vector3> Local_origin = new List<Vector3>(); //3d vetor
    public List<Vector3> Local_gaze = new List<Vector3>(); //3d vetor
    public List<Vector3> World_origin = new List<Vector3>(); //3d vetor
    public List<Vector3> World_gaze = new List<Vector3>(); //3d vetor
    public List<Vector3> H_world_origin = new List<Vector3>(); //3d vetor
    public List<Vector3> H_world_gaze = new List<Vector3>(); //3d vetor
    public List<Vector3> test_gaze = new List<Vector3>();
    public List<Vector3> test_origin = new List<Vector3>();
    public List<bool> LeftEye = new List<bool>();
    public List<bool> RightEye = new List<bool>();
    public bool check = false;
    public bool isPaused;


    // ADDED FROM TRACKAOI
    public TrackAtGaze[] Item; //get component at trackatgaze.item
    //public bool checker; // default is private
    public List<bool> boolList = new List<bool>();
    public List<string> data = new List<string>();

    //public float delay = 0.003f;
    //float timer;
    private void Update()
    {
        GetData();
        //timer += Time.deltaTime;
        //if (timer > delay)
        //{
        //    GetData();
        //    timer -= delay;
        //}
    }

    public void GetData()
    {
        //var provider = TobiiXR.Internal.Provider as HTCProvider;
        //provider?.Tick();
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
        var eyeTrackingLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);
        // UnityEngine.Debug.Log(eyeTrackingData.GazeRay.Direction); //printing in unity
        // Check if gaze ray is valid
        //var transform = Camera.main.transform;
        //var transform = CameraHelper.GetCameraTransform(); // head pose
       // UnityEngine.Debug.Log(transform);
        //var testTransform = GameObject.Find("Cube").transform;
        //UnityEngine.Debug.Log(testTransform);
       

        if (eyeTrackingData.GazeRay.IsValid) // world.space
        {
            isvalidlist.Add(true);
            converge.Add(eyeTrackingData.ConvergenceDistance);
            convergeValid.Add(eyeTrackingData.ConvergenceDistanceIsValid);
            var timestamp = eyeTrackingData.Timestamp;
            duration.Add(timestamp);
            time.Add(System.DateTime.Now.ToString("hh.mm.ss.ffff")); 
            
            var local_origin = eyeTrackingLocal.GazeRay.Direction; //Local 
            Local_origin.Add(local_origin);
            var local_direction = eyeTrackingLocal.GazeRay.Direction;
            Local_gaze.Add(local_direction);
            
            var world_origin = eyeTrackingData.GazeRay.Origin; // World
            World_origin.Add(world_origin);
            var world_direction = eyeTrackingData.GazeRay.Direction; 
            World_gaze.Add(world_direction);

            var head_origin = transform.TransformPoint(eyeTrackingData.GazeRay.Origin); //World fused with camera
            H_world_origin.Add(head_origin);
            var head_direction = transform.TransformPoint(eyeTrackingData.GazeRay.Direction);
            H_world_gaze.Add(head_direction);

            //var Test_origin = testTransform.TransformPoint(eyeTrackingData.GazeRay.Origin); //World fused with camera
            //test_origin.Add(Test_origin);
            //var test_direction = testTransform.TransformPoint(eyeTrackingData.GazeRay.Direction);
            //test_gaze.Add(test_direction);

            var isLeftEyeBlinking = eyeTrackingLocal.IsLeftEyeBlinking;
            LeftEye.Add(isLeftEyeBlinking);
            var isRightEyeBlinking = eyeTrackingLocal.IsRightEyeBlinking;
            RightEye.Add(isRightEyeBlinking);
        }
        else
        {
            isvalidlist.Add(false);
            converge.Add(eyeTrackingData.ConvergenceDistance);
            convergeValid.Add(eyeTrackingData.ConvergenceDistanceIsValid);
            duration.Add(eyeTrackingData.Timestamp);
            time.Add(System.DateTime.Now.ToString("hh.mm.ss.ffff"));
            Local_origin.Add(Vector3.zero);
            Local_gaze.Add(Vector3.zero);
            World_origin.Add(Vector3.zero);
            World_gaze.Add(Vector3.zero);
            H_world_origin.Add(Vector3.zero);
            H_world_gaze.Add(Vector3.zero);
            //test_origin.Add(Vector3.zero);
            //test_gaze.Add(Vector3.zero);
            LeftEye.Add(false);
            RightEye.Add(false);

        }

        for (int j = 0; j < Item.Length; j++)
        {
            boolList.Add(Item[j].islooking);
        }
        if (boolList.All(a => !a))
        {
            data.Add("NaN");
        }
        else
        {
            int ind = boolList.IndexOf(true);
            data.Add(Item[ind].name);
        }
        boolList.Clear();

    }
    //void Start()
    //{
    //    InvokeRepeating("GetData", 0.001f, 0.016f);
    //    InvokeRepeating("TrackAoi", 0f, 0.25f);
    //    GetData();
    //    TrackAoi();
    //    StartCoroutine(SampleRate());
    //}

    //wait for one second and call the getdata()
    //private IEnumerator SampleRate()
    //{
    //    //yield return new WaitForSecondsRealtim`e(0.008f); //change to 0.5f for 0.5seconds
    //    GetData();
    //    //TrackAoi();
    //    check = true;
    //}
    //private IEnumerator SampleRate()
    //{
    //    yield return StartCoroutine(Frames(1));
    //    GetData();
    //    //TrackAoi();
    //    check = true;
    //}



    //wait for one second and call functions again


    //void FixedUpdate()
    //{
    //    Time.fixedDeltaTime = 0.016f;
    //    if (check == true)
    //    {
    //        check = false;
    //        StartCoroutine(SampleRate());
    //    }
    //}

    //public static IEnumerator Frames(int frameCount)
    //{
    //    if (frameCount <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less than 1 frame");
    //    }

    //    while (frameCount > 0)
    //    {
    //        frameCount--;
    //        yield return null;
    //    }
    //}


}