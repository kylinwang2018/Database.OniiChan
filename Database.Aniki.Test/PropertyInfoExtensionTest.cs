using System.ComponentModel.DataAnnotations.Schema;
using Database.Aniki.Extensions;

namespace Database.Aniki.Test
{
    [TestClass()]
    public class PropertyInfoExtensionTests
    {
        [TestMethod()]
        public void GetColumnNameWithColumnAttr()
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

        [TestMethod()]
        public void GetColumnNameTestWithoutColumnAttr()
        {
            var type = typeof(TestClass);
            string columnName = string.Empty;
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.Name == "User")
                {
                    columnName = propertyInfo.GetColumnName();
                    break;
                }
            }

            Assert.AreEqual(columnName, "User");
        }

        class TestClass
        {
            [Column("User_Id")]
            public int Id { get; set; }

            public int User { get; set; }
        }
    }
}