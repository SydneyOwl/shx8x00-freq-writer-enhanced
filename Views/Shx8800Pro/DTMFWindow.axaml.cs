﻿using System;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;
using MsBox.Avalonia;
using SenhaixFreqWriter.DataModels.Shx8800Pro;

namespace SenhaixFreqWriter.Views.Shx8800Pro;

public partial class DtmfWindow : Window
{
    private ObservableCollection<DtmpObject> _dtmfs = new();

    private int _idleTime = AppData.GetInstance().Dtmfs.IdleTime;

    private string _myId = AppData.GetInstance().Dtmfs.LocalId;

    private int _pttid = AppData.GetInstance().Dtmfs.Pttid;

    private int _wordTime = AppData.GetInstance().Dtmfs.WordTime;

    public DtmfWindow()
    {
        InitializeComponent();
        var dtmfOrig = AppData.GetInstance().Dtmfs;
        for (var i = 0; i < 15; i++)
        {
            var tmp = new DtmpObject();
            tmp.Id = (i + 1).ToString();
            tmp.GroupName = dtmfOrig.GroupName[i];
            tmp.Group = dtmfOrig.Group[i];
            _dtmfs.Add(tmp);
        }

        DataContext = this;
        Closing += (sender, args) =>
        {
            var length = AppData.GetInstance().Dtmfs.Group.Length;
            for (var i = 0; i < length; i++) AppData.GetInstance().Dtmfs.Group[i] = Dtmfs[i].Group;
        };
    }

    public int PttId
    {
        get => _pttid;
        set
        {
            _pttid = value;
            AppData.GetInstance().Dtmfs.Pttid = value;
        }
    }

    public int WordTime
    {
        get => _wordTime;
        set
        {
            _wordTime = value;
            AppData.GetInstance().Dtmfs.WordTime = value;
        }
    }

    public int IdleTime
    {
        get => _idleTime;
        set
        {
            _idleTime = value;
            AppData.GetInstance().Dtmfs.IdleTime = value;
        }
    }


    public string MyId
    {
        get => _myId;
        set
        {
            _myId = value ?? throw new ArgumentNullException(nameof(value));
            AppData.GetInstance().Dtmfs.LocalId = value;
        }
    }

    public ObservableCollection<DtmpObject> Dtmfs
    {
        get => _dtmfs;
        set => _dtmfs = value ?? throw new ArgumentNullException(nameof(value));
    }

    private void GroupCodeInputElement_OnLostFocus(object? sender, TextChangedEventArgs e)
    {
        var textbox = (TextBox)sender;
        var inputText = textbox.Text;
        if (inputText.Length > 8)
        {
            MessageBoxManager.GetMessageBoxStandard("注意", "最多8位！").ShowWindowDialogAsync(this);
            textbox.Text = "#EDIT#";
            return;
        }

        foreach (var c in inputText)
            if ((c < '0' || c > '9') && (c < 'A' || c > 'D') && c != '*' && c != '#')
            {
                MessageBoxManager.GetMessageBoxStandard("注意", "码只能是数字、大写字母以及*#").ShowWindowDialogAsync(this);
                textbox.Text = "#EDIT#";
                return;
            }
    }

    private void GroupNameInputElement_OnLostFocus(object? sender, TextChangedEventArgs e)
    {
        var textbox = (TextBox)sender;
        var inputText = textbox.Text;
        var bytes = Encoding.GetEncoding("gb2312").GetBytes(inputText);
        if (bytes.Length > 12) textbox.Text = Encoding.GetEncoding("gb2312").GetString(bytes, 0, 12);
    }
}