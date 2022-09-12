using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScpDeconnexion
{
    public class EventHandlers
    {
        //variable using for anti-spawn ragdoll and Scp Termination Announcement.
        public bool HasQuitted = false;
        public Player QuittedPlayer;

        //variable using for scp 079.
        float Experience;
        float Energy;
        float MaxEnergy;
        byte Level;
        Exiled.API.Features.Camera camera;
        Mirror.SyncList<uint> door;

        public List<Player> SpectatorPlayer = new List<Player>();

        public void Disconnect(LeftEventArgs ev)
        {
            if (ev.Player.Role.Side == Exiled.API.Enums.Side.Scp)
            {
                HasQuitted = true;
                QuittedPlayer = ev.Player;
                foreach (Player plr in Player.List)
                {
                    if (plr != ev.Player)
                    {
                        if (plr.Role.Type == RoleType.Spectator)
                        {
                            SpectatorPlayer.Add(plr);
                        }
                    }
                }
                System.Random random = new System.Random();
                if (SpectatorPlayer.Count > 0)
                {
                    int test = random.Next(SpectatorPlayer.Count);
                    Player chech = SpectatorPlayer[test];
                    chech.Role.Type = ev.Player.Role.Type;
                    Vector3 position = ev.Player.Position;
                    Vector3 scale = ev.Player.Scale;
                    float hp = ev.Player.Health;
                    int maxhp = ev.Player.MaxHealth;
                    Vector3 rotation = ev.Player.Rotation;
                    float Ahp = ev.Player.ArtificialHealth;
                    float Maxahp = ev.Player.MaxArtificialHealth;
                    Player quitter = ev.Player;
                    RoleType quitterRole = ev.Player.Role.Type;
                    if (ev.Player.Role is Scp079Role Scp)
                    {
                        Experience = Scp.Experience;
                        Level = Scp.Level;
                        Energy = Scp.Energy;
                        MaxEnergy = Scp.MaxEnergy;
                        door = Scp.LockedDoors;
                        camera = Scp.Camera;
                    }
                    Timing.CallDelayed(0.5f, () =>
                    {
                        if (ev.Player.Role.Type != RoleType.Scp079)
                        {
                            chech.Position = position;
                            chech.Health = hp;
                            chech.MaxHealth = maxhp;
                            chech.Rotation = rotation;
                            chech.ArtificialHealth = Ahp;
                            chech.MaxArtificialHealth = Maxahp;
                            chech.Scale = scale;
                        }
                        else if (ev.Player.Role is Scp079Role scp079)
                        {
                            scp079.Level = Level;
                            scp079.Experience = Experience;
                            scp079.Camera = camera;
                            scp079.MaxEnergy = MaxEnergy;
                            scp079.Energy = Energy;
                            scp079.LockedDoors = door;
                        }
                        foreach (Player player in Player.List)
                        {
                            if (player.Role.Side == Exiled.API.Enums.Side.Scp)
                            {
                                player.Broadcast(5, $"{Plugin.Singleton.Config.ScpDeconnextionTitle}\n<size={Plugin.Singleton.Config.DescriptionSize}>{quitterRole} ({quitter.Nickname}){Plugin.Singleton.Config.ScpDeconnextionDescription}{chech.Nickname}</size>");
                            }
                        }
                        HasQuitted = false;
                        QuittedPlayer = null;
                    });
                    SpectatorPlayer.Clear();
                }
                

            }
        }
        public void SpawnRagdoll(SpawningRagdollEventArgs ev)
        {
            if (HasQuitted)
            {
                if (ev.Owner == QuittedPlayer)
                {
                    ev.IsAllowed = false;
                }
                
            }
        }
        public void ScpTerminationAnnouncement(AnnouncingScpTerminationEventArgs ev)
        {
            if (HasQuitted)
            {
                if (ev.Player == QuittedPlayer)
                {
                    ev.IsAllowed = false;
                }
            }
        }
    }
}