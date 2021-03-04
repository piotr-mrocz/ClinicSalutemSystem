using NUnit.Framework;
using PESEL.Models;
using PESEL.Validators.Impl;
using System;

namespace ClinicSalutemSystem
{
    class PeselValidation
    {
        string Pesel;

        public PeselValidation(string Pesel)
        {
            this.Pesel = Pesel;
        }

        public bool CheckPesel()
        {
            var validator = new PeselValidator();
            var entity = new Entity(Pesel);


            try
            {
                var validationResult = validator.Validate(entity);
                Assert.IsTrue(validationResult.IsValid);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string Gender()
        {
            if (CheckPesel() == true)
            {
                char[] cyfry = Pesel.ToCharArray();
                char genderNumber = cyfry[9];
                if ((int)genderNumber % 2 == 0)
                {
                    return "Kobieta";
                }
                else
                {
                    return "Mężczyzna";
                }
            }
            else
            {
                return "";
            }
        }
    }
}



