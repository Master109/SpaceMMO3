import System.Collections.Generic;

    //GUI vars
    private  var hostPlayers : int= 16;
    private  var hostSettingTitle : String= "No name";

    private  var currentSubMenu : String= "";
    private  var levelNrToLoad : int= -1;
    
    var mmo = false;
    var menuName = "MMO Menu";

    function Awake () : void {
        GameSettings.Clear(); 
    }

    function Start ()  {

        //Default join values
        joinPort = MultiplayerFunctions.SP.defaultServerPort;
        joinIP = joinPW = "";

        //Default host values
        hostTitle = PlayerPrefs.GetString("hostTitle", "Guests server");
        hostDescription = PlayerPrefs.GetString("hostDescription", "Servers description");
        hostMOTD = PlayerPrefs.GetString("hostMOTD", "Servers message of the day");
        hostPW = PlayerPrefs.GetString("hostPassword", "");
        hostMaxPlayers = PlayerPrefs.GetInt("hostPlayers" , 8);
        hostPort = PlayerPrefs.GetInt("hostPort", MultiplayerFunctions.SP.defaultServerPort);

        hostDataList = new List.<MyHostData>();
        MultiplayerFunctions.SP.SetHostListDelegate(FullHostListReceived);

    }



    function EnableMenu () : void {
        ReloadSettings();
    }
    function DisableMenu () : void {
        AbortRandomConnect();
        if (MultiplayerFunctions.SP.IsConnecting()) MultiplayerFunctions.SP.CancelConnection();
    }


    function ReloadSettings () : void {
        MultiplayerFunctions.SP.FetchHostList();
    }

    function OpenSubMenu ( newMenu : String  ) : void {
        currentSubMenu = newMenu;
    }


    function OnConnectedToServer () : void {
        GameSettings.Clear();
        //Stop communication until in the game
        Network.isMessageQueueRunning = false;
        Application.LoadLevel(Application.loadedLevel+1);  
    }

    function OnServerInitialized () : void {
        //Load game
        Application.LoadLevel(Application.loadedLevel+1); 
    }

    function OnGUI () : void {
        GUILayout.Window(2, new Rect(Screen.width/ 2 - 600/2, Screen.height/2-550/2, 600, 550), WindowGUI, "");
    }

  


    function hostGUI () : void {

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Host title: ", GUILayout.Width(125));
        hostSettingTitle = GUILayout.TextField(hostSettingTitle, GUILayout.Width(125));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Use port: ", GUILayout.Width(125));
        hostPort = int.Parse(GUILayout.TextField(hostPort.ToString(), GUILayout.Width(50)));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max. players: ", GUILayout.Width(125));
        hostPlayers = int.Parse(GUILayout.TextField(hostPlayers.ToString(), GUILayout.Width(50)));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        // Start a new server
        if (GUILayout.Button("Start hosting a server"))
        {
            StartHostingGame(hostSettingTitle, hostPlayers, hostPort, "MOTD", "DESC", "");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }



    private var currentGUIMethod = "join";



    function WindowGUI () : void {
        GUILayout.BeginHorizontal();
        GUILayout.Label(menuName);
        GUILayout.FlexibleSpace();        
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Select an option:");
        GUILayout.Space(5);
        if (currentGUIMethod == "join")
        {
            GUILayout.Label("Join", GUILayout.Width(75));
        }else{
            if (GUILayout.Button("Join", GUILayout.Width(75)))
            {
                currentGUIMethod = "join";
            }
        }
        if (currentGUIMethod == "host"){
            GUILayout.Label("Host", GUILayout.Width(75));
        }else{
            if (GUILayout.Button("Host", GUILayout.Width(75)))
            {
                currentGUIMethod = "host";
            }
        }
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(25);
            
        
        if (currentGUIMethod == "join") 
            JoinMenu();      
        else        
            HostMenu();
                 
    }

    private var JoinScrollPosition : Vector2;
    private var hostDataList :  List.<MyHostData> = new List.<MyHostData>();

    private  var joinPort : int;
    private  var joinIP : String= "";
    private  var joinPW : String= "";
    private  var joinUsePW : boolean= false;

    private  var failConnectMesage : String= "";

    function JoinMenu () : void {
        if (MultiplayerFunctions.SP.IsConnecting())
        {
             var timeSince : float= Mathf.Round(MultiplayerFunctions.SP.TimeSinceLastConnect() * 10) / 10;
             var status : String= "Trying to connect to [" + MultiplayerFunctions.SP.ConnectingToAddress() + "]";
            if (joinPW != "")
            {
                status += " using password.";
            }
            GUILayout.Label(status);
            GUILayout.Label("Waiting: " + timeSince);
            if (timeSince >= 2 && GUILayout.Button("Cancel"))
            {
                MultiplayerFunctions.SP.CancelConnection();
            }
        }
        else if (failConnectMesage != "")
        {
            GUILayout.Label("The game failed to connect:\n" + failConnectMesage);
            if (lastConnectError == NetworkConnectionError.InvalidPassword)
            {
                GUILayout.Label("You entered a wrong password, try again here:");
                joinIP = MultiplayerFunctions.SP.LastIP()[0];
                joinPort = MultiplayerFunctions.SP.LastPort();
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                GUILayout.Label("IP");
                GUILayout.Label(joinIP, GUILayout.Width(100));
                GUILayout.Label("Port");
                GUILayout.Label(joinPort + "", GUILayout.Width(50));
                GUILayout.Label("Password");
                joinPW = GUILayout.TextField(joinPW, GUILayout.Width(100));
                if (GUILayout.Button("Connect"))
                {
                    MultiplayerFunctions.SP.DirectConnect(joinIP, joinPort, joinPW, true, OnFinalFailedToConnect);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Cancel"))
            {
                failConnectMesage = "";
            }
        }
        else
        {
            if (joiningRandomGame)
            {
                GUILayout.Label("Trying to connect to first possible game...");
                if (GUILayout.Button("Cancel"))
                {
                    joiningRandomGame = false;
                    MultiplayerFunctions.SP.CancelConnection();
                }
            }
            else
            {     
                //Masterlist
                GUILayout.BeginHorizontal();
                GUILayout.Label("Game list:");

                GUILayout.FlexibleSpace();
                if (hostDataList != null && hostDataList.Count > 0 && GUILayout.Button("Join random game"))
                {
                    StartCoroutine(StartJoinRandom());
                }
                if (GUILayout.Button("Refresh list"))
                {
                    MultiplayerFunctions.SP.FetchHostList();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                GUILayout.Space(24);

                GUILayout.Label("Title", GUILayout.Width(200));
                GUILayout.Label("Players", GUILayout.Width(55));
                GUILayout.Label("IP", GUILayout.Width(150));
                GUILayout.Label("Dedicated", GUILayout.Width(70));
                GUILayout.EndHorizontal();


                JoinScrollPosition = GUILayout.BeginScrollView(JoinScrollPosition);
                for(var  hData : MyHostData  in hostDataList )
                {
                   GUILayout.BeginHorizontal();

                    if (hData.passwordProtected)
                        GUILayout.Label("PW", GUILayout.MaxWidth(16));
                    else
                        GUILayout.Space(24);

                    if (GUILayout.Button("" + hData.title, GUILayout.Width(200)))
                    {
                        MultiplayerFunctions.SP.HostDataConnect(hData.realHostData, "", true, OnFinalFailedToConnect);
                    }
                    GUILayout.Label(hData.connectedPlayers + "/" + hData.maxPlayers, GUILayout.Width(55));
                    GUILayout.Label(hData.IP[0] + ":" + hData.port, GUILayout.Width(150));

                    //Options
                    GUILayout.Space(35 - 8);
                    if (hData.isDedicated)
                        GUILayout.Label("D", GUILayout.Width(70));
                    else
                        GUILayout.Space(70);



                    GUILayout.EndHorizontal();
                }
                if(hostDataList.Count==0){
                    GUILayout.Label("No servers running right now");
                }
                GUILayout.EndScrollView();

                 var text : String= hostDataList.Count + " total servers";
                GUILayout.Label(text);

                //DIRECT JOIN

                GUILayout.BeginHorizontal();
                GUILayout.Label("Direct join:");
                GUILayout.Space(5);
                GUILayout.Label("IP");
                joinIP = GUILayout.TextField(joinIP, GUILayout.Width(100));
                GUILayout.Label("Port");
                joinPort = int.Parse(GUILayout.TextField(joinPort + "", GUILayout.Width(50)) + "");
                GUILayout.Label("Password");
                joinUsePW = GUILayout.Toggle(joinUsePW, "", GUILayout.MaxWidth(22));
                if (joinUsePW)
                {
                    joinPW = GUILayout.TextField(joinPW, GUILayout.Width(100));
                }
                if (GUILayout.Button("Connect"))
                {
                    MultiplayerFunctions.SP.DirectConnect(joinIP, joinPort, joinPW, true, OnFinalFailedToConnect);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
            }
        }
    }

 
    private  var joiningRandomGame : boolean= false;
    private  var randConnectNr : int= 0;


    function StartJoinRandom ( ) : IEnumerator {
        if (joiningRandomGame) return;
        joiningRandomGame = true;

        while (joiningRandomGame && (!hasParsedHostListOnce || !MultiplayerFunctions.SP.ReadyLoading() || !MultiplayerFunctions.SP.HasReceivedHostList()))
        {
            yield 0;
        }
        if (joiningRandomGame)
        {
            randConnectNr = 1;
            for( var hData : MyHostData  in  hostDataList)
            {
                MultiplayerFunctions.SP.HostDataConnect(hData.realHostData, "", true, OnFinalFailedToConnect);
                yield new WaitForSeconds(2);
                if (Network.isClient || !joiningRandomGame) break;
                randConnectNr++;
            }
        }
        joiningRandomGame = false;
    }

    function AbortRandomConnect () : void {
        joiningRandomGame = false;
    }
    function IsDoingRandomConnect () : boolean {
         return joiningRandomGame;
    }
    function RandConnectNr () : String {
        return randConnectNr + "/" + hostDataList.Count;
    }


     private var lastConnectError : NetworkConnectionError;

    function OnFinalFailedToConnect () : void {
        lastConnectError = MultiplayerFunctions.SP.LastConnectionError();
        failConnectMesage = failConnectMesage + "Attempting to connect to [" + MultiplayerFunctions.SP.LastIP()[0] + ":" + MultiplayerFunctions.SP.LastPort() + "]: " + lastConnectError + "\n";
        Debug.Log("OnFinalFailedToConnect=" + failConnectMesage);
    }



    private var hostTitle : String;
    private var hostMOTD : String;
    private var hostDescription : String;
    private var hostPW : String;
    private var hostMaxPlayers : int;
    private var hostPort : int;
    private var hostUsePassword : boolean= false;


    function HostMenu () : void {


        GUILayout.BeginHorizontal();
        GUILayout.Label("Host a new game:");
        GUILayout.EndHorizontal();

        //GUILayout.Toggle(true, "Construction gamemode");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:");
        GUILayout.FlexibleSpace();
        hostTitle = GUILayout.TextField(hostTitle, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Server description");
        GUILayout.FlexibleSpace();
        hostDescription = GUILayout.TextField(hostDescription, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("MOTD");
        GUILayout.FlexibleSpace();
        hostMOTD = GUILayout.TextField(hostMOTD, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Server password ", GUILayout.Width(200), GUILayout.Height(23));
        GUILayout.FlexibleSpace();
        hostUsePassword = GUILayout.Toggle(hostUsePassword, "", GUILayout.MaxWidth(40));
        if (hostUsePassword)
        {
            hostPW = GUILayout.TextField(hostPW, GUILayout.Width(200));

        }
        else
        {
            GUILayout.Label("", GUILayout.Height(23), GUILayout.Width(200));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max players");
        GUILayout.FlexibleSpace();
        hostMaxPlayers = int.Parse(GUILayout.TextField(hostMaxPlayers + "", GUILayout.Width(50)) + "");
        GUILayout.EndHorizontal();

        CheckHostVars();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Start server", GUILayout.Width(150)))
        {
            StartHostingGame(hostTitle,hostMaxPlayers, hostPort,hostMOTD, hostDescription, hostPW);
        }
        GUILayout.EndHorizontal();
    }

    function CheckHostVars () : void {
        //hostMaxPlayers = Mathf.Clamp(hostMaxPlayers, 1, 64);
        //hostPort = Mathf.Clamp(hostPort, 10000, 100000);
        //hostTitle = (hostTitle);
        //hostMOTD = (hostMOTD);
        //hostDescription = (hostDescription);
        //hostPW = (hostPW);
    }


    function StartHostingGame ( hostSettingTitle : String ,   hostPlayers : int ,   hostPort : int ,  motd : String ,   description : String ,   password : String  ) : void {
        if (Network.isServer)
        {
            Network.Disconnect();

        }
        if (hostSettingTitle == "")
        {
            hostSettingTitle = "NoTitle";
        }
        
        //hostPlayers = Mathf.Clamp(hostPlayers, 0, 64);
        //hostPort = Mathf.Clamp(hostPort, 10000, 100000);
        //hostSettingTitle = (hostSettingTitle);
        //description = (description);
        //password = (password);
        
        GameSettings.Clear();
        GameSettings.motd = motd;
        GameSettings.description = description;
        GameSettings.serverTitle = hostSettingTitle;
        GameSettings.port = hostPort;
        GameSettings.IP = "localhost";
        GameSettings.players = hostPlayers;
        GameSettings.password = password;

        //maxplayers =2 should open only 1 more connection.
        //if (!isDedicated)
        //{
            hostPlayers -= 1;
        //}

        MultiplayerFunctions.SP.StartServer(password, hostPort, hostPlayers, true);
        
    }


    function FullHostListReceived () : void {
        StartCoroutine(ReloadHosts());
    }

     private var hasParsedHostListOnce : boolean= false;
     private var parsingHostList : boolean= false;

    function ReloadHosts () : IEnumerator {
        
        if (parsingHostList) return;
        parsingHostList = true;
         var newData : HostData[]= MultiplayerFunctions.SP.GetHostData();
             
          var hostLenght : int= -1;
        while (hostLenght != newData.Length)
        {
            yield new WaitForSeconds(0.5f);
            newData = MultiplayerFunctions.SP.GetHostData();
            hostLenght = newData.Length;
        }

        hostDataList.Clear();
        for(var hData  : HostData    in newData )
        {
             var cHost : MyHostData= new MyHostData();
            cHost.realHostData = hData;
            cHost.connectedPlayers = hData.connectedPlayers;
            cHost.IP = hData.ip;
            cHost.port = hData.port;
            cHost.maxPlayers = hData.playerLimit;

            cHost.passwordProtected = hData.passwordProtected;
            cHost.title = hData.gameName;
            cHost.useNAT = hData.useNat;
            
            /*//EXAMPLE CUSTOM FIELDS
            var comments : String[]= hData.comment.Split("#"[0]);
            cHost.gameVersion = int.Parse(comments[2]);

            //cHost.isDedicated = comments[1] == "1";         
            if (cHost.isDedicated)
            {
                cHost.connectedPlayers -= 1;
                cHost.maxPlayers -= 1;
            }*/

            hostDataList.Add(cHost);
            
            if (hostDataList.Count % 3 == 0)
            {
                yield 0;
            }
        }
        parsingHostList = false;
        hasParsedHostListOnce = true;
    }



class MyHostData
{
     var realHostData : HostData;
     var title : String;
     var useNAT : boolean;
     var connectedPlayers : int;
     var maxPlayers : int;
     var IP : String[];
     var port : int;
     var passwordProtected : boolean;
     
     //Example custom fields
     var isDedicated : boolean= false;
     var gameVersion : int;
}

