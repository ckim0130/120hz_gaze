using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine.Events;
using ViveSR.anipal.Eye;
using ViveSR.anipal;
using ViveSR;

namespace Tobii.XR.Examples
{
    public class Initialize : MonoBehaviour
    {
        public bool recordEyeData = false;
        public bool result_cal;
        public string Path = Directory.GetCurrentDirectory();
        public int subjectNr, subjectNrRand, practice_num = 0, totalMoves, correctMoves, incorrectMoves, cur_trial_num = 0, cur_trial_ID, cur_object_num = 0, cur_object_ID;
        public float completion;

        public string File_Path_Trial, File_Path_Clicks, File_Path_ET;
        public string _subjectNr_, target0name, target1name, target0name_p;
        // public TextAsset _subjectNr;
        // public Vector3 pos, temp1, dist;
        //public float change_x, change_y, change_z, addPos, AnimationTime = 2f, totMoved, timePassed, score, totalScore;
        public DateTime trialStartTime, targ1StartTime, trialFinishTime;
        public static int trialStartTime_h, trialStartTime_m, trialStartTime_s, trialStartTime_ms, targ1StartTime_h, targ1StartTime_m, targ1StartTime_s, targ1StartTime_ms, trialFinishTime_h, trialFinishTime_m, trialFinishTime_s, trialFinishTime_ms;
        public TimeSpan trialTime;
        //public GameObject player, cam, world, target0, target1, endtrialmenu, starttrialmenu, starttrialmenu_p, instructions2, instructions3, instructions4, instructions4b, all_geons, pedestal1, pedestal2, pedestal1b, pedestal2b, welcome1, welcome2, welcome3, welcome4, welcome5, welcome6, welcome7, ExptEnd, ConveyorBelt;
        public GameObject cam;


        // Start is called before the first frame update
        void Start()
        {
            //Calibration();
            string _subjNrpath = Path + "/subjectNr.txt";
            Debug.Log(_subjNrpath);
            StreamReader reader = new StreamReader(_subjNrpath);
            _subjectNr_ = reader.ReadToEnd();
            //_subjectNr_ = _subjectNr.text;
            subjectNr = int.Parse(_subjectNr_);
            subjectNrRand = UnityEngine.Random.Range(0, 100);
            Debug.Log(subjectNrRand);

            GameObject player = GameObject.Find("Player Variant");
            player.SetActive(true);
            GameObject cam = GameObject.Find("Camera");

            GameObject world = GameObject.Find("EverythingButPlayer");
            //Vector3 pos = cam.transform.position;
            //player.transform.position -= pos;
            //world.transform.position += pos;

        }


        void SetupDataFiles()
        {
            //File_Path_ET = Directory.GetCurrentDirectory() + "/Data/ETdata_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            File_Path_ET = Directory.GetCurrentDirectory() + "/Data/ETdata_" + ".txt"; 
            Debug.Log(File_Path_ET);
            File_Path_Clicks = Directory.GetCurrentDirectory() + "/Data/ClickData_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            File_Path_Trial = Directory.GetCurrentDirectory() + "/Data/Trialdata_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            Data_txt();
            //InputUserID();                              // Check if the file with the same ID exists.

            //Invoke("SystemCheck", 0.5f);                // System check.
            //SRanipal_Eye_v2.LaunchEyeCalibration();     // Perform calibration for eye tracking.
            //Calibration();
            //TargetPosition();                           // Implement the targets on the VR view.
            //Invoke("Measurement", 0.5f);                // Start the measurement of ocular movements in a separate callback function.  
        }


        void Data_txt()
        {
            string variable =
            "subjectNr" + "," +
            "practiceNr" + "," +
            "trialNr" + "," +
            "SysTime" + "," +
            "SysTime_h" + "," +
            "SysTime_m" + "," +
            "SysTime_s" + "," +
            "SysTime_ms" + "," +
            "time_stamp" + "," +
            "gaze_origin_L.x(mm)" + "," +
            "gaze_origin_L.y(mm)" + "," +
            "gaze_origin_L.z(mm)" + "," +
            "gaze_origin_R.x(mm)" + "," +
            "gaze_origin_R.y(mm)" + "," +
            "gaze_origin_R.z(mm)" + "," +
            "gaze_direct_L.x" + "," +
            "gaze_direct_L.y" + "," +
            "gaze_direct_L.z" + "," +
            "gaze_direct_R.x" + "," +
            "gaze_direct_R.y" + "," +
            "gaze_direct_R.z" + "," +
            "camPos_x" + "," +
            "camPos_y" + "," +
            "camPos_z" + "," +
            "camRot_x" + "," +
            "camRot_y" + "," +
            "camRot_z" + "," +
            "frame" + "," +
            "eye_valid_L" + "," +
            "eye_valid_R" + "," +
            "openness_L" + "," +
            "openness_R" + "," +
            "pupil_diameter_L(mm)" + "," +
            "pupil_diameter_R(mm)" + "," +
            "pos_sensor_L.x" + "," +
            "pos_sensor_L.y" + "," +
            "pos_sensor_R.x" + "," +
            "pos_sensor_R.y" + "," +
            "gaze_sensitive" + "," +
            //"frown_L" + "," +
            //"frown_R" + "," +
            //"squeeze_L" + "," +
            //"squeeze_R" + "," +
            //"wide_L" + "," +
            //"wide_R" + "," +
            "distance_valid_C" + "," +
            "distance_C(mm)" + "," +
            "beepStatus" + "," +
            "beepOn" + "," +
            "beepVol" +
            Environment.NewLine;

            File.AppendAllText(File_Path_ET, variable);

            //string variable2 =
            //"subjectNr" + "," +
            //"practiceNr" + "," +
            //"trialNr" + "," +
            //"SysTime" + "," +
            //"SysTime_h" + "," +
            //"SysTime_m" + "," +
            //"SysTime_s" + "," +
            //"SysTime_ms" + "," +
            //"geonClicked" + "," +
            //"correct" +
            //Environment.NewLine;

            //File.AppendAllText(File_Path_Clicks, variable2);

                //string variable3 =
                //"subjectNr" + "," +
                //"practiceNr" + "," +
                //"trialNr" + "," +
                //"trialStartTime" + "," +
                //"trialStartTime_h" + "," +
                //"trialStartTime_m" + "," +
                //"trialStartTime_s" + "," +
                //"trialStartTime_ms" + "," +
                //"targ1StartTime" + "," +
                //"targ1StartTime_h" + "," +
                //"targ1StartTime_m" + "," +
                //"targ1StartTime_s" + "," +
                //"targ1StartTime_ms" + "," +
                //"trialFinishTime" + "," +
                //"trialFinishTime_h" + "," +
                //"trialFinishTime_m" + "," +
                //"trialFinishTime_s" + "," +
                //"trialFinishTime_ms" + "," +
                //"trialID" + "," +
                //"target0" + "," +
                //"target1" + "," +
                //"totalMoves" + "," +
                //"correctMoves" + "," +
                //"completion" + "," +
                //Environment.NewLine;

                //File.AppendAllText(File_Path_Trial, variable3);
            }

        public void End()
        {
            Debug.Log("quitting");
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }

    }
}





