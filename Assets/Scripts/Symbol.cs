using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol
{

    public int x, y, z, t;
    public long time; //time
    
    // Start is called before the first frame update
    public Symbol (int a, int b, int c)
    {
        x = a;
        y = b;
        z = c; // choses symbol
    }

    public Symbol(int a, int b, int c, int d, long l)
    {
        x = a;
        y = b;
        z = c; // choses symbol
        t = d; // rotation
        time = l; // time
    }


    public Symbol(int a, int b, int c, int d)
    {
        x = a;
        y = b;
        z = c; // choses symbol
        t = d; // rotation
    }

    public Symbol(int a, int b)
    {
        x = a;
        y = b;
    }

    public int getx ()
    {
        return x;
    }

    public int gety()
    {
        return y;
    }

    public long getTime()
    {
        return time;
    }

    public int getz()
    {
        return z;
    }

    public int gett()
    {
        return t;
    }

    public string toString() // stirng seperator whiting point
    {
        return ""+ x + "$" + y + "$" + z + "$" + t;
    }


}
