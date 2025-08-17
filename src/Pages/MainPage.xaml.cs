using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.ApplicationModel.Resources;
using MiniPos.Constants;
using MiniPos.Models;
using MiniPos.Services;
using MiniPos.Settings;
using MiniPos.Strings;
using MiniPos.ViewModels;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;

namespace MiniPos.Pages;

public sealed partial class MainPage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    public MainPageViewModel ViewModel { get; } = new();

    public MainPage()
    {
        InitializeComponent();

        ViewModel.ProductReceiptItemList.CollectionChanged += OnProductReceiptItemListChange;
    }

    private async Task<ContentDialogResult> ShowDialogSafeAsync(ContentDialog dialog)
    {
        try
        {
            dialog.XamlRoot = XamlRoot;
            return await dialog.ShowAsync();
        }
        catch
        {
            return ContentDialogResult.None;
        }
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        EnterCodeTextBox.IsEnabled = false;

        try
        {
            var filepath = AppSettings.Instance.GetProductsSpreadsheetFilepath();
            var dataset = SpreadsheetService.Instance.ReadSpreadsheet(filepath);

            if (dataset.GetLength(0) == 0)
            {
                await ShowDialogSafeAsync(new()
                {
                    Title = resourceLoader.GetString(AppStrings.Error),
                    Content = resourceLoader.GetString(AppStrings.Could_not_find_the_items_data_The_app_will_shutdown),
                    CloseButtonText = resourceLoader.GetString(AppStrings.Accept),
                });

                Application.Current.Exit();

                return;
            }

            try
            {
                ProductsService.Instance.LoadProductsFromDataSet(dataset);
            }
            catch
            {
                await ShowDialogSafeAsync(new()
                {
                    Title = resourceLoader.GetString(AppStrings.Warning),
                    Content = resourceLoader.GetString(AppStrings.An_error_occurred_reading_an_item_Make_sure_the_data_is_clean_and_no_data_is_missing_The_app_will_shut_down_now),
                    CloseButtonText = resourceLoader.GetString(AppStrings.Accept),
                });

                Application.Current.Exit();
                return;
            }
        }
        catch
        {
            await ShowDialogSafeAsync(new()
            {
                Title = resourceLoader.GetString(AppStrings.Error),
                Content = resourceLoader.GetString(AppStrings.Could_not_read_the_items_data_file_It_is_likely_the_file_is_opened_by_another_app_Close_the_other_app_and_try_again_The_app_will_shut_down_now),
                CloseButtonText = resourceLoader.GetString(AppStrings.Accept),
            });

            Application.Current.Exit();

            return;
        }


        EnterCodeTextBox.IsEnabled = true;

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);
    }

    private async void OnCodeTextBoxHitsEnter(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key != VirtualKey.Enter) return;

        var code = EnterCodeTextBox.Text.Trim().ToLower();
        var product = ProductsService.Instance.GetProducts().FirstOrDefault(p => p.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));

        if (product == null)
        {
            await ShowDialogSafeAsync(new()
            {
                Title = resourceLoader.GetString(AppStrings.Error),
                Content = resourceLoader.GetString(AppStrings.Item_not_found),
                CloseButtonText = resourceLoader.GetString(AppStrings.Accept),
            });

            return;
        }

        ViewModel.ScannedProductCode = product.Code;
        ViewModel.ScannedProductName = product.Name;
        ViewModel.ScannedProductUnitPrice = product.Price;
        ViewModel.ScannedProductQty = 1;
        ViewModel.EnableAddButton = true;

        EnterCodeTextBox.Text = string.Empty;

        await FocusManager.TryFocusAsync(AddProductButton, FocusState.Programmatic);
    }

    private async void OnDiscardReceiptButtonClick(object sender, RoutedEventArgs e)
    {
        var result = await ShowDialogSafeAsync(new()
        {
            Title = resourceLoader.GetString(AppStrings.Warning),
            Content = resourceLoader.GetString(AppStrings.Are_you_sure_you_want_to_discard_the_receipt_Everything_added_will_be_lost),
            PrimaryButtonText = resourceLoader.GetString(AppStrings.Discard),
            CloseButtonText = resourceLoader.GetString(AppStrings.Cancel),
        });

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);

        if (result != ContentDialogResult.Primary) return;

        ViewModel.ScannedProductCode = AppConstants.DefaultProductCode;
        ViewModel.ScannedProductName = AppConstants.DefaultProductName;
        ViewModel.ScannedProductUnitPrice = AppConstants.DefaultProductUnitPrice;
        ViewModel.ScannedProductQty = AppConstants.DefaultProductQuantity;
        ViewModel.ScannedProductTotalPrice = AppConstants.DefaultProductTotalPrice;
        ViewModel.ProductReceiptItemList.Clear();

        EnterCodeTextBox.Text = string.Empty;
    }

    private async void OnAddProductButtonClick(object sender, RoutedEventArgs e)
    {
        var existingProduct = ViewModel.ProductReceiptItemList.FirstOrDefault(p => p.Code.Equals(ViewModel.ScannedProductCode, StringComparison.InvariantCultureIgnoreCase));

        if (existingProduct != null)
        {
            var index = ViewModel.ProductReceiptItemList.IndexOf(existingProduct);
            var newQty = existingProduct.Quantity + ViewModel.ScannedProductQty;
            ViewModel.ProductReceiptItemList[index] = new ProductReceiptItem(existingProduct, newQty);
        }
        else
        {
            ViewModel.ProductReceiptItemList.Insert(0, new ProductReceiptItem
            {
                Code = ViewModel.ScannedProductCode,
                Name = ViewModel.ScannedProductName,
                Price = ViewModel.ScannedProductUnitPrice,
                Quantity = ViewModel.ScannedProductQty
            });
        }

        ViewModel.ScannedProductCode = AppConstants.DefaultProductCode;
        ViewModel.ScannedProductName = AppConstants.DefaultProductName;
        ViewModel.ScannedProductUnitPrice = AppConstants.DefaultProductUnitPrice;
        ViewModel.ScannedProductQty = AppConstants.DefaultProductQuantity;
        ViewModel.ScannedProductTotalPrice = AppConstants.DefaultProductTotalPrice;
        ViewModel.EnableAddButton = false;

        EnterCodeTextBox.Text = string.Empty;

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);
    }

    private void OnProductReceiptItemListChange(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var hasProducts = ViewModel.ProductReceiptItemList.Any();
        ViewModel.EnableFinishButton = hasProducts;
        ViewModel.ReceiptProductsVisibility = hasProducts ? Visibility.Visible : Visibility.Collapsed;
        ViewModel.ReceiptTotal = ViewModel.ProductReceiptItemList.Sum(p => p.TotalPrice);
    }

    private async void OnRemoveProductButtonClick(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;

        var product = btn.DataContext as ProductReceiptItem;
        if (product == null) return;

        var result = await ShowDialogSafeAsync(new()
        {
            Title = resourceLoader.GetString(AppStrings.Warning),
            Content = resourceLoader.GetString(AppStrings.Are_you_sure_you_want_to_remove_this_item_from_the_receipt),
            PrimaryButtonText = resourceLoader.GetString(AppStrings.Remove),
            CloseButtonText = resourceLoader.GetString(AppStrings.Cancel),
        });

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);

        if (result != ContentDialogResult.Primary) return;

        ViewModel.ProductReceiptItemList.Remove(product);
    }

    private async void OnFinishReceiptButtonClick(object sender, RoutedEventArgs e)
    {
        var result = await ShowDialogSafeAsync(new()
        {
            Title = resourceLoader.GetString(AppStrings.Warning),
            Content = resourceLoader.GetString(AppStrings.Are_you_sure_you_want_to_finish_the_receipt),
            PrimaryButtonText = resourceLoader.GetString(AppStrings.Yes),
            CloseButtonText = resourceLoader.GetString(AppStrings.Cancel),
        });

        if (result != ContentDialogResult.Primary) return;

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);

        var dataset = ProductReceiptService.Instance.BuildDataSet(ViewModel.ProductReceiptItemList);
        var filename = AppSettings.Instance.GenerateReceiptFilepath();
        SpreadsheetService.Instance.WriteSpreadsheet(filename, dataset);

        await ShowDialogSafeAsync(new()
        {
            Title = resourceLoader.GetString(AppStrings.Information),
            Content = resourceLoader.GetString(AppStrings.Receipt_saved_as) + filename,
            CloseButtonText = resourceLoader.GetString(AppStrings.Accept),
        });

        ViewModel.ScannedProductCode = AppConstants.DefaultProductCode;
        ViewModel.ScannedProductName = AppConstants.DefaultProductName;
        ViewModel.ScannedProductUnitPrice = AppConstants.DefaultProductUnitPrice;
        ViewModel.ScannedProductQty = AppConstants.DefaultProductQuantity;
        ViewModel.ScannedProductTotalPrice = AppConstants.DefaultProductTotalPrice;
        ViewModel.ProductReceiptItemList.Clear();

        EnterCodeTextBox.Text = string.Empty;

        await FocusManager.TryFocusAsync(EnterCodeTextBox, FocusState.Programmatic);
    }
}

