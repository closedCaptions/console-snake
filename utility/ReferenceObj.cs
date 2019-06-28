using System;
using System.Collections.Generic;
using System.Text;

namespace console_snake.utility {
    public class ReferenceObj <T> {
        private Func<T> _get { get; set; }
        private Action<T> _set { get; set; }

        public ReferenceObj (Func<T> @get, Action<T> @set) {
            _get = @get;
            _set = @set;
        }
        
        public T Value {
            get { return _get(); }
            set { _set(value); }
        }
    }
}
