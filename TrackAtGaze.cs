using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;

public class TrackAtGaze : MonoBehaviour, IGazeFocusable
{
    public bool islooking = false;
    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            islooking = true;
            //Debug.Log("looking at" + gameObject.name);
            //this.GetComponent<MeshRenderer>().material = Material.Color;
            // Debug.Log(this.name); //printing only if GameObject is viewed
            //this.GetComponent<Renderer>().material.color = Color.red;
        }

        else
        {
            islooking = false;
            //this.GetComponent<Renderer>().material.color = Color.black;
        }
    }
}