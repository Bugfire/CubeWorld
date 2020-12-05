/* eslint-disable @typescript-eslint/no-unused-vars */
export class PhotonClient extends Photon.LoadBalancing.LoadBalancingClient {
  private readonly EVENT_MESSAGE_ID = 1;
  // エラー周り適切にハンドリングしていないので、1回試してダメなら諦める。
  private tryiedOnce = false;

  public constructor(
    isHttps: boolean,
    appId: string,
    appVersion: string,
    private systemMessage: (msg: string) => void,
    private userMessage: (msg: string) => void
  ) {
    super(
      isHttps ? Photon.ConnectionProtocol.Wss : Photon.ConnectionProtocol.Ws,
      appId,
      appVersion
    );
    this.setLogLevel(Exitgames.Common.Logger.Level.INFO);
  }

  public Connect(): void {
    this.connectToRegionMaster("JP");
  }

  public CreateOrJoinRoom(roomId?: string): void {
    if (this.tryiedOnce) {
      return;
    }
    this.tryiedOnce = true;
    if (roomId) {
      this.joinRoom(roomId);
    } else {
      this.createRoom(roomId);
    }
  }

  public LeaveRoom(): void {
    this.leaveRoom();
  }

  public SendMessage(message: string) {
    try {
      if (!this.isJoinedToRoom()) {
        this.systemMessage(`ルームに接続していません: [${message}]`);
        return;
      }
      const senderName = `User${this.myActor().actorNr}`;
      this.raiseEvent(this.EVENT_MESSAGE_ID, { message, senderName });
      this.userMessage(`${senderName}: ${message}`);
    } catch (err) {
      this.systemMessage(`エラーです: [${message}]`);
    }
  }

  onError(errorCode: number, errorMsg: string) {
    console.log(`Error: ${errorCode}: ${errorMsg}`);
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  onEvent(code: number, content: any, actorNr: number) {
    switch (code) {
      case this.EVENT_MESSAGE_ID:
        this.userMessage(`${content.senderName}: ${content.message}`);
        break;
      default:
        console.log(`Event: ${code}: ${actorNr}: ${content}`);
        break;
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  onStateChange(state: number): void {}
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  onActorPropertiesChange(actor: Photon.LoadBalancing.Actor): void {}
  onMyRoomPropertiesChange(): void {}
  onRoomListUpdate(
    rooms: Photon.LoadBalancing.Room[],
    _roomsUpdated: Photon.LoadBalancing.Room[],
    _roomsAdded: Photon.LoadBalancing.Room[],
    _roomsRemoved: Photon.LoadBalancing.Room[]
  ): void {
    console.log(`onRoomListUpdate: ${rooms}`);
    this.onRoomList(rooms);
  }
  onRoomList(rooms: Photon.LoadBalancing.Room[]) {
    console.log(`onRoomList: ${rooms}`);
    if (this.isInLobby() && !this.isJoinedToRoom()) {
      const avaiableRooms = this.availableRooms();
      if (avaiableRooms.length > 0) {
        this.CreateOrJoinRoom(avaiableRooms[0].name);
      } else {
        this.CreateOrJoinRoom();
      }
    }
  }
  onJoinRoom() {
    this.systemMessage(`Joined ${this.myRoom().name}`);
  }
  onActorJoin(actor: Photon.LoadBalancing.Actor) {
    this.systemMessage(`User(${actor.actorNr}) joined`);
  }
  onActorLeave(actor: Photon.LoadBalancing.Actor) {
    this.systemMessage(`User(${actor.actorNr}) left`);
  }
}
