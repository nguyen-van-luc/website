namespace Doanweb.Areas.Admin.Models
{
    public class SummerNote
    {
        public SummerNote(string idEditor, bool loadLibrary = true) 
        {
            IDEditor = idEditor;
            LoadLibrary = loadLibrary;
        }
        public string Selector { get; set; }

        // Add a constructor that takes one argument
       
        // Add this property to fix CS1061
        public bool LoadLibrary { get; set; }

        // Existing properties
        public string IDEditor { get; set; }
        public int Height { get; set; }
        public string toolBar { get; set; } = @"
            [
                ['style', [style']],
                ['font' , ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul' , 'ol', 'paragraph']]'
                ['table', ['table']],
                ['insert', ['link', elfinderFiles', 'video', elfinder']],
                ['view', ['fullscreen', 'codeview' ,'help']]
            ]
";
    }
}
