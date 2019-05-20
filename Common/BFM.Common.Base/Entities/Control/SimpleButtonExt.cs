using System;

namespace XL.CSharp.Commom.Entities.Control
{
    public class SimpleButtonExt
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Action<object, EventArgs> actionClick;
        public Action<object, EventArgs> ActionClick
        {
            get { return actionClick; }
            set { actionClick = value; }
        }

        public SimpleButtonExt(string name, Action<object, EventArgs> act)
        {
            this.Name = name;
            this.ActionClick = act;
        }

    }
}
