using Notebook.Classes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Notebook
{
    public partial class MainPage : ContentPage
    {
        public static ObservableCollection<TextNote> Notes = new();
        public MainPage()
        {
            InitializeComponent();

            // Loading the notes (if saved) from the disk
            LoadNotesFromDisk();

            // Setting the item source of the UI collection
            NotesCollectionView.ItemsSource = Notes;

            // Trigger the disk saver on note collection change event
            Notes.CollectionChanged += OnNotesChanged;
        }

        // Method for loading notes collection from the disk
        private void LoadNotesFromDisk()
        {
            // Get the saved file path
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "noteslist.json");

            // Check if the file exists
            if (!File.Exists(filePath)) return;

            // Read the json file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON back to ObservableCollection
            ObservableCollection<TextNote>? NotesFromDisk = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<TextNote>>(json);

            // Check if the list is null
            if (NotesFromDisk == null) return;

            // Set the notes collection from the disk data
            Notes = NotesFromDisk;
        }

        // Note collection change event handler for saving the notes to json save
        private void OnNotesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Get the save file path
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "noteslist.json");
            // Create the json data from the notes collection
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Notes);
            // Saving the json to the save file
            File.WriteAllText(filePath, json);
        }

        // Plus button event handler
        private void OnAddItemClicked(object sender, EventArgs e)
        {
            // A for loop for finding the a new ID for the new note
            int NewNoteId = 0;
            for (int i = 0; i < Notes.Count; i++)
            {
                // Current loop item
                TextNote Note = Notes[i];

                // Check if the current NewId is smaller or equal to the loop item id
                if (Note.Id >= NewNoteId)
                {
                    // Add 1 to the loop item id
                    NewNoteId = Note.Id + 1;
                }
            }

            // Creating new note object
            TextNote NewTextNote = new TextNote
            {
                Id = NewNoteId,
                Name = "New note",
                ModificationDate = DateTime.Now,
                Text = ""
            };

            // Adding the note object to 
            Notes.Add(NewTextNote);
        }

        // Note item tap event handler
        private async void OnNoteTapped(object sender, EventArgs e)
        {
            // get the frame from the event parameters
            Frame? frame = sender as Frame;
            // Nullable frame handler
            if (frame == null) return;
            // Get the note object from the frame binding
            TextNote? tappedItem = frame.BindingContext as TextNote;
            // Nullable note object handler
            if (tappedItem == null) return;
            // Additional check for item object being present in the notes collection
            if (!Notes.Contains(tappedItem)) return;
            // Opening the text note page with the note object
            await Navigation.PushAsync(new TextNotePage(tappedItem));
        }
    }

}