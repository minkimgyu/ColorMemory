using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;

    private string coin1000 = "coin1000";
    private string coin2000 = "coin2000";
    private string coin4000 = "coin4000";

    public static IAPManager Instance { get; private set; }

    private IStoreController _controller;
    private IExtensionProvider _extensions;
    private Action _onPurchaseSuccess;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitIAP();
    }

    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(coin1000, ProductType.Consumable);
        builder.AddProduct(coin2000, ProductType.Consumable);
        builder.AddProduct(coin4000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
       Debug.Log($"IAP Init Failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"IAP Init Failed: {error} / {message}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase Failed: {product.definition.id} / {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"Purchase Successful: {args.purchasedProduct.definition.id}");

        _onPurchaseSuccess?.Invoke();
        _onPurchaseSuccess = null;

        return PurchaseProcessingResult.Complete;
    }

    public void BuyProduct(string productId, Action onSuccess)
    {
        _onPurchaseSuccess = onSuccess;

        if (storeController != null && storeController.products != null)
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Product not available");
            }
        }
    }
}
