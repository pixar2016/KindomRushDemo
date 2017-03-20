using System;
using System.Collections.Generic;
using UnityEngine;

public class SoliderDead : StateBase
{
    public SoliderInfo soliderInfo;

    public SoliderDead(SoliderInfo _soliderInfo)
    {
        soliderInfo = _soliderInfo;
    }

    public void SetParam(params object[] args)
    {

    }

    public void EnterExcute()
    {
        soliderInfo.DoAction("die");
    }

    public void Excute()
    {

    }

    public void ExitExcute()
    {

    }
}