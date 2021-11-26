
namespace importVtd.Business
{
    public class StatusBookmark
    {
        public bool IsSelected   { get; private set; }
        public string NameBookmark { get; private set; }

       
        //stateProcess - первая закладка 1
        //newImport - вторая закладка 2
        //dictdefect - третья хакладка 3
        //linkReper - четвертая закладка 4
        //trubJornal - пятая закладка 5
        //status - шестая закладка 6

       // public ObservableCollection<Dict> DictListStatusBookmark { get; set; }

        //public ObservableCollection<GridAct> GridActList { get; private set; }

        public StatusBookmark(bool isselected, string namebookmark)
        {
            IsSelected = isselected;
            NameBookmark = namebookmark;

        }

    }
}
