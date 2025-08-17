using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MiniPos.Models;
using MiniPos.Constants;

namespace MiniPos.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    private string _scannedProductCode = AppConstants.DefaultProductCode;
    public string ScannedProductCode
    {
        get => _scannedProductCode;
        set
        {
            if (_scannedProductCode != value)
            {
                _scannedProductCode = value;
                OnPropertyChanged(nameof(ScannedProductCode));
            }
        }
    }


    private string _scannedProductName = AppConstants.DefaultProductName;
    public string ScannedProductName
    {
        get => _scannedProductName;
        set
        {
            if (_scannedProductName != value)
            {
                _scannedProductName = value;
                OnPropertyChanged(nameof(ScannedProductName));
            }
        }
    }

    private decimal _scannedProductUnitPrice = AppConstants.DefaultProductUnitPrice;
    public decimal ScannedProductUnitPrice
    {
        get => _scannedProductUnitPrice;
        set
        {
            if (_scannedProductUnitPrice != value)
            {
                _scannedProductUnitPrice = value;
                OnPropertyChanged(nameof(ScannedProductUnitPrice));
            }
        }
    }


    private int _scannedProductQty = AppConstants.DefaultProductQuantity;
    public int ScannedProductQty
    {
        get => _scannedProductQty;
        set
        {
            if (_scannedProductQty != value)
            {
                _scannedProductQty = value;
                OnPropertyChanged(nameof(ScannedProductQty));
            }

            ScannedProductTotalPrice = value * ScannedProductUnitPrice;
        }
    }


    private decimal _scannedProductTotalPrice = AppConstants.DefaultProductTotalPrice;
    public decimal ScannedProductTotalPrice
    {
        get => _scannedProductTotalPrice;
        set
        {
            if (_scannedProductTotalPrice != value)
            {
                _scannedProductTotalPrice = value;
                OnPropertyChanged(nameof(ScannedProductTotalPrice));
            }
        }
    }


    private decimal _receiptTotal = AppConstants.DefaultReceiptTotal;
    public decimal ReceiptTotal
    {
        get => _receiptTotal;
        set
        {
            if (_receiptTotal != value)
            {
                _receiptTotal = value;
                OnPropertyChanged(nameof(ReceiptTotal));
            }
        }
    }

    public ObservableCollection<ProductReceiptItem> ProductReceiptItemList = [];

    private Visibility _receiptProductsVisibility = Visibility.Collapsed;
    public Visibility ReceiptProductsVisibility
    {
        get => _receiptProductsVisibility;
        set
        {
            if (_receiptProductsVisibility != value)
            {
                _receiptProductsVisibility = value;
                OnPropertyChanged(nameof(ReceiptProductsVisibility));
            }

            ReceiptProductsPlaceholderVisibility = value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }


    private Visibility _receiptProductsPlaceholderVisibility = Visibility.Visible;
    public Visibility ReceiptProductsPlaceholderVisibility
    {
        get => _receiptProductsPlaceholderVisibility;
        set
        {
            if (_receiptProductsPlaceholderVisibility != value)
            {
                _receiptProductsPlaceholderVisibility = value;
                OnPropertyChanged(nameof(ReceiptProductsPlaceholderVisibility));
            }
        }
    }


    private bool _enableAddButton = false;
    public bool EnableAddButton
    {
        get => _enableAddButton;
        set
        {
            if (_enableAddButton != value)
            {
                _enableAddButton = value;
                OnPropertyChanged(nameof(EnableAddButton));
            }
        }
    }


    private bool _enableFinishButton = false;
    public bool EnableFinishButton
    {
        get => _enableFinishButton;
        set
        {
            if (_enableFinishButton != value)
            {
                _enableFinishButton = value;
                OnPropertyChanged(nameof(EnableFinishButton));
            }
        }
    }

    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }    
}
