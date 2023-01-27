using System;

public class Observable<T> 
{
    private T _value;

    public class ChangedEventArgs : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }

    public EventHandler<ChangedEventArgs> Changed;

    public T Value
    {
        get { return _value; }

        set
        {
            if(!value.Equals(_value))
            {
                T oldValue = value;
                _value = value;

                EventHandler<ChangedEventArgs> handler = Changed;

                if(handler != null)
                {
                    handler(this, new ChangedEventArgs { OldValue = oldValue, NewValue = value });
                }
            }
        }
    }

    protected virtual void ValueChanged(T oldValue, T newValue) { }
}