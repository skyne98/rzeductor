using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Person
    {
        public string FirstName;
        public string Lastname;
        public Country Country;
        public string DateOfBirth;
        public string ExpiryDate;
    }
}