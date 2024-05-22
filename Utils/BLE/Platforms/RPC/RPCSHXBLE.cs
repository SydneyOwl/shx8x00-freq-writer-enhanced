﻿using System;
using System.Threading.Tasks;
using CookComputing.XmlRpc;
using SenhaixFreqWriter.Utils.BLE.Interfaces;
using SenhaixFreqWriter.Views.Common;

namespace SenhaixFreqWriter.Utils.BLE.Platforms.RPC;

public class RPCSHXBLE : IBluetooth
{
    private ProxyInterface proxy = (ProxyInterface)XmlRpcProxyGen.Create(typeof(ProxyInterface));
    // See BLEPlugin.py
    public async Task<bool> GetBleAvailabilityAsync()
    {
        try
        {
            return proxy.GetBleAvailability();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // DebugWindow.GetInstance().updateDebugContent("GetBleAvailabilityAsync-无法获取RPC客户端数据！");
            return false;
        }
    }

    public async Task<bool> ScanForShxAsync()
    {
        try
        {
            return proxy.ScanForShx();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // DebugWindow.GetInstance().updateDebugContent("GetBleAvailabilityAsync-无法获取RPC客户端数据！");
            return false;
        }
    }

    public async Task ConnectShxDeviceAsync()
    {
        if (!proxy.ConnectShxDevice())
        {
            throw new Exception("连接设备失败！");
        }
    }

    public async Task<bool> ConnectShxRwCharacteristicAsync()
    {
        try
        {
            return proxy.ConnectShxRwCharacteristic();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // DebugWindow.GetInstance().updateDebugContent("GetBleAvailabilityAsync-无法获取RPC客户端数据！");
            return false;
        }
    }

    public async Task<bool> ConnectShxRwServiceAsync()
    {
        try
        {
            return proxy.ConnectShxRwService();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // DebugWindow.GetInstance().updateDebugContent("GetBleAvailabilityAsync-无法获取RPC客户端数据！");
            return false;
        }
    }

    public void RegisterHid()
    {
        throw new System.NotImplementedException();
    }

    public void RegisterSerial()
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void SetStatusUpdater(Updater up)
    {
        throw new System.NotImplementedException();
    }
}

[XmlRpcUrl("http://localhost:8563")]
public interface ProxyInterface : IXmlRpcProxy
{
    [XmlRpcMethod("GetBleAvailability")]
    bool GetBleAvailability();
    
    [XmlRpcMethod("ScanForShx")]
    bool ScanForShx();
    
    [XmlRpcMethod("ConnectShxDevice")]
    bool ConnectShxDevice();
    
    [XmlRpcMethod("ConnectShxRwService")]
    bool ConnectShxRwService();
    
    [XmlRpcMethod("ConnectShxRwCharacteristic")]
    bool ConnectShxRwCharacteristic();
    
    [XmlRpcMethod("ReadCachedData")]
    byte[] ReadCachedData();
    
    [XmlRpcMethod("WriteData")]
    bool WriteData(byte[] data);
}