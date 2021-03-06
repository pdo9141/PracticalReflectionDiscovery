﻿namespace OrderTaker.SharedObjects
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName {
            get {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        
        public int Rating { get; set; }

        public string CustomerSince { get; set; }
    }
}
