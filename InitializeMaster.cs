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
    public class InitializeMaster : MonoBehaviour
    {
        public bool recordEyeData = false;
        public bool result_cal;
        public string Path = Directory.GetCurrentDirectory();
        public int subjectNr, subjectNrRand, practice_num = 0, totalMoves, correctMoves, incorrectMoves, cur_trial_num = 0, cur_trial_ID, cur_object_num = 0, cur_object_ID;
        public float completion;

        public string File_Path_Trial, File_Path_Clicks, File_Path_ET;
        public string _subjectNr_, target0name, target1name, target0name_p;
        public TextAsset _subjectNr;
        public Vector3 pos, temp1, dist;
        public float change_x, change_y, change_z, addPos, AnimationTime = 2f, totMoved, timePassed, score, totalScore;
        public DateTime trialStartTime, targ1StartTime, trialFinishTime;
        public static int trialStartTime_h, trialStartTime_m, trialStartTime_s, trialStartTime_ms, targ1StartTime_h, targ1StartTime_m, targ1StartTime_s, targ1StartTime_ms, trialFinishTime_h, trialFinishTime_m, trialFinishTime_s, trialFinishTime_ms;
        public TimeSpan trialTime;
        public bool practice = true; private bool animate = false;
        public AudioSource BeepSource;
        private bool trialActive = false;
        public bool beepOn = false;
        public int beepStatus = 0;
        public GameObject player, cam, world, target0, target1, endtrialmenu, starttrialmenu, starttrialmenu_p, instructions2, instructions3, instructions4, instructions4b, all_geons, pedestal1, pedestal2, pedestal1b, pedestal2b, welcome1, welcome2, welcome3, welcome4, welcome5, welcome6, welcome7, ExptEnd, ConveyorBelt;
        public GameObject house_prefab, castle_prefab, train_prefab, taxi_prefab, unicorn_prefab, cat_prefab, robot_prefab, phone_prefab;
        public Text target0textbox, target1textbox, target0textbox_p, trialnum_textbox, completion_textbox, incorrect_textbox, score_textbox, totalScore_textbox, avgScore_textbox;
        private List<Vector3> locations = new List<Vector3>();
        private List<Vector3> model_locations = new List<Vector3>();

        //public static int[] trial_IDs = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        //public static int[] practice_IDs_0 = { 0, 1, 2, 3 };
        //public static int[] practice_IDs_1 = { 0, 1, 2, 3 };
        //public static int[] practice_IDs_2 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public static List<Obj> objs = new List<Obj>();
        public static List<Trial> trials = new List<Trial>();

        private const ControllerButton Touchpad = ControllerButton.Touchpad;

        // Start is called before the first frame update
        void Start()
        {
            Calibration();
            string _subjNrpath = Path + "/subjectNr.txt";
            StreamReader reader = new StreamReader(_subjNrpath);
            _subjectNr_ = reader.ReadToEnd();
            //_subjectNr_ = _subjectNr.text;
            subjectNr = int.Parse(_subjectNr_);
            subjectNrRand = UnityEngine.Random.Range(0, 100);
            Debug.Log(subjectNrRand);

            GameObject player = GameObject.Find("Player Variant");
            player.SetActive(true);
            //GameObject cam = GameObject.Find("Camera");

            GameObject world = GameObject.Find("EverythingButPlayer");
            //Vector3 pos = cam.transform.position;
            //player.transform.position -= pos;
            //world.transform.position += pos;

            AudioSource[] audioSources = GameObject.Find("Camera").GetComponents<AudioSource>();
            BeepSource = audioSources[3];

            GameObject endtrialmenu = GameObject.Find("EndTrial");
            endtrialmenu.SetActive(false);
            GameObject starttrialmenu = GameObject.Find("BeginTrial");
            starttrialmenu.SetActive(false);
            GameObject starttrialmenu_p = GameObject.Find("BeginTrial_p");
            starttrialmenu_p.SetActive(false);
            GameObject welcome1 = GameObject.Find("Welcome1");
            welcome1.SetActive(true);
            GameObject welcome2 = GameObject.Find("Welcome2");
            welcome2.SetActive(false);
            GameObject welcome3 = GameObject.Find("Welcome3");
            welcome3.SetActive(false);
            GameObject welcome4 = GameObject.Find("Welcome4");
            welcome4.SetActive(false);
            GameObject welcome5 = GameObject.Find("Welcome5");
            welcome5.SetActive(false);
            GameObject welcome6 = GameObject.Find("Welcome6");
            welcome6.SetActive(false);
            GameObject welcome7 = GameObject.Find("Welcome7");
            welcome7.SetActive(false);
            GameObject instructions2 = GameObject.Find("Instructions2");
            instructions2.SetActive(false);
            GameObject instructions3 = GameObject.Find("Instructions3");
            instructions3.SetActive(false);
            GameObject instructions4 = GameObject.Find("Instructions4");
            instructions4.SetActive(false);
            GameObject instructions4b = GameObject.Find("Instructions4b");
            instructions4b.SetActive(false);
            GameObject ExptEnd = GameObject.Find("ExptEnd");
            ExptEnd.SetActive(false);

            GameObject ConveyorBelt = GameObject.Find("ConveyorBelt");
            ConveyorBelt.SetActive(false);

            Vector3 model_loc1 = GameObject.Find("Pedestal1").GetComponent<Renderer>().bounds.center;
            model_loc1[1] += GameObject.Find("Pedestal1").GetComponent<Renderer>().bounds.extents.y;
            model_locations.Add(model_loc1);
            Vector3 model_loc2 = GameObject.Find("Pedestal2").GetComponent<Renderer>().bounds.center;
            model_loc2[1] += GameObject.Find("Pedestal2").GetComponent<Renderer>().bounds.extents.y;
            model_locations.Add(model_loc2);

            Vector3 model_loc3 = GameObject.Find("Pedestal1b").GetComponent<Renderer>().bounds.center;
            model_loc3[1] += GameObject.Find("Pedestal1b").GetComponent<Renderer>().bounds.extents.y;
            model_locations.Add(model_loc3);
            Vector3 model_loc4 = GameObject.Find("Pedestal2b").GetComponent<Renderer>().bounds.center;
            model_loc4[1] += GameObject.Find("Pedestal2b").GetComponent<Renderer>().bounds.extents.y;
            model_locations.Add(model_loc4);

            pedestal1 = GameObject.Find("Pedestal1");
            pedestal2 = GameObject.Find("Pedestal2");
            pedestal1b = GameObject.Find("Pedestal1b");
            pedestal1b.SetActive(false);
            pedestal2b = GameObject.Find("Pedestal2b");
            pedestal2b.SetActive(false);

            foreach (GameObject _moveChild in GameObject.FindGameObjectsWithTag("moveChild"))
            {
                locations.Add(_moveChild.transform.position);
            }


            ReadTrialInfo();
            RandomizeLocations();
            //RandomizeColors();
            //RandomizeTrialOrder();
            //RandomizePracticeOrder();
            SetupDataFiles();

            all_geons.SetActive(false);
        }

        void Calibration()
        {

            result_cal = SRanipal_Eye_v2.LaunchEyeCalibration();

            if (result_cal == true)
            {
                Debug.Log("Calibration is done successfully.");
            }

            else
            {
                Debug.Log("Calibration is failed.");
                Application.Quit();
            }

        }

        void SetupDataFiles()
        {
            //File_Path_ET = Directory.GetCurrentDirectory() + "/Data/ETdata_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            File_Path_ET = Directory.GetCurrentDirectory() + "/Data/ETdata_" + ".txt";
            Debug.Log(File_Path_ET);
            File_Path_Clicks = Directory.GetCurrentDirectory() + "/Data/ClickData_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            File_Path_Trial = Directory.GetCurrentDirectory() + "/Data/Trialdata_" + subjectNr.ToString() + "_" + subjectNrRand.ToString() + ".txt";
            InputUserID();                              // Check if the file with the same ID exists.

            //Invoke("SystemCheck", 0.5f);                // System check.
            //SRanipal_Eye_v2.LaunchEyeCalibration();     // Perform calibration for eye tracking.
            //Calibration();
            //TargetPosition();                           // Implement the targets on the VR view.
            //Invoke("Measurement", 0.5f);                // Start the measurement of ocular movements in a separate callback function.  
        }

        void InputUserID()
        {
            //Debug.Log(File_Path_ET);

            if (File.Exists(File_Path_ET))
            {
                Application.Quit();
                //Debug.Log("File with the same UserID already exists. Please change the UserID in the C# code.");

                //  When the same file name is found, we stop playing Unity.

                /*if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }*/
            }
            else
            {
                Data_txt();
            }
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

            string variable2 =
            "subjectNr" + "," +
            "practiceNr" + "," +
            "trialNr" + "," +
            "SysTime" + "," +
            "SysTime_h" + "," +
            "SysTime_m" + "," +
            "SysTime_s" + "," +
            "SysTime_ms" + "," +
            "geonClicked" + "," +
            "correct" +
            Environment.NewLine;

            File.AppendAllText(File_Path_Clicks, variable2);

            string variable3 =
            "subjectNr" + "," +
            "practiceNr" + "," +
            "trialNr" + "," +
            "trialStartTime" + "," +
            "trialStartTime_h" + "," +
            "trialStartTime_m" + "," +
            "trialStartTime_s" + "," +
            "trialStartTime_ms" + "," +
            "targ1StartTime" + "," +
            "targ1StartTime_h" + "," +
            "targ1StartTime_m" + "," +
            "targ1StartTime_s" + "," +
            "targ1StartTime_ms" + "," +
            "trialFinishTime" + "," +
            "trialFinishTime_h" + "," +
            "trialFinishTime_m" + "," +
            "trialFinishTime_s" + "," +
            "trialFinishTime_ms" + "," +
            "trialID" + "," +
            "target0" + "," +
            "target1" + "," +
            "totalMoves" + "," +
            "correctMoves" + "," +
            "completion" + "," +
            "score" +
            Environment.NewLine;

            File.AppendAllText(File_Path_Trial, variable3);
        }

        //public void NewTrial()
        //{
        //    Destroy(target0);
        //    Destroy(target1);

        //    all_geons.SetActive(true);
        //    endtrialmenu.SetActive(false);

        //    cur_object_num = 0;

        //    Debug.Log("cur_trial_num " + cur_trial_num.ToString());
        //    RandomizeLocations();
        //    RandomizeColors();
        //    all_geons.SetActive(false);

        //    int _cur_trial_num = cur_trial_num + 1;
        //    trialnum_textbox.text = _cur_trial_num.ToString();

        //    if (practice == false)
        //    {

        //        cur_trial_ID = trial_IDs[cur_trial_num];
        //        cur_object_ID = trials[cur_trial_ID].cur_object[cur_object_num];
        //        target0name = objs[trials[cur_trial_ID].cur_object[0]].name.ToString();
        //        target0textbox.text = target0name;
        //        target1name = objs[trials[cur_trial_ID].cur_object[1]].name.ToString();
        //        target1textbox.text = target1name;

        //        starttrialmenu.SetActive(true);

        //    }
        //    else
        //    {

        //        if (practice_num == 2)
        //        {

        //            if (cur_trial_num == practice_IDs_2.Length)
        //            {
        //                cur_trial_num = 0;
        //                practice_num += 1;
        //                practice = false;
        //                instructions4.SetActive(true);
        //                ConveyorBelt.SetActive(true);
        //                pedestal1.SetActive(false);
        //                pedestal2.SetActive(false);

        //            }
        //            else
        //            {
        //                pedestal1.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        //                pedestal2.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //                //Destroy(target0);
        //                //Destroy(target1);
        //                cur_trial_ID = practice_IDs_2[cur_trial_num];
        //                cur_object_ID = trials[cur_trial_ID].cur_object[cur_object_num];
        //                target0name = objs[trials[cur_trial_ID].cur_object[0]].name.ToString();
        //                target0textbox.text = target0name;
        //                target1name = objs[trials[cur_trial_ID].cur_object[1]].name.ToString();
        //                target1textbox.text = target1name;
        //                starttrialmenu.SetActive(true);
        //            }

        //        }
        //        if (practice_num == 1)
        //        {
        //            if (cur_trial_num == practice_IDs_1.Length)
        //            {
        //                Destroy(target0);
        //                cur_trial_num = 0;
        //                practice_num += 1;
        //                instructions3.SetActive(true);

        //            }
        //            else
        //            {
        //                pedestal1.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        //                pedestal2.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //                //Destroy(target0);
        //                cur_object_ID = practice_IDs_1[cur_trial_num];
        //                target0name_p = objs[cur_object_ID].name.ToString();
        //                target0textbox_p.text = target0name_p;
        //                starttrialmenu_p.SetActive(true);
        //            }

        //        }
        //        if (practice_num == 0)
        //        {
        //            if (cur_trial_num == practice_IDs_0.Length)
        //            {
        //                Destroy(target0);
        //                cur_trial_num = 0;
        //                practice_num += 1;
        //                instructions2.SetActive(true);

        //            }
        //            else
        //            {
        //                pedestal1.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        //                pedestal2.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //                //
        //                cur_object_ID = practice_IDs_0[cur_trial_num];
        //                target0name_p = objs[cur_object_ID].name.ToString();
        //                target0textbox_p.text = target0name_p;
        //                starttrialmenu_p.SetActive(true);
        //            }

        //        }


        //    }

        //    CreateTargetModel();

        //}


        public void StartTrial()
        {
            //if( practice_num == 3)
            //{
            //    recordEyeData = true;
            //}
            recordEyeData = true;

            //trialActive = true;
            //beepOn = false;

            trialStartTime = DateTime.UtcNow;
            trialStartTime_h = trialStartTime.Hour;
            trialStartTime_m = trialStartTime.Minute;
            trialStartTime_s = trialStartTime.Second;
            trialStartTime_ms = trialStartTime.Millisecond;
            all_geons.SetActive(true);
            totalMoves = 0;
            correctMoves = 0;

            foreach (GameObject _GlowBox_p in GameObject.FindGameObjectsWithTag("GlowBox_p"))
            {
                //_GlowBox_p.GetComponent<Renderer>().enabled = false;
                //cycle through children of target0 for name "cube_target"

                var renderers = _GlowBox_p.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = false;

                }

                if (practice_num < 1)
                {
                    foreach (Transform t in GameObject.Find("target0").transform)
                    {
                        if (t.name == _GlowBox_p.transform.parent.name)
                        {
                            //_GlowBox_p.GetComponent<Renderer>().enabled = true;
                            //continue;
                            var renderers2 = _GlowBox_p.GetComponentsInChildren<Renderer>();
                            foreach (Renderer r in renderers2)
                            {
                                r.enabled = true;

                            }
                        }

                    }
                }


            }


        }

        public void NextObj()
        {
            AudioSource[] audioSources = GameObject.Find("Camera").GetComponents<AudioSource>();

            AudioSource source = audioSources[1];
            source.Play();


            cur_object_num = 1;
            //Debug.Log("cur_object_num " + cur_object_num.ToString());
            cur_object_ID = trials[cur_trial_ID].cur_object[cur_object_num];
            //Debug.Log("cur_object_ID " + cur_object_ID.ToString());
            CreateTargetModel();

            if (practice == true)
            {
                pedestal2.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                pedestal1.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            }

            targ1StartTime = DateTime.UtcNow;
            targ1StartTime_h = targ1StartTime.Hour;
            targ1StartTime_m = targ1StartTime.Minute;
            targ1StartTime_s = targ1StartTime.Second;
            targ1StartTime_ms = targ1StartTime.Millisecond;

        }

        //public void FinishTrial()
        //{
        //    trialActive = false;
        //    beepOn = false;
        //    recordEyeData = false;
        //    timePassed = 0;

        //    if (BeepSource.isPlaying)
        //    {
        //        BeepSource.Stop();
        //    }

        //    trialFinishTime = DateTime.UtcNow;
        //    trialFinishTime_h = trialFinishTime.Hour;
        //    trialFinishTime_m = trialFinishTime.Minute;
        //    trialFinishTime_s = trialFinishTime.Second;
        //    trialFinishTime_ms = trialFinishTime.Millisecond;

        //    trialTime = trialFinishTime - trialStartTime;

        //    incorrectMoves = totalMoves - correctMoves;
        //    incorrect_textbox.text = incorrectMoves.ToString();

        //    if (practice_num < 2)
        //    {
        //        completion = (float)correctMoves / 5;
        //    }
        //    else
        //    {
        //        completion = (float)correctMoves / 10;
        //    }
        //    float _completion = completion * 100;
        //    completion_textbox.text = _completion.ToString();

        //    score = correctMoves - incorrectMoves;

        //    if (score < 0)
        //    {
        //        score = 0;
        //    }

        //    if (practice == false)
        //    {
        //        totalScore += score;
        //    }

        //    score_textbox.text = score.ToString();

        //    UpdateData();

        //    AudioSource[] audioSources = GameObject.Find("Camera").GetComponents<AudioSource>();

        //    AudioSource source = audioSources[1];
        //    source.Play();

        //    all_geons.SetActive(false);

        //    if (practice == true)
        //    {
        //        pedestal1.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //        pedestal2.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        //        endtrialmenu.SetActive(true);
        //    }

        //    cur_trial_num += 1;

        //    if (practice == false)
        //    {
        //        //Debug.Log("animate?");
        //        dist = model_locations[2] - target0.transform.position;
        //        change_x = dist.x / AnimationTime;
        //        //change_y = dist.y / 180;
        //        change_z = dist.z / AnimationTime;
        //        //counter = 0;
        //        totMoved = 0f;
        //        ConveyorSound();
        //        animate = true;
        //        StartCoroutine(waitShow());


        //    }
        //    else
        //    {
        //        if (cur_trial_num == trial_IDs.Length + 1)
        //        {
        //            totalScore_textbox.text = totalScore.ToString();
        //            float _avgScore = totalScore / 36f;
        //            avgScore_textbox.text = _avgScore.ToString();

        //            ExptEnd.SetActive(true);
        //        }
        //        else
        //        {
        //            endtrialmenu.SetActive(true);
        //        }

        //    }



        //}

        void UpdateData()
        {

            string value =
                subjectNr.ToString() + "," +
                practice_num.ToString() + "," +
                cur_trial_num.ToString() + "," +
                trialStartTime.ToString() + "," +
                trialStartTime_h.ToString() + "," +
                trialStartTime_m.ToString() + "," +
                trialStartTime_s.ToString() + "," +
                trialStartTime_ms.ToString() + "," +
                targ1StartTime.ToString() + "," +
                targ1StartTime_h.ToString() + "," +
                targ1StartTime_m.ToString() + "," +
                targ1StartTime_s.ToString() + "," +
                targ1StartTime_ms.ToString() + "," +
                trialFinishTime.ToString() + "," +
                trialFinishTime_h.ToString() + "," +
                trialFinishTime_m.ToString() + "," +
                trialFinishTime_s.ToString() + "," +
                trialFinishTime_ms.ToString() + "," +
                cur_trial_ID.ToString() + "," +
                target0name.ToString() + "," +
                target1name.ToString() + "," +
                totalMoves.ToString() + "," +
                correctMoves.ToString() + "," +
                completion.ToString() + "," +
                score.ToString() +

                Environment.NewLine;

            File.AppendAllText(File_Path_Trial, value);
        }

        void RandomizeLocations()
        {


            int[] order = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            for (int i = order.Length - 1; i > 0; i--)
            {
                int swapIndex = UnityEngine.Random.Range(0, i + 1);
                int tmp = order[i];
                order[i] = order[swapIndex];
                order[swapIndex] = tmp;
            }

            int counter = 0;
            foreach (GameObject _moveChild in GameObject.FindGameObjectsWithTag("moveChild"))
            {
                _moveChild.transform.position = locations[order[counter]];

                counter = counter + 1;
            }

        }

        //void RandomizeColors()
        //{
        //    float[][] colors = new float[][]
        //    {
        //    new float[] {216f / 255f, 70f / 255f, 107f / 255f},
        //    new float[] {91f / 255f, 184f / 255f, 77f / 255f},
        //    new float[] {162f / 255f, 90f / 255f, 202f / 255f},
        //    new float[] {182f / 255f, 178f / 255f, 54f / 255f},
        //    new float[] {101f / 255f, 107f / 255f, 197f / 255f},
        //    new float[] {215f / 255f, 146f / 255f, 67f / 255f},
        //    new float[] {94f / 255f, 153f / 255f, 211f / 255f},
        //    new float[] {208f / 255f, 80f / 255f, 50f / 255f},
        //    new float[] {82f / 255f, 190f / 255f, 163f / 255f},
        //    new float[] {202f / 255f, 73f / 255f, 161f / 255f},
        //    new float[] {65f / 255f, 128f / 255f, 71f / 255f},
        //    new float[] {205f / 255f, 138f / 255f, 200f / 255f},
        //    new float[] {150f / 255f, 170f / 255f, 89f / 255f},
        //    new float[] {161f / 255f, 77f / 255f, 112f / 255f},
        //    new float[] {129f / 255f, 111f / 255f, 43f / 255f},
        //    new float[] {194f / 255f, 110f / 255f, 91f / 255f}
        //    };

        //    int[] order = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        //    for (int i = order.Length - 1; i > 0; i--)
        //    {
        //        int swapIndex = UnityEngine.Random.Range(0, i + 1);
        //        int tmp = order[i];
        //        order[i] = order[swapIndex];
        //        order[swapIndex] = tmp;
        //    }

        //    Renderer[] allChildren = GameObject.Find("in_play").GetComponentsInChildren<Renderer>();
        //    int counter = 0;
        //    foreach (Renderer child in allChildren)
        //    {

        //        if (child.name != "child")
        //        {
        //            continue;
        //        }

        //        Color cur_color = new Color(colors[order[counter]][0], colors[order[counter]][1], colors[order[counter]][2], 1);

        //        child.material.SetColor("_Color", cur_color);

        //        counter = counter + 1;

        //    }

        //}

        //void RandomizeTrialOrder()
        //{
        //    for (int i = trial_IDs.Length - 1; i > 0; i--)
        //    {
        //        int swapIndex = UnityEngine.Random.Range(0, i + 1);
        //        int tmp = trial_IDs[i];
        //        trial_IDs[i] = trial_IDs[swapIndex];
        //        trial_IDs[swapIndex] = tmp;
        //    }


        //}

        //void RandomizePracticeOrder()
        //{


        //    for (int i = practice_IDs_0.Length - 1; i > 0; i--)
        //    {
        //        int swapIndex = UnityEngine.Random.Range(0, i + 1);
        //        int tmp = practice_IDs_0[i];
        //        practice_IDs_0[i] = practice_IDs_0[swapIndex];
        //        practice_IDs_0[swapIndex] = tmp;
        //    }

        //    for (int i = practice_IDs_1.Length - 1; i > 0; i--)
        //    {
        //        int swapIndex = UnityEngine.Random.Range(0, i + 1);
        //        int tmp = practice_IDs_1[i];
        //        practice_IDs_1[i] = practice_IDs_1[swapIndex];
        //        practice_IDs_1[swapIndex] = tmp;
        //    }

        //    for (int i = practice_IDs_2.Length - 1; i > 0; i--)
        //    {
        //        int swapIndex = UnityEngine.Random.Range(0, i + 1);
        //        int tmp = practice_IDs_2[i];
        //        practice_IDs_2[i] = practice_IDs_2[swapIndex];
        //        practice_IDs_2[swapIndex] = tmp;
        //    }


        //}

        void ReadTrialInfo()
        {
            TextAsset ObjectList = Resources.Load<TextAsset>("ObjectList2");
            string[] objectlist = ObjectList.text.Split(new char[] { '\n' });
            for (int i = 1; i < objectlist.Length - 1; i++)
            {
                //Debug.Log("i = " + i.ToString());
                string[] row = objectlist[i].Split(new char[] { ',' });
                //Debug.Log(row[0]);
                Obj o = new Obj();
                o.name = row[0];
                int.TryParse(row[1], out o.id);
                o.AcceptGeons[0] = row[2];
                o.AcceptGeons[1] = row[3];
                o.AcceptGeons[2] = row[4];
                o.AcceptGeons[3] = row[5];
                o.AcceptGeons[4] = row[6];

                objs.Add(o);
            }

            TextAsset TrialList = Resources.Load<TextAsset>("TrialList2");
            string[] triallist = TrialList.text.Split(new char[] { '\n' });
            for (int i = 1; i < triallist.Length - 1; i++)
            {
                string[] row = triallist[i].Split(new char[] { ',' });
                Trial t = new Trial();
                int.TryParse(row[0], out t.TrialID);
                int.TryParse(row[1], out t.cur_object[0]);
                int.TryParse(row[2], out t.cur_object[1]);

                trials.Add(t);
            }
        }

        void CreateTargetModel()
        {
            if (cur_object_num == 0)
            {
                switch (objs[cur_object_ID].name)
                {
                    case "house":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(house_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "castle":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(castle_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "train":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(train_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "taxi":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(taxi_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "unicorn":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(unicorn_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "cat":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(cat_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "robot":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(robot_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "phone":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target0 = Instantiate(phone_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    default:
                        break;
                }

                target0.name = "target0";
                temp1 = model_locations[0];
                temp1.y += target0.GetComponent<Collider>().bounds.extents.y;
                target0.transform.position = temp1;
            }
            else
            {
                switch (objs[cur_object_ID].name)
                {
                    case "house":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(house_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "castle":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(castle_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "train":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(train_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "taxi":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(taxi_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "unicorn":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(unicorn_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "cat":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(cat_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "robot":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(robot_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    case "phone":
                        //Debug.Log(Initialize.objs[cur_object_ID].name);

                        target1 = Instantiate(phone_prefab, new Vector3(0, 0, 0), Quaternion.identity);

                        break;
                    default:
                        break;
                }

                target1.name = "target1";
                temp1 = model_locations[1];
                temp1.y += target1.GetComponent<Collider>().bounds.extents.y;
                target1.transform.position = temp1;
            }


        }

        public class Trial
        {
            public int TrialID;
            public int[] cur_object = new int[] { 0, 1 };

        }

        public class Obj
        {
            public string name;
            public int id;
            public string[] AcceptGeons = new string[] { "Geon1", "Geon2", "Geon3", "Geon4", "Geon5" };

        }

        IEnumerator waitShow()
        {
            //Wait for 2 seconds
            yield return new WaitForSeconds(2f);

            endtrialmenu.SetActive(true);

        }


        //void Update()
        //{
        //    if (trialActive == true)
        //    {
        //        timePassed += Time.deltaTime;
        //    }

        //    if (beepStatus == 0 && practice_num == 3 && (cur_trial_num == 11 || cur_trial_num == 35))
        //    {
        //        beepStatus = 1;
        //    }

        //    if (beepStatus == 1 && trialActive == true && timePassed > 5)
        //    {
        //        if (!BeepSource.isPlaying)
        //        {
        //            BeepSource.Play();
        //            beepOn = true;
        //        }

        //        //timePassed += Time.deltaTime;
        //        if (BeepSource.volume < .01f)
        //        {
        //            BeepSource.volume += (Time.deltaTime / 60) * .01f;
        //        }

        //        if (ControllerManager.Instance.GetButtonPressDown(Touchpad))
        //        {
        //            BeepSource.volume = 0;
        //            BeepSource.Stop();
        //            beepOn = false;
        //            beepStatus = 2;
        //            timePassed = 0;
        //        }
        //    }

        //    if (beepStatus == 2 && practice_num == 3 && cur_trial_num != 11 && cur_trial_num != 35)
        //    {
        //        beepStatus = 0;
        //    }


        //    if (animate == true)
        //    {

        //        if (Mathf.Abs(totMoved) < Mathf.Abs(dist.x))
        //        {
        //            temp1 = new Vector3(target0.transform.position.x + change_x * Time.deltaTime, target0.transform.position.y, target0.transform.position.z + change_z * Time.deltaTime);
        //            target0.transform.position = temp1;

        //            temp1 = new Vector3(target1.transform.position.x + change_x * Time.deltaTime, target1.transform.position.y, target1.transform.position.z + change_z * Time.deltaTime);
        //            target1.transform.position = temp1;

        //            totMoved += change_x * Time.deltaTime;
        //        }
        //        else
        //        {
        //            animate = false;
        //        }

        //    }
        //}

        void ConveyorSound()
        {
            AudioSource[] audioSources = GameObject.Find("Camera").GetComponents<AudioSource>();

            AudioSource source = audioSources[2];
            source.Play();
        }

        public void End()
        {
            //Debug.Log("quitting");
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }

    }
}





