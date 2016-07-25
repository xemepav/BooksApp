using System;
using System.Xml.Serialization;
using BooksApp.Interfaces.Entities;

namespace BooksApp.BLL
{
    [Serializable]
    public class XmlBook : IBook
    {
        [XmlElement("ISBN")]
        public string Isbn { get; set; }

        [XmlElement("Author")]
        public string Author { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }
    }

    [XmlRoot("Books")]
    public class XmlBookCollection
    {
        [XmlElement("Book")]
        public XmlBook[] XmlBooks { get; set; }
    }
}