using Stateless;

namespace StatelessLib
{
    public class Document
    {
        public DocumentState State { get; private set; }
        public string CurrentPerson { get; private set; } = "Adam";

        private readonly StateMachine<DocumentState, DocumentTrigger> _stateMachine;

        public Document()
        {
            _stateMachine = new StateMachine<DocumentState, DocumentTrigger>(
                () => State,
                newState => State = newState);

            _stateMachine.Configure(DocumentState.FirstEmployeeInput)
                .OnEntry(FirstEmployeeOnEnter)
                .Permit(DocumentTrigger.FirstEmployeeInputFinished, DocumentState.SecondEmployeeInput);

            _stateMachine.Configure(DocumentState.SecondEmployeeInput)
                .OnEntry(SecondEmployeeOnEntry)
                .Permit(DocumentTrigger.SecondEmployeeInputFinished, DocumentState.PendingAcceptance);

            _stateMachine.Configure(DocumentState.PendingAcceptance)
                .OnEntry(AcceptanceOnEnter)
                .Permit(DocumentTrigger.AcceptedBySupervisor, DocumentState.Accepted)
                .Permit(DocumentTrigger.RejectedBySupervisor, DocumentState.FirstEmployeeFix);

            _stateMachine.Configure(DocumentState.FirstEmployeeFix)
                .OnEntry(FirstEmployeeOnEnter)
                .Permit(DocumentTrigger.FirstEmployeeInputFinished, DocumentState.SecondEmployeeFix);

            _stateMachine.Configure(DocumentState.SecondEmployeeFix)
                .OnEntry(SecondEmployeeOnEntry)
                .Permit(DocumentTrigger.SecondEmployeeInputFinished, DocumentState.PendingAcceptance);

            _stateMachine.Configure(DocumentState.Accepted)
                .Permit(DocumentTrigger.SentToArchive, DocumentState.Archived);
        }

        public void FinishFirstEmployeeEntry() => _stateMachine.Fire(DocumentTrigger.FirstEmployeeInputFinished);

        public void FinishSecondEmployeeEntry() => _stateMachine.Fire(DocumentTrigger.SecondEmployeeInputFinished);

        public void Reject() => _stateMachine.Fire(DocumentTrigger.RejectedBySupervisor);
        
        public void Accept() => _stateMachine.Fire(DocumentTrigger.AcceptedBySupervisor);

        public void Archive() => _stateMachine.Fire(DocumentTrigger.SentToArchive);
        
        public void SecondEmployeeOnEntry() => CurrentPerson = "Juliette";
        
        public void AcceptanceOnEnter() => CurrentPerson = "Samantha";

        public void FirstEmployeeOnEnter() => CurrentPerson = "Adam";
    }
}
