using Xunit;
using Env0.Act3.Terminal;
using Env0.Act3.Terminal.Commands;
using Env0.Act3.Network;
using Env0.Act3.Config.Pocos;
using System.Collections.Generic;

namespace Env0.Act3.Tests.Commands
{
    public class PingCommandTests
    {
        [Fact]
        public void PingCommand_PingsDevice()
        {
            var devices = new List<DeviceInfo>
            {
                new DeviceInfo { Ip = "10.10.10.1", Hostname = "workstation", Subnet = "255.255.255.0" }
            };
            var session = new SessionState
            {
                NetworkManager = new NetworkManager(devices, devices[0])
            };

            var command = new PingCommand();
            var result = command.Execute(session, new string[] { "10.10.10.1" });

            Assert.NotNull(result);
            Assert.Contains("ping", result.Output.ToLower());
        }
    }
}
