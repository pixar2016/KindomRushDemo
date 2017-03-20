using System.Collections;
using System.Collections.Generic;

public class OpenSpaceConstructing : StateBase
{
    public OpenSpaceInfo openSpaceInfo;

    public int changeTowerId;
    public OpenSpaceConstructing(OpenSpaceInfo _openSpaceInfo)
    {
        openSpaceInfo = _openSpaceInfo;
    }

    public void SetParam(params object[] args)
    {
        if (args == null || args.Length < 1)
        {
            return;
        }
        changeTowerId = (int)args[0];
    }

    public void EnterExcute()
    {

    }

    public void Excute()
    {

    }

    public void ExitExcute()
    {

    }
}

