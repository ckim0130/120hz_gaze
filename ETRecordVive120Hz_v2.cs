using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.IO;
using ViveSR.anipal.Eye;
using ViveSR.anipal;
using ViveSR;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Example usage for eye tracking callback
/// Note: Callback runs on a separate thread to report at ~120hz.
/// Unity is not threadsafe and cannot call any UnityEngine api from within callback thread.
/// </summary>
/// 

namespace Tobii.XR.Examples
{
    public class ETRecordVive120Hz_v2 : MonoBehaviour
    {
        public Initialize other;
        public static string File_Path_ET;
        public static bool recordEyeData = true;
        public static DateTime SysTime;
        public static int SysTime_h, SysTime_m, SysTime_s, SysTime_ms;
        private static int subjectNr, trialNr, practiceNr;
        public static Vector3 camPos, camRot;
        public static float camPos_x, camPos_y, camPos_z, camRot_x, camRot_y, camRot_z;
        //public static int beepStatus;
        //public static bool beepOn;
        //public static float beepVol;

        private static float time_stamp;
        private static int frame;

        public static TrackAtGaze[] Item;
        // ********************************************************************************************************************
        //
        //  Parameters for eye data.
        //
        // ********************************************************************************************************************
        private static EyeData_v2 eyeData = new EyeData_v2();
        public EyeParameter eye_parameter = new EyeParameter();
        public GazeRayParameter gaze = new GazeRayParameter();
        private static bool eye_callback_registered = false;
        private static UInt64 eye_valid_L, eye_valid_R;                 // The bits explaining the validity of eye data.
        private static float openness_L, openness_R;                    // The level of eye openness.
        private static float pupil_diameter_L, pupil_diameter_R;        // Diameter of pupil dilation.
        private static Vector2 pos_sensor_L, pos_sensor_R;              // Positions of pupils.
        private static Vector3 gaze_origin_L, gaze_origin_R;            // Position of gaze origin.
        private static Vector3 gaze_direct_L, gaze_direct_R;            // Direction of gaze ray.
        private static float frown_L, frown_R;                          // The level of user's frown.
        private static float squeeze_L, squeeze_R;                      // The level to show how the eye is closed tightly.
        private static float wide_L, wide_R;                            // The level to show how the eye is open widely.
        private static double gaze_sensitive;                           // The sensitive factor of gaze ray.
        private static float distance_C;                                // Distance from the central point of right and left eyes.
        private static bool distance_valid_C;                           // Validity of combined data of right and left eyes.


        //private List<bool> boolList = new List<bool>();
        //private List<string> data = new List<string>();


        //private static EyeData eyeData = new EyeData();
        //private static bool eye_callback_registered = false;

        //public Text uiText;
        private static float lastTime, currentTime, updateSpeed;

        // ********************************************************************************************************************
        //
        //  Start is called before the first frame update. The Start() function is performed only one time.
        //
        // ********************************************************************************************************************
        void Start()
        {
            File_Path_ET = other.File_Path_ET;
            Item = GameObject.FindObjectsOfType<TrackAtGaze>();
            //Invoke("SystemCheck", 0.5f);                // System check.
            // Implement the targets on the VR view.
            Invoke("Measurement", 1f);                // Start the measurement of ocular movements in a separate callback function.  
        }


        // ********************************************************************************************************************
        //
        //  Check if the system works properly.
        //
        // ********************************************************************************************************************
        void SystemCheck()
        {
            if (SRanipal_Eye_API.GetEyeData_v2(ref eyeData) == ViveSR.Error.WORK)
            {
                Debug.Log("Device is working properly.");
            }

            //Debug.Log("System Check " + SRanipal_Eye_API.GetEyeData_v2(ref eyeData).ToString());

            if (SRanipal_Eye_API.GetEyeParameter(ref eye_parameter) == ViveSR.Error.WORK)
            {
                Debug.Log("Eye parameters are measured.");
            }

            //  Check again if the initialisation of eye tracking functions successfully. If not, we stop playing Unity.
            Error result_eye_init = SRanipal_API.Initial(SRanipal_Eye_v2.ANIPAL_TYPE_EYE_V2, IntPtr.Zero);

            if (result_eye_init == Error.WORK)
            {
                Debug.Log("[SRanipal] Initial Eye v2: " + result_eye_init);
            }
            else
            {
                Application.Quit();
            }
        }

        // ********************************************************************************************************************
        //
        //  Calibration is performed if the calibration is necessary.
        //
        // ********************************************************************************************************************


        void Measurement()
        {
            EyeParameter eye_parameter = new EyeParameter();
            SRanipal_Eye_API.GetEyeParameter(ref eye_parameter);

            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }

            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }

        private void OnDisable()
        {
            Release();
        }

        void OnApplicationQuit()
        {
            Release();
        }

        /// <summary>
        /// Release callback thread when disabled or quit
        /// </summary>
        private static void Release()
        {
            if (eye_callback_registered == true)
            {
                SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }

        // <summary>
        // Required class for IL2CPP scripting backend support
        // </summary>
        internal class MonoPInvokeCallbackAttribute : System.Attribute
        {
            public MonoPInvokeCallbackAttribute() { }
        }

        /// <summary>
        /// Eye tracking data callback thread.
        /// Reports data at ~120hz
        /// MonoPInvokeCallback attribute required for IL2CPP scripting backend
        /// </summary>
        /// <param name="eye_data">Reference to latest eye_data</param>
        [MonoPInvokeCallback]
        public static void EyeCallback(ref EyeData_v2 eye_data)
        {
            EyeParameter eye_parameter = new EyeParameter();
            SRanipal_Eye_API.GetEyeParameter(ref eye_parameter);

            eyeData = eye_data;
            // do stuff with eyeData...

            lastTime = currentTime;
            currentTime = eyeData.timestamp;

            ViveSR.Error error = SRanipal_Eye_API.GetEyeData_v2(ref eyeData);
            List<bool> boolList = new List<bool>();
            //dList<string> data = new List<string>();

            if (error == ViveSR.Error.WORK && recordEyeData == true)
            {
                SysTime = DateTime.UtcNow;
                SysTime_h = SysTime.Hour;
                SysTime_m = SysTime.Minute;
                SysTime_s = SysTime.Second;
                SysTime_ms = SysTime.Millisecond;
                time_stamp = eyeData.timestamp;
                gaze_origin_L = eyeData.verbose_data.left.gaze_origin_mm;
                gaze_origin_R = eyeData.verbose_data.right.gaze_origin_mm;
                gaze_direct_L = eyeData.verbose_data.left.gaze_direction_normalized;
                gaze_direct_R = eyeData.verbose_data.right.gaze_direction_normalized;
                gaze_sensitive = eye_parameter.gaze_ray_parameter.sensitive_factor;
                //hello
                //frame = eyeData.frame_sequence;
                eye_valid_L = eyeData.verbose_data.left.eye_data_validata_bit_mask;
                eye_valid_R = eyeData.verbose_data.right.eye_data_validata_bit_mask;
                openness_L = eyeData.verbose_data.left.eye_openness;
                openness_R = eyeData.verbose_data.right.eye_openness;
                pupil_diameter_L = eyeData.verbose_data.left.pupil_diameter_mm;
                pupil_diameter_R = eyeData.verbose_data.right.pupil_diameter_mm;
                pos_sensor_L = eyeData.verbose_data.left.pupil_position_in_sensor_area;
                pos_sensor_R = eyeData.verbose_data.right.pupil_position_in_sensor_area;
                //frown_L = eyeData.expression_data.left.eye_frown;
                //frown_R = eyeData.expression_data.right.eye_frown;
                //squeeze_L = eyeData.expression_data.left.eye_squeeze;
                //squeeze_R = eyeData.expression_data.right.eye_squeeze;
                //wide_L = eyeData.expression_data.left.eye_wide;
                //wide_R = eyeData.expression_data.right.eye_wide;
                distance_valid_C = eyeData.verbose_data.combined.convergence_distance_validity;
                distance_C = eyeData.verbose_data.combined.convergence_distance_mm;

                foreach (TrackAtGaze i in Item)
                {
                    boolList.Add(i.islooking);
                    Debug.Log($"Object: {i.gameObject.name}, isLooking: {i.islooking}");
                    //Debug.Log(Item.Length);
                    //if (i.islooking)
                    //{
                    //    Debug.Log(i.gameObject.name);
                    //}
                }
                //boolList.Add(i.islooking);
                //Debug.Log(string.Join(", ", boolList));
                //if (!boolList.Any(isLooking =>isLooking))
                //{
                //    data.Add("X");
                //}
                //else
                //{
                //    int ind = boolList.IndexOf(true);
                //    Debug.Log(Item[ind].name);
                //    //data.Add(Item[ind].name);
                //}

                //boolList.Clear();
                //Debug.Log(string.Join(", ", data));
                //boolList.Clear();
                //for (int j = 0; j < Item.Length; j++)
                //{
                //    Debug.Log(Item[j].islooking);
                //}
                //    Debug.Log(Item.Length);
                //    boolList.Add(Item[j].islooking);
                //}
                //if (boolList.TrueForAll(a => !a))
                //{
                //    data.Add("X");
                //}
                //else
                //{
                //    int ind = boolList.IndexOf(true);
                //    data.Add(Item[ind].name);
                //}
                //boolList.Clear();

                //Debug.Log(data);


                //  Convert the measured data to string data to write in a text file.
                string value =

                    //string.Join(", ", data).ToString() + "," +
                    //data.ToString() + "," +
                    SysTime_h.ToString() + "," +
                    SysTime_m.ToString() + "," +
                    SysTime_ms.ToString() + "," +
                    gaze_origin_L.x.ToString() + "," +
                    gaze_origin_L.y.ToString() + "," +
                    gaze_origin_L.z.ToString() + "," +
                    gaze_origin_R.x.ToString() + "," +
                    gaze_origin_R.y.ToString() + "," +
                    gaze_origin_R.z.ToString() + "," +
                    gaze_direct_L.x.ToString() + "," +
                    gaze_direct_L.y.ToString() + "," +
                    gaze_direct_L.z.ToString() + "," +
                    gaze_direct_R.x.ToString() + "," +
                    gaze_direct_R.y.ToString() + "," +
                    gaze_direct_R.z.ToString() + "," +
                    pupil_diameter_L.ToString() + "," +
                    pupil_diameter_R.ToString() + "," +
                    camPos_x.ToString() + "," +
                    camPos_y.ToString() + "," +
                    camPos_z.ToString() + "," +
                    camRot_x.ToString() + "," +
                    camRot_y.ToString() + "," +
                    camRot_z.ToString() + "," +
                    Environment.NewLine;

                //Debug.Log(File_Path_ET);
                File.AppendAllText("120hz_testing.txt", value);
            }

        }
    }
}




