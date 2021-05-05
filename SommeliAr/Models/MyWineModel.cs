using System;
namespace SommeliAr.Models
{
    public class MyWineModel
    {
        public string Name { get; set; }        //nome del vino
        public string Detail { get; set; }      //sottotilo: colore vino, effervescenza, anno raccolta
        public string Image { get; set; }       //url immagine
        public string Description { get; set; } //dettaglio vino: produttore e descrizione vino
        public string Rating { get; set; }         //voto vino

    }
}
