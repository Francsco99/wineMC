using System;
namespace SommeliAr.Models
{
    public class MyWineModel
    {
        public string Name { get; set; }        //nome del vino
        public string Detail { get; set; }      //sottotilo: colore vino
        public string Image { get; set; }       //url immagine
        public string Description { get; set; }  //breve descrizione
        public string SensorialNotes { get; set; }  //note sensoriali
        public string ProductionArea { get; set; }  //area produzione
        public string Dishes { get; set; }         //abbinamenti pietanze
        public string Rating { get; set; }         //voto vino

    }
}
