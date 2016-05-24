using FluentAssertions;
using Xunit;

namespace StatelessLib.Tests
{
    public class DocumentStateMachineTests
    {
        [Fact]
        public void when_workflow_with_rejection_fired_then_persons_and_states_assigned()
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
