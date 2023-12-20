using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrackAOI : MonoBehaviour
{
    public TrackAtGaze[] Item; //get component at trackatgaze.item
    public bool checker; // default is private
    public List<bool> boolList = new List<bool>();
    public List<string> data = new List<string>();

    private IEnumerator ControlRate()
    {
        yield return new WaitForSeconds(1f); //change to 0.5f for 0.5seconds
        checker = true;
    }

    private void Start()
    {
        StartCoroutine(ControlRate());
    }
    // Update is called once per frame

    void Update()
    {
        if (checker) //checker is true bc checker is changed to true at line 15
        {
            // CHANGE
            for (int i = 0; i < Item.Length; i++)
            {
                boolList.Add(Item[i].islooking);
            }

            if (boolList.All(a => !a))
            {
                data.Add("X");
                //this.GetComponent<EyeTrackingResult>().data.Add("X");
            }

            else
            {
                int ind = boolList.IndexOf(true);
                data.Add(Item[ind].name);
                //this.GetComponent<EyeTrackingResult>().data.Add(Item[f].name);
            }

            boolList.Clear();
            checker = false;
            StartCoroutine(ControlRate());
        }
    }

    //void checkAOI()
    //{
    //    bool hehe = false;
    //    for (int i = 0; i < Item.Length; i++)
    //    {
    //        if (Item[i].islooking == true)
    //        {
    //            this.GetComponent<EyeTrackingResult>().data.Add(Item[i].name);
    //            hehe = true;
    //            continue;
    //        }
    //        if (i == Item.Length - 1 && !hehe)
    //        {
    //            this.GetComponent<EyeTrackingResult>().data.Add("X");
    //        }
    //    }
    //}
}
