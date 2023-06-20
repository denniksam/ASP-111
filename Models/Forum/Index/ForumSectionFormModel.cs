using Microsoft.AspNetCore.Mvc;

namespace ASP_111.Models.Forum.Index
{
    public class ForumSectionFormModel
    {
        [FromForm(Name = "section-title")]
        public String Title { get; set; } = null!;


        [FromForm(Name = "section-description")]
        public String Description { get; set; } = null!;


        [FromForm(Name = "section-image")]
        public IFormFile ImageFile { get; set; } = null!;

    }
}
