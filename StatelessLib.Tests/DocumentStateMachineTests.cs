using System;
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

        [Fact]
        public void test_additional_check()
        {
            var doc = new Document {AddtionalCheck = true};

            doc.FinishFirstEmployeeEntry();
            doc.FinishSecondEmployeeEntry();
            doc.State.Should().Be(DocumentState.AdditionalCheck);
            doc.FinishAdditionalCheck();
            doc.State.Should().Be(DocumentState.PendingAcceptance);
        }

        [Fact]
        public void when_invalid_transition_then_exception_thrown()
        {
            var doc = new Document();

            Action action = () => doc.FinishSecondEmployeeEntry();

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void export_to_dot_graph()
        {
            var doc = new Document();

            string graph = doc.StateMachine.ToDotGraph();
        }
    }
}
