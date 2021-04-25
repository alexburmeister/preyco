using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.IO;


public class GameLogic : MonoBehaviour
{    
    public static List<GameObject> overlaySignal = new List<GameObject>();
    public static List<GameObject> overlaySignalGreen = new List<GameObject>();

    public static StreamWriter streamWriter;

    public GameObject[] game1overlayGreen;
    public GameObject[] game1overlay;

    public GameObject[] game2overlayGreen;
    public GameObject[] game2overlay;


    public GameObject[] game3overlayGreen;
    public GameObject[] game3overlay;

    public GameObject[] game4overlayGreen;
    public GameObject[] game4overlay;


    public Dictionary<int, GameObject[]> gameOverlayDict = new Dictionary<int, GameObject[]>();
    public Dictionary<int, GameObject[]> gameOverlayGreenDict = new Dictionary<int, GameObject[]>();

    public static Symbol lastOverlay; // store last overlay point

    //stores the exact gaze point from each identifier when an overlay is called
    public static Dictionary<string, EyeTrackerPoint> lastOverlayGazePoints= new Dictionary<string,EyeTrackerPoint>();

    public GameObject gameOverGameObject;

    public Transform trans; // drag and drop GameLogic

    public static long sysTime;
    public static double time;

    public int overlayTime; // in milliseconds

    public static System.DateTime start2020;
    public static System.TimeSpan ts;

    public TMP_Text te;
    public TMP_Text tex;

    // Start is called before the first frame update
    void Start()
    {
        //lastOverlay = new Symbol(10000000,1000000);
        start2020 = new System.DateTime(2020, 1, 1, 0, 0, 0,0);
        
        gameOverlayDict.Add(1, game1overlay);
        gameOverlayGreenDict.Add(1, game1overlayGreen);
        gameOverlayDict.Add(2, game2overlay);
        gameOverlayGreenDict.Add(2, game2overlayGreen);
        gameOverlayDict.Add(3, game3overlay);
        gameOverlayGreenDict.Add(3, game3overlayGreen);
        gameOverlayDict.Add(4, game4overlay);
        gameOverlayGreenDict.Add(4, game4overlayGreen);

        string helpStringDate = DateTime.Now.ToString("ddMMHHmm");
        var systemPath = Directory.GetCurrentDirectory();
        streamWriter = new StreamWriter(Path.Combine(systemPath, helpStringDate + "Program" + ".csv"));

        GameEvents.current.newOverlayEvent += displayOverlay;
        GameEvents.current.newLongOverlayEvent += displayLongOverlay;
        //GameEvents.current.newCountingEvent += checkForLongOverlay;

        
    }




    //started as a thread with parameter of overlay
    private void startCountingThenCheckForLongOverlay(Symbol symbolPassed, Dictionary<string,EyeTrackerPoint> dictPassed)
    {
        print("Start Count Thread.");
        //wait for seconds to display long overlay
        Thread.Sleep(2000);

        if (symbolPassed == lastOverlay)
        {
            //GameEvents.current.newCountingEventMethodMainThread();

            int j = 0;

            if (checkSymbolForMoreOverlays2(lastOverlay.getx(), lastOverlay.gety()))
            { 
                foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
                {
                     j++;
                }
            }

            if (j == Model.AllEyeInfo.Count)
            {
                string se = "" + lastOverlay.getx() + "@" + lastOverlay.gety() + "@" + lastOverlay.getz() + "@" + lastOverlay.gett() + "\n";
                byte[] hit = System.Text.Encoding.ASCII.GetBytes(se);
                main.handler.Send(hit);
                print("Long overlay info sent.");
            }
        }
    }


    /*
    private void checkForLongOverlay()
    {
        int j = 0;
        foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
        {
            if (System.Math.Round(w.Value[w.Value.Count - 1].getX(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getX(), 5) &&
                System.Math.Round(w.Value[w.Value.Count - 1].getY(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getY(), 5))
            {

                j++;
            }
        }
                
        if (j == Model.AllEyeInfo.Count)
        {
            string se = "" + lastOverlay.getx() + "@" + lastOverlay.gety() + "@" + lastOverlay.getz() + "@" + lastOverlay.gett() + "\n";
            byte[] hit = System.Text.Encoding.ASCII.GetBytes(se);
            main.handler.Send(hit);
            print("Long overlay info sent.");
        }
    }*/


    private void displayOverlay()
    {
        //delete all overlays and then clear list   
        //so only last overlay is shown as red symbol
        foreach (GameObject g in overlaySignal)
        {
            Destroy(g);
        }

        foreach (GameObject g in overlaySignalGreen)
        {
            Destroy(g);
        }


        StringBuilder buildToWrite = new StringBuilder();
        //identifier;type;gameNumber;symbolPositionX,symbolPositionY;
        DateTime dateEyeTracking = DateTime.Now;
        buildToWrite.Append(SymbolHandler.ownIdentifier + ";" +  dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayOverlay;;" + SymbolHandler.receivedGameParameters[1] + ";;;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");

        if (SymbolHandler.showVisualHints)
        {
            overlaySignal.Add((GameObject)Instantiate(gameOverlayDict[System.Int32.Parse(SymbolHandler.receivedGameParameters[1])][System.Int32.Parse(SymbolHandler.xyzOverlay[2])],
                new Vector2(System.Int32.Parse(SymbolHandler.xyzOverlay[0]),
                System.Int32.Parse(SymbolHandler.xyzOverlay[1])), trans.rotation * Quaternion.Euler(0f, 0f, (float)System.Int32.Parse(SymbolHandler.xyzOverlay[4]))));

            //destroy red overlay after x seconds
            Destroy(overlaySignal[overlaySignal.Count - 1], 4);
        }

        lastOverlay = new Symbol(System.Int32.Parse(SymbolHandler.xyzOverlay[0]), System.Int32.Parse(SymbolHandler.xyzOverlay[1]), System.Int32.Parse(SymbolHandler.xyzOverlay[2]), System.Int32.Parse(SymbolHandler.xyzOverlay[4]), System.Int64.Parse(SymbolHandler.xyzOverlay[3]));
        //update gaze points from last overlay

        int f = 5; //because first 4 parameters are with symbols

        //print("länge: " + SymbolHandler.xyzOverlay.Length);

        while (f < (SymbolHandler.xyzOverlay.Length - 1))
        {
            lastOverlayGazePoints[SymbolHandler.xyzOverlay[f+2]] = new EyeTrackerPoint(float.Parse(SymbolHandler.xyzOverlay[f]), float.Parse(SymbolHandler.xyzOverlay[f+1]));
            //identifier;eyepoitX,eyepointY
            buildToWrite.Append(SymbolHandler.xyzOverlay[f + 2] + ";" + SymbolHandler.xyzOverlay[f] + ";" + SymbolHandler.xyzOverlay[f+1] + ";" + EyeTrackerPoint.inverseX(float.Parse(SymbolHandler.xyzOverlay[f])) + ";" + EyeTrackerPoint.inverseY(float.Parse(SymbolHandler.xyzOverlay[f+1])) + ";");
            f = f + 3;
        }

        streamWriter.WriteLine(buildToWrite.ToString());
        streamWriter.Flush();

        Thread thread = new Thread(() => startCountingThenCheckForLongOverlay(lastOverlay,lastOverlayGazePoints));
        thread.Start();
    }



    private void displayLongOverlay()
    {
        if (SymbolHandler.showVisualHints)
        {
            overlaySignalGreen.Add((GameObject)Instantiate(gameOverlayGreenDict[System.Int32.Parse(SymbolHandler.receivedGameParameters[1])][System.Int32.Parse(SymbolHandler.hitter[2])],
                new Vector2(System.Int32.Parse(SymbolHandler.hitter[0]),
                System.Int32.Parse(SymbolHandler.hitter[1])), trans.rotation * Quaternion.Euler(0f, 0f, (float)System.Int32.Parse(SymbolHandler.hitter[3]))));
        }    

        checkForCorrectSolution();
    }

    private void checkForCorrectSolution()
    {
        //hitter 2 ist der wert des symbols
        if (System.Int32.Parse(SymbolHandler.hitter[2]) < 10)
        {
            if (System.Int32.Parse(SymbolHandler.hitter[2]) == 3 || System.Int32.Parse(SymbolHandler.hitter[2]) == 4 || System.Int32.Parse(SymbolHandler.hitter[2]) == 5) 
            {
                StringBuilder buildToWrite = new StringBuilder();
                //identifier;type;gameNumber;symbolPositionX,symbolPositionY;
                DateTime dateEyeTracking = DateTime.Now;
                buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayLongOverlay;" + "CorrectSolution;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");
                streamWriter.WriteLine(buildToWrite.ToString());
                streamWriter.Flush();

                print("right solution found");
                te.text = "CONGRATS!";
                tex.text = "Next Round!";
                GameEvents.current.newdeleteEventMethodMainThread();

               
            }
            else
            {
                if (SymbolHandler.identifierOrder.Count == 1) 
                {
                    foreach (KeyValuePair<string,string> q in SymbolHandler.identifierOrder)
                    {
                        if (System.Int32.Parse(q.Value) == 0)
                        {
                            if (System.Int32.Parse(SymbolHandler.hitter[2]) == 6 || System.Int32.Parse(SymbolHandler.hitter[2]) == 7)
                            {
                                print("right solution found");
                                StringBuilder buildToWrite = new StringBuilder();
                                DateTime dateEyeTracking = DateTime.Now;
                                buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayLongOverlay;" + "WrongSolution;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");
                                streamWriter.WriteLine(buildToWrite.ToString());
                                streamWriter.Flush();
                            }
                        }

                        if (System.Int32.Parse(q.Value) == 1)
                        {
                            if (System.Int32.Parse(SymbolHandler.hitter[2]) == 8 || System.Int32.Parse(SymbolHandler.hitter[2]) == 9)
                            {
                                print("right solution found");
                                StringBuilder buildToWrite = new StringBuilder();
                                DateTime dateEyeTracking = DateTime.Now;
                                buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayLongOverlay;" + "WrongSolution;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");
                                streamWriter.WriteLine(buildToWrite.ToString());
                                streamWriter.Flush();
                            }
                        }

                        if (System.Int32.Parse(q.Value) == 2)
                        {
                            if (System.Int32.Parse(SymbolHandler.hitter[2]) == 10 || System.Int32.Parse(SymbolHandler.hitter[2]) == 11)
                            {
                                print("right solution found");
                                StringBuilder buildToWrite = new StringBuilder();
                                DateTime dateEyeTracking = DateTime.Now;
                                buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayLongOverlay;" + "WrongSolution;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");
                                streamWriter.WriteLine(buildToWrite.ToString());
                                streamWriter.Flush();
                            }
                        }
                    }

                }
            }
        }

        else 
        {
            print("wrong solution");
            StringBuilder buildToWrite = new StringBuilder();
            DateTime dateEyeTracking = DateTime.Now;
            buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplayLongOverlay;" + "WrongSolution;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";;" + SymbolHandler.xyzOverlay[0] + ";" + SymbolHandler.xyzOverlay[1] + ";");
            streamWriter.WriteLine(buildToWrite.ToString());
            streamWriter.Flush();
        }


    }


    public static bool checkSymbolForMoreOverlays(int x, int y)
    {
        int count = 0;

        foreach (KeyValuePair<string, List<EyeTrackerPoint>> entry in Model.AllEyeInfo)
        {
            //System.Math.Round
            if ((x - entry.Value[entry.Value.Count - 1].getX()) < 2 && 
                (x - entry.Value[entry.Value.Count - 1].getX()) > -2 &&
                (y - entry.Value[entry.Value.Count - 1].getY()) < 2 && 
                (y - entry.Value[entry.Value.Count - 1].getY()) > -2)
            {                
                count++;
            }
        }

        //
        //after every identifier is checked, checks if we have an overlay of all identifiers
        //if (count == Model.identifier.Count)
        if (Model.identifier.Count == 1)
        {
            if (count == Model.identifier.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (count >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public static bool checkSymbolForMoreOverlays2(int x, int y)
    {
        int count = 0;

        foreach (KeyValuePair<string, List<EyeTrackerPoint>> entry in Model.AllEyeInfo)
        {
            //System.Math.Round
            if ((x - entry.Value[entry.Value.Count - 1].getX()) < 2 &&
                (x - entry.Value[entry.Value.Count - 1].getX()) > -2 &&
                (y - entry.Value[entry.Value.Count - 1].getY()) < 2 &&
                (y - entry.Value[entry.Value.Count - 1].getY()) > -2)
            {
                count++;
            }
        }

        //
        //after every identifier is checked, checks if we have an overlay of all identifiers
        //if (count == Model.identifier.Count)

        if (count == Model.identifier.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static void checkForOverlay()
    {
        if (SymbolHandler.SymbolAreCurrentlyDisplayed)
        {
            foreach (KeyValuePair<string, List<EyeTrackerPoint>> entry in Model.AllEyeInfo)
            {
                foreach (Symbol s in SymbolHandler.symbolReceivedList)
                {
                    //check if we have an overlay of symbol and eyetrackeing point in specific range -+2
                    if ((s.getx() - entry.Value[(entry.Value.Count - 1)].getX()) < 2 &&
                        (s.getx() - entry.Value[(entry.Value.Count - 1)].getX()) > -2 &&
                        (s.gety() - entry.Value[(entry.Value.Count - 1)].getY()) < 2 &&
                        (s.gety() - entry.Value[(entry.Value.Count - 1)].getY()) > -2)
                    {
                        //we found a match of a (last) gazepoint and a symbol
                        // now check if other gazepoints match for the same symbol
                        if (checkSymbolForMoreOverlays(s.getx(), s.gety()))
                        {
                            long currentSysTime = (long)System.DateTime.UtcNow.Subtract(start2020).TotalMilliseconds;
                            string notifyString = "" + s.getx() + "&" + s.gety() + "&" + s.getz() + "&" + currentSysTime + "&" + s.gett();

                            foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
                            {
                                notifyString += "&" + w.Value[w.Value.Count - 1].getX() + "&" + w.Value[w.Value.Count - 1].getY() + "&" + w.Key;
                            }

                            notifyString += "&\n";
                            print("notify string" + notifyString);

                            //send to all other clients
                            byte[] bbb = System.Text.Encoding.ASCII.GetBytes(notifyString);
                            main.handler.Send(bbb);
                            print("Overlay info sent for symbol x: " + s.getx() + " and y:" + s.gety());   
                        }
                    }
                }
            }
        }
    }
        

    

}


/*private bool checkPassedTime()
{
    bool b;
    try
    {
        b = ((long)System.DateTime.UtcNow.Subtract(start2020).TotalMilliseconds - lastOverlay.getTime()) > 5000;
    }
    catch (Exception e)
    {
        b = false;
    }
    return b;
}*/


/*
//check if eye tracking points overlay with symbol and other eye tracking points
bool checkOverlay(int x, int y, long z)
{
    int count = 0;        

    foreach (KeyValuePair<string, List<EyeTrackerPoint>> entry in Model.AllEyeInfo)
    {
        bool check = false;
        // checks every stored points of the respective identifier if we got an overlay
        // also time delay is considered
        foreach (EyeTrackerPoint e in entry.Value){
            //overlay within a timespan of 6 seconds defined, might be too much
            if((e.getTime() - z) < 3000 && (e.getTime() - z > -3000))
            {
                if ((x - e.getX()) < 2 && (x - e.getX()) > -2 &&
                    (y - e.getY()) < 2 && (y - e.getY()) > -2)
                {
                    check = true;
                }
            }
        }
        // if we have a minimum of one points that has a overlay then check = true 
        // and we increae the count 
        if (check)
        {
            count++;
        }
    }

    //after every identifier is checked, check if we have an overlay of all identifiers
    if (count == Model.identifier.Count)
    {
        return true;
    }
    else
    {
        return false;
    }

}*/



/*
// Update is called once per frame
void Update() //iteriere über alle gameobjekte und erneuer die position
{

    //tickcount was before
    //check if five seconds have passed since overlay


    /*
    if (checkPassedTime())
    {
        if (checkSymbolForMoreOverlays(lastOverlay.getx(),lastOverlay.gety()))
        {
            int j = 0;
            foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
            {

                if (System.Math.Round(w.Value[w.Value.Count - 1].getX(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getX(), 5) &&
                    System.Math.Round(w.Value[w.Value.Count - 1].getY(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getY(), 5))
                {
                    j++;
                }
            }


            if (j == Model.AllEyeInfo.Count)
            {
                string se = "" + lastOverlay.getx() + "@" + lastOverlay.gety() + "@" + lastOverlay.getz() + "@" + lastOverlay.gett() + "\n";
                byte[] hit = System.Text.Encoding.ASCII.GetBytes(se);
                main.handler.Send(hit);
                //currOv = false;
                print("green overlay sent");
                //SymbolHandler.newOverl = false;
            }

        }
        //currentLongOverlayDisplayed = true;
    }   /*
         * 
         * 
         * 
         * 

    /*
    //is called when we receive a new long overlay to finish game
    if (receiveHit) //for long green overlay
    {

        displayLongOverlay();
        SymbolHandler.newOverl = false;
    }*/



/*
//is called when we receive a new overlay
if (SymbolHandler.newOverl)
{
    senderhelp = true;            
    //delete all overlays and then clear list   
    //so only last overlay is shown as red symbol
    foreach (GameObject g in GameLogic.overlaySignal)
    {
        Destroy(g);
    }

    overlaySignal.Add((GameObject)Instantiate(game1overlay[System.Int32.Parse(SymbolHandler.xyzOverlay[2])], 
        new Vector2(System.Int32.Parse(SymbolHandler.xyzOverlay[0]),
        System.Int32.Parse(SymbolHandler.xyzOverlay[1])),trans.rotation * Quaternion.Euler(0f, 0f,(float)System.Int32.Parse(SymbolHandler.xyzOverlay[4]))));

    lastOverlay = new Symbol(System.Int32.Parse(SymbolHandler.xyzOverlay[0]), System.Int32.Parse(SymbolHandler.xyzOverlay[1]), System.Int32.Parse(SymbolHandler.xyzOverlay[2]), System.Int32.Parse(SymbolHandler.xyzOverlay[4]));

    //update gaze points from last overlay
    int f = 5;
    lastOverleyEye.Clear();


    while (f < SymbolHandler.xyzOverlay.Length)
    {         
        lastOverleyEye.Add(new EyeTrackerPoint(float.Parse(SymbolHandler.xyzOverlay[f]), float.Parse(SymbolHandler.xyzOverlay[f+1])));
        f = f + 2;
    }



    //destroy red overlay after x seconds
    Destroy(overlaySignal[overlaySignal.Count - 1], 7);

    //sysTime = System.Environment.TickCount;
    ts = System.DateTime.UtcNow.Subtract(start2020);
    sysTime = (long)ts.TotalMilliseconds;

    numberP.Clear();
    foreach (KeyValuePair<string, List<EyeTrackerPoint>> q in Model.AllEyeInfo)
    {
        numberP.Add(q.Value.Count);
    }
}*/





/*
if (Model.identifier.Count > 0) // wenn es identifier gibt dann gibts auch eyetracking pooint
{       
        if (gaze.Count < Model.identifier.Count) 
        {
            gaze.Add(new GameObject()); // füge für jeden client ein gameobject zur list hinzu
            gaze[Model.identifier.Count - 1].AddComponent<SpriteRenderer>().sprite = a1; // visualisiere gameobjekt mit sprite
        }

        int c = 0;  

        foreach (KeyValuePair<string, List<EyeTrackerPoint>>  entry in Model.AllEyeInfo)
        {
            //vizalize new eye position
            gaze[c].transform.position = new Vector2(entry.Value[(entry.Value.Count - 1)].getX(),
                entry.Value[(entry.Value.Count - 1)].getY());


            foreach (Symbol s in SymbolHandler.symbolReceivedList)
            {
                //check if we have an overlay of symbol and eyetrackeing point in specific range -+2
                if ((s.getx() - entry.Value[(entry.Value.Count - 1)].getX()) < 2 &&
                    (s.getx() - entry.Value[(entry.Value.Count - 1)].getX()) > -2 &&
                    (s.gety() - entry.Value[(entry.Value.Count - 1)].getY()) < 2 &&
                    (s.gety() - entry.Value[(entry.Value.Count - 1)].getY()) > -2)
                {

                    //we identified a symbol, with a current overlay from a gazepoint
                    if (checkSymbolForMoreOverlays(s.getx(), s.gety()))
                    {
                        foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
                        {
                            if(System.Math.Round(w.Value[w.Value.Count - 1].getX(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getX(), 5) &&
                                System.Math.Round(w.Value[w.Value.Count - 1].getY(), 5) == System.Math.Round(lastOverlayGazePoints[w.Key].getY(), 5))
                            {
                                gazeStillSame = false;
                            }

                    }


                        //if (lastOverlay.getx() != s.getx() && lastOverlay.gety() != s.gety())
                        if (gazeStillSame && senderhelp)
                        {

                            long currentSysTime = (long)System.DateTime.UtcNow.Subtract(start2020).TotalMilliseconds;
                            string notifyString = "" + s.getx() + "&" + s.gety() + "&" + s.getz() + "&" + currentSysTime + "&" + s.gett();


                            foreach (KeyValuePair<string, List<EyeTrackerPoint>> w in Model.AllEyeInfo)
                            {

                                notifyString += "&" + w.Value[w.Value.Count - 1].getX() + "&" + w.Value[w.Value.Count - 1].getY();
                            }

                            notifyString += "\n";
                            print("notify string" + notifyString);

                            senderhelp = false;


                            byte [] bbb = System.Text.Encoding.ASCII.GetBytes(notifyString);
                            main.handler.Send(bbb);
                            print("Overlay info sent for symbol x: " + s.getx() + " and y:" + s.gety());    
                        }                                

                    }
                }


            }

            c++;
        }


    gazeStillSame = true;
     }


} */