using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Aniki.Extensions.Tests
{
    [TestClass()]
    public class PropertyInfoExtensionTests
    {
        [TestMethod()]
        public void GetColumnNameTest()
        {
            var type = typeof(TestClass);
            string columnName = string.Empty;
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.Name == "Id")
                {
                    columnName = propertyInfo.GetColumnName(); 
                    break;
                }
            }

            Assert.AreEqual(columnName, "User_Id");
        }

        class TestClass
        {
            [Column("User_Id")]
            public int Id { get; set; }
        }
    }
}