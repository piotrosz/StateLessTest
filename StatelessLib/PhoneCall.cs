using System;
using Stateless;

namespace StatelessLib
{
    public class PhoneCall
    {
        private readonly StateMachine<State, Trigger> _stateMachine;
        private DateTime? _callBegun;
        private TimeSpan _totalMinutes;

        public State State { get; private set; } = State.OffHook;

        public PhoneCall()
        {
            _stateMachine = new StateMachine<State, Trigger>(
                () => State,
                newState => State = newState);

            _stateMachine.Configure(State.OffHook)
                .Permit(Trigger.CallDialed, State.Ringing);

            _stateMachine.Configure(State.Ringing)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.CallConnected, State.Connected);

            _stateMachine.Configure(State.Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .Permit(Trigger.LeftMessage, State.OffHook)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.PlacedOnHold, State.OnHold);

            _stateMachine.Configure(State.OnHold)
                .SubstateOf(State.Connected)
                .Permit(Trigger.TakenOffHold, State.Connected)
                .Permit(Trigger.HungUp, State.OffHook)
                .Permit(Trigger.PhoneHurledAgainstWall, State.PhoneDestroyed);
        }

        public void Dial() => _stateMachine.Fire(Trigger.CallDialed);

        public void Connect() => _stateMachine.Fire(Trigger.CallConnected);

        public void EndCall() => _stateMachine.Fire(Trigger.HungUp);

        private void StartCallTimer() => _callBegun = DateTime.Now;

        private void StopCallTimer()
        {
            _totalMinutes = DateTime.Now - _callBegun.Value;
            _callBegun = null;
        }
    }

    public enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        LeftMessage,
        TakenOffHold,
        PlacedOnHold,
        PhoneHurledAgainstWall
    }

    public enum State
    {
        OffHook,
        Ringing,
        Connected,
        OnHold,
        PhoneDestroyed
    }
}
