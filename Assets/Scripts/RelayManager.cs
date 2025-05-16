using System;
using UnityEngine;
using UnityEngine.UI;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class RelayManager : MonoBehaviour
{
    
    public int maxPlayers = 4;
    [HideInInspector] public string joinCode;
    public Text joinCodeText;
    async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await SignInAnonymously();

        Debug.Log("[Relay] Servicios Unity iniciados.");
    }

    async System.Threading.Tasks.Task SignInAnonymously()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log($"[Relay] Signed in: {AuthenticationService.Instance.PlayerId}");
    }

    public async void CreateRelay()
    {
        try
        {
          
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            SetupHost(allocation);
            NetworkManager.Singleton.StartHost();
            Debug.Log($"[Relay] Host creado. Join Code: {joinCode}");
            joinCodeText.text = joinCode;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Relay] Error creando host: {e}");
        }
    }

    public async void JoinRelay(string code)
    {
        try
        {
            joinCode = code;
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            SetupClient(joinAllocation);
            
            NetworkManager.Singleton.StartClient();
            Debug.Log("[Relay] Cliente conectado al host");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Relay] Error al unirse al relay: {e}");
        }
    }

    void SetupHost(Allocation allocation)
    {
        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        utp.SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData,
            true // enableEncryption
        );
    }

    void SetupClient(JoinAllocation joinAllocation)
    {
        var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        utp.SetClientRelayData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key, 
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData,
            true // enableEncryption
        );
    }
}
