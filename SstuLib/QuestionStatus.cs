using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SstuLib
{
    public enum QuestionStatus
    {
        NotReceived,        //Не поступило
        NotRegistered,      //Не зарегистрировано
        InWork,             //Находится на рассмотрении *
        Explained,          //Рассмотрено. Разъяснено *
        Supported,          //Рассмотрено. Поддержано
        NotSupported,       //Рассмотрено. Не поддержано
        Transferred,        //Направлено по компетенции
        Answered,           //Дан ответ автору
        LeftWithoutAnswer   //Оставлено без ответа автору
    }
}
