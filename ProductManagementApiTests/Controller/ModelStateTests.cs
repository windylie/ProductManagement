using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.InputDto;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class ModelStateTests
    {
        private ProductController _productController;

        [SetUp]
        public void Setup()
        {
            _productController = new ProductController();
        }

        [TestCaseSource(typeof(TestData), "TestCases")]
        public void When_create_or_update_product_then_validate_all_input(
            CreateAndEditProductDto dto, bool expectedValidStatus, int expectedNoOfError)
        {
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), result); 

            Assert.AreEqual(expectedValidStatus, isValid); 
            Assert.AreEqual(expectedNoOfError, result.Count);
        }

        public class TestData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "",
                            Model = "",
                            Brand = ""
                        }, false, 3).SetDescription("Test empty all data");
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "",
                            Model = "Model",
                            Brand = "Brand"
                        }, false, 1).SetDescription("Test empty description data");
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "Description",
                            Model = "",
                            Brand = ""
                        }, false, 2).SetDescription("Test empty model and brand data");
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "Description",
                            Model = "Model",
                            Brand = "Brand"
                        }, true, 0).SetDescription("Test valid data");
                }
                
            }
        }
    }
}
