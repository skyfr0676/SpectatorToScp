using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;

namespace ScpDeconnexion
{
    public class EventHandlers
    {
        public List<Player> SpectatorPlayer = new List<Player>();
        public void Disconnect(LeftEventArgs ev)
        {
            if (ev.Player.Role.Side == Exiled.API.Enums.Side.Scp)
            {
                Log.Debug("test");
            }
            foreach (Player plr in Player.List)
            {
                SpectatorPlayer.Add(plr);
            }
            Random random = new Random();
            int test = random.Next(SpectatorPlayer.Count);
            Log.Warn(test);
        }
    }
}