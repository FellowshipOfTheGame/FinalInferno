namespace FinalInferno.EventSystem {
    public interface IGenericEventListenerFI<T> {
        void OnEventRaised(T value);
    }
}
