using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using GH.Player;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class PhotonCustomSample : Photon.PunBehaviour
{
    bool isMaster;
    PlayerProfile thisPlayer;
    public Text text;
    public override void OnConnectedToMaster()
    {
        text.text = "Connected";
        base.OnConnectedToMaster();
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {

        Debug.Log("Failed to Connected");
        base.OnFailedToConnectToPhoton(cause);
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("RandomRoomNotFound: Create  new Room");
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        CreateRoom();
    }
    private void CreateRoom()
    {
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RandomString(256), room, TypedLobby.Default);
    }
    private System.Random random = new System.Random();
    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        isMaster = true;
        Debug.Log("Room Created: I'm Master");
        initPlayer();
        setCustomProperties();
    }
    private void setCustomProperties()
    {
        int playerPhotonId = photonView.ownerId;
        string playerName = thisPlayer.Name;
        Sprite playerAvatar = thisPlayer.PlayerAvatar;

        Hashtable hash = new Hashtable();

        hash.Add("YAYA", playerName);
        //hash.Add(2, playerPhotonId);
        Debug.Log(hash["YAYA"]);
        PhotonNetwork.player.SetCustomProperties(hash);

        Hashtable test = PhotonNetwork.player.CustomProperties;

        Debug.Log(test.Count);
        Debug.Log("Test = " + test["YAYA"]);
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        Debug.Log("CustomPlayerProperties Changed");
    }



    public void Init()
    {
        PhotonNetwork.ConnectUsingSettings("1");
        text.text = "PhotonConnected";
        //Use this to add something in future
    }

    private void initPlayer()
    {
        thisPlayer = new PlayerProfile();
        Debug.Log("PlayerInitialised");
        if (isMaster)
        {
            thisPlayer.Name = "GameHolic";
            PhotonNetwork.player.UserId = "GameHolic";
        }
        else
        {
            thisPlayer.Name = "FitnessHolic";
            PhotonNetwork.player.UserId = "FitnessHolic";
        }
        setCustomProperties();
    }
    private void Start()
    {

        PhotonPeer.RegisterType(typeof(PlayerProfile), (byte)'P', PlayerProfile.Serialize, PlayerProfile.Deserialize);


        text.text = "StartProgram";
        PhotonNetwork.autoCleanUpPlayerObjects = false;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = false;
        Init();

    }

    private void Update()
    {

        //text.text =  PhotonNetwork.otherPlayers[0].UserId;
        if (Input.GetMouseButtonDown(0) && !PhotonNetwork.inRoom)
        {
            Debug.Log("MouseButtonOn");
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            if (rooms == null)
            {
                Debug.Log("There is no existing room");
                OnCreatedRoom();
            }
            else
            {
                OnPlayGame();

            }
        }
        else if (Input.GetMouseButtonDown(0) && PhotonNetwork.inRoom)
        {
            text.text = "Im in room already";
            //Hashtable hash = PhotonNetwork.otherPlayers[0].CustomProperties;
            Hashtable hash = PhotonNetwork.otherPlayers[0].CustomProperties;
            if (hash.Count > 0)
            {
                Debug.LogWarning("Hash count : " + hash.Count);
                for (int i = 0; i < hash.Count; i++)
                {

                    Debug.Log(hash["YAYA"]);
                }
            }
            else
            {
                
                Debug.Log(hash.Count);
                text.text = "CustomProperty set";
            }
        }
    }

    public void OnPlayGame()
    {
        JoinRandomRoom();
    }
    private void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        text.text = "Waiting other player";
        if(!isMaster)
        {
            initPlayer();            
        }
        
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        text.text = "PlayerConnected";
    }
}

