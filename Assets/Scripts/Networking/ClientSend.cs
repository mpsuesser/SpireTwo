using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour {
    private static void SendTCPData(Packet _packet) {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet) {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    public static void RequestToJoinServer() {
        using (Packet _packet = new Packet((int)ClientPackets.requestToJoinServer)) {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void ChooseHero(Heroes _hero) {
        using (Packet _packet = new Packet((int)ClientPackets.chooseHero)) {
            _packet.Write(Client.instance.myId);
            _packet.Write((int)_hero);

            SendTCPData(_packet);
        }
    }

    public static void MovementInput(Movement.Direction _dir, Vector3 _rotationEulerAngles) {
        using (Packet _packet = new Packet((int)ClientPackets.movement)) {
            _packet.Write(Client.instance.myId);
            _packet.Write((int)_dir);
            _packet.Write(_rotationEulerAngles);

            SendUDPData(_packet);
        }
    }

    public static void JumpInput() {
        using (Packet _packet = new Packet((int)ClientPackets.jumpInput)) {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void AbilityPressed(int _ownerId, AbilityID _abilityID) {
        using (Packet _packet = new Packet((int)ClientPackets.abilityPressed)) {
            _packet.Write(Client.instance.myId);
            _packet.Write(_ownerId);
            _packet.Write((int)_abilityID);

            SendTCPData(_packet);
        }
    }

    public static void DungeonLoaded() {
        using (Packet _packet = new Packet((int)ClientPackets.dungeonLoaded)) {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }
}