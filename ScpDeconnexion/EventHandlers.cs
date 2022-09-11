using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScpDeconnexion
{
    public class EventHandlers
    {
        public bool HasQuitted = false;
        public Player QuittedPlayer;

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
                        SpectatorPlayer.Add(plr);
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
                    Timing.RunCoroutine(SpawnProperties(chech, position, scale, hp, rotation, maxhp, Ahp, Maxahp, quitter, quitterRole));
                    SpectatorPlayer.Clear();
                }
                

            }


        }

        public IEnumerator<float> SpawnProperties(Player ev, Vector3 position,Vector3 scale,float hp,Vector3 rotation,int maxhp,float Ahp,float MaxAhp,Player quitter,RoleType quitterRole)
        {
            yield return Timing.WaitForSeconds(0.5f);
            if (ev.Role.Type != RoleType.Scp079)
            {
                ev.Position = position;
                ev.Health = hp;
                ev.MaxHealth = maxhp;
                ev.Rotation = rotation;
                ev.ArtificialHealth = Ahp;
                ev.MaxArtificialHealth = MaxAhp;
                ev.Scale = scale;
            }
            foreach (Player player in Player.List)
            {
                if (player.Role.Side == Exiled.API.Enums.Side.Scp)
                {
                    player.Broadcast(5, $"Scp Disconnect Alert \n<size=30>{quitterRole} ({quitter.Nickname}) has disconnect and Replace by {ev.Nickname}</size>");
                }
            }
            HasQuitted = false;
            QuittedPlayer = null;
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