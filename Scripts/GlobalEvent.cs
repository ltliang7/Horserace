using System.Collections;
using System.Collections.Generic;
using Framework;

public class GlobalEvent : EventDispatcher.BaseEvent
{

    //Game Life Circle
    public static readonly int OnTouchPoint = ++id;

    public static readonly int OnTouchPoint_Big = ++id;

    public static readonly int OnFinish = ++id;

    public static readonly int OnFinish_Last = ++id;

    public static readonly int OnStart = ++id;

    public static readonly int OnTotal = ++id;

    public static readonly int OnTouchDel = ++id;

    public static readonly int OnFinishDel = ++id;

    public static readonly int OnSection = ++id;

    public static readonly int OnLogoStar = ++id;
}
