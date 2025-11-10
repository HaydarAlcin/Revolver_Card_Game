using System;

public class CoreRevolverGameSignals
{
    public Action OnSpinCompleted = delegate { };
    public Action OnRevolverGameEnded = delegate { };
    public Action OnRevolverGameRevived = delegate { };
}
