﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.AppService;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ServiceInApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

    private AppServiceConnection inventoryService;

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      // Add the connection.
      if (this.inventoryService == null)
      {
        this.inventoryService = new AppServiceConnection();

        // Here, we use the app service name defined in the app service 
        // provider's Package.appxmanifest file in the <Extension> section.
        this.inventoryService.AppServiceName = "InProcessAppService";
        this.inventoryService.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;

        // Use Windows.ApplicationModel.Package.Current.Id.FamilyName 
        // within the app service provider to get this value.

        var status = await this.inventoryService.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
          textBox.Text = "Failed to connect";
          this.inventoryService = null;
          return;
        }
      }

      // Call the service.
      int idx = int.Parse(textBox.Text);
      var message = new ValueSet();
      message.Add("Command", "Item");
      message.Add("ID", idx);
      AppServiceResponse response = await this.inventoryService.SendMessageAsync(message);
      string result = "";

      if (response.Status == AppServiceResponseStatus.Success)
      {
        // Get the data  that the service sent to us.
        if (response.Message["Status"] as string == "OK")
        {
          result = response.Message["Result"] as string;
        }
      }

      message.Clear();
      message.Add("Command", "Price");
      message.Add("ID", idx);
      response = await this.inventoryService.SendMessageAsync(message);

      if (response.Status == AppServiceResponseStatus.Success)
      {
        // Get the data that the service sent to us.
        if (response.Message["Status"] as string == "OK")
        {
          result += " : Price = " + response.Message["Result"] as string;
        }
      }

      textBox.Text = result;
    }

  }
}
