using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameNetworkManager : NetworkManager {

    /// <PROTOCOL>
    const short INPUT_MSG = 0x1000;
    //note : we should register exact same messages for every clients since we want to simulate a p2p
    void RegisterProtocol(NetworkConnection _conn)
    {
       _conn.RegisterHandler(INPUT_MSG, OnInputMessage);
    }
    /// </PROTOCOL>

    /// <P2P>

    private bool m_isHost = false;
    //Send message to all : abstract client/server distinction from basic UNet module
    public void SendToAll(short _msgType, MessageBase _msg)
    {
        if (!IsClientConnected())
            return;
        if(m_isHost)
        {
            NetworkServer.SendToAll(_msgType, _msg);
        }
        else 
        {
            client.Send(_msgType, _msg);
        }
    }
    /// </P2P>

    static public  new GameNetworkManager singleton;
    void Awake()
    {
        //Check if singleton already exists
        if (singleton == null)
        {
            //if not, set singleton to this
            singleton = this;
        }
        //If singleton already exists and it's not this:
        else if (singleton != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one singleton of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        //for each client we register handler server to this client connection
        RegisterProtocol(conn);
        m_isHost = true;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        //register handlers for client to server connection 
        //except for host to avoid callback on our own message
        if (!m_isHost)
        {
            RegisterProtocol(conn);
            Completed.GameManager.instance.playersTurn = 1;
        }
    }


    /// <GAMEPLAY>

    //return true if character linked to its id is handle locally
    public bool IsLocalPlayer(int _CharacterID)
    {
        //since we have 2 players only here, use simple trick :
        // 1 is host, 2 is client
        if (_CharacterID == 1 && m_isHost)
            return true;
        if (_CharacterID == 2 && !m_isHost)
            return true;
        return false;

    }

    //Last input from network
    public NetworkInput CharacterNetInput;

    //shortcut : NetworkInput IS a message base, should not 
    public class NetworkInput : MessageBase
    {
        public int horizontal;
        public int vertical;
    } 

    public void SendInput(int _horizontal, int _vertical)
    {
        NetworkInput input = new NetworkInput();
        input.horizontal = _horizontal;
        input.vertical = _vertical;
        SendToAll(INPUT_MSG, input);
    }
    
    void OnInputMessage(NetworkMessage msg)
    {
        NetworkInput netInput = msg.ReadMessage<NetworkInput>();
        CharacterNetInput = netInput;
    }

    /// </GAMEPLAY>


}
