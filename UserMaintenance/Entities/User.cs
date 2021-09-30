using System;

namespace UserMaintenance.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        public User()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
