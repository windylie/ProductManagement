using NUnit.Framework;
using ProductManagementApi.InputDto;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class ModelStateTests
    {
        [TestCaseSource(typeof(TestData), "TestCases")]
        public void When_create_or_update_product_then_validate_all_input(
            CreateAndEditProductDto dto, int expectedNoOfError)
        {
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), result); 

            Assert.IsFalse(isValid); 
            Assert.AreEqual(expectedNoOfError, result.Count);
        }

        [Test]
        public void When_all_properties_are_provided_then_return_valid()
        {
            var dto = new CreateAndEditProductDto()
            {
                Description = "Description",
                Model = "Model",
                Brand = "Brand"
            };
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), result);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(result);
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
                        }, 3).SetName("When_all_data_empty_return_invalid");
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "",
                            Model = "Model",
                            Brand = "Brand"
                        }, 1).SetName("When_description_empty_return_invalid");
                    yield return new TestCaseData(
                        new CreateAndEditProductDto()
                        {
                            Description = "Description",
                            Model = "",
                            Brand = ""
                        }, 2).SetName("When_model_and_brand_empty_return_invalid");
                }
                
            }
        }
    }
}
