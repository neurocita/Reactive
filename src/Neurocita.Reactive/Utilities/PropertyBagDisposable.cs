using System.ComponentModel;
using System.Reactive.Disposables;

namespace Neurocita.Reactive.Utilities
{
    public class PropertyBagDisposable : ICancelable
    {
        ICancelable _innerCancelable;
        PropertyDescriptorCollection _properties;

        public PropertyBagDisposable()
            : this(new BooleanDisposable()) { }
            
        public PropertyBagDisposable(ICancelable cancelable)
        {
            _innerCancelable = cancelable;
            _properties = TypeDescriptor.GetProperties(this);
        }

        public object this[string key]
        {
            get { return _properties[key].GetValue(this); }
            set { _properties[key].SetValue(this, value); }
        }

        public bool IsDisposed => _innerCancelable.IsDisposed;

        public void Dispose() => _innerCancelable.Dispose();
    }
}