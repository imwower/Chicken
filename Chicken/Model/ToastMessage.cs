using System;

namespace Chicken.Model
{
    public class ToastMessage
    {
        public string Message { get; set; }

        public Action Complete { get; set; }
    }
}
