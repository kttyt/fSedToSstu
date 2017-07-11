using System;
using System.Linq;
using Domino;
using LotusLib.Auxiliary;

namespace LotusLib.Documents
{
    public class OgDocument : Document
    {
        public string Number                    { get; private set; }
        public string DeliveryType              { get; private set; }
        public Solution Solution                { get; private set; }
        public string Specialization            { get; private set; }
        public DateTime? OutRegistrationDate    { get; private set; }
        public string OutRegistrationNumber     { get; private set; }
        public DateTime AcceptanceDate          { get; private set; }
        public DateTime RegistrationDate        { get; private set; }
        public string Subject                   { get; private set; }
        public string DeclarantTitle            { get; private set; }
        public string GroudId                   { get; private set; }
        public OgResult[] Results               { get; internal set; }

        internal OgDocument(string id) : base(id)
        {
        }


        public bool ValidateResults()
        {
            return Results?.All(result => result.ReferenceNumber.Equals(Number)) ?? true;
        }

        internal static OgDocument FromNotes(NotesDocument document)
        {
            var outRegStr = document.GetFieldValue("Out_RegistrationDate");
            DateTime? outRegDate = null;
            if (!string.IsNullOrEmpty(outRegStr))
                outRegDate = DateTime.Parse(outRegStr);

            return new OgDocument(document.GetFieldValue("id"))
            {
                DeliveryType = document.GetFieldValue("delivery_type"),
                Solution = ParseSolution(document.GetFieldValue("solution")),
                Specialization = document.GetFieldValue("Specialization"),
                OutRegistrationDate = outRegDate,
                OutRegistrationNumber = document.GetFieldValue("Out_RegistrationNumber"),
                AcceptanceDate = DateTime.Parse(document.GetFieldValue("acceptance_date")),
                RegistrationDate = DateTime.Parse(document.GetFieldValue("registration_date")),
                Subject = document.GetFieldValue("subject"),
                GroudId = document.GetFieldValue("group_id"),
                DeclarantTitle = document.GetFieldValue("declarant_title"),
                Number = document.GetFieldValue("registration_number")
            };
        }

        private static Solution ParseSolution(string str)
        {
            if (string.IsNullOrEmpty(str))
                return Solution.InWork;
            if (str.Equals("не поддержано"))
                return Solution.NotSupp;
            if (str.Equals("поддержано"))
                return Solution.Supp;
            if (str.Equals("разъяснено"))
                return Solution.Answered;
            if (str.Equals("направлено по подведомственности"))
                return Solution.Redirected;
            throw new NotImplementedException($"Неизвестное решение '{str}'");
        }
    }
}