using System;
using System.Linq;
using System.Text.RegularExpressions;
using LotusLib.Auxiliary;
using LotusLib.Documents;
using SstuLib;
using SstuLib.Exceptions;
using SstuLib.Questions;

namespace fSedToSstu
{
    public static class Utils
    {
        public static Request CreateRequest(this OgDocument ogDoc)
        {
            var reqFormat = ogDoc.DeliveryType.Contains("Электронное сообщение")
                ? RequestFormat.Electronic
                : RequestFormat.Other;
            var isDirect = string.IsNullOrEmpty(ogDoc.Specialization);
            var number = isDirect
                ? ogDoc.Number
                : ogDoc.OutRegistrationNumber;
            var request = new Request
            {
                RequestFormat = reqFormat,
                CreateDate = ogDoc.OutRegistrationDate,
                IsDirect = isDirect,
                Name = ogDoc.DeclarantTitle,
                Number = number
            };
            var question = CreateQuestion(request, ogDoc);
            request.Questions.Add(question);
            if (!request.Validate())
                throw new ValidationException();

            return request;
        }

        private static SstuLib.Auxiliary.Attachment GetSstuAttach(this Attachment lotusAttach)
        {
            return new SstuLib.Auxiliary.Attachment(lotusAttach.Name, lotusAttach.GetContent());
        }

        private static Question CreateQuestion(Request req, OgDocument ogDoc)
        {
            var code = GetCode(ogDoc.Subject);
            switch (ogDoc.Solution)
            {
                case Solution.InWork:
                    return new InWork(req)
                    {
                        Code = code,
                        IncomingDate = ogDoc.AcceptanceDate,
                        RegistrationDate = ogDoc.RegistrationDate
                    };
                case Solution.Redirected:
                    throw new NotImplementedException("Статус направление по подведомости не поддерживается");
                case Solution.LeftWithoutAnswer:
                    throw new NotImplementedException("Статус оставленно без рассмотрения не поддерживается");
                default:
                    if (ogDoc.Attachments.Length != 1)
                        throw new Exception("Неверное кол-во вложений");
                    return new Answered(req)
                    {
                        Code = code,
                        IncomingDate = ogDoc.AcceptanceDate,
                        RegistrationDate = ogDoc.RegistrationDate,
                        ResponseDate = ogDoc.Results.First().CloseDate,
                        Attachment = ogDoc.Attachments.First().GetSstuAttach()
                    };
            }
        }

        private static string GetCode(string subject)
        {
            Regex regex = new Regex(@"^\s*(\d{4}).*");
            var match = regex.Match(subject);
            if (match.Groups[1].Success)
            {
                return match.Groups[1].Value;
            }
            return "0833";
        }
    }
}
