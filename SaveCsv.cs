using UnityEngine;
using System.IO;


public static class SaveCsv
{

    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[26] {
        "Item",
        "Duration",
        "Time",
        "Converg_distance",
        "Converg_valid",
        "Gaze_valid",
        "Local_origin_x",
        "Local_origin_y",
        "Local_origin_z",
        "World_origin_x",
        "World_origin_y",
        "World_origin_z",
        "Local_gaze_x",
        "Local_gaze_y",
        "Local_gaze_z",
        "World_gaze_x",
        "World_gaze_y",
        "World_gaze_z",
        "Head_origin_x",
        "Head_origin_y",
        "Head_origin_z",
        "Head_gaze_x",
        "Head_gaze_y",
        "Head_gaze_z",
        "Left_eye",
        "Right_eye",
    };

    // timestamp
    private static string timeStampHeader = " ";

    //public static void checker()
    //{
    //    Debug.Log("check");
    //}

    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory(); //check dir & file exist
        VerifyFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
            finalString += reportSeparator;
            sw.WriteLine(finalString);
        }
    }

    public static void BackupNote()
    {
        using(StreamWriter sw = File.AppendText(GetFilePath()))
        {
            sw.WriteLine("Backup");
        }
    }

    public static void PauseNote()
    {
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            sw.WriteLine("Pause Note");
        }
    }

    public static void ResumeNote()
    {
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            sw.WriteLine("Resume Note");
        }
    }

    public static void CreateReport()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = ""; //starts empty
            for (int i = 0; i < reportHeaders.Length; i++) //loop through reportHeaders
            {
                if (finalString != "")
                {
                    finalString += reportSeparator; // gets the seperator added
                }
                finalString += reportHeaders[i];
            }
            finalString += reportSeparator + timeStampHeader; //adding the timestamp (might be redudant) //don't need
            sw.WriteLine(finalString);
        }
    }

    // helper methods below
    public static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
    }

    public static void VerifyFile() 
    {
        string file = GetFilePath();
        if (!File.Exists(file)) {
            CreateReport();
        }
    }

    public static string GetDirectoryPath() {
        return Application.dataPath + '/' + reportDirectoryName;
    }

   public static string GetFilePath() {
        return GetDirectoryPath() + '/' + reportFileName;
    }

    //static string GetTimeStamp()
    //{
    //    return System.DateTime.Now.ToString("hh.mm.ss.ff");
    //}
}

