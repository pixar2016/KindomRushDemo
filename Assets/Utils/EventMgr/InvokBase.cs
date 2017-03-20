using System;

public class InvokBase
{
    public int key;
    public object[] param;
    public bool save;

    public InvokBase(int _key, bool _save, params object[] _param)
    {
        this.key = _key;
        this.param = _param;
        this.save = _save;
    }
}
