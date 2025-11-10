using System;

public class CoreUISignals
{
    public Action<int> OnTotalGoldChanged = delegate { };
    public Action<int> OnTotalCashChanged = delegate { };
}