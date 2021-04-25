using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class Model : MonoBehaviour
{
    public static Dictionary<string, List<EyeTrackerPoint>> AllEyeInfo =
        new Dictionary<string, List<EyeTrackerPoint>>(); // dictonary enthält alle identifier + liste der punkte

    //is needed to test
    private static List<EyeTrackerPoint> testList = new List<EyeTrackerPoint>();
    public static string str, i;
    public static string[] coll;
    public static List<string> identifier = new List<string>();
    public static List<string> readyIdentifier = new List<string>();
    public GameObject rdyButt;

    public GameObject startUI1;

    public GameObject gameOverGameObject;

    public static bool gazeVisualization;

    public static Dictionary<string, GameObject> gaze = new Dictionary<string, GameObject>();


    public static float flX, flY;
    public static long lo;

    public TMP_Text playerAmountText;
    public TMP_Text playerAmountReadyText;

    private void Start()
    {
        GameEvents.current.updatePlayerTextEvent += updatePlayAmountText;
        GameEvents.current.updatePlayerReadyTextEvent += updatePlayerReadyText;
        GameEvents.current.disableDeletedGazePointEvent += disableGazePoint;
    }

    public static IEnumerator updateModel(string s)
    {
        coll = s.Split('-');
        i = coll[0]; // current identifier


        try
        {
        float.TryParse(coll[1], System.Globalization.NumberStyles.Any, 
            System.Globalization.CultureInfo.InvariantCulture, out flX);

        float.TryParse(coll[2], System.Globalization.NumberStyles.Any,
           System.Globalization.CultureInfo.InvariantCulture, out flY); 

        long.TryParse(coll[3], System.Globalization.NumberStyles.Any, 
           System.Globalization.CultureInfo.InvariantCulture, out lo);// systemzeit in millisekunden
        }
        catch(Exception e)
        {
            print(e);
            print("Exception bei dem String:" + s);
        }

        // if key does not already exitst/is part of dict
        if (!AllEyeInfo.TryGetValue(i, out testList)) 
        {
            //ad new key to dictionary of identifiers linked to list of eye tracker points
            AllEyeInfo.Add(i, new List<EyeTrackerPoint>());
            // to vizualize gaze point
            GameObject newGO = GameObject.Find("watch");            
            GameObject o = new GameObject();
            o.AddComponent<SpriteRenderer>().sprite = GameObject.Find("Model").GetComponent<SpriteRenderer>().sprite;
            gaze.Add(i, o);
            gaze[i].SetActive(false);
            //gaze[i].SetActive(false);
            //add to list of identifiers
            identifier.Add(i);
            //add identifier to list of last overlay points with dummy eye trackerpoint
            //GameLogic.lastOverlayGazePoints.Add(i,new EyeTrackerPoint(10000,10000));
            print("Added new Identifier:" + i);
            GameLogic.lastOverlayGazePoints.Add(i, new EyeTrackerPoint(1000,1000));
            GameEvents.current.newupdatePlayerTextEventMethodMainThread();
        }

        
        //access via dictionay to list of eye tracker points
        //adds new point at the end
        AllEyeInfo[i].Add(new EyeTrackerPoint(flX, flY, lo));

        //as we save the last ten points
        //we delete the first element 
        if (AllEyeInfo[i].Count > 10) 
        {
            //when first element is deleted all others move one spot prior
            AllEyeInfo[i].RemoveAt(0); 
        }
        if (SymbolHandler.dontcheckforoverlays)
        {
            GameLogic.checkForOverlay();
        }

        updateGazePointVisualization(i);

        yield return null;
    }

    private void updatePlayerReadyText()
    {
        playerAmountReadyText.text = readyIdentifier.Count.ToString();
        //when should button appear? when all clients are registered, should be =3
        
        if (Model.readyIdentifier.Count == Model.identifier.Count)
        {
                       
            startUI1.SetActive(false);
            //start thread counting down from 5
            gameOverGameObject.SetActive(true);
        }
    }

    /*
    private void countdown()
    {
        for (int i = 0; i < 6; i++)
        {
            Thread.Sleep(1000);

        }
    }*/



    private void disableGazePoint()
    {
        gaze[SymbolHandler.helpToDeleteId].SetActive(false);
        gaze.Remove(SymbolHandler.helpToDeleteId);
    }

    private void updatePlayAmountText()
    {
        playerAmountText.text = identifier.Count.ToString();

        if (Model.identifier.Count == 1)
        {
            rdyButt.SetActive(true);
        }
    }




    public static void updateGazePointVisualization(string identifier)
    {
        gaze[identifier].transform.position = new Vector2(0,0);
        gaze[identifier].transform.position = new Vector2(Model.AllEyeInfo[identifier][Model.AllEyeInfo[identifier].Count - 1].getX(),
            Model.AllEyeInfo[identifier][Model.AllEyeInfo[identifier].Count - 1].getY());
    }


    public void Toggle_Changed(bool newValue)
    {
        foreach (KeyValuePair<string, GameObject> entry in gaze)
        {
            entry.Value.SetActive(newValue);
        }
    }
}


