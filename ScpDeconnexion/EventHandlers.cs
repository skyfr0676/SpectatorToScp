using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using Camera = Exiled.API.Features.Camera;

namespace ScpDeconnexion
{
    public class EventHandlers
    {
        //variable using for anti-spawn ragdoll and Scp Termination Announcement.
        public Player QuittedPlayer;

        //variable using for scp 079.
        int Experience;
        float Energy;
        float MaxEnergy;
        int Level;
        Camera camera;
        Random random = new Random();

        public List<Player> SpectatorPlayer = new List<Player>();

        public void Disconnect(LeftEventArgs ev)
        {
            if (ev.Player.Role.Side == Side.Scp)
            {
                QuittedPlayer = ev.Player;
                foreach (Player plr in Player.List.Where(x=>x.Role.Type == RoleTypeId.Spectator))
                        SpectatorPlayer.Add(plr);

                if (SpectatorPlayer.Count > 0)
                {
                    int PlayerRandom = random.Next(SpectatorPlayer.Count);
                    Player NewPlayer = SpectatorPlayer[PlayerRandom];
                    NewPlayer.Role.Set(ev.Player.Role, SpawnReason.RoundStart);
                    Vector3 position = ev.Player.Position;
                    Vector3 scale = ev.Player.Scale;
                    float hp = ev.Player.Health;
                    float maxhp = ev.Player.MaxHealth;
                    Vector3 rotation = ev.Player.Rotation;
                    float Ahp = ev.Player.ArtificialHealth;
                    float Maxahp = ev.Player.MaxArtificialHealth;
                    Player quitter = ev.Player;
                    RoleTypeId quitterRole = ev.Player.Role.Type;
                    if (ev.Player.Role is Scp079Role Scp)
                    {
                        Experience = Scp.Experience;
                        Level = Scp.Level;
                        Energy = Scp.Energy;
                        MaxEnergy = Scp.MaxEnergy;
                        camera = Scp.Camera;
                    }
                    string replace = Plugin.Singleton.Config.ScpDeconnextionDescription.Replace("{name}", NewPlayer.Nickname).Replace("{oldRole}", ev.Player.Role.Type.ToString()).Replace("{oldName}", ev.Player.Nickname);
                    foreach (Player player in Player.List.Where(x => x.Role.Side == Side.Scp))
                    {
                        Log.Debug(replace);

                        player.Broadcast(5, $"{Plugin.Singleton.Config.ScpDeconnextionTitle}\n<size={Plugin.Singleton.Config.DescriptionSize}>{replace}</size>");
                    }

                    Timing.CallDelayed(0.5f, () =>
                    {
                        if (NewPlayer.Role is Scp079Role scp079)
                        {
                            scp079.Level = Level;
                            scp079.Experience = Experience;
                            scp079.MaxEnergy = MaxEnergy;
                            scp079.Energy = Energy;
                            scp079.Camera = camera;
                            QuittedPlayer = null;
                        }
                        else if (NewPlayer.Role.Type != RoleTypeId.Scp079)
                        {
                            NewPlayer.Position = position;
                            NewPlayer.Health = hp;
                            NewPlayer.MaxHealth = maxhp;
                            NewPlayer.Rotation = rotation;
                            NewPlayer.ArtificialHealth = Ahp;
                            NewPlayer.MaxArtificialHealth = Maxahp;
                            NewPlayer.Scale = scale;
                        }
                    });
                    SpectatorPlayer.Clear();
                }
                else
                {
                    string replace = Plugin.Singleton.Config.ScpCantBeReplace.Replace("{role}", ev.Player.Role.Type.ToString()).Replace("{name}", ev.Player.Nickname);
                    foreach (Player plr in Player.List.Where(x => x.Role.Side == Side.Scp))
                    {
                        plr.Broadcast(5,$"<color=#FF5252>{Plugin.Singleton.Config.ScpDeconnextionTitle}\n<size={Plugin.Singleton.Config.DescriptionSize}>{Plugin.Singleton.Config.ScpCantBeReplace}</size></color>");
                    }
                }

            }
        }

        public void SpawnRagdoll(SpawningRagdollEventArgs ev)
        {
            if (ev.Player == QuittedPlayer)
            {
                ev.IsAllowed = false;
            }
        }
        public void ScpTerminationAnnouncement(AnnouncingScpTerminationEventArgs ev)
        {
            if (ev.Player == QuittedPlayer)
            {
                ev.IsAllowed = false;
            }
        }
    }
}