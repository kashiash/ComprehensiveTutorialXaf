using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ComphrehensiveTuririalXaf.Module.BusinessObjects
{
    using DevExpress.ExpressApp.Model;
    using DevExpress.Persistent.Base;
    using DevExpress.Persistent.BaseImpl;
    using DevExpress.Xpo;

    // ...
    [DefaultClassOptions]
    [ModelDefault("Caption", "Task")]
    public class DemoTask : Task
    {
        public DemoTask(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }

        private Priority priority;
        public Priority Priority
        {
            get { return priority; }
            set
            {
                SetPropertyValue(nameof(Priority), ref priority, value);
            }
        }

        [Association("Contact-DemoTask")]
        public XPCollection<Contact> Contacts
        {
            get
            {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }
    }
    public enum Priority
    {
        Low = 0,
        Normal = 1,
        High = 2
    }
}
