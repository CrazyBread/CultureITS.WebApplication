using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CultureITS.Models.Api
{
    public struct TestInfoIn
    {
        public string code { set; get; }
    }

    public struct TestInfoOut
    {
        public bool success { set; get; }

        public int id { set; get; }
        public string title { set; get; }
        public string topic { set; get; }
        public string author { set; get; }
        public int questionsCount { set; get; }
        public int resultsCount { set; get; }
        public string dateLastResult { set; get; }
        public bool isOpened { set; get; }
    }

    public struct TestStartIn
    {
        public int id { set; get; }
    }

    public struct TestStartOut
    {
        public bool success { set; get; }

        public int testSessionId { set; get; }
        public int questionLeft { set; get; }
    }

    public struct TestQuestionIn
    {
        public int testSessionId { set; get; }
        public int questionNumber { set; get; }
    }

    public struct TestQuestionOut
    {
        public bool success { set; get; }

        public int number { set; get; }
        public string text { set; get; }
        public bool hasImage { set; get; }
        public bool allowMultiChoise { set; get; }
        public TestAnswers[] answers { set; get; }
    }

    public struct TestAnswerIn
    {
        public int testSessionId { set; get; }
        public int questionNumber { set; get; }
        public int[] answersNumbers { set; get; }
    }

    public struct TestAnswerOut
    {
        public bool success { set; get; }

        public int result { set; get; }
        public int questionNext { set; get; }
    }

    public struct TestResultIn
    {
        public int testSessionId { set; get; }
    }

    public struct TestResultOut
    {
        public bool success { set; get; }

        public DateTime date { set; get; }
        public TestResults[] questionsResults { set; get; }
    }

    #region Вспомогательные структуры
    public struct TestAnswers
    {
        public int id { set; get; }
        public string text { set; get; }
        public bool hasImage { set; get; }
    }

    public struct TestResults
    {
        public int number { set; get; }
        public int result { set; get; }
        public int maxResult { set; get; }
    }

    public struct TestData
    {
        public TestDataItem[] Queue { set; get; }
    }

    public struct TestDataItem
    {
        public int id { set; get; }
        public int result { set; get; }
        public int maxResult { set; get; }
        public bool complete { set; get; }
    }
    #endregion
}