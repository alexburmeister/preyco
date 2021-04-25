using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action newOverlayEvent;
    public event Action newLongOverlayEvent;
    public event Action newCountingEvent;
    public event Action newSymbolPointsEvent;
    public event Action genANDsendEvent;
    public event Action deleteEvent;
    public event Action updatePlayerTextEvent;
    public event Action disableDeletedGazePointEvent;
    public event Action updatePlayerReadyTextEvent;


    public void newupdatePlayerReadyTextEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newupdatePlayerReadyTextEventEventMethod());
    }

    public IEnumerator newupdatePlayerReadyTextEventEventMethod()
    {
        print("updateplayertext Ready Event fired");
        if (newOverlayEvent != null) updatePlayerReadyTextEvent();
        yield return null;
    }

    public void newdisableDeletedGazePointEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newdisableDeletedGazePointEventMethod());
    }

    public IEnumerator newdisableDeletedGazePointEventMethod()
    {
        print("delete gaze point Event fired");
        if (newOverlayEvent != null) disableDeletedGazePointEvent();
        yield return null;
    }

    public void newupdatePlayerTextEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newupdatePlayerTextEventEventMethod());
    }

    public IEnumerator newupdatePlayerTextEventEventMethod()
    {
        print("updateplayertext Event fired");
        if (newOverlayEvent != null) updatePlayerTextEvent();
        yield return null;
    }

    public void newdeleteEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newdeleteEventMethod());
    }

    public IEnumerator newdeleteEventMethod()
    {
        print("deleteEvent Event fired");
        if (newOverlayEvent != null) deleteEvent();
        yield return null;
    }


    public void newgenANDsendEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newgenANDsendEventMethod());
    }

    public IEnumerator newgenANDsendEventMethod()
    {
        print("newgendANDsend Event fired");
        if (newOverlayEvent != null) genANDsendEvent();
        yield return null;
    }


    public void newOverlayEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newOverlayEventMethod());
    }

    public IEnumerator newOverlayEventMethod()
    {
        print("new overlay event fired");
        if (newOverlayEvent != null) newOverlayEvent();
        yield return null;
    }

    public void newSymbolPointsEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newSymbolPointsEventMethod());
    }

    public IEnumerator newSymbolPointsEventMethod()
    {
        print("new symbol points event fired");
        if (newOverlayEvent != null) newSymbolPointsEvent();
        yield return null;
    }



    public void newLongOverlayEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newLongOverlayEventMethod());
    }
    public IEnumerator newLongOverlayEventMethod()
    {
        print("new long overlay event fired");
        if (newLongOverlayEvent != null) newLongOverlayEvent();
        yield return null;
    }

    /*
    public void newCountingEventMethodMainThread()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(newCountingEventMethod());
    }
    public IEnumerator newCountingEventMethod()
    {
        print("new counting event fired");
        if (newCountingEvent != null) newCountingEvent();
        yield return null;
    }*/

    public void newUpdateModelMethodMainThread(string s)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(Model.updateModel(s));
    }



}
