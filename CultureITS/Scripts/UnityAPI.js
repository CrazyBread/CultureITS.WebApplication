//main function
Request = function (command, data, result) {
    $.ajax({
        type: "POST",
        url: "/Api/" + command,
        data: { data: JSON.stringify(data) },
        traditional: true,
        success: result
    });
};

//unity interaction
showExhibitPopup = function (code) {
    getExhibitInfo(code, function (data) {
        $('#ExhibitHeader').text(data.name + " (" + data.location + ")");
        $('#ExhibitApplication *').remove();

        if (data.haveApplication) {
            $('#ExhibitApplication').append($('<img>').attr('src', '/Exhibit/GetImage/' + data.id));
        }
        $('#ExhibitInfo').html(data.description);
        $('#ExhibitLike *').remove();

        var $button = $('<a>');
        $button.attr('src', 'javascript:void(0)');
        $button.attr('id', 'ExhibitLikeButton');
        $button.attr('class', 'button');

        exhibitContig["Id"] = data.id;
        $button.click(function () {
            markExhibit(exhibitContig["Id"], function (data) {
                if (data.state) {
                    $button.text('Снять отметку');
                } else {
                    $button.text('Отметить для себя');
                }
            });
        });

        if (data.state) {
            $button.text('Снять отметку');
        } else {
            $button.text('Отметить для себя');
        }
        $('#ExhibitLike').append($button);

        openPopup("#ExhibitPopup");
    });
}

showTestPopup = function (code) {
    if (testConfig["State"] == false) {
        getTestInfo(code);
    } else {
        alert("Вы уже проходите тестирование");
    }
};

//exhibit subsystem
var exhibitContig = [];
exhibitContig["Id"] = 0;

markExhibit = function (id, func) {
    Request("markExhibit", { id: id }, function (data) {
        if (data.success)
            func(data);
        else
            alert(data.message);
    });
};

checkExhibit = function (id, func) {
    Request("checkExhibit", { id: id }, function (data) {
        if (data.success)
            func(data);
        else
            alert(data.message);
    });
};

getExhibitInfo = function (code, func) {
    Request("getExhibitInfo", { code: code }, function (data) {
        if (data.success)
            func(data);
        else
            alert(data.message);
    });
};

//test subsystem
var testConfig = [];
testConfig["State"] = false;
testConfig["Id"] = 0;

getTestInfo = function (code) {
    Request("getTestInfo", { code: code }, function (data) {
        if (data.success) {
            testConfig["Code"] = code;
            testConfig["Id"] = data.id;
            testConfig["QuestionsCount"] = data.questionsCount;

            $('#TestHeader').text(data.title + " (" + data.topic + ")");

            $('#TestStartTitle').text(data.title);
            $('#TestStartTopic').text(data.topic);
            $('#TestStartAuthor').text(data.author);
            $('#TestStartQuestionsCount').text(data.questionsCount);
            $('#TestStartResultsCount').text(data.resultsCount);
            $('#TestLastDate').text((data.resultsCount > 0) ? data.dateLastResult : "-");
            $('#TestStartOpened').text((data.isOpened) ? "Да" : "Нет");
            $('#TestMainQuestionCount').text(data.questionsCount);

            $('#TestStart').show();
            $('#TestMain').hide();
            $('#TestResult').hide();

            openPopup("#TestPopup");
        } else {
            alert(data.message);
        }
    });
}

startTest = function (id) {
    Request("startTest", { id: id }, function (data) {
        if (data.success) {
            testConfig["State"] = true;
            testConfig["SessionId"] = data.testSessionId;
            testConfig["QuestionsLeft"] = data.questionLeft;
            testConfig["CurrentQuestion"] = testConfig["QuestionsCount"] - testConfig["QuestionsLeft"] + 1;

            getTestQuestion(testConfig["SessionId"], testConfig["CurrentQuestion"]);

            $('#TestStart').hide();
            $('#TestMain').show();
            $('#TestResult').hide();
        } else {
            alert(data.message);
        }
    });
}

getTestQuestion = function (sessionId, questionNumber) {
    Request("getTestQuestion", { testSessionId: sessionId, questionNumber: questionNumber }, function (data) {
        if (data.success) {
            $('#TestMainQuestionNumber').text(data.number);
            $('#TestMainText').text(data.text); // +++++ Application
            testConfig["MultiChoise"] = data.allowMultiChoise;

            $('#TestMainAnswers *').remove();
            data.answers.forEach(function (item) {
                $('#TestMainAnswers').append(
                    $('<li>')
                    .append($('<input>').attr('id', item.id).attr('name', 'testAnswer').attr('type', testConfig["MultiChoise"] ? 'checkbox' : 'radio'))
                    .append($('<label>').attr('for', item.id).text(item.text))
                );
                // ++++++ Application
            });
        } else {
            alert(data.message);
        }
    });
}

setTestAnswer = function (sessionId, questionNumber, answers) {
    Request("setTestAnswer", { testSessionId: sessionId, questionNumber: questionNumber, answersNumbers: answers }, function (data) {
        if (data.success) {
            testConfig["QuestionsLeft"]--;
            testConfig["CurrentQuestion"] = data.questionNext;

            if (data.result == 1) {
                getTestQuestion(testConfig["SessionId"], testConfig["CurrentQuestion"]);
            } else {
                getTestResult(testConfig["SessionId"]);

                $('#TestStart').hide();
                $('#TestMain').hide();
                $('#TestResult').show();

                testConfig["State"] = false;
            }
        } else {
            alert(data.message);
        }
    });
}

getTestResult = function (sessionId) {
    Request("getTestResult", { testSessionId: sessionId }, function (data) {
        if (data.success) {
            $table = $('#TestResultTable');
            $('#TestResultTable *').remove();
            var $row1 = $('<tr>').append($('<th>').text("Вопрос"));
            var $row2 = $('<tr>').append($('<th>').text("Баллов"));
            var $row3 = $('<tr>').append($('<th>').text("Максимально"));

            data.questionsResults.forEach(function (item) {
                var $cellQuestion = $('<td>').text(item.number);
                if (item.result == 0)
                    $cellQuestion.addClass('error');
                else if (item.result == item.maxResult)
                    $cellQuestion.addClass('success');
                else
                    $cellQuestion.addClass('warning');

                $row1.append($cellQuestion);
                $row2.append($('<td>').text(item.result));
                $row3.append($('<td>').text(item.maxResult));
            });
            $table.append($row1);
            $table.append($row2);
            $table.append($row3);
        } else {
            alert(data.message);
        }
    });
}

$('#TestStartButton').click(function () {
    if ((!testConfig["State"]) && (testConfig["Id"] > 0)) {
        startTest(testConfig["Id"]);
    } else {
        alert("Невозможно начать тест, так как вы уже проходите тестирование или не выбрали ни одного теста.");
    }
});

$('#TestMainButton').click(function () {
    if (!testConfig["State"]) {
        alert('Вы не проходите тестирование.');
        return;
    }

    var $items = $("#TestMainAnswers input[type=" + ((testConfig["MultiChoise"]) ? "checkbox" : "radio") + "]:checked");
    var answersCount = $items.size();
    if (answersCount == 0) {
        alert('Выберите хотя бы один правильный ответ.');
        return;
    }

    var answersArray = new Array();
    $items.each(function (i) {
        answersArray.push($(this).attr('id'));
    });

    setTestAnswer(testConfig["SessionId"], testConfig["CurrentQuestion"], answersArray);
});

$(".popup_close, .popup").click(function () {
    testConfig["State"] = false;
});