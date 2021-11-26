using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-crovinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualResult = accountController.ValidateEmail(email);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
            Test,
            TestCase("Abcdefghij", false),
            TestCase("ABCDEFGHIJ", false),
            TestCase("abcdefghij", false),
            TestCase("Rovid1", false),
            TestCase("Megfelelo123", true)
            /*"A jelszó legalább 8 karakter hosszú kell legyen, csak az angol ABC betűiből
             * és számokból állhat, és tartalmaznia kell legalább egy kisbetűt,
             * egy nagybetűt és egy számot." */
            ]
        public void TestValidatePassword(string password, bool expectedResult)
        {

            var accountController = new AccountController();

            var actualResult = accountController.ValidatePassword(password);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
