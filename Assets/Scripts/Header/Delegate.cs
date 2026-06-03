using System;
using UnityEngine.Events;
public static class Delegate 
{
    public static Action OnMainGameLoaded;
    public static Action OnPrologueLoaded;
    public static Action OnTitleLoaded;
    public static Action OnShopLoaded;
    public static Action OnNextDialogueRequeated;
    public static Action<string> OnItemBuy;
}
