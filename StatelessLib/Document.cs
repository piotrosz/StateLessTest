using Stateless;

namespace StatelessLib
{
    public class Document
    {
        public DocumentState State { get; private set; }
        public string CurrentPerson { get; private set; } = "Adam";

        public readonly StateMachine<DocumentState, DocumentTrigger> StateMachine;

        public Document()
        {
            StateMachine = new StateMachine<DocumentState, DocumentTrigger>(
                () => State,
                newState => State = newState);

            StateMachine.Configure(DocumentState.FirstEmployeeInput)
                .OnEntry(FirstEmployeeOnEnter)
                .Permit(DocumentTrigger.FirstEmployeeInputFinished, DocumentState.SecondEmployeeInput);

            StateMachine.Configure(DocumentState.SecondEmployeeInput)
                .OnEntry(SecondEmployeeOnEntry)
                .Permit(DocumentTrigger.SecondEmployeeInputFinished, DocumentState.PendingAcceptance);

            StateMachine.Configure(DocumentState.PendingAcceptance)
                .OnEntry(AcceptanceOnEnter)
                .Permit(DocumentTrigger.AcceptedBySupervisor, DocumentState.Accepted)
                .Permit(DocumentTrigger.RejectedBySupervisor, DocumentState.FirstEmployeeFix);

            StateMachine.Configure(DocumentState.FirstEmployeeFix)
                .OnEntry(FirstEmployeeOnEnter)
                .Permit(DocumentTrigger.FirstEmployeeInputFinished, DocumentState.SecondEmployeeFix);

            StateMachine.Configure(DocumentState.SecondEmployeeFix)
                .OnEntry(SecondEmployeeOnEntry)
                .Permit(DocumentTrigger.SecondEmployeeInputFinished, DocumentState.PendingAcceptance);

            StateMachine.Configure(DocumentState.Accepted)
                .Permit(DocumentTrigger.SentToArchive, DocumentState.Archived);
        }

        public void FinishFirstEmployeeEntry() => StateMachine.Fire(DocumentTrigger.FirstEmployeeInputFinished);

        public void FinishSecondEmployeeEntry() => StateMachine.Fire(DocumentTrigger.SecondEmployeeInputFinished);

        public void Reject() => StateMachine.Fire(DocumentTrigger.RejectedBySupervisor);
        
        public void Accept() => StateMachine.Fire(DocumentTrigger.AcceptedBySupervisor);

        public void Archive() => StateMachine.Fire(DocumentTrigger.SentToArchive);
        
        public void SecondEmployeeOnEntry() => CurrentPerson = "Juliette";
        
        public void AcceptanceOnEnter() => CurrentPerson = "Samantha";

        public void FirstEmployeeOnEnter() => CurrentPerson = "Adam";
    }
}
