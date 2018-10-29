using FluentValidation.Attributes;

using WebSurveying2017.Validation;

namespace WebSurveying2017.BindingModels
{
    [Validator(typeof(CategoryValidation))]
    public class CategoryBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CategoryId { get; set; }


    }
}