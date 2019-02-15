using UnityEngine;
using SA.CrossPlatform.InApp;
using SA.CrossPlatform.UI;
using UnityEngine.UI;

public class UM_InAppExample : MonoBehaviour
{

    [Header("Unified API Buttons")] 
    [SerializeField] private Button m_Connect;
    [SerializeField] private Button m_PurchaseConsumable;
    [SerializeField] private Button m_PurchaseNonConsumable;
    
    [SerializeField] private Button m_TestFailedPurchase;
    [SerializeField] private Button m_RestoreTransactions;
    
    [Header("Info Panel")] 
    [SerializeField] private Text m_StateInfo;
    
    private void Start()
    {
        SetStoreActiveState(false);
        m_Connect.onClick.AddListener(() =>
        {
            var observer = new UM_TransactionObserverExample();
            UM_InAppService.Client.SetTransactionObserver(observer);
            
            UM_InAppService.Client.Connect((connectionResult) => {
                if(connectionResult.IsSucceeded) {
                    //You are now connected to the payment service.
                    //Also all product's info are updated at this point from according to server values.
                    SetStoreActiveState(true);
                    PrintAvailableProductsInfo();
                    UM_DialogsUtility.ShowMessage("Connection Succeeded", "In App Service is now connected and ready to use!");
                } else {
                    //Connection failed.
                    UM_DialogsUtility.ShowMessage("Connection Failed", connectionResult.Error.FullMessage);
                }
            });
        });
        
        m_PurchaseConsumable.onClick.AddListener(() => { StartPayment(UM_ProductType.Consumable); });
        m_PurchaseNonConsumable.onClick.AddListener(() => { StartPayment(UM_ProductType.NonConsumable); });
        m_TestFailedPurchase.onClick.AddListener(() => { UM_InAppService.Client.AddPayment("non_existed_product_id"); });
        
        m_RestoreTransactions.onClick.AddListener(() =>
        {
            Restore();
        });

    }

    private void StartPayment(UM_ProductType productType)
    {
        UM_iProduct validProduct = null;
        foreach(var product in UM_InAppService.Client.Products)
        {
            if (product.Type == productType && product.IsActive)
            {
                validProduct = product;
                break;
            }
        }

        if (validProduct != null)
        {
            UM_InAppService.Client.AddPayment(validProduct.Id);
        }
        else
        {
            var message = string.Format("You don't have any {0} products set.", productType);
            UM_DialogsUtility.ShowMessage("Not Found", message);
        }
    }

    private void SetStoreActiveState(bool isActive)
    {
        m_Connect.interactable = !isActive;
        
        m_PurchaseConsumable.interactable = isActive;
        m_PurchaseNonConsumable.interactable = isActive;
        m_TestFailedPurchase.interactable = isActive;
        m_RestoreTransactions.interactable = isActive;

        string statusText;
        if (!isActive)
        {
            statusText = "Store service not yet connected!";
        }
        else
        {
            statusText = "Store service connected!";
            statusText += "\n";
            foreach(var product in UM_InAppService.Client.Products)
            {
                var productInfo = product.Title + "(" + product.Id + ")";
                productInfo += " Type: " + product.Type;
                productInfo += " Price: " + product.Price;
                productInfo += " IsActive: " + product.IsActive;
                
                statusText += "\n";
                statusText += productInfo;
            }
        }

        m_StateInfo.text = statusText;

    }
    
    private void Restore() {
        UM_InAppService.Client.RestoreCompletedTransactions();
    }

    private void PrintAvailableProductsInfo() {
        foreach(var product in UM_InAppService.Client.Products) {
            Debug.Log("product.Id: " + product.Id);
            Debug.Log("product.Price: " + product.Price);
            Debug.Log("product.PriceInMicros: " + product.PriceInMicros);
            Debug.Log("product.Title: " + product.Title);
            Debug.Log("product.Description: " + product.Description);
            Debug.Log("product.PriceCurrencyCode: " + product.PriceCurrencyCode);
            Debug.Log("product.Icon: " + product.Icon);
            Debug.Log("product.Type: " + product.Type);
            Debug.Log("product.IsActive: " + product.IsActive);
        }
    }
}
