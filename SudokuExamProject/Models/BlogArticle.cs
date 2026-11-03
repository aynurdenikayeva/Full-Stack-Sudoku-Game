using System.Collections.Generic;

namespace SudokuExamProject.Models
{
    public class BlogArticle
    {
        public string Slug { get; set; } = "";
        public string Category { get; set; } = "";
        public string Title { get; set; } = "";
        public string Date { get; set; } = "";
        public string Introduction { get; set; } = "";
        public List<BlogSection> Sections { get; set; } = new();
    }

    public class BlogSection
    {
        public string Heading { get; set; } = "";
        public List<string> Paragraphs { get; set; } = new();
        public List<string> Points { get; set; } = new();
    }
}

