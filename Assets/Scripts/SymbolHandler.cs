using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;
using System.Threading;



public class SymbolHandler : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject[] spawnee;

    public static string ownIdentifier;
    public static bool GameOverIsDisplayed;

    public static int currentCorrectSolution;

    public static string helpToDeleteId;

    public static bool showVisualHints;

    //public static List<Symbol> symbolReceivedList = new List<Symbol>(); //positions of symbols;
    public static List<Symbol> symbolReceivedList = new List<Symbol>(); //positions of symbols;
    public static List<Symbol> symbolSendList = new List<Symbol>(); //positions of symbols;
    public static Dictionary<string, string> identifierOrder = new Dictionary<string, string>(); //store order of identifiers to allocate basic symbols
    public static List<int> randomOrderInteger = new List<int>();

    //liste an gameobjects die als klone geschaffen wurden
    public static List<GameObject> clone = new List<GameObject>();

    int helpIntforOwnIdentifier;

    public static int selectGame;

    int randomInt;
    int randomInt2;
    int randomInt3;
    static int anzahlSymbols; // legt fest wieviele symbole genereriert werden
    string symbolString;

    public static string[] allIcomingSymbolPoints;
    public static string[] splittedInSymbolsAndIdentifiers;
    public static string[] onlyIdentifers;
    public static string[] xyzOverlay;
    public static string[] hitter;
    //public static bool newSymbolPoints;
    //public static bool newOverl;

    public GameObject gameOverGameObject;

    public static bool SymbolAreCurrentlyDisplayed;

    public static bool dontcheckforoverlays;

    //here are real playing symbols
    public GameObject[] game1;
    public GameObject[] game2;
    public GameObject[] game3;
    public GameObject[] game4;

    public static string[] receivedGameParameters;

    public Dictionary<int, GameObject[]> gameDict = new Dictionary<int, GameObject[]>();

    //public static List<GameObject> game1List = new List<GameObject>();


    StringBuilder build; // hilfe um an andere clients zu senden


    // Update is called once per frame
    void Start()
    {
        dontcheckforoverlays = true;
        anzahlSymbols = 17;
        SymbolAreCurrentlyDisplayed = false;
        GameEvents.current.newSymbolPointsEvent += displaySymbols;
        GameEvents.current.genANDsendEvent += genANDsend;
        GameEvents.current.deleteEvent += deleteAllSymbols;

        gameDict.Add(1, game1);
        gameDict.Add(2, game2);
        gameDict.Add(3, game3);
        gameDict.Add(4, game4);


        //newSymbolPoints = false;
        //newOverl = false;
        //game1List.Add(GameObject.Find("game1_basic1"));
    }



    void Update()
    {
        //generates symbol points and sends them dont display themm
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            foreach (GameObject g in clone)
            {
                Destroy(g);
            }
            clone.Clear();

            //delete all red overlays and then clear list        
            foreach (GameObject g in GameLogic.overlaySignal)
            {
                Destroy(g);
            }
            GameLogic.overlaySignal.Clear();

            //delete all green overlays and then clear list        
            foreach (GameObject g in GameLogic.overlaySignalGreen)
            {
                Destroy(g);
            }
            clone.Clear();
            GameLogic.overlaySignalGreen.Clear();

            generateSymbolPoints();
            sendSymbolPoints();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (dontcheckforoverlays) 
            { 
                dontcheckforoverlays = false;
            }
            else
            {
                dontcheckforoverlays = true;
            }


        }




        /*
        //Display them after the symols have been sent through ptp
        if (newSymbolPoints)
        {
            recreateSymbolsNew();
            print("Updated symbols.");
            newSymbolPoints = false;
            SymbolAreCurrentlyDisplayed = true;
        }*/


    }
    public void genANDsend()
    {
        
        generateSymbolPoints();
        sendSymbolPoints();
    }

    public void generateSymbolPoints()
    {
        symbolSendList.Clear();
        //chooses from nofits
        int id = 0; // zum zählen
        while (id < (anzahlSymbols - 7)) // kreiert auf einemal 25 icons
        {
            do
            {
                randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
                randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
            } while (randomInt2 > 10 && randomInt3 < -3);

            int i = 0;
            int a;
            while (i == 0)
            {
                a = 0;

                foreach (Symbol p in symbolSendList) // checke auf overlay EIGENTLICH müsste kreisformel dastehen, noch einbauen
                {

                    while ((p.getx() - randomInt2) < 3.3 && (p.getx() - randomInt2) > -3.3 &&
                        (p.gety() - randomInt3) < 3.3 && (p.gety() - randomInt3) > -3.3)
                    {
                        do
                        {
                            randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
                            randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
                        } while (randomInt2 > 10 && randomInt3 < -3);
                        a = -1;
                        //print(a); // to check how often to loops repeats to find the nexst valid point
                    }
                }

                if (a == 0)
                {
                    i = -1;
                }

            }

            //wählt spiel aus // max is exclusive
            selectGame = Random.Range(1, 5);

            randomInt = Random.Range(10, gameDict[selectGame].Length); // wählt symbol aus
            symbolSendList.Add(new Symbol(randomInt2, randomInt3, randomInt,Random.Range(0,360)));
            id++;
        }

        //chooses from basic symbols
        int id2 =6; // zum zählen
        while (id2 < 12) // kreiert auf einemal 25 icons
        {
            do
            {
                randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
                randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
            } while (randomInt2 > 10 && randomInt3 < -3);

            int i = 0;
            int a;
            while (i == 0)
            {
                a = 0;

                foreach (Symbol p in symbolSendList) // checke auf overlay EIGENTLICH müsste kreisformel dastehen, noch einbauen
                {

                    while ((p.getx() - randomInt2) < 3.2 && (p.getx() - randomInt2) > -3.2 &&
                        (p.gety() - randomInt3) < 3.2 && (p.gety() - randomInt3) > -3.2)
                    {
                        do
                        {
                            randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
                            randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
                        } while (randomInt2 > 10 && randomInt3 < -3);
                        a = -1;
                        //print(a); // to check how often to loops repeats to find the nexst valid point
                    }
                }

                if (a == 0)
                {
                    i = -1;
                }

            }

            symbolSendList.Add(new Symbol(randomInt2, randomInt3, id2,Random.Range(0, 360)));

            id2++;
        }
        //max value is exclusive
        int id3 = Random.Range(3,6);

        do
        {
            randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
            randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
        } while (randomInt2 > 10 && randomInt3 < -3);

        int a1;
        int i1 = 0;

        while (i1 == 0)
        {
            a1 = 0;

            foreach (Symbol p in symbolSendList) // checke auf overlay EIGENTLICH müsste kreisformel dastehen, noch einbauen
            {

                while ((p.getx() - randomInt2) < 3.2 && (p.getx() - randomInt2) > -3.2 &&
                    (p.gety() - randomInt3) < 3.2 && (p.gety() - randomInt3) > -3.2)
                {
                    do
                    {
                        randomInt2 = Random.Range(-13, 14); //selects x coordinate for symbol point
                        randomInt3 = Random.Range(-8, 9); // selects y coordinate for symbol point
                    } while (randomInt2 > 10 && randomInt3 < -3);
                    a1 = -1;
                    //print(a); // to check how often to loops repeats to find the nexst valid point
                }
            }

            if (a1 == 0)
            {
                i1 = -1;
            }

        }

        symbolSendList.Add(new Symbol(randomInt2, randomInt3, id3, Random.Range(0, 360)));

    }


    //creates string out of current pointlist symbolSendList and sends them to Java Client
    public void sendSymbolPoints()
    {
        //convert generates symbol from symbolSendList to string, which can be sent
        for (int i = 0; i < symbolSendList.Count; i++)
        {
            if (i == 0)
            {
                build = new StringBuilder();
                build.Append(symbolSendList[i].toString() + "?");
            }
            else
            {
                
                //treat the last point different, dont append ?
                if (i == (symbolSendList.Count - 1))
                {
                    build.Append(symbolSendList[i].toString());
                }
                
                else
                {
                    
                      build.Append(symbolSendList[i].toString() + "?");
                }
            }
        }


        int randomHelpInt;
        randomOrderInteger.Clear();
        //create string out of each identifier
        while (randomOrderInteger.Count < Model.identifier.Count)
        {
            do
            {
                randomHelpInt = Random.Range(0, 3);
            } while (randomOrderInteger.Contains(randomHelpInt));
            randomOrderInteger.Add(randomHelpInt);
        }



        string integerString = "";

        for (int i = 0; i < randomOrderInteger.Count; i++)
        {
            integerString += Model.identifier[i] + "-" + randomOrderInteger[i] + "*";
        }

        integerString += "final";


        //wählt aus ob visuelles feedback gegeben wird
        int feedbackYesNo = Random.Range(0, 2);

        //wählt solution aus
        //int selectSolution = Random.Range(3, 6);

        integerString += "-" + selectGame + "-" + feedbackYesNo + "-\n";

        //treat last one different, dontt append dollar
        //integerString += Model.identifier[randomOrderInteger.Count - 1];


        //symbolString = build.ToString() + "*" + integerString + "final\n";
        //print("created string to be sent: "+ symbolString);

        byte[] ccc = System.Text.Encoding.ASCII.GetBytes(integerString);
        main.handler.Send(ccc);
        print("created string to be sent: "+ integerString);

        symbolString = build.ToString() + "\n";
        byte[] bbb = System.Text.Encoding.ASCII.GetBytes(symbolString);
        main.handler.Send(bbb);
        print("created string to be sent: " + symbolString);
    }



    //string in punkte umrechnen ODER auf update checken
    public static void translateIncomingPoints(string s)
    {
        // differetiate between if = new overlay, mark red ... 
        if (s.Contains("&") || s.Contains("@") || s.Contains("%") || s.Contains("#"))
        {
            if (s.Contains("@") || s.Contains("%") || s.Contains("#"))
            {
                if (s.Contains("@") || s.Contains("#"))
                {
                    if (s.Contains("@"))
                    {
                        hitter = s.Split('@');
                        GameEvents.current.newLongOverlayEventMethodMainThread();
                    }
                    else
                    {
                        int helper = 0;
                        string[] asdfString = s.Split('#');
                        if (Model.readyIdentifier.Count > 0)
                        {
                            foreach (string se in Model.readyIdentifier)
                            {
                                if (se.Contains(asdfString[0]))
                                {
                                    helper++;
                                }
                            }
                        }
                        if (helper == 0)
                        {
                            Model.readyIdentifier.Add(asdfString[0]);
                            GameEvents.current.newupdatePlayerReadyTextEventMethodMainThread();
                        }
                    }
                }

                else
                {
                    string[] ownIdentiferArray = s.Split('%');
                    ownIdentifier = ownIdentiferArray[0];
                }
            }

            //... or else = new points incoming
            else
            {
                xyzOverlay = s.Split('&');
                GameEvents.current.newOverlayEventMethodMainThread();
            }

        }
        
        else
        {
            if (s.Contains("*") || s.Contains("!"))
            {
                if (s.Contains("*"))
                {
                    identifierOrder.Clear();
                    onlyIdentifers = s.Split('*');
                    foreach (string p in onlyIdentifers)
                    {                    
                        if (!p.Contains("final"))
                        {
                            string[] helpA = p.Split('-');
                            identifierOrder.Add(helpA[0],helpA[1]);
                        }
                        else
                        {                           
                            receivedGameParameters = p.Split('-');
                            // incoming order: selectgame receivedGameParameters[1], feedbackyesno receivedGameParameters[2], selectsolution receivedGameParameters[3]

                                if(receivedGameParameters[2] == "0")
                                {
                                    showVisualHints = false;
                                    print("size 0 "+ receivedGameParameters[2]);
                                }
                                else
                                {
                                    print("size 1 " + receivedGameParameters[2]);
                                    showVisualHints = true;
                                }
                            
                        }

                    }
                }
                else
                {
                    try
                    {                   
                    string[] helpString = s.Split('!');
                    Model.identifier.Remove(helpString[0]);

                    if (Model.readyIdentifier.Count > 0)
                    {
                        foreach (string se in Model.readyIdentifier)
                        {
                            if (se.Contains(helpString[0]))
                            {
                                    Model.readyIdentifier.Remove(helpString[0]);
                                }
                        }    
                    }    

                    print("lösches des identifiers: " + helpString[0]);
                    Model.AllEyeInfo.Remove(helpString[0]);
                    helpToDeleteId = helpString[0];
                    //Model.gaze[helpString[0]].SetActive(false);
                    GameEvents.current.newdisableDeletedGazePointEventMethodMainThread();
                    GameEvents.current.newupdatePlayerTextEventMethodMainThread();
                    GameEvents.current.newupdatePlayerReadyTextEventMethodMainThread();
                    //Model.gaze.Remove(helpString[0]);
                    //print(Model.identifier.Count);

                    }
                    catch(System.Exception e)
                    {
                        print("something went wrong here  "+ e);
                    }
                }

            }

            else
            {
                symbolReceivedList.Clear(); // delete all symbol points and chosen gameobject (as ints)   
                allIcomingSymbolPoints = s.Split('?');

                foreach (string o in allIcomingSymbolPoints)
                {
                    string[] xANDy = o.Split('$');
                    symbolReceivedList.Add(new Symbol(System.Int32.Parse(xANDy[0]),
                        System.Int32.Parse(xANDy[1]), System.Int32.Parse(xANDy[2]), System.Int32.Parse(xANDy[3])));                
                }
                //calls displaySymbols() method on main thread
                GameEvents.current.newSymbolPointsEventMethodMainThread();
            }

        }

    }




    // when update needed
    private void displaySymbols()
    {
        gameOverGameObject.SetActive(false);
        GameOverIsDisplayed = false;
        //delete all symbols from before
        foreach (GameObject g in clone)
        {
            Destroy(g);
        }

        //delete all red overlays and then clear list        
        foreach (GameObject g in GameLogic.overlaySignal)
        {
            Destroy(g);
        }
        GameLogic.overlaySignal.Clear();

        //delete all green overlays and then clear list        
        foreach (GameObject g in GameLogic.overlaySignalGreen)
        {
            Destroy(g);
        }
        GameLogic.overlaySignalGreen.Clear();


        clone.Clear();
        // instatiates a gameobject for each point and adds it to the list of gameobjects
        foreach (Symbol p in symbolReceivedList)
        {
            //select game from dictionary receivedGameParameters[1]
            
            clone.Add((GameObject)Instantiate(gameDict[System.Int32.Parse(receivedGameParameters[1])][p.getz()], new Vector2(p.getx(), p.gety()), spawnPos.rotation * Quaternion.Euler(0f, 0f, (float)p.gett())));

            if (p.getz() == 3 || p.getz() == 4 || p.getz() == 5)
            {
                currentCorrectSolution = p.getz();
            }
        }

        StringBuilder buildToWrite = new StringBuilder();
        System.DateTime dateEyeTracking = System.DateTime.Now;
        buildToWrite.Append(SymbolHandler.ownIdentifier + ";" + dateEyeTracking.ToString("ddMMHHmmssff") + ";" + SymbolHandler.showVisualHints.ToString() + ";DisplaySymbols;;" + SymbolHandler.receivedGameParameters[1] + ";" + SymbolHandler.currentCorrectSolution + ";" + gameDict[System.Int32.Parse(receivedGameParameters[1])][helpIntforOwnIdentifier]);
        GameLogic.streamWriter.WriteLine(buildToWrite.ToString());
        GameLogic.streamWriter.Flush();

        /*
        for (int i = 0; i < identifierOrder.Count;i++)
        {
            if (identifierOrder[i] == ownIdentifier)
            {
                helpIntforOwnIdentifier = i;
            }
        }*/

        helpIntforOwnIdentifier = System.Int32.Parse(identifierOrder[ownIdentifier]);

        clone.Add((GameObject)Instantiate(gameDict[System.Int32.Parse(receivedGameParameters[1])][helpIntforOwnIdentifier], new Vector2((main.windowWidth)-2, (main.windowHeight*-1)+2), spawnPos.rotation));
        SymbolAreCurrentlyDisplayed = true;
    }




    public void deleteAllSymbols()
    {
        GameOverIsDisplayed = true;
        gameOverGameObject.SetActive(true);
        //delete all symbols from before
        foreach (GameObject g in clone)
        {
            Destroy(g);
        }
        clone.Clear();

        //delete all red overlays and then clear list        
        foreach (GameObject g in GameLogic.overlaySignal)
        {
            Destroy(g);
        }
        GameLogic.overlaySignal.Clear();

        //delete all green overlays and then clear list        
        foreach (GameObject g in GameLogic.overlaySignalGreen)
        {
            Destroy(g);
        }
        clone.Clear();
        GameLogic.overlaySignalGreen.Clear();

        //clone.Add((GameObject)Instantiate(game1[int.Parse(hitter[2])], new Vector2(100,0), spawnPos.rotation * Quaternion.Euler(0f, 0f, int.Parse(hitter[3]))));

        //display layover menu
        //gameOverGameObject.SetActive(true);
    }

}



/*
//generates points with specific space in between where symbols will be placed
public void OldgenerateSymbolPoints()
{
    symbolReceivedList.Clear();
    int id = 0; // zum zählen
    while (id < anzahlSymbols) // kreiert auf einemal 25 icons
    {

        randomInt2 = Random.Range(-16, 16); //selects x coordinate for symbol point
        randomInt3 = Random.Range(-9, 9); // selects y coordinate for symbol point

        int i = 0;
        int a;
        while (i == 0)
        {
            a = 0;

            foreach (Symbol p in symbolReceivedList) // checke auf overlay EIGENTLICH müsste kreisformel dastehen, noch einbauen
            {

                while ((p.getx() - randomInt2) < 3 && (p.getx() - randomInt2) > -3 &&
                    (p.gety() - randomInt3) < 3 && (p.gety() - randomInt3) > -3)
                {
                    randomInt2 = Random.Range(-16, 16);
                    randomInt3 = Random.Range(-9, 9);
                    a = -1;
                    //print(a); // to check how often to loops repeats to find the nexst valid point
                }
            }

            if (a == 0)
            {
                i = -1;
            }

        }
        randomInt = Random.Range(0, spawnee.Length); // wählt symbol aus
        symbolReceivedList.Add(new Symbol(randomInt2, randomInt3, randomInt));
        id++;
    }
}*/

/*
// when update needed
public void recreateSymbols()
{
    //delete all symbols from before
    foreach (GameObject g in clone)
    {
        Destroy(g);
    }

    //delete all red overlays and then clear list        
    foreach(GameObject g in GameLogic.overlaySignal)
    {
        Destroy(g);
    }
    GameLogic.overlaySignal.Clear();

    //delete all green overlays and then clear list        
    foreach (GameObject g in GameLogic.overlaySignalGreen)
    {
        Destroy(g);
    }
    GameLogic.overlaySignalGreen.Clear();


    clone.Clear();
    // instatiates a gameobject for each point and adds it to the list of gameobjects
    foreach (Symbol p in symbolReceivedList)
    {
        clone.Add((GameObject)Instantiate(spawnee[p.getz()], new Vector2(p.getx(), p.gety()), spawnPos.rotation));
    }

}*/