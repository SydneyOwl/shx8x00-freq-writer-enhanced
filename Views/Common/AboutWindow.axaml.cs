﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MsBox.Avalonia;
using Newtonsoft.Json.Linq;
using Version = SenhaixFreqWriter.Properties.Version;

namespace SenhaixFreqWriter.Views.Common;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) windows.Background = Brushes.BlanchedAlmond;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) linux.Background = Brushes.BlanchedAlmond;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) macos.Background = Brushes.BlanchedAlmond;
    }

    private void RepoButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenUrl("https://github.com/SydneyOwl/senhaix-freq-writer-enhanced");
    }

    private void OpenUrl(string url)
    {
        try
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                }
            }
        }
        catch
        {
        }
    }

    private async void UpdateButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var repoOwner = "SydneyOwl";
        var repoName = "senhaix-freq-writer-enhanced";
        var apiUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";

        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "agent");

        try
        {
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var currentTagTime = Version.BuildTime;
                if (currentTagTime.Equals("@BUILD_TIME@"))
                {
                    MessageBoxManager.GetMessageBoxStandard("注意", "此版本未被发行！").ShowWindowDialogAsync(this);
                    return;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var releaseJson = JObject.Parse(jsonContent);
                var tagTime = (string)releaseJson["published_at"];
                var tagTimeParsed = DateTime.Parse(tagTime);
                var curTagTimeParsed = DateTime.Parse(currentTagTime);
                // 转UTC
                curTagTimeParsed = curTagTimeParsed.ToUniversalTime();

                var minuteSpan = Math.Abs(new TimeSpan(tagTimeParsed.Ticks - curTagTimeParsed.Ticks).TotalMinutes);

                if (minuteSpan > 10)
                    MessageBoxManager.GetMessageBoxStandard("注意", "有新版本可用~").ShowWindowDialogAsync(this);
                else
                    MessageBoxManager.GetMessageBoxStandard("注意", "您已是最新版~").ShowWindowDialogAsync(this);
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandard("注意", "无法获取最新版本信息~").ShowWindowDialogAsync(this);
            }
        }
        catch
        {
            MessageBoxManager.GetMessageBoxStandard("注意", "无法获取最新版本信息~").ShowWindowDialogAsync(this);
        }
    }
}