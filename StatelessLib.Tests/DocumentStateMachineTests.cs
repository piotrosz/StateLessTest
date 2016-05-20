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

            doc.CurrentPerson.Should().Be("Adam");
            doc.FinishFirstEmployeeEntry();
            doc.State.Should().Be(DocumentState.SecondEmployeeInput);
            doc.CurrentPerson.Should().Be("Juliette");

            doc.FinishSecondEmployeeEntry();
            doc.State.Should().Be(DocumentState.PendingAcceptance);
            doc.CurrentPerson.Should().Be("Samantha");

            doc.Reject();
            doc.State.Should().Be(DocumentState.FirstEmployeeFix);
            doc.CurrentPerson.Should().Be("Adam");

            doc.FinishFirstEmployeeEntry();
            doc.State.Should().Be(DocumentState.SecondEmployeeFix);
            doc.CurrentPerson.Should().Be("Juliette");

            doc.FinishSecondEmployeeEntry();
            doc.State.Should().Be(DocumentState.PendingAcceptance);
            doc.CurrentPerson.Should().Be("Samantha");

            doc.Accept();
            doc.State.Should().Be(DocumentState.Accepted);
            
            doc.Archive();
            doc.State.Should().Be(DocumentState.Archived);
        }
    }
}
