// ***********************************************************************
// Company          : 
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
//#define UNITY_IAP

namespace Game.IAP
{
    using System;
    using UnityEngine;
#if UNITY_IAP
    using UnityEngine.Purchasing;
    using UnityEngine.Purchasing.Extension;
    using UnityEngine.Purchasing.Security;
#endif


#if UNITY_IAP
    public class IAPUnityWrapper : IAPWrapper, IStoreListener

#else
    public class IAPUnityWrapper : IAPWrapper
#endif
    {


        #region Field

        /// <summary>
        /// 
        /// </summary>
        private string _payProduct = string.Empty;
#if UNITY_IAP
        /// <summary>
        /// 
        /// </summary>
        private IAppleExtensions _appleExtensions;
        /// <summary>
        /// 
        /// </summary>
        private IStoreController _storeController;
#endif
        #endregion

        #region Method 

        public override void Buy(string payProduct)
        {
#if UNITY_IAP
            if (_storeController == null)
            {
                _payProduct = payProduct;
                this.Start();
            }
            else
            {
                _payProduct = string.Empty;
                var product = _storeController.products.WithID(payProduct);
                if (product != null)
                {
                    _storeController.InitiatePurchase(product);
                }
                else
                {
                    Debug.LogError("Unknown Product " + payProduct);
                }
            }
#endif
        }

#if UNITY_IAP

        private void OnDeferred(Product product)
        {

        }

#endif

        #endregion

        #region Interface 

#if UNITY_IAP

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _appleExtensions = extensions.GetExtension<IAppleExtensions>();
            _appleExtensions.RegisterPurchaseDeferredListener(this.OnDeferred);

            foreach (Product product in controller.products.all)
            {
                IAPGoods goods = IAPGoodsBank.GetBySKU(product.definition.id);
                if (product.availableToPurchase && (goods != null))
                {
                    if ((product.metadata.localizedPrice > 0.1M) && !string.IsNullOrEmpty(product.metadata.localizedPriceString))
                    {
                        goods.localizedPrice = product.metadata.localizedPrice;
                        goods.isoCurrencyCode = product.metadata.isoCurrencyCode;

                        goods.localizedTitle = product.metadata.localizedTitle;
                        goods.localizedPriceString = product.metadata.localizedPriceString;
                        goods.localizedDescription = product.metadata.localizedDescription;
                    }
                    else
                    {
                        string[] textArray = new string[] { product.receipt, product.metadata.localizedTitle, product.metadata.localizedDescription, product.metadata.isoCurrencyCode, product.metadata.localizedPrice.ToString(), product.metadata.localizedPriceString };
                        Debug.LogError("Strange price for " + string.Join(" - ", textArray));
                    }
                }
                else if (goods != null)
                {
                    goods.canPurchase = false;
                }
                else
                {
                    Debug.LogError("Unknown bundle [" + product.definition.id + "]");
                }
            }

            if (!string.IsNullOrEmpty(this._payProduct))
            {
                this.Buy(this._payProduct);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason reason)
        {
            if (onInitialize != null)
            {
                onInitialize(1, reason.ToString(), null);
            }

            Debug.Log("Billing failed to initialize!");
            if (reason != InitializationFailureReason.AppNotKnown)
            {
                if (reason == InitializationFailureReason.PurchasingUnavailable)
                {
                    Debug.Log("Billing disabled!");
                }
                else if (reason == InitializationFailureReason.NoProductsAvailable)
                {
                    Debug.Log("No products available for purchase!");
                }
            }
            else
            {
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
            }
        }

        public void OnPurchaseFailed(Product item, PurchaseFailureReason reason)
        {
            if (onPurchase != null)
            {
                onPurchase(1, reason.ToString(), item.definition.id);
            }
        }

        private void OnTransactionsRestored(bool success)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                try
                {
                    if (onPurchase != null)
                    {
                        onPurchase(0, "", e.purchasedProduct.receipt);
                    }

                    //foreach (var receipt in this.validator.Validate(e.purchasedProduct.receipt))
                    //{
                    //GooglePlayReceipt googleReceipt = receipt as GooglePlayReceipt;
                    //    AppleInAppPurchaseReceipt appleReceipt = receipt as AppleInAppPurchaseReceipt;
                    //}
                }
                catch (IAPSecurityException ex)
                {
                    if (onPurchase != null)
                    {
                        onPurchase(2, ex.ToString(), e.purchasedProduct.receipt);
                    }
                    return PurchaseProcessingResult.Complete;
                }
            }
            return PurchaseProcessingResult.Complete;
        }

#endif

        #endregion

        #region Unity 

        private void Awake()
        {
            IAPWrapper.Instance = this;
        }

        private void Start()
        {
#if UNITY_IAP
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);

            foreach (string id in IAPGoodsBank.ProductArray())
            {
                IDs ids;
                var platform = GetCurrentPlatform();
                switch (platform)
                {
                    case IAPPlatform.GooglePlay:
                        {
                            ids = new IDs
                            {
                                { id, GooglePlay.Name }
                            };
                            builder.AddProduct(id, ProductType.Consumable, ids);
                            continue;
                        }
                    case IAPPlatform.AppleAppStore:
                        {
                            ids = new IDs
                            {
                                { id, AppleAppStore.Name }
                            };
                            builder.AddProduct(id, ProductType.Consumable, ids);
                            continue;
                        }
                }
            }

            //builder.Configure<IGooglePlayConfiguration>().SetPublicKey(""); //本地验证
            //this.validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);//本地验证
            UnityPurchasing.Initialize(this, builder);
#endif
        }

        #endregion
    }
}

