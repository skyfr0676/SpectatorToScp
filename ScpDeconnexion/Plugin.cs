﻿using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event = Exiled.Events.Handlers;

namespace ScpDeconnexion
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "sky";

        public override string Name { get; } = "ScpDeconnexion";

        public override string Prefix { get; } = "AntiScpD";
        
        public override Version Version { get; } = new Version(1, 0, 0);

        public override Version RequiredExiledVersion { get; } = new Version(5, 3, 0);
        
        public static Plugin Singleton;

        public static EventHandlers Handlers;

        public override void OnEnabled()
        {
            Log.Debug("Hello ! i'm actually Registering All Events and List...");
            Singleton = this;
            Handlers = new EventHandlers();
            Event.Map.AnnouncingScpTermination += Handlers.ScpTerminationAnnouncement;
            Event.Player.SpawningRagdoll += Handlers.SpawnRagdoll;
            Event.Player.Left += Handlers.Disconnect;
            Log.Debug("Loaded !");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Log.Info("goodbye !");
            Event.Player.Left -= Handlers.Disconnect;
            Event.Map.AnnouncingScpTermination -= Handlers.ScpTerminationAnnouncement;
            Event.Player.SpawningRagdoll -= Handlers.SpawnRagdoll;
            Handlers = null;
            Singleton = null;
            base.OnDisabled();
        }
    }
}
