using FluentAssertions;
using Xunit;

namespace StatelessLib.Tests
{
    public class DocumentStateMachineTests
    {
        [Fact]
        public void test_entry_action()
        {
            var doc = new Document();

            doc.FinishFirstEmployeeEntry();
            doc.State.Should().Be(DocumentState.SecondEmployeeInput);

            doc.FinishSecondEmployeeEntry();
            doc.State.Should().Be(DocumentState.PendingAcceptance);

            doc.Reject();
            doc.State.Should().Be(DocumentState.FirstEmployeeFix);

            doc.FinishFirstEmployeeEntry();
            doc.State.Should().Be(DocumentState.SecondEmployeeFix);

            doc.FinishSecondEmployeeEntry();
            doc.State.Should().Be(DocumentState.PendingAcceptance);

            doc.Accept();
            doc.State.Should().Be(DocumentState.Accepted);
            
            doc.Archive();
            doc.State.Should().Be(DocumentState.Archived);
        }
    }
}
