using Stateless;

namespace StatelessLib
{
    public class Document
    {
        private StateMachine<DocumentState, DocumentTrigger> _stateMachine;
        
        public DocumentState State { get; private set; }

        public Document()
        {
            _stateMachine = new StateMachine<DocumentState, DocumentTrigger>(
                () => State,
                newState => State = newState);

            _stateMachine.Configure(DocumentState.FirstEmployeeInput)
                .OnExit(TestOnExit)
                .Permit(DocumentTrigger.FirstEmployeeInputFinished, DocumentState.SecondEmployeeInput);

            _stateMachine.Configure(DocumentState.SecondEmployeeInput)
                .OnEntry(TestOnEntry)
                .OnExit(TestOnExit)
                .Permit(DocumentTrigger.SecondEmployeeInputFinished, DocumentState.PendingAcceptance);

            _stateMachine.Configure(DocumentState.PendingAcceptance)
                .Permit(DocumentTrigger.AcceptedBySupervisor, DocumentState.Accepted)
                .Permit(DocumentTrigger.RejectedBySupervisor, DocumentState.FirstEmployeeFix);
        }

        public void FinishFirstEmployeeEntry()
        {
            _stateMachine.Fire(DocumentTrigger.FirstEmployeeInputFinished);
        }

        public void FinishSecondEmployeeEntry()
        {
            _stateMachine.Fire(DocumentTrigger.SecondEmployeeInputFinished);
        }

        public void TestOnEntry()
        {
            
        }

        public void TestOnExit()
        {
            
        }
    }
}
