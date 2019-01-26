using System;
using ExitGames.Client.Photon.LoadBalancing;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    [EcsInject]
    internal class CallForChangeRoleSystem : IEcsRunSystem
    {
        private PhotonServer _photonServer;
        private GameState _gameState;
        public void Run ()
        {
            if (Input.GetKeyDown (KeyCode.Keypad1))
            {
                Try (PlayerRole.Fly);
            }
            else
            {
                if (Input.GetKeyDown (KeyCode.Keypad2))
                {
                    Try (PlayerRole.Shoot);
                }
                else
                {
                    if (Input.GetKeyDown (KeyCode.Keypad3))
                    {
                        Try (PlayerRole.Defend);
                    }
                    else
                    {
                        if (Input.GetKeyDown (KeyCode.Keypad4))
                        {
                            Try (PlayerRole.Heal);
                        }
                    }
                }
            }
        }

        private readonly RaiseEventOptions toServerOnly = new RaiseEventOptions ()
        {
            Receivers = ReceiverGroup.MasterClient
        };

        private void Try (PlayerRole newRole)
        {
            if (_photonServer.CurrentRoom != null && _photonServer.LocalPlayer != null)
            {
                var id = _photonServer.LocalPlayer.ID;
                if (_gameState.Roles.TryGetValue (id, out var role))
                {
                    if (role != newRole)
                    {
                        var playerWithRole = _gameState.GetPlayerWithRole (newRole);
                        if (playerWithRole == -1)
                        {
                            _photonServer.OpRaiseEvent (GameEventCode.ChangeRole, (byte) newRole, true, toServerOnly);
                        }
                    }
                }
            }
        }
    }
}
