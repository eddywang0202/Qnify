using Qnify.Model.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qnify.Service.Interface
{
    public interface IAnswerService
    {
        IEnumerable<Answer> GetAnswers();
        IEnumerable<Answer> GetAnswer(List<uint> answerIds);
        IEnumerable<Answer> GetAnswer(List<string> answerTitles);
    }
}
