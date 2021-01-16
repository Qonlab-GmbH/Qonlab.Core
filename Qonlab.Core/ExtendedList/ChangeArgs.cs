
namespace Qonlab.Core.ExtendedList {
    public class ChangeArgs<T> {
        public ChangeType ChangeType { get; set; }
        public T Item { get; set; }
    }
}
