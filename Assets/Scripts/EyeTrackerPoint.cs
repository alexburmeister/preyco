using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackerPoint // darf nicht von monobehavior erben sonst kann man keine instanz erzeugen
{
    
    float x;
    float y;
    long l; // time


    public EyeTrackerPoint(float a, float b)
    {
        x = a;
        y = b;
    }

    public EyeTrackerPoint(float a, float b, long c)
    {

        //teile in vier qudranten -> (0,0) is top left corner
        if (a > 0.5f)
        {
            x = ((a - 0.5f) * 2) * main.windowWidth;//prozenzahl in 0.x wiviel rechts der punkt von der mitte liegt
            if (b > 0.5f) // right bottom rectangle
            {
                y = ((b - 0.5f) * -2) * main.windowHeight;
            }
            y = ((0.5f - b) * 2) * main.windowHeight;

        }

        if (a <= 0.5f)
        {
            x = ((0.5f - a) * -2) * main.windowWidth;
            if (b > 0.5f)
            {
                y = ((b - 0.5f) * -2) * main.windowHeight;
            }
            y = ((0.5f - b) * 2) * main.windowHeight;
        }

        l = c;

    }

    public static string inverseX(float x)
    {
        double help;
        help = 0.5 + (x / (main.windowWidth*2));
        return help.ToString();        
    }

    public static string inverseY(float y)
    {
        double help;
        help = 0.5 - (y / (main.windowWidth * 2));
        return help.ToString();
    }

    public string toStr()
    {
        return "" + x + " " +  y + " " + l;
     }

    public float getX()
    {
        return x;
    }

    public float getY()
    {
        return y;
    }

    public long getTime()
    {
        return l;
    }

    public void updatePoint(float a, float b, long c)
    {
        if (a > 0.5f)
        {
            x = ((a - 0.5f) * 2) * main.windowWidth;//prozenzahl in 0.x wiviel rechts der punkt von der mitte liegt
            if(b > 0.5f)
            {
                y = ((b - 0.5f) * 2) * main.windowHeight;
            }
            y = ((0.5f - b) * -2) * main.windowHeight;
        }
        
        if(a <= 0.5f)
        {
            x = ((0.5f - a) * -2) * main.windowWidth;
            if (b > 0.5f)
            {
                y = ((b - 0.5f) * 2) * main.windowHeight;
            }
            y = ((0.5f - b) * -2) * main.windowHeight;
        }

        l = c;

    }
}