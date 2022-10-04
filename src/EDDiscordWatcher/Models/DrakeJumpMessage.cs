using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Models
{
    internal class DrakeJumpMessage
    {
        public string schemaRef { get; set; }
        public Header header { get; set; }
        public Message message { get; set; }

        public class Header
        {
            public DateTime gatewayTimestamp { get; set; }
            public string softwareName { get; set; }
            public string softwareVersion { get; set; }
            public string uploaderID { get; set; }
        }

        public class Message
        {
            public string Body { get; set; }
            public int BodyID { get; set; }
            public string BodyType { get; set; }
            public bool Docked { get; set; }
            public Faction[] Factions { get; set; }
            public long MarketID { get; set; }
            public long Population { get; set; }
            public float[] StarPos { get; set; }
            public string StarSystem { get; set; }
            public Stationeconomy[] StationEconomies { get; set; }
            public string StationEconomy { get; set; }
            public Stationfaction StationFaction { get; set; }
            public string StationGovernment { get; set; }
            public string StationName { get; set; }
            public string[] StationServices { get; set; }
            public string StationType { get; set; }
            public long SystemAddress { get; set; }
            public string SystemAllegiance { get; set; }
            public string SystemEconomy { get; set; }
            public Systemfaction SystemFaction { get; set; }
            public string SystemGovernment { get; set; }
            public string SystemSecondEconomy { get; set; }
            public string SystemSecurity { get; set; }
            public string _event { get; set; }
            public bool horizons { get; set; }
            public bool odyssey { get; set; }
            public DateTime timestamp { get; set; }
        }

        public class Stationfaction
        {
            public string Name { get; set; }
        }

        public class Systemfaction
        {
            public string FactionState { get; set; }
            public string Name { get; set; }
        }

        public class Faction
        {
            public string Allegiance { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public string Happiness { get; set; }
            public float Influence { get; set; }
            public string Name { get; set; }
            public Activestate[] ActiveStates { get; set; }
        }

        public class Activestate
        {
            public string State { get; set; }
        }

        public class Stationeconomy
        {
            public string Name { get; set; }
            public float Proportion { get; set; }
        }

    }
}
