using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public interface IIAPManager : IStoreListener
{
    public void BuyProduct(string productId, System.Action onSuccess) { }
}

public class NullIAPManager : IIAPManager
{
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return default;
    }
}
