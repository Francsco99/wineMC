using System;
using System.Collections.Generic;
using System.Text;

namespace SommeliAr.Views
{
    public class Prediction
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public double Probability { get; set; }
        public (double, double, double, double) BoundingBox { get; set; }
    }

    public class Response
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public IList<Prediction> Predictions { get; set; }
    }
}
