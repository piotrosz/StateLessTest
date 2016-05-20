using FluentAssertions;
using Xunit;

namespace StatelessLib.Tests
{
    public class StateMachineTest
    {
        [Fact]
        public void test()
        {
            var phoneCall = new PhoneCall();

            phoneCall.Dial();
            phoneCall.State.Should().Be(State.Ringing);

            phoneCall.Connect();
            phoneCall.State.Should().Be(State.Connected);

            phoneCall.EndCall();
            phoneCall.State.Should().Be(State.OffHook);
        }
    }
}
