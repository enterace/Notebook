using Notebook.Classes;
namespace Notebook;

public partial class TextNotePage : ContentPage
{
    // Current note that is open
    private TextNote? CurrentTextNote;
    public TextNotePage(TextNote SelectedTextNote)
    {
        InitializeComponent();

        // Save the current open note
        CurrentTextNote = SelectedTextNote;

        // Write the name and text of the selected note on UI
        NoteNameEntry.Text = CurrentTextNote.Name;
        TextNoteEntry.Text = CurrentTextNote.Text;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Nullable handling
        if (CurrentTextNote == null) return;

        // Looping notes
        for (int i = 0; i < MainPage.Notes.Count; i++)
        {
            // Current note on loop
            TextNote textNote = MainPage.Notes[i];

            // Find the matching note in the list to edit
            if (CurrentTextNote.Id == textNote.Id)
            {
                // Removing the note to replace it later
                MainPage.Notes.Remove(textNote);

                // Overriding note
                CurrentTextNote.Name = NoteNameEntry.Text;
                CurrentTextNote.Text = TextNoteEntry.Text;

                // Creating the note object
                TextNote EditedTextNote = new TextNote
                {
                    Id = textNote.Id,
                    Name = CurrentTextNote.Name,
                    ModificationDate = DateTime.Now,
                    Text = CurrentTextNote.Text
                };

                // Adding the edited note to the list
                MainPage.Notes.Add(EditedTextNote);

                // Opening the main page
                await Navigation.PopAsync();
            }
        }
    }
}