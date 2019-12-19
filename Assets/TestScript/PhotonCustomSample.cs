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
        Debug.Log("FinalCheckPlayerName: "+thisPlayer.Name);
        int playerPhotonId = photonView.ownerId;
        string playerId = thisPlayer.UniqueId;
        Sprite playerAvatar = thisPlayer.PlayerAvatar;
        //byte[] player = PlayerProfile.Serialize(thisPlayer);
        //Debug.Log("Serialized Player: " + player);

        //player = (byte[])PhotonNetwork.room.CustomProperties["MasterProfile"];
        //thisPlayer = (PlayerProfile)PhotonNetwork.room.CustomProperties["MasterProfile"];
        //Debug.Log("Check Player: " + thisPlayer);

        Hashtable hash = new Hashtable();
        hash.Add("MasterProfile", player);
        //hash.Add("MasterProfile", thisPlayer);

        PhotonNetwork.player.SetCustomProperties(hash);
        PhotonNetwork.room.SetCustomProperties(hash);

    }


    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        Debug.Log("CustomPlayerProperties Changed");


        Hashtable testPlayer = PhotonNetwork.player.CustomProperties;


        if (testPlayer.ContainsKey("MasterProfile"))
        {
            Debug.Log("MasterProfile added succesfully");
            Debug.Log(testPlayer["MasterProfile"]);
        }
        else
            Debug.Log("PlayerFail");
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
        if (isMaster)
            thisPlayer.Name = "GameHolic";
        else
            thisPlayer.Name = "FitnessHolic";
        Debug.Log(thisPlayer.Name);
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
        
        if(Input.GetMouseButtonDown(0) && !PhotonNetwork.inRoom)
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
            //Hashtable hash = PhotonNetwork.player.CustomProperties;
            Hashtable hash = PhotonNetwork.room.CustomProperties;


            if (hash.Count > 0)
                Debug.LogWarning("Hash count : " + hash.Count);
            else
                Debug.Log(hash.Keys);

            if(hash.ContainsKey("MasterProfile"))
            {
                Debug.Log(hash["MasterProfile"]);
                text.text = "MasterProfile";
            }
            else
            {
                Debug.LogWarning("There is no value for MasterProfile");
            }
            if (hash.ContainsKey("ClientProfile"))
            {
                PlayerProfile p = (PlayerProfile)hash["Clientprofile"];
                text.text = "Client Profile is " + p.Name;
            }
            else
            {
                Debug.LogWarning("There is no value for ClientProfile");
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
            //Hashtable masterHash = PhotonNetwork.otherPlayers[0].CustomProperties;
            Hashtable masterHash = PhotonNetwork.room.CustomProperties;
            Debug.Log("MasterHash count: " + masterHash.Count);
            //thisPlayer = (PlayerProfile)PhotonNetwork.player.CustomProperties["ClientProfile"]; Hashtable hash = new Hashtable();
            if (thisPlayer.Name == null)
                thisPlayer.Name = "ClientHolic";
            thisPlayer = (PlayerProfile)PhotonNetwork.room.CustomProperties["ClientProfile"]; Hashtable hash = new Hashtable();
            masterHash.Add("ClientProfile", thisPlayer);
            if (masterHash.ContainsKey("ClientProfile"))
                text.text = "ClientProfile added succesfully";
            PhotonNetwork.room.SetCustomProperties(masterHash);
        }
        
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        text.text = "PlayerConnected";
    }
}

