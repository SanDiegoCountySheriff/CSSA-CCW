using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToQualifyingQuestionsMapper : IMapper<UserPermitApplicationRequestModel, QualifyingQuestions>
{
    public QualifyingQuestions Map(UserPermitApplicationRequestModel source)
    {
        return new QualifyingQuestions
        {
            QuestionOne = source.Application.QualifyingQuestions?.QuestionOne,
            QuestionOneExp = source.Application.QualifyingQuestions.QuestionOneExp,
            QuestionOneAgency = source.Application.QualifyingQuestions.QuestionOneAgency,
            QuestionOneIssueDate = source.Application.QualifyingQuestions.QuestionOneIssueDate,
            QuestionOneNumber = source.Application.QualifyingQuestions.QuestionOneNumber,
            QuestionTwo = source.Application.QualifyingQuestions?.QuestionTwo,
            QuestionTwoExp = source.Application.QualifyingQuestions.QuestionTwoExp,
            QuestionThree = source.Application.QualifyingQuestions?.QuestionThree,
            QuestionThreeExp = source.Application.QualifyingQuestions.QuestionThreeExp,
            QuestionFour = source.Application.QualifyingQuestions?.QuestionFour,
            QuestionFourExp = source.Application.QualifyingQuestions.QuestionFourExp,
            QuestionFive = source.Application.QualifyingQuestions?.QuestionFive,
            QuestionFiveExp = source.Application.QualifyingQuestions.QuestionFiveExp,
            QuestionSix = source.Application.QualifyingQuestions?.QuestionSix,
            QuestionSixExp = source.Application.QualifyingQuestions.QuestionSixExp,
            QuestionSeven = source.Application.QualifyingQuestions?.QuestionSeven,   
            QuestionSevenExp = source.Application.QualifyingQuestions.QuestionSevenExp,
            QuestionEight = source.Application.QualifyingQuestions?.QuestionEight,
            QuestionEightExp = source.Application.QualifyingQuestions.QuestionEightExp,
            QuestionNine = source.Application.QualifyingQuestions?.QuestionNine,
            QuestionNineExp = source.Application.QualifyingQuestions.QuestionNineExp,
            QuestionTen = source.Application.QualifyingQuestions?.QuestionTen,
            QuestionTenExp = source.Application.QualifyingQuestions.QuestionTenExp,
            QuestionEleven = source.Application.QualifyingQuestions?.QuestionEleven,
            QuestionElevenExp = source.Application.QualifyingQuestions.QuestionElevenExp,
            QuestionTwelve = source.Application.QualifyingQuestions?.QuestionTwelve,
            QuestionTwelveExp = source.Application.QualifyingQuestions.QuestionTwelveExp,
            QuestionThirteen = source.Application.QualifyingQuestions?.QuestionThirteen,
            QuestionThirteenExp = source.Application.QualifyingQuestions.QuestionThirteenExp,
            QuestionFourteen = source.Application.QualifyingQuestions?.QuestionFourteen,
            QuestionFourteenExp = source.Application.QualifyingQuestions.QuestionFourteenExp,
            QuestionFifteen = source.Application.QualifyingQuestions?.QuestionFifteen,
            QuestionFifteenExp = source.Application.QualifyingQuestions.QuestionFifteenExp,
            QuestionSixteen = source.Application.QualifyingQuestions?.QuestionSixteen,
            QuestionSixteenExp = source.Application.QualifyingQuestions.QuestionSixteenExp,
            QuestionSeventeen = source.Application.QualifyingQuestions?.QuestionSeventeen,
            QuestionSeventeenExp = source.Application.QualifyingQuestions.QuestionSeventeenExp,
        };
    }
}