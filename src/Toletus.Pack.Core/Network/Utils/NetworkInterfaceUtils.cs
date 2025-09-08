﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Toletus.Pack.Core.Network.Utils;

public abstract class NetworkInterfaceUtils
{
    public static NetworkInterface? GetDefaultNetworkInterface()
    {
        NetworkInterface? defaultInterface = null;
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            // Check if the network interface is up and active
            if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                 networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel))
            {
                // Get the properties of the network interface
                IPInterfaceProperties properties = networkInterface.GetIPProperties();

                // Check if the interface has a gateway address (indicating it is used for outbound connections)
                foreach (GatewayIPAddressInformation gateway in properties.GatewayAddresses)
                {
                    if (!gateway.Address.Equals(IPAddress.None))
                    {
                        defaultInterface = networkInterface;
                        break; // Exit the loop if a valid default interface is found
                    }
                }

                if (defaultInterface != null)
                    break; // Exit the outer loop once the default interface is determined
            }
        }

        return defaultInterface; // Return the default network interface or null if not found
    }

    public static IPAddress? GetDefaultNetworkIPAddress()
    {
        NetworkInterface? defaultInterface = GetDefaultNetworkInterface();
        if (defaultInterface != null)
        {
            IPInterfaceProperties properties = defaultInterface.GetIPProperties();
            foreach (UnicastIPAddressInformation ipInfo in properties.UnicastAddresses)
            {
                // Check if the IP address is not a loopback and is IPv4 (you can modify for IPv6 if needed)
                if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(ipInfo.Address))
                {
                    return ipInfo.Address; // Return the first valid IPv4 address found
                }
            }
        }

        return null; // Return null if no IP address is found
    }
    
    public static Dictionary<string, IPAddress> GetNetworkInterfaces()
    {
        var redes = new Dictionary<string, IPAddress>();

        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        foreach (var ip in nic.GetIPProperties().UnicastAddresses)
            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                redes.TryAdd(nic.Name, ip.Address);

        return redes;
    }

    public static IPAddress? GetNetworkInterfaceIpAddressByName(string networkInterfaceName)
    {
        var adaptors = NetworkInterface.GetAllNetworkInterfaces().ToList();
        var adaptor = adaptors.FirstOrDefault(c => c.Name == networkInterfaceName);

        return adaptor?.GetIPProperties()
            .UnicastAddresses
            .FirstOrDefault(c => c.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
    }
}