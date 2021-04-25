using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LoadingMain : MonoBehaviour
{
    public static Socket handler;
    private bool serverIsReady;
    private int port;
    private StreamReader reader;
    private Camera camerae;
    //private Model model;
    private GameLogic mov;
    public static float windowHeight;
    public static float windowWidth;

    public event EventHandler newMessageEvent;
    public static event Action onD;

    public string scheck;


    public static GameLogic fi; //new to show overlay bc method can only be called from main thread
    public static Quaternion fit; //same reason as fi



    // Start is called before the first frame update
    void Awake()
    {
        camerae = Camera.main;
        windowHeight = camerae.orthographicSize;
        windowWidth = camerae.aspect * windowHeight;

        serverIsReady = false;




        //access GameLogic class
        //fi = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        //fit = GameObject.Find("GameLogic").transform.rotation;

        print("Start");
        Thread thread = new Thread(tcp);
        thread.Start();
        port = 11010; // to connect with ui
        /*
      //startet server - - - würde man normalerweise extern starten
      System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
      myProcess.StartInfo.FileName = "java";
      //myProcess.StartInfo.Arguments = "-jar Digilog.jar 11000"; //öffnet die JAR und übergibt den port 11000
      myProcess.StartInfo.Arguments = "-jar C:\\Users\\uniho\\Downloads\\Test\\asdf\\Digilog.jar"; //starter JAR ohne übergabe von argumenten
      myProcess.Start();*///


        //startet client normalerweise würde man nur den client start
        System.Diagnostics.Process myProcess2 = new System.Diagnostics.Process();
        myProcess2.StartInfo.FileName = "java";
        //myProcess.StartInfo.Arguments = "-jar Digilog.jar 11000"; //öffnet die JAR und übergibt den port 11000
        //myProcess2.StartInfo.Arguments = "-jar Digilog.jar " + port;
        myProcess2.StartInfo.Arguments = "-jar Digilog.jar " + port; //starter JAR ohne übergabe von argumenten
        myProcess2.Start();






    }






    //wird als Thread gestartet
    public void tcp()
    {
        print("Thread gestartet");


        // Establish the local endpoint for the socket.  
        IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry("localhost").AddressList,
            a => a.AddressFamily == AddressFamily.InterNetwork);
        IPAddress ipAddress = ipv4Addresses[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
        print("ready1");
        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Bind the socket to the local endpoint and
            // listen for incoming connections.
            listener.Bind(localEndPoint);
            listener.Listen(10);
            print("Waiting for TCP connection to Java back end using port " + port + " ...");
            // Program is suspended while waiting for an incoming connection.  
            handler = listener.Accept();
            print("Connected to Java back end.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Close();
        }

        serverIsReady = true;


        // thread listens for new data from java client
        while (true)
        {
            while (true)
            {
                byte[] bytes = new byte[256];
                int byteCount = handler.Receive(bytes, 0);
                scheck = Encoding.UTF8.GetString(bytes);
                print("incoming string: " + scheck);

                //new incoming symbols for points ($ used as seperator)
                if (scheck.Contains("$") || scheck.Contains("&") || scheck.Contains("@") || scheck.Contains("%") || scheck.Contains("*"))
                {
                    SymbolHandler.translateIncomingPoints(scheck);
                }



                //new eye tracking data incoming (- used as seperator)
                else
                {
                    //Model.updateModel(scheck);
                    if (!SymbolHandler.GameOverIsDisplayed)
                    {
                        GameEvents.current.newUpdateModelMethodMainThread(scheck);
                    }

                }

                //if (newMessageEvent != null) newMessageEvent(this, EventArgs.Empty);
                break;
            }
        }
    }


    private void Close()
    {
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }

}
