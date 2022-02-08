namespace FinalInferno {
    public interface IVariableObserver<T> {
        void ValueChanged(T value);
    }
}
