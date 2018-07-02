using System;
using System.Collections.Generic;
using System.Text;

namespace MagazineStore.Models
{
    public class AnswerResponse
    {
        public string totalTime { get; set; }
        public bool answerCorrect { get; set; }

        public List<string> shouldBe { get; set; }

    }
}
