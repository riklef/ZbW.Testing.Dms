using System;

namespace ZbW.Testing.Dms.Client.Model
{
    public class MetadataItem
    {
        // TODO: Write your Metadata properties here

        public string _benutzer { get; set; }

        public string _bezeichnung { get; set; }

        public DateTime _erfassungsdatum { get; set; }

        public string _filePath { get; set; }

        public string _selectedTypItem { get; set; }

        public string _stichwoerter { get; set; }

        public DateTime? _valutaDatum { get; set; }

        public MetadataItem(string benutzer, string bezeichnung, DateTime erfassungsdatum, string filePath,
            string selectedTypItem, string stichwoerter, DateTime? valutaDatum)
        {
            _benutzer = benutzer;
            _bezeichnung = bezeichnung;
            _erfassungsdatum = erfassungsdatum;
            _filePath = filePath;
            _selectedTypItem = selectedTypItem;
            _stichwoerter = stichwoerter;
            _valutaDatum = valutaDatum;
        }
        public MetadataItem(){}
    }
}