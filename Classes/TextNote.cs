namespace Notebook.Classes
{
    public class TextNote
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime ModificationDate { get; set; }
        public required string Text { get; set; }

        public override string ToString()
        {
            return $"{Name} - {ModificationDate:MMMM dd, yyyy}";
        }
    }
}