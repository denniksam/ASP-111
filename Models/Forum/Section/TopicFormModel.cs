using ASP_111.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ASP_111.Models.Forum.Section
{
    public class TopicFormModel
    {
        [FromForm(Name = "topic-title")]
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Title { get; set; } = null!;


        [FromForm(Name = "topic-description")]
        public String Description { get; set; } = null!;


        [FromForm(Name = "topic-image")]
        public IFormFile ImageFile { get; set; } = null!;


        [FromForm(Name = "section-id")]
        [ValidationRules(ValidationRule.NotEmpty)]
        public Guid SectionId { get; set; }
    }
}
