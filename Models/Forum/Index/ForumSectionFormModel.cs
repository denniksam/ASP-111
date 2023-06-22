using ASP_111.Services.Validation;
using Microsoft.AspNetCore.Mvc;

namespace ASP_111.Models.Forum.Index
{
    public class ForumSectionFormModel
    {
        [FromForm(Name = "section-title")]
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Title { get; set; } = null!;


        [FromForm(Name = "section-description")]
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Description { get; set; } = null!;


        [FromForm(Name = "section-image")]
        public IFormFile ImageFile { get; set; } = null!;

    }
}
